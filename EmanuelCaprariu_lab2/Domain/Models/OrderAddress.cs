using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Domain.Models
{
    public class OrderAddress
    {
        public static readonly Regex validCode = new("^[a-zA-Z]");
        public string Address { get; }
        public OrderAddress(string address)
        {
            if (IsValid(address))
            {
                Address = address;
            }
            else
            {
                throw new InvalidatedCustomerOrder("");
            }

        }

        private static bool IsValid(string stringValue) => validCode.IsMatch(stringValue);

        public override string ToString()
        {
            return $" Adress : {Address}";
        }

        public static bool TryParse(string stringValue, out OrderAddress orderAddress)
        {
            bool isValid = false;
            orderAddress = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                orderAddress = new(stringValue);
            }

            return isValid;
        }
    }
}
