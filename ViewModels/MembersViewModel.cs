using Grit.Models;
using System.Collections.Generic;

namespace Grit.ViewModels
{
    public class MembersViewModel
    {
        public IList<ApplicationUser> Users { get; set; }
        public IList<string> RoleName { get; set; }
    }
}