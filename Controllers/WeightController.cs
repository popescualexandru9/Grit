using Grit.Models;
using Grit.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Grit.Controllers
{
    public class WeightController : Controller
    {
        private ApplicationDbContext _context;

        public WeightController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Weight
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Progress()
        {
            // Basically UserManager.FindById(User,Identity.GetUserId())
            var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        
            var weightsUser = _context.Weights.Where(x => x.UserId == user.Id).ToList();
            weightsUser.Sort((x, y) => x.Date.CompareTo(y.Date));

            var timeFrame = DateTime.Now.AddDays(-1);
            var model = new ProgressViewModel
            {
                Weights = weightsUser,
                TodaysWeight = _context.Weights.Where(x => x.UserId == user.Id && DateTime.Compare(DbFunctions.TruncateTime(x.Date) ?? DateTime.Now,
                                                                                                    timeFrame.Date) > 0).SingleOrDefault()
            };

            return View(model);
        }

        public ActionResult AddWeight(ProgressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Progress", "Weight", new { status = "bad" });
            }

            int weightEntityId;
            var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            decimal weight = Math.Round(decimal.Parse(model.TodaysWeight.Weigth.ToString("F")), 2);

            var weightEntity = _context.Weights.Where(x => x.UserId == user.Id && DateTime.Compare(DbFunctions.TruncateTime(x.Date) ?? DateTime.Now,
                                                                                                   model.TodaysWeight.Date) == 0).SingleOrDefault();
            if (weightEntity != null)
            {
                // If found, rewrite it
                weightEntity.Weigth = weight;
                weightEntityId = weightEntity.Id;
                _context.SaveChanges();
            }
            else
            {   // Create new weight and store its id
                weightEntityId = Create(weight, user.Id, model.TodaysWeight.Date);
            }
          
            user.DailyWeight_Id = weightEntityId;
            _context.SaveChanges();

            return RedirectToAction("Progress", "Weight");
        }


        public ActionResult ChangeWeight( string weightInput , DateTime dateInput)
        {
            Regex rx = new Regex(@"^\d+\.?\d{0,2}$");
            var rgx = rx.IsMatch(weightInput);
            if (!rgx)
            {
                return RedirectToAction("Progress", "Weight", new { status = "bad"});
            }

            decimal d = decimal.Parse(weightInput);
            decimal weight = Math.Round(decimal.Parse(d.ToString("F")), 2);

            // Basically UserManager.FindById(User,Identity.GetUserId())
            var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());


            var weightUser = _context.Weights.Where(x => x.UserId == user.Id && DateTime.Compare(DbFunctions.TruncateTime(x.Date) ?? DateTime.Now, 
                                                                                                dateInput.Date) == 0 ).SingleOrDefault();
            weightUser.Weigth = weight;
            _context.SaveChanges();

            return RedirectToAction("Progress", "Weight");
        }

        public int Create(decimal weight,string userId)
        {
            var newWeight = new Models.Weight
            {
                Weigth = weight,
                Date = DateTime.Now,
                UserId = userId
            };

            _context.Weights.Add(newWeight);
            _context.SaveChanges();

            return newWeight.Id;
        }

        public int Create(decimal weight, string userId, DateTime date)
        {
            var newWeight = new Models.Weight
            {
                Weigth = weight,
                Date = date,
                UserId = userId
            };

            _context.Weights.Add(newWeight);
            _context.SaveChanges();

            return newWeight.Id;
        }


    }
}
