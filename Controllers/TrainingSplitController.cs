using Grit.ViewModels;
using Grit.ViewModels.Training;
using Microsoft.AspNet.Identity;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Grit.Models
{
    [Authorize]
    public class TrainingSplitController : Controller
    {
        private ApplicationDbContext _context;
        private static readonly double METlow = 3.5; // Metabolic equivalent of task for weight lifting low intensity
        private static readonly double METhigh = 6; // Metabolic equivalent of task for weight lifting high intensity

        public TrainingSplitController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        public ActionResult Training()
        {
            var user = _context.Users.Find(User.Identity.GetUserId());

            var activeSplit = _context.TrainingSplits.SingleOrDefault(x => x.Id == user.ActiveWorkout_Id);
            if (activeSplit != null)
            {
                var workouts = _context.Workouts.Where(x => x.TrainingSplit_Id == activeSplit.Id).GroupBy(x => x.Name)
                                                  .Select(x => x.OrderByDescending(w => w.Date).FirstOrDefault()).ToList();

                var workoutDays = new List<WorkoutDay>();

                foreach (Workout workout in workouts)
                {
                    var exercises = _context.Exercises.Where(x => x.Workout_Id == workout.Id).ToList();
                    foreach (var exercise in exercises)
                    {
                        exercise.Sets = _context.Sets.Where(x => x.Exercise_Id == exercise.Id).ToList();
                    }

                    var workoutDay = new WorkoutDay
                    {
                        Workout = workout,
                        Exercises = exercises

                    };

                    workoutDays.Add(workoutDay);
                }

                var model = new TrainingViewModel
                {
                    Splits = _context.UserSplits.Where(x => x.UserID == user.Id).Select(x => x.Split).ToList(),
                    ActiveSplit = activeSplit,
                    WorkoutDays = workoutDays
                };

                return View(model);
            }
            else
            {
                var model = new TrainingViewModel
                {
                    Splits = _context.UserSplits.Where(x => x.UserID == user.Id).Select(x => x.Split).ToList(),
                    ActiveSplit = activeSplit
                };

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Training(int? activeId, FinishWorkoutViewModel model)
        {
            if (activeId.HasValue && activeId > 0)
            {
                var split = _context.TrainingSplits.SingleOrDefault(x => x.Id == activeId);
                var user = _context.Users.Find(User.Identity.GetUserId());
                user.ActiveWorkout_Id = split.Id;
                _context.SaveChanges();

                return Json(new { redirectToUrl = Url.Action("Training", "TrainingSplit") });
            }
            else
            {
                var workout = _context.Workouts.FirstOrDefault(x => x.Id == model.WorkoutId);
                // If the workout happened today, modify this one. Else create a copy of that workout for todays date

                if (workout.Date.Date == DateTime.Now.Date)
                {
                    workout.TimeSpan = model.TimeSpan;

                    var exercises = _context.Exercises.Where(x => x.Workout_Id == workout.Id).ToList();
                    var setCounter = 0;
                    var exerciseCounter = 0;
                    foreach (var exercise in exercises)
                    {
                        var sets = _context.Sets.Where(x => x.Exercise_Id == exercise.Id).ToList();

                        exercise.Name = model.ExerciseNames[exerciseCounter];
                        exerciseCounter += 1;
                        foreach (var set in sets)
                        {
                            set.ActualWeight = decimal.Round(model.ActualWeight[setCounter], 2);
                            set.ActualReps = model.ActualReps[setCounter];
                            setCounter += 1;
                        }
                    }

                }
                else
                {
                    // Create new workout from the previous one. Only the Date and the weight are different
                    var newWorkout = new Workout(workout.Name, workout.TrainingSplit_Id, model.TimeSpan);
                    var exercises = _context.Exercises.Where(x => x.Workout_Id == workout.Id).ToList();
                    var setCounter = 0;
                    var exerciseCounter = 0;
                    foreach (var exercise in exercises)
                    {
                        var newExercise = new Exercise(model.ExerciseNames[exerciseCounter], exercise.MuscleGroup, newWorkout.Id);
                        exerciseCounter += 1;

                        var sets = _context.Sets.Where(x => x.Exercise_Id == exercise.Id).ToList();

                        foreach (var set in sets)
                        {
                            var newSet = new Set(set.RestTime, set.ExpectedWeight, set.Intensity, set.ExpectedRepsFst, set.ExpectedRepsSnd, newExercise.Id);
                            newSet.ActualWeight = decimal.Round(model.ActualWeight[setCounter], 2);
                            newSet.ActualReps = model.ActualReps[setCounter];
                            setCounter += 1;

                            newExercise.Sets.Add(newSet);
                        }

                        newWorkout.Exercises.Add(newExercise);
                    }

                    var trainingSplit = _context.TrainingSplits.FirstOrDefault(x => x.Id == workout.TrainingSplit_Id);
                    trainingSplit.Workouts.Add(newWorkout);


                }
                _context.SaveChanges();
                return Json(new { redirectToUrl = Url.Action("History", "TrainingSplit") });
            }
        }

        [HttpPost]
        public ActionResult AddSplit(AddSplitViewModel model)
        {
            var user = _context.Users.Find(User.Identity.GetUserId());

            var trainingSplit = new TrainingSplit(model.TrainingSplitValues.Name, model.TrainingSplitValues.Description, model.TrainingSplitValues.Equipment, model.TrainingSplitValues.Goal, model.TrainingSplitValues.Experience, model.TrainingSplitValues.Length, int.Parse(model.TrainingSplitValues.Frequency.Substring(0, 1)));

            foreach (var workoutModel in model.Workouts)
            {
                var newWorkout = new Workout(workoutModel.Name, trainingSplit.Id, trainingSplit.Length);
                foreach (var exerciseModel in workoutModel.Exercises)
                {
                    var newExercise = new Exercise(exerciseModel.Name, exerciseModel.MuscleGroup, newWorkout.Id);
                    foreach (var setModel in exerciseModel.Sets)
                    {
                        var newSet = new Set(setModel.RestTime, setModel.ExpectedWeight, setModel.Intensity, setModel.ExpectedReps.fst, setModel.ExpectedReps.snd, newExercise.Id);

                        newExercise.Sets.Add(newSet);
                    }

                    newWorkout.Exercises.Add(newExercise);
                    newWorkout.TrainingSplit = trainingSplit;
                }

                trainingSplit.Workouts.Add(newWorkout);

            }


            var userSplit = new UserSplit
            {
                User = user,
                Split = trainingSplit
            };

            _context.UserSplits.Add(userSplit);
            _context.SaveChanges();

            return Json(new { redirectToUrl = Url.Action("Training", "TrainingSplit") });
        }

        public ActionResult AddSplit(int? id)
        {
            if (id > 0)
            {
                var split = _context.TrainingSplits.Find(id);
                var workouts = _context.Workouts.Where(x => x.TrainingSplit_Id == split.Id).ToList();
                var workoutDays = new List<WorkoutDay>();

                foreach (Workout workout in workouts)
                {
                    var workoutDay = new WorkoutDay
                    {
                        Workout = workout,
                        Exercises = _context.Exercises.Where(x => x.Workout_Id == workout.Id).ToList()
                    };

                    workoutDays.Add(workoutDay);
                }

                var model = new AddSplitFillViewModel
                {
                    Split = split,
                    WorkoutDays = workoutDays
                };

                return View(model);
            }
            return View();
        }

        public ActionResult Rename(string name)
        {
            var user = _context.Users.Find(User.Identity.GetUserId());
            var split = _context.TrainingSplits.Find(user.ActiveWorkout_Id);

            split.Name = name;
            _context.SaveChanges();

            return Json(new { redirectToUrl = Url.Action("Training", "TrainingSplit") });
        }

        public ActionResult Delete(int id)
        {
            var user = _context.Users.Find(User.Identity.GetUserId());
            user.ActiveWorkout_Id = null;
            _context.SaveChanges();


            var split = _context.TrainingSplits.Find(id);
            _context.TrainingSplits.Remove(split);

            _context.SaveChanges();

            return RedirectToAction("Training", "TrainingSplit");
        }

        public ActionResult History()
        {
            var user = _context.Users.Find(User.Identity.GetUserId());
            var trainingSplits = _context.UserSplits.Where(x => x.UserID == user.Id).Select(x => x.Split.Id).ToList();
            var workouts = _context.Workouts.Where(x => trainingSplits.Contains(x.TrainingSplit_Id)).OrderByDescending(x => x.Date).ToList();

            var modelArray = new List<WorkoutSplitViewModel>();


            foreach (var workout in workouts)
            {
                workout.Exercises = _context.Exercises.Where(x => x.Workout_Id == workout.Id).ToList();
                var bestSets = new List<BestSet>();

                foreach (var exercise in workout.Exercises)
                {
                    exercise.Sets = _context.Sets.Where(x => x.Exercise_Id == exercise.Id).ToList();

                    var exercisesVolume = exercise.Sets.Select(x =>
                        new
                        {
                            volume = x.ActualWeight * x.ActualReps,
                            weight = x.ActualWeight,
                            reps = x.ActualReps
                        });


                    bestSets.Add(exercisesVolume.MaxBy(x => x.volume).Select(x =>
                                  new BestSet { Weight = x.weight, Repetitions = x.reps }).First());


                }

                // If the values for weight are null it means it's the workout created as default (NOT performed yet)
                if (bestSets.MaxBy(x => x.Weight).Select(x => x.Weight).First() == null)
                {
                    continue;
                }

                var trainingSplitName = _context.TrainingSplits.FirstOrDefault(x => x.Id == workout.TrainingSplit_Id).Name;
                var userWeight = _context.Weights.FirstOrDefault(x => x.UserId == user.Id).Weigth;

                var kcalLow = Convert.ToInt32((double)userWeight * METlow * 0.0175 * workout.TimeSpan);
                var kcalHigh = Convert.ToInt32((double)userWeight * METhigh * 0.0175 * workout.TimeSpan);

                modelArray.Add(new WorkoutSplitViewModel(
                                    new WorkoutExerciseList { Workout = workout, BestSets = bestSets },
                                    trainingSplitName,
                                    new METrange { low = kcalLow, high = kcalHigh }));
            }

            var model = new HistoryViewModel
            {
                UserId = user.Id,
                Workouts = modelArray
            };

            return View(model);
        }

        public ActionResult HistoryDetails(int id)
        {
            var user = _context.Users.Find(User.Identity.GetUserId());
            var workout = _context.Workouts.FirstOrDefault(x => x.Id == id);
            workout.Exercises = _context.Exercises.Where(x => x.Workout_Id == workout.Id).ToList();

            foreach (var exercise in workout.Exercises)
            {
                exercise.Sets = _context.Sets.Where(x => x.Exercise_Id == exercise.Id).ToList();
            }

            var trainingSplitName = _context.TrainingSplits.FirstOrDefault(x => x.Id == workout.TrainingSplit_Id).Name;
            var userWeight = _context.Weights.FirstOrDefault(x => x.UserId == user.Id).Weigth;

            var kcalLow = Convert.ToInt32((double)userWeight * METlow * 0.0175 * workout.TimeSpan);
            var kcalHigh = Convert.ToInt32((double)userWeight * METhigh * 0.0175 * workout.TimeSpan);

            return PartialView(new WorkoutSplitViewModel(
                                new WorkoutExerciseList { Workout = workout, BestSets = null },
                                trainingSplitName,
                                new METrange { low = kcalLow, high = kcalHigh }));
        }


        public ActionResult ModifyWorkout(FinishWorkoutViewModel model)
        {
            var workout = _context.Workouts.FirstOrDefault(x => x.Id == model.WorkoutId);
            workout.TimeSpan = model.TimeSpan;
            workout.Date = model.WorkoutDate;


            workout.Exercises = _context.Exercises.Where(x => x.Workout_Id == workout.Id).ToList();
            var setCounter = 0;
            var exerciseCounter = 0;
            foreach (var exercise in workout.Exercises)
            {
                exercise.Sets = _context.Sets.Where(x => x.Exercise_Id == exercise.Id).ToList();

                exercise.Name = model.ExerciseNames[exerciseCounter];
                exerciseCounter += 1;

                foreach (var set in exercise.Sets)
                {
                    set.ActualWeight = model.ActualWeight[setCounter];
                    set.ActualReps = model.ActualReps[setCounter];
                    setCounter += 1;
                }
            }

            _context.SaveChanges();
            return Json(new { redirectToUrl = Url.Action("History", "TrainingSplit") });
        }

        public ActionResult DeleteWorkoutFromHistory(int id)
        {
            var workout = _context.Workouts.FirstOrDefault(x => x.Id == id);

            var numberWorkouts = _context.Workouts.Count(x => x.Name == workout.Name);

            if (numberWorkouts <= 1)
            {
                return RedirectToAction("Index", "Home");
            }
            _context.Workouts.Remove(workout);
            _context.SaveChanges();

            return RedirectToAction("History");
        }
    }
}