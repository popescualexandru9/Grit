using Grit.Models;
using Grit.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Grit.Controllers
{
    [Authorize]
    public class WeightController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WeightController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Progress()
        {
            // Basically UserManager.FindById(User.Identity.GetUserId())
            var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            // Get users all time weights objects and store them into a list. Then sort the list
            var weightsUser = _context.Weights.Where(x => x.UserId == user.Id).ToList();
            weightsUser.Sort((x, y) => x.Date.CompareTo(y.Date));

            var timeFrame = DateTime.Now.AddDays(-1);
            // Search if any weight was already registered today for this user
            var todaysWeight = _context.Weights.SingleOrDefault(x => x.UserId == user.Id && DateTime.Compare(DbFunctions.TruncateTime(x.Date) ?? DateTime.Now, timeFrame) > 0);

            var model = new ProgressViewModel
            {
                Weights = weightsUser,
                TodaysWeight = todaysWeight,
                Height = user.Height ?? 0
            };

            return View(model);
        }

        public ActionResult AddWeight(ProgressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Progress", "Weight", new { status = Resources.WeightInvalid });
            }

            // Basically UserManager.FindById(User,Identity.GetUserId())
            var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            // Format weight have 2 decimals
            decimal weight = Math.Round(decimal.Parse(model.TodaysWeight.Weigth.ToString("F")), 2);

            // Check if the database contains an object at input date
            var weightEntity = _context.Weights.SingleOrDefault(x => x.UserId == user.Id && DateTime.Compare(DbFunctions.TruncateTime(x.Date) ?? DateTime.Now,
                                                                                                   model.TodaysWeight.Date) == 0);
            if (weightEntity != null)
            {
                // If found, rewrite its weight
                weightEntity.Weigth = weight;
                user.DailyWeight_Id = weightEntity.Id;
            }
            else
            {   // If not, create new weight and store its id
                user.DailyWeight_Id = Create(weight, user.Id, model.TodaysWeight.Date);
                HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Update(user);
            }
            _context.SaveChanges();
            return RedirectToAction("Progress", "Weight");
        }

        public ActionResult ChangeWeight(string weightInput, DateTime dateInput)
        {
            // Regex to check if weightInput has a decimal format since we don't store it in a model with client validations.
            // If it is in fact a decimal string, convert it into decimal and parse it to have only 2 decimals
            Regex rx = new Regex(@"^\d+\.?\d{0,2}$");
            var rgx = rx.IsMatch(weightInput);
            if (!rgx)
            {
                return RedirectToAction("Progress", "Weight", new { status = Resources.WeightInvalid });
            }

            decimal d = decimal.Parse(weightInput);
            decimal weight = Math.Round(decimal.Parse(d.ToString("F")), 2);

            // Basically UserManager.FindById(User,Identity.GetUserId())
            var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            // Check if the database contains an object at input date. It has to, so update weight
            var weightUser = _context.Weights.SingleOrDefault(x => x.UserId == user.Id && DateTime.Compare(DbFunctions.TruncateTime(x.Date) ?? DateTime.Now,
                                                                                                dateInput.Date) == 0);
            weightUser.Weigth = weight;
            _context.SaveChanges();

            return RedirectToAction("Progress", "Weight");
        }

        public ActionResult RemoveEntry(DateTime dateInput)
        {
            // Basically UserManager.FindById(User,Identity.GetUserId())
            var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var weightEntry = _context.Weights.SingleOrDefault(x => x.UserId == user.Id && DateTime.Compare(DbFunctions.TruncateTime(x.Date) ?? DateTime.Now,
                                                                                                dateInput.Date) == 0);

            if (weightEntry == null)
            {
                return Json(new { redirectToUrl = Url.Action("Progress", "Weight") });
            }
            if (user.DailyWeight_Id == weightEntry.Id) // Avoid EF conflict when deleting foreign key by removing the foreign key first.
            {
                user.DailyWeight = null;
                user.DailyWeight_Id = null;

                // ApplicationUserManager. Needed for updating the AspNetUser
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                userManager.Update(user);
            }

            _context.Weights.Remove(weightEntry);
            _context.SaveChanges();

            return Json(new { redirectToUrl = Url.Action("Progress", "Weight") });
        }


        public int Create(decimal weight, string userId, DateTime? date = null)
        {
            var newWeight = new Models.Weight
            {
                Weigth = weight,
                Date = date ?? DateTime.Now,
                UserId = userId
            };

            _context.Weights.Add(newWeight);
            _context.SaveChanges();

            return newWeight.Id;
        }
    }
}
