using Grit.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Grit.Models
{
    [Authorize]
    public class TrainingSplitController : Controller
    {
        private ApplicationDbContext _context;

        public TrainingSplitController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: TrainingPlan
        [HttpGet]
        public ActionResult Training()
        {
            var user = _context.Users.Find(User.Identity.GetUserId());

            var activeSplit = _context.TrainingSplits.SingleOrDefault(x => x.Id == user.ActiveWorkout_Id);
            if (activeSplit != null)
            {
                var workouts = _context.Workouts.Where(x => x.TrainingSplit_Id == activeSplit.Id).ToList();
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
                var exercises = _context.Exercises.Where(x => x.Workout_Id == model.WorkoutId).ToList();
                var noExercises = exercises.Count();

                for (int i = 0; i < noExercises; i++)
                {
                    exercises[i].ActualWeight = decimal.Round(model.ActualWeight[i],2);
                    exercises[i].ActualReps = model.ActualReps[i];
                }

                _context.SaveChanges();
                return Json(new { redirectToUrl = Url.Action("Index", "Home") });
            }
         
        }

        [HttpPost]
        public ActionResult AddSplit(AddSplitViewModel model)
        {
            var user = _context.Users.Find(User.Identity.GetUserId());

            var trainingSplit = new TrainingSplit(model.TrainingSplitValues.Name, model.TrainingSplitValues.Description, model.TrainingSplitValues.Equipment, model.TrainingSplitValues.Goal, model.TrainingSplitValues.Experience, model.TrainingSplitValues.Length, int.Parse(model.TrainingSplitValues.Frequency.Substring(0, 1)));

            foreach (var workoutModel in model.Workouts)
            {
                var newWorkout = new Workout(workoutModel.Name, trainingSplit.Id);
                foreach (var exerciseModel in workoutModel.Sets)
                {
                    var newExercise = new Exercise(exerciseModel.Name, exerciseModel.RestTime, exerciseModel.ExpectedWeight, exerciseModel.Intensity, exerciseModel.MuscleGroup, exerciseModel.ExpectedReps.fst, exerciseModel.ExpectedReps.snd, newWorkout.Id);

                    newWorkout.Exercises.Add(newExercise);
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

            return RedirectToAction("Training","TrainingSplit");
        }

    }
}