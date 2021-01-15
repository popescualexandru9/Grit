using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grit.Models
{
    public class UserSplit
    {
        [Key, Column(Order = 1)]
        public string UserID { get; set; }
        [Key, Column(Order = 2)]
        public int SplitID { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual TrainingSplit Split { get; set; }

    }
}