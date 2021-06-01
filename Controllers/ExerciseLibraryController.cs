using Grit.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Grit.Controllers
{
    public class ExerciseLibraryController : Controller
    {

        public ActionResult Index(List<string> muscleGroups)
        {
            using (var _context = new ApplicationDbContext())
            {
                var exercises = new List<ExerciseLibrary>();
                if (muscleGroups != null)
                {
                    exercises = _context.ExercisesLibrary.OrderBy(x => x.MuscleGroup).Where(x => muscleGroups.Contains(x.MuscleGroup)).ToList();
                }
                else
                {
                    exercises = _context.ExercisesLibrary.OrderBy(x => x.MuscleGroup).ToList();
                }
                return View(exercises);
            }


        }

        public ActionResult FilterExercises(List<string> muscleGroups)
        {
            var muscleGroupsString = "?";
            if (muscleGroups != null)
            {
                foreach (var muscle in muscleGroups)
                {
                    muscleGroupsString += "muscleGroups=" + muscle.ToString() + "&";
                }
            }

            return Json(
                    new
                    {
                        redirectToUrl = Url.Action("Index", "ExerciseLibrary"),
                        muscleGroupsString = muscleGroupsString
                    },
                    JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModifyExercise(ExerciseLibrary exercise)
        {
            if (exercise.Description == null || exercise.Url == null)
            {
                return HttpNotFound();
            }

            using (var _context = new ApplicationDbContext())
            {
                var originalExercise = _context.ExercisesLibrary.FirstOrDefault(x => x.Id == exercise.Id);
                if (!originalExercise.Description.Equals(exercise.Description))
                {
                    originalExercise.Description = exercise.Description;
                }

                if (!originalExercise.Url.Equals(exercise.Url))
                {
                    originalExercise.Url = exercise.Url;
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

        }
    }
}