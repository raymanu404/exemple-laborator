using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Task2_EmanuelCaprariu.Domain
{
    public record Product
    {
        private static readonly Regex ValidPattern = new("^[0-9]{6}$");
        public string LabelProduct { get; }
        public string ProductCode { get; }
        public float Value { get; }
        public Product(string productCode,string labelProduct, float value)
        {
            if (ValidPattern.IsMatch(productCode) && value > 0)
            {
                ProductCode = productCode;
                LabelProduct = labelProduct;
                Value = value;
            }
            else
            {
                throw new InvalidProductException("Invalid product!");
            }
        }     
      
        public override string ToString()
        {
            return $"{LabelProduct:.###} / ${Value} Lei";
        }
    }
}
