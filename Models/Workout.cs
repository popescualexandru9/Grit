﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grit.Models
{
    public class Workout
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        [ForeignKey("TrainingSplit")]
        public int TrainingSplit_Id { get; set; }

        public TrainingSplit TrainingSplit { get; set; }
        public ICollection<Exercise> Exercises { get; set; }

        public Workout()
        {

        }

        public Workout(string name, int trainingSplitId)
        {
            Name = name;
            TrainingSplit_Id = trainingSplitId;
            Exercises = new List<Exercise>();
        }
    }
}