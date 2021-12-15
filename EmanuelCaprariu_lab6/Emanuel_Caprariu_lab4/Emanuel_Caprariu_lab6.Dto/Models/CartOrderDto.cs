using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emanuel_Caprariu_lab6.Dto.Models
{
    public class CartOrderDto
    {
        public string Name { get; init; }
        public string OrderRegistrationCode { get; init; }
        public string Description { get; init; }
        public float Amount { get; init; }
        public string Address { get; init; }
        public float Price { get; init; }
        public float FinalPrice{ get; init; }
    }
}
