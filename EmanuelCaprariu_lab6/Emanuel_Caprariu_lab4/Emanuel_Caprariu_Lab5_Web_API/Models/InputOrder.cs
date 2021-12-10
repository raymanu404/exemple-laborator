using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Emanuel_Caprariu_Lab5_Web_API.Models
{
    public class InputOrder
    {
        [Required]
        [RegularExpression(OrderRegistrationCode.Pattern)]
        public string RegistrationCode { get; set; }

        [Required]
        [RegularExpression(OrderDescription.Pattern)]
        public string Description { get; set; }

        [Required]
        [Range(1,300)]
        public float Amount { get; set; }
        [Required]
        [RegularExpression(OrderAddress.Pattern)]
        public string Address { get; set; }

        [Required]
        [Range(1,999999)]
        public float Price { get; set; }
    }
}
