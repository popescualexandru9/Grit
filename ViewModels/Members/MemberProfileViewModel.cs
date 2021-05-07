using Grit.Models;

namespace Grit.ViewModels
{
    public class MemberProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public Weight Weight { get; set; }

        public int Age { get; set; }
    }
}