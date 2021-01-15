using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grit.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

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
        public string MuscleGroup { get; set; }

        [Required]
        [ForeignKey("Workout")]
        public int Workout_Id { get; set; }

        public Workout Workout { get; set; }

        public Exercise()
        {

        }

        public Exercise(string name, decimal restTime, string expectedWeight, string intensity, string muscleGroup, int expectedRepsFst, int expectedRepsSnd, int workoutId)
        {
            Name = name;
            RestTime = restTime;
            ExpectedWeight = expectedWeight;
            Intensity = intensity;
            MuscleGroup = muscleGroup;
            ExpectedRepsFst = expectedRepsFst;
            ExpectedRepsSnd = expectedRepsSnd;
            Workout_Id = workoutId;
        }

    }
}