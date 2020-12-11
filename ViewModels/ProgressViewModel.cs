using Grit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grit.ViewModels
{
    public class ProgressViewModel
    {
        public IEnumerable<Weight> Weights { get; set; }
        public Weight TodaysWeight { get; set; }
    }
}