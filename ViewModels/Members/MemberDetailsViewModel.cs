using Grit.Models;

namespace Grit.ViewModels
{
    public class MemberDetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public Weight Weight { get; set; }

        public string RoleName { get; set; }
        public string ActiveSplitName { get; set; }
    }
}