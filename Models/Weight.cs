using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Grit.Models
{
    public class Weight
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Today's date")]
        public DateTime Date { get; set; }

        [Required]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Display(Name = "Today's weight")]
        public decimal Weigth { get; set; }

    }
}