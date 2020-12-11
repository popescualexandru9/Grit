using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Grit.Models
{
    public class Weight
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Weight")]
        [RegularExpression(@"^\d+\.?\d{0,2}$", ErrorMessage = "This should contain between 0 and 2 decimals. Ex : 80.2 kg")]
        public decimal Weigth { get; set; }

    }
}