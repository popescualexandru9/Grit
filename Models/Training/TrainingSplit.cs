using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Grit.Models
{
    public class TrainingSplit
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(1, 7)]
        public int Frequency { get; set; }
        [Required]
        public string Equipment { get; set; }
        [Required]
        public string Goal { get; set; }
        [Required]
        public string Experience { get; set; }
        [Required]
        [Range(10, 150)]
        public int Length { get; set; }

        public ICollection<Workout> Workouts { get; set; }

        public virtual ICollection<UserSplit> UserSplits { get; set; }

        public TrainingSplit()
        {

        }

        public TrainingSplit (string name, string description, string equipment, string goal, string experience, int length, int frequency)
        {
            Name = name;
            Description = description;
            Equipment = equipment;
            Goal = goal;
            Experience = Regex.Replace(experience.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
            Length = length;
            Frequency = frequency;
            Workouts = new List<Workout>();
        }

    }
}