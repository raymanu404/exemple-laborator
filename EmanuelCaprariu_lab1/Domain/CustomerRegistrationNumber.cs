using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Task2_EmanuelCaprariu.Domain
{
    public record CustomerRegistrationNumber
    {
        private static readonly Regex ValidPattern = new("^[0-9]{4}$");

        public string CodeCustomer { get; }

        public CustomerRegistrationNumber(string value)
        {

            CodeCustomer = value;
            //if (ValidPattern.IsMatch(value))
            //{
            //    CodeCustomer = value;
            //}
            //else
            //{
            //    throw new InvalidCustomerRegistrationNumberExcepton("COD Client Invalid!");

            //}
        }

        public override string ToString()
        {
            return CodeCustomer;
        }
    }
}
