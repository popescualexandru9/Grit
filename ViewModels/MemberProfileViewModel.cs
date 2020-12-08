using Grit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grit.ViewModels
{
    public class MemberProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public Weight Weight { get; set; }

        public int Age { get; set; }
    }
}