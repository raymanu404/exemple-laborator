using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using static Domain.Models.UnvalidatedCustomerOrder;


namespace EmanuelCaprariu_lab2
{
    class Program
    {
        //private static List<UnvalidatedCustomerOrder> listOfOrders;
        static void Main(string[] args)
        {

            var listOfOrders = ReadListOfOrders().ToArray();
            PlacingOrdersCommand command = new(listOfOrders);
            Domain.PlacingOrderWorkflow workflow = new Domain.PlacingOrderWorkflow();

            var result = workflow.Execute(command, (checkOrderExist) => true);
            result.Match(

                whenPlacingOrderEventFailedEvent: @event =>
                {
                    Console.WriteLine($"Placing the order was failed : {@event.Reason}");
                    return @event;
                },
                whenPlacingOrderEventSuccedeedEvent: @event =>
                {
                    Console.WriteLine($"Number Of order : {@event.NumberOfOrder} at Date: {@event.PlacedDate}");
                    return @event;
                }
                );
            string option;
            List<UnvalidatedCustomerOrder> listOfOrdersCopied = new List<UnvalidatedCustomerOrder>(listOfOrders);
            do
            {
                Console.WriteLine();
                menu();
                Console.WriteLine();
                option = ReadValue("Option :");              
                switch (option)
                {
                    case "0":
                        var listOfOrders1 = ReadListOfOrders().ToArray();
                        PlacingOrdersCommand command1= new(listOfOrders1);
                        Domain.PlacingOrderWorkflow workflow1 = new Domain.PlacingOrderWorkflow();

                        var result1 = workflow1.Execute(command1, (checkOrderExist) => true);
                        result.Match(

                            whenPlacingOrderEventFailedEvent: @event =>
                            {
                                Console.WriteLine($"Placing the order was failed : {@event.Reason}");
                                return @event;
                            },
                            whenPlacingOrderEventSuccedeedEvent: @event =>
                            {
                                Console.WriteLine($"Number Of order : {@event.NumberOfOrder} at Date: {@event.PlacedDate}");
                                return @event;
                            }
                            );
                        listOfOrdersCopied.CopyTo(0,listOfOrders1,0, listOfOrders1.Length);
                        break;
                    case "1":
                        printThecart(listOfOrdersCopied);
                        break;
                    case "2":
                        string code = ReadValue("Check code...");
                        Console.WriteLine(checkOrderExist(listOfOrdersCopied, code));
                        break;
                    case "3":
                        string address = ReadValue("Check address...");
                        Console.WriteLine(checkOrderAddress(listOfOrdersCopied, address));
                        break;
                    case "4":
                        string codeForStock = ReadValue("Check stock...");
                        Console.WriteLine(checkStockForSpecificOrder(listOfOrdersCopied, codeForStock));
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                }
                
            } while (option != "q");
            
        }

        private static List<UnvalidatedCustomerOrder> ReadListOfOrders()
        {
            List<UnvalidatedCustomerOrder> listOfOrders = new();           
            do
            {
                //read registration number and grade and create a list of greads
                var orderRegistrationCode = ReadValue("Registration Code: ");
                if (string.IsNullOrEmpty(orderRegistrationCode))
                {
                    break;
                }

                var orderDescription = ReadValue("Description of order: ");
                if (string.IsNullOrEmpty(orderDescription))
                {
                    break;
                }

                var orderAmount = ReadValue("Amount of order: ");
                if (string.IsNullOrEmpty(orderAmount))
                {
                    break;
                }
                var orderAddress = ReadValue("Address of delivery : ");
                if (string.IsNullOrEmpty(orderAddress))
                {
                    break;
                }
                var orderPrice = ReadValue("Price of order: ");
                if (string.IsNullOrEmpty(orderPrice))
                {
                    break;
                }
                var more = ReadValue("more? y/n");
                if (!more.Equals("y"))
                {
                    break;
                }
               
                listOfOrders.Add(new(orderRegistrationCode, orderDescription, orderAmount, orderAddress, orderPrice));
            } while (true);
            return listOfOrders;
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        private static string checkOrderExist(List<UnvalidatedCustomerOrder> ordersList,string orderCode)
        {
            foreach(var order in ordersList)
            {
                if (order.OrderRegistrationCode.Equals(orderCode))
                {
                    return $"Your order is here: {order.OrderRegistrationCode}";
                }
            }
            return $"Unfortunately, your order is not in our storage or wrong code!";
        }
        private static void printThecart(List<UnvalidatedCustomerOrder> ordersList)
        {
            foreach (var order in ordersList)
            {
                Console.WriteLine(order.toStringOrder());
                Console.WriteLine("-----------------------------------");
            }
        }
        private static string checkOrderAddress(List<UnvalidatedCustomerOrder> ordersList,string address)
        {
            foreach (var order in ordersList)
            {
                if (order.OrderAddress.Equals(address))
                {
                    return $"The address is correct : {order.OrderAddress}";
                }
            }
            return $"This address is invalid!";
        }

        private static string checkStockForSpecificOrder(List<UnvalidatedCustomerOrder> ordersList,string regCode)
        {
            foreach (var order in ordersList)
            {
                if (order.OrderRegistrationCode.Equals(regCode) && Decimal.Parse(order.OrderAmount) > 0)
                {
                   return ($"We have in storage this product {order.OrderDescription}");
                }
               
            }
            return $"Wrong code or stock exceeded";
        }
        private static void menu()
        {
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("0.Add more orders in your cart:");
            Console.WriteLine("1.Print list of orders:");
            Console.WriteLine("2.Check order by its reg code:");
            Console.WriteLine("3.Check address of order:");
            Console.WriteLine("4.Check stock for your order:");
            Console.WriteLine("clear - to clear the screen");
            Console.WriteLine("q - exit");
            Console.WriteLine("Your option is ...");
            
        }
    }
}
