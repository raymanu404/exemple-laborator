﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderAmount
    {
        public float Amount { get; }
        public OrderAmount(float amount)
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
        private static bool IsValid(float decValue) => decValue > 0 && decValue <= 100;

        public override string ToString()
        {
            return $"amount: {Amount}";
        }

        public static bool TryParse(string value, out OrderAmount orderAmount)
        {
            bool isValid = false;
            orderAmount = null;
            if(float.TryParse(value, out float decValue))
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
