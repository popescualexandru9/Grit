using System.Collections.Generic;
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
        public string MuscleGroup { get; set; }

        [Required]
        [ForeignKey("Workout")]
        public int Workout_Id { get; set; }

        public Workout Workout { get; set; }

        public IList<Set> Sets { get; set; }

        public Exercise()
        {

        }

        public Exercise(string name, string muscleGroup, int workoutId)
        {
            Name = name;
            MuscleGroup = muscleGroup;
            Workout_Id = workoutId;
            Sets = new List<Set>();
        }

    }
}