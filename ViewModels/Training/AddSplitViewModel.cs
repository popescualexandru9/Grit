﻿using System.Collections.Generic;

namespace Grit.ViewModels
{
    public class AddSplitViewModel
    {
        public TrainingSplitValues TrainingSplitValues { get; set; }

        public ICollection<WorkoutViewModel> Workouts { get; set; }
    }

    public class TrainingSplitValues
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; }
        public string Equipment { get; set; }
        public string Goal { get; set; }
        public string Experience { get; set; }
        public int Length { get; set; }
    }

    public class WorkoutViewModel
    {
        public string Name { get; set; }
        public ICollection<ExerciseViewModel> Exercises { get; set; }
    }
    public class ExerciseViewModel
    {
        public string Name { get; set; }
        public string MuscleGroup { get; set; }
        public ICollection<SetViewModel> Sets { get; set; }
    }


    public class SetViewModel
    {
        public ExpectedReps ExpectedReps { get; set; }
        public decimal RestTime { get; set; }
        public string ExpectedWeight { get; set; }
        public string Intensity { get; set; }

    }

    public class ExpectedReps
    {
        public int fst { get; set; }
        public int snd { get; set; }
    }
}