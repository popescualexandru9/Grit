using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Grit.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public decimal? Height { get; set; }

        public string Gender { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? Birthdate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? SignUpDate { get; set; }


        [ForeignKey("DailyWeight")]
        public int? DailyWeight_Id { get; set; }
        public virtual Weight DailyWeight { get; set; }

        // Id of one particular TrainingSplit
        [ForeignKey("ActiveWorkout")]
        public int? ActiveWorkout_Id { get; set; }
        public virtual TrainingSplit ActiveWorkout { get; set; }

        public virtual ICollection<UserSplit> UserSplits { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Weight> Weights { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<TrainingSplit> TrainingSplits { get; set; }
        public DbSet<UserSplit> UserSplits { get; set; }
        public DbSet<Set> Sets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}