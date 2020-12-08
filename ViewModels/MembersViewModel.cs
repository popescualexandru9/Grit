using Grit.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grit.ViewModels
{
    public class MembersViewModel
    { 
        public IList<ApplicationUser> Users { get; set; }
        public IList<string> RoleName { get; set; }
    }
}