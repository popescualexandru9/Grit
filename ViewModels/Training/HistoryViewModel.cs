using Grit.Models;
using System.Collections.Generic;

namespace Grit.ViewModels.Training
{
    public class HistoryViewModel
    {
        public List<WorkoutSplitViewModel> Workouts { get; set; }
    }

    public class WorkoutSplitViewModel
    {
        public WorkoutExerciseList WorkoutList { get; set; }
        public string TrainingSplitName { get; set; }
        public METrange KcalBurned { get; set; }
        public int WorkoutLength { get; set; }



        public WorkoutSplitViewModel(WorkoutExerciseList workoutList, string trainingSplitName, METrange kcalBurned, int workoutLength)
        {
            WorkoutList = workoutList;
            TrainingSplitName = trainingSplitName;
            KcalBurned = kcalBurned;
            WorkoutLength = workoutLength;
        }
    }

    public class BestSet
    {
        public decimal? Weight { get; set; }
        public int? Repetitions { get; set; }
    }

    public class WorkoutExerciseList
    {
        public Workout Workout { get; set; }
        public IEnumerable<BestSet> BestSets { get; set; }
    }

    public class METrange
    {
        public int low { get; set; }
        public int high { get; set; }
    }
}