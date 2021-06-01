using System.ComponentModel.DataAnnotations;

namespace Grit.Models
{
    public class ExerciseLibrary
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string MuscleGroup { get; set; }
        [Required]

        public string Description { get; set; }
    }
}