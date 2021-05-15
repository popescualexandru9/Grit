using Grit.Models;
using System.Collections.Generic;

namespace Grit.ViewModels.Training
{
    public class HistoryViewModel
    {
        public List<WorkoutSplitViewModel> Workouts { get; set; }
        public string UserId { get; set; }
    }

    public class WorkoutSplitViewModel
    {
        public WorkoutExerciseList WorkoutBundle { get; set; }
        public string TrainingSplitName { get; set; }
        public METrange KcalBurned { get; set; }


        public WorkoutSplitViewModel(WorkoutExerciseList workoutBundle, string trainingSplitName, METrange kcalBurned)
        {
            WorkoutBundle = workoutBundle;
            TrainingSplitName = trainingSplitName;
            KcalBurned = kcalBurned;
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
        public IList<BestSet> BestSets { get; set; }
    }

    public class METrange
    {
        public int low { get; set; }
        public int high { get; set; }
    }
}