using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grit.Models
{
    public class Set : ICloneable
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 100)]
        public int ExpectedRepsFst { get; set; }

        [Required]
        [Range(1, 100)]
        public int ExpectedRepsSnd { get; set; }

        [Range(1, 100)]
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
        [ForeignKey("Exercise")]
        public int Exercise_Id { get; set; }

        public Exercise Exercise { get; set; }

        public Set()
        {

        }

        public Set(decimal restTime, string expectedWeight, string intensity, int expectedRepsFst, int expectedRepsSnd, int exerciseId)
        {
            RestTime = restTime;
            ExpectedWeight = expectedWeight;
            Intensity = intensity;
            ExpectedRepsFst = expectedRepsFst;
            ExpectedRepsSnd = expectedRepsSnd;
            Exercise_Id = exerciseId;
        }
        public Set(int id, decimal restTime, string expectedWeight, string intensity, int expectedRepsFst, int expectedRepsSnd, int exerciseId, decimal? actualWeight, int? actualReps)
        {
            Id = id;
            RestTime = restTime;
            ExpectedWeight = expectedWeight;
            Intensity = intensity;
            ExpectedRepsFst = expectedRepsFst;
            ExpectedRepsSnd = expectedRepsSnd;
            Exercise_Id = exerciseId;
            ActualWeight = actualWeight;
            ActualReps = actualReps;
        }

        public object Clone()
        {
            return new Set(this.Id, this.RestTime, this.ExpectedWeight, this.Intensity, this.ExpectedRepsFst, this.ExpectedRepsSnd, this.Exercise_Id, this.ActualWeight, this.ActualReps);
        }
    }

    public struct Intensity
    {
        public const string WarmUp = "Warm up";
        public const string Moderate = "Moderate";
        public const string Hard = "Hard";
        public const string Harder = "Harder";
        public const string Failure = "Failure";
    }

    public struct ExpectedWeight
    {
        public const string Light = "Light";
        public const string Moderate = "Moderate";
        public const string Heavy = "Heavy";
        public const string Heavier = "Heavier";
        public const string Same = "Same";
        public const string DropSet = "Drop Set";

    }
}

