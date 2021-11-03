﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Domain.Models
{
    public class OrderAmount
    {
        public float Amount { get; }
        public static float MAX_OF_AMOUNT { get; set; }
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
        private static bool IsValid(float floatValue) => floatValue > 0 && floatValue <= MAX_OF_AMOUNT;

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
        public static Option<OrderAmount> TryParseOrderAmount(string stringValue)
        {
            if(float.TryParse(stringValue,out float floatAmount) && IsValid(floatAmount))
            {
                return Some <OrderAmount>(new(floatAmount));
            }
            else
            {
                return None;
            }
        }
    }
}