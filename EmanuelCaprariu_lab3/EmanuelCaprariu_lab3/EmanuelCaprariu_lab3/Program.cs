using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using static Domain.Models.UnvalidatedCustomerOrder;
using LanguageExt;
using static LanguageExt.Prelude;

namespace EmanuelCaprariu_lab2
{
    class Program
    {
        private static List<UnvalidatedCustomerOrder> copyListOfOrders = new();
        private static Option<OrderRegistrationCode> testRegCode;
        static async Task Main(string[] args)
        {

            var listOfOrders = ReadListOfOrders().ToArray();
            copyListOfOrders = new List<UnvalidatedCustomerOrder>(listOfOrders);

            PlacingOrdersCommand command = new(listOfOrders);
            Domain.PlacingOrderWorkflow workflow = new Domain.PlacingOrderWorkflow();

            var result = await workflow.ExecuteAsync(command, CheckOrderExists);
            result.Match(

                whenPlacingOrderEventFailedEvent: @event =>
                {
                  
                    Console.WriteLine($"Placing the order was failed : {@event.Reason}");
                    return @event;
                },
                whenPlacingOrderEventSuccedeedEvent: @event =>
                {
                    foreach (var calc in @event.CalculatedOrder)
                    {
                        Console.WriteLine($"Final price -- {calc.OrderRegistrationCode}: {calc.FinalPrice} LEI");
                      
                    }
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
                        copyListOfOrders = new List<UnvalidatedCustomerOrder>(listOfOrders1);

                        PlacingOrdersCommand command1= new(listOfOrders1);
                        Domain.PlacingOrderWorkflow workflow1 = new Domain.PlacingOrderWorkflow();

                        var result1 = await workflow1.ExecuteAsync(command1, CheckOrderExists);
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
                        string code = ReadValue("Check code for your order... {00000}");
                        testRegCode = OrderRegistrationCode.TryParseRegistrationCode(code);
                        var orderExists = await testRegCode.Match(
                            Some: testRegCode => CheckOrderExists(testRegCode).Match(Succ: value => value, exeption => false),
                            None: () => Task.FromResult(false)
                        );

                        var myResult = from registrationCode in OrderRegistrationCode.TryParseRegistrationCode(code)
                                                                        .ToEitherAsync( () => "Invalid Registration Code of your order...")
                                       from exists in CheckOrderExists(registrationCode)
                                                      .ToEither(ex =>
                                                      {
                                                          Console.Error.WriteLine(ex.ToString());
                                                          return "Could not validate Registration Code of order";
                                                      })
                                       select exists;

                        await myResult.Match(
                             Left: message => Console.WriteLine(message),
                             Right: flag => Console.WriteLine(flag));
                            
                        break;
                    case "3":
                        string address = ReadValue("Check address...");
                        Console.WriteLine(checkOrderAddress(listOfOrdersCopied, address));
                        break;
                    case "4":
                        string code2 = ReadValue("Check code for your order... {00000}");
                        testRegCode = OrderRegistrationCode.TryParseRegistrationCode(code2);
                        string stock = ReadValue("Check stock...");                 
                        var testAmount = OrderAmount.TryParseOrderAmount(stock);
                        var orderExists1 = await testAmount.Match(
                            Some: testAmount => CheckOrderByStock(testAmount).Match(Succ: value => value, fail => false),
                            None: () => Task.FromResult(false)
                        );
                                            
                        var myResult2 = from regCode in testRegCode
                                                       .ToEitherAsync(() => "Invalid Amount of your order...")
                                        from amount in OrderAmount.TryParseOrderAmount(stock)
                                                                        .ToEitherAsync(() => "Invalid Amount of your order...")
                                        from inStock in CheckOrderByStock(amount)
                                                      .ToEither(ex =>
                                                      {
                                                          Console.Error.WriteLine(ex.ToString());
                                                          return "Could not validate Amount of order";
                                                      })
                                        select inStock;

                        await myResult2.Match(
                             Left: message => Console.WriteLine(message),
                             Right: flag => Console.WriteLine(flag));
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

        private static TryAsync<bool> CheckOrderExists(OrderRegistrationCode order)
        {
            Func<Task<bool>> func = async () =>
            {
                bool flag = false;
                foreach (UnvalidatedCustomerOrder ord in copyListOfOrders)
                {
                    if (ord.OrderRegistrationCode.Equals(order.Value))
                    {
                        flag = true;
                    }
                }


                return flag;
            };
            return TryAsync(func);
        }

        private static TryAsync<bool> CheckOrderByStock(OrderAmount order)
        {
            Func<Task<bool>> func = async () =>
            {
                float stock = OrderAmount.MAX_OF_AMOUNT;
                bool flag = false;

                return flag;
            };
            return TryAsync(func);
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
