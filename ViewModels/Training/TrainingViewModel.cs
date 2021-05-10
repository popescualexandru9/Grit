using Grit.Models;
using System.Collections.Generic;

namespace Grit.ViewModels
{
    public class TrainingViewModel
    {
        public List<TrainingSplit> Splits { get; set; }
        public TrainingSplit ActiveSplit { get; set; }
        public List<WorkoutDay> WorkoutDays { get; set; }

    }

    public class WorkoutDay
    {
        public Models.Workout Workout { get; set; }
        public List<Exercise> Exercises { get; set; }
    }

    public class FinishWorkoutViewModel
    {
        public int WorkoutId { get; set; }

        public int TimeSpan { get; set; }
        public List<decimal> ActualWeight { get; set; }
        public List<int> ActualReps { get; set; }
    }

    public class AddSplitFillViewModel
    {
        public TrainingSplit Split { get; set; }
        public List<WorkoutDay> WorkoutDays { get; set; }
    }
}