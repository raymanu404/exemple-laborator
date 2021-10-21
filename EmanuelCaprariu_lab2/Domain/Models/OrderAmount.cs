using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderAmount
    {
        public decimal Amount { get; }
        public OrderAmount(decimal amount)
        {
            if(IsValid(amount))
            {
                Amount = amount;
            }
            else
            {
                throw new InvalidatedCustomerOrder("");
            }
        }
        public static OrderAmount operator +(OrderAmount a, OrderAmount b) => new OrderAmount((a.Amount + b.Amount));
        private static bool IsValid(decimal decValue) => decValue > 0 && decValue <= 100;

        public override string ToString()
        {
            return $"amount: {Amount}";
        }

        public static bool TryParse(string value, out OrderAmount orderAmount)
        {
            bool isValid = false;
            orderAmount = null;
            if(decimal.TryParse(value, out decimal decValue))
            {
                if (IsValid(decValue))
                {
                    isValid = true;
                    orderAmount = new(decValue);
                }
            }            

            return isValid;
        }
    }
}
