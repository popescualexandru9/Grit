using Grit.Models;
using System.Collections.Generic;

namespace Grit.ViewModels
{
    public class ProgressViewModel
    {
        public IEnumerable<Weight> Weights { get; set; }
        public Weight TodaysWeight { get; set; }

        public decimal Height;
    }
}