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
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Weight")]
        public decimal Weigth { get; set; }

    }
}