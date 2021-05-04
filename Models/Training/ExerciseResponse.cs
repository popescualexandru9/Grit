using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Grit.Models
{
    public class ExerciseResponse
    {
        [JsonPropertyName("results")]
        public List<ExerciseObj> ExerciseObjs { get; set; }

    }

    public class ExerciseObj
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("category")]
        public int CategoryId { get; set; }

        [JsonPropertyName("muscles")]
        public List<int> MuscleId { get; set; }

        [JsonPropertyName("equipment")]
        public List<int> EquiptmentId { get; set; }
    }

    public class ExerciseApi
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public List<int> MuscleIds { get; set; }
        public List<int> EquipmentIds { get; set; }

    }

}