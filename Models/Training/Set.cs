using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grit.Models
{
    public class Set
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 30)]
        public int ExpectedRepsFst { get; set; }

        [Required]
        [Range(1, 30)]
        public int ExpectedRepsSnd { get; set; }

        [Range(1, 30)]
        public int? ActualReps { get; set; }

        [Required]
        [Range(0.15, 5.0)]
        public decimal RestTime { get; set; }

        [Required]
        public string ExpectedWeight { get; set; }
        [Range(0, 300)]
        public decimal? ActualWeight { get; set; }

        [Required]
        public string Intensity { get; set; }

        [Required]
        [ForeignKey("Exercise")]
        public int Exercise_Id { get; set; }

        public Exercise Exercise { get; set; }

        public Set()
        {

        }

        public Set(decimal restTime, string expectedWeight, string intensity, int expectedRepsFst, int expectedRepsSnd, int exerciseId)
        {
            RestTime = restTime;
            ExpectedWeight = expectedWeight;
            Intensity = intensity;
            ExpectedRepsFst = expectedRepsFst;
            ExpectedRepsSnd = expectedRepsSnd;
            Exercise_Id = exerciseId;
        }
    }
}