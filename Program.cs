using System;
using Task2_EmanuelCaprariu.Domain;
using System.Collections.Generic;
using static Task2_EmanuelCaprariu.Domain.Cart;
using System.Text.RegularExpressions;

namespace Task2_EmanuelCaprariu
{
    class Program
    {
        private static float MaxOfCart = 0;
        private static Random randomCodeCustomer = new Random();
        private static readonly Regex regForNum = new Regex(@"(^\d+$)|(^\d+.\d+$)");
        private static float TotalPrice = 0;
        private static int numberOfProducts = 0;
        static void Main(string[] args)
        {
            var listOfProducts = ReadListOfProducts().ToArray();
            UnvalidatedCartProducts unvalidatedProducts = new(listOfProducts);
            ICart result = ValidatedCartProducts(unvalidatedProducts);

            result.Match(
                whenEmptyCart: emptyResult => emptyResult,
                whenUnvalidatedCartProducts: unvalidatedResults => unvalidatedResults,
                whenPaidCartProducts: paidResults => paidResults,
                whenInvalidatedCartProducts: invalidResult => invalidResult,
                whenValidatedCartProducts: validatedResult => PaidCartProducts(validatedResult)
            );
            numberOfProducts = 1;
            foreach(var product in listOfProducts)
            {

                Console.WriteLine();
                Console.WriteLine("=====>Product-{0}<======================", numberOfProducts);
                Console.WriteLine("--{0}: {1}-------", product.Product.ProductCode,product.Product.LabelProduct);                
                Console.WriteLine("--Amount: " + product.AmountProducts);
                Console.WriteLine("--Price per product: {0} LEI",product.Product.Value);
                TotalPrice += product.Product.Value * float.Parse(product.AmountProducts);
                Console.WriteLine();
                numberOfProducts++;
            }
            Console.WriteLine("-Total:{0}", TotalPrice);

        }
        private static List<UnvalidatedProducts> EmptyCart()
        {
            Console.WriteLine("Empty Product!");
            return new List<UnvalidatedProducts>();
        }
        private static List<UnvalidatedProducts> ReadListOfProducts()
        {
            var listOfProducts = EmptyCart();
            Console.WriteLine("Unvalidated Product!");
            do
            {

                var amountOfProducts = ReadValue("Amount of products: ");
                if (string.IsNullOrEmpty(amountOfProducts))
                {
                    break;
                }
                if (!regForNum.IsMatch(amountOfProducts))
                {
                    break;
                }

                var codeProduct = ReadValue("Product code: ");
                if (string.IsNullOrEmpty(codeProduct))
                {
                    break;
                }

                var addressProduct = ReadValue("Address of product:");
                if (string.IsNullOrEmpty(addressProduct))
                {
                    break;
                }

                var priceProduct = ReadValue("Price of product: ( LEI ) : ");
                if (!regForNum.IsMatch(priceProduct))
                {
                    break;
                }
                var labelProduct = ReadValue("Label of product:");
                if (string.IsNullOrEmpty(labelProduct))
                {
                    break;
                }
                MaxOfCart += float.Parse(amountOfProducts);
                listOfProducts.Add(new(amountOfProducts, addressProduct, new(codeProduct,labelProduct, float.Parse(priceProduct))));

            } while (MaxOfCart < 100);
            if (MaxOfCart > 100)
            {
                Console.WriteLine("You exhausted the cart!");
            }
            return listOfProducts;
        }

        private static ICart ValidatedCartProducts(UnvalidatedCartProducts unvalidatedProducts) =>
            new ValidatedCartProducts(new List<ValidatedProducts>());
       
          
        private static ICart PaidCartProducts(ValidatedCartProducts validCartProducts){
            var codeCustomer = randomCodeCustomer.Next(1000, 9999);
            var currentDate = DateTime.Now;
            if (numberOfProducts > 1)
            {
                Console.WriteLine("----- Products were paid successfully at {0} -----", currentDate.ToLocalTime());
            }
            else if(numberOfProducts == 1)
            {
                Console.WriteLine("----- Product was paid successfully at {0} -----", currentDate.ToLocalTime());
            }else
            {
                Console.WriteLine("Invalid Product!");
                return new InvalidatedCartProducts(new List<UnvalidatedProducts>(), "Invalid Cart!");
            }
            
            return new PaidCartProducts(new List<ValidatedProducts>(), new (codeCustomer.ToString()),currentDate);
        }          
         
        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }      
      
    }
}
