using Grit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // GET: Weight/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: Weight/Create
        public int Create(decimal weight)
        {
            var newWeight = new Models.Weight
            {
                Weigth = weight,
                Date = DateTime.Now
            };

            _context.Weights.Add(newWeight);
            _context.SaveChanges();

            return newWeight.Id;
        }

       
    }
}
