using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.UnvalidatedCustomerOrder;
using LanguageExt;
using static LanguageExt.Prelude;
using Domain.Models;
using Emanuel_Caprariu_lab4;
using Emanuel_Caprariu_lab4.Data.Repositories;
using Microsoft.Extensions.Logging;
using Emanuel_Caprariu_lab4.Data;
using Microsoft.EntityFrameworkCore;

namespace Emanuel_Caprariu_lab4
{
    class Program
    {
        private static List<ValidatedCustomerOrder> listOfValidatedOrders = new();
        private static Option<OrderRegistrationCode> testRegCode;
        private static readonly Random random = new Random();

        private static string ConnectionString = @"Server=DESKTOP-T4A7BVN;Database=PSSC;Trusted_Connection=True;MultipleActiveResultSets=true";
        static async Task Main(string[] args)
        {
            using ILoggerFactory loggerFactory = ConfigureLoggerFactory();
            ILogger<PlacingOrderWorkflow> logger = loggerFactory.CreateLogger<PlacingOrderWorkflow>();

            var listOfOrders = ReadListOfOrders().ToArray();
            listOfValidatedOrders = new List<ValidatedCustomerOrder>();

            var dbContextBuilder = new DbContextOptionsBuilder<OrdersContext>()
                                               .UseSqlServer(ConnectionString)
                                               .UseLoggerFactory(loggerFactory);

            OrdersContext context = new(dbContextBuilder.Options);
            OrderHeaderRepository orderHeaderRepository = new(context);
            OrderLineRepository orderLineRepository = new(context);
            ProductRepository productRepository = new(context);

            PlacingOrdersCommand command = new(listOfOrders);
            PlacingOrderWorkflow workflow = new(orderHeaderRepository,productRepository,orderLineRepository, logger);

            var result = await workflow.ExecuteAsync(command);
            result.Match(

                whenPlacingOrderEventFailedEvent: @event =>
                {

                    Console.WriteLine($"Placing the order was failed : {@event.Reason}");
                    return @event;
                },
                whenPlacingOrderEventSuccedeedEvent: @event =>
                {
                    Console.WriteLine("Placing order was succeed...");
                    Console.WriteLine(@event.Csv);
                    //foreach (var a in @event.CalculatedOrder)
                    //{

                    //    listOfValidatedOrders.Add(new(a.OrderRegistrationCode, a.OrderDescription, a.OrderAmount, a.OrderAddress, a.OrderPrice));
                    //}
                    Console.WriteLine($"Number Of order : {@event.NumberOfOrder} at Date: {@event.PlacedDate}");
                    return @event;
                }
                );
            //string option;

            //do
            //{
            //    Console.WriteLine();
            //    menu();
            //    Console.WriteLine();
            //    option = ReadValue("Option :");
            //    switch (option)
            //    {

            //        case "1":
            //            printThecart(listOfValidatedOrders);
            //            break;
            //        case "2":
            //            string code = ReadValue("Check code for your order... {00000} ");
            //            testRegCode = OrderRegistrationCode.TryParseRegistrationCode(code);
            //            var orderExists = await testRegCode.Match(
            //                Some: testRegCode => CheckOrderExists(testRegCode).Match(Succ: value => value, exeption => false),
            //                None: () => Task.FromResult(false)
            //            );

            //            var myResult = from registrationCode in testRegCode
            //                                                            .ToEitherAsync(() => "Invalid Registration Code of your order...")
            //                           from exists in CheckOrderExists(registrationCode)
            //                                          .ToEither(ex =>
            //                                          {
            //                                              Console.Error.WriteLine(ex.ToString());
            //                                              return "Could not validate Registration Code of order";
            //                                          })
            //                           select exists;

            //            await myResult.Match(
            //                 Left: message => Console.WriteLine(message),
            //                 Right: flag => Console.WriteLine(flag));

            //            break;
            //        case "3":
            //            string address = ReadValue("Check address...");
            //            var testAddress = OrderAddress.TryParseOrderAddress(address);
            //            var addressExists = await testAddress.Match(
            //                Some: testAddress => CheckOrderByAddress(testAddress).Match(Succ: value => value, exception => false),
            //                None: () => Task.FromResult(false)
            //                );

            //            var myResult3 = from addressOrder in testAddress
            //                                                    .ToEitherAsync(() => "Invalid address for your order...")
            //                            from exists in CheckOrderByAddress(addressOrder)
            //                                                    .ToEither(ex =>
            //                                                    {
            //                                                        Console.Error.WriteLine(ex.ToString());
            //                                                        return "Could not validate address of order...";
            //                                                    })
            //                            select exists;

            //            await myResult3.Match(
            //                Left: message => Console.WriteLine(message),
            //                Right: flag => Console.WriteLine(flag)
            //                );
            //            break;
            //        case "4":
            //            string code2 = ReadValue("Check code for your order... {00000} ");
            //            testRegCode = OrderRegistrationCode.TryParseRegistrationCode(code2);
            //            var regExists = await testRegCode.Match(
            //                Some: testRegCode => CheckOrderExists(testRegCode).Match(Succ: value => value, exeption => false),
            //                None: () => Task.FromResult(false)
            //            );

            //            string stock = ReadValue("Check stock...");
            //            var testAmount = OrderAmount.TryParseOrderAmount(stock);

            //            var myResult2 = from regCode in testRegCode
            //                                           .ToEitherAsync(() => "Invalid Amount of your order...")
            //                            from amount in OrderAmount.TryParseOrderAmount(stock)
            //                                                            .ToEitherAsync(() => "Invalid Amount of your order...")
            //                            from inStock in CheckOrderByStock(regCode, amount)
            //                                          .ToEither(ex =>
            //                                          {
            //                                              Console.Error.WriteLine(ex.ToString());
            //                                              return "Could not validate Amount of order";
            //                                          })
            //                            select inStock;

            //            await myResult2.Match(
            //                 Left: message => Console.WriteLine(message),
            //                 Right: flag => Console.WriteLine(flag));
            //            break;
            //        case "5":
            //            string code3 = ReadValue("Check code for your order... {00000} ");
            //            testRegCode = OrderRegistrationCode.TryParseRegistrationCode(code3);
            //            var regExists1 = await testRegCode.Match(
            //                Some: testRegCode => CheckOrderExists(testRegCode).Match(Succ: value => value, exeption => false),
            //                None: () => Task.FromResult(false)
            //            );
            //            var myResult4 = from regCode in testRegCode
            //                                          .ToEitherAsync(() => "Invalid Amount of your order...")
            //                            from price in CheckPriceForOrder(regCode)
            //                                          .ToEither(ex =>
            //                                          {
            //                                              Console.Error.WriteLine(ex.ToString());
            //                                              return "Could not validate Amount of order";
            //                                          })
            //                            select price;

            //            await myResult4.Match(
            //                 Left: message => Console.WriteLine(message),
            //                 Right: flag => Console.WriteLine("The price for your order: " + flag + " LEI "));
            //            break;
            //        case "clear":
            //            Console.Clear();
            //            break;
            //    }

            //} while (option != "q");

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

                listOfOrders.Add(new(orderRegistrationCode, orderDescription, orderAmount, orderAddress, orderPrice));
            } while (true);
            return listOfOrders;
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        private static ILoggerFactory ConfigureLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
                                builder.AddSimpleConsole(options =>
                                {
                                    options.IncludeScopes = true;
                                    options.SingleLine = true;
                                    options.TimestampFormat = "hh:mm:ss ";
                                })
                                .AddProvider(new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()));
        }

        private static TryAsync<bool> CheckOrderExists(OrderRegistrationCode order)
        {
            Func<Task<bool>> func = async () =>
            {
                bool flag = false;
                foreach (var ord in listOfValidatedOrders)
                {
                    if (ord.OrderRegistrationCode.Value.Equals(order.Value))
                    {
                        flag = true;
                    }
                }

                return flag;
            };
            return TryAsync(func);
        }

        private static TryAsync<bool> CheckOrderByStock(OrderRegistrationCode regCode, OrderAmount order)
        {
            Func<Task<bool>> func = async () =>
            {
                bool flag = false;
                foreach (var ord in listOfValidatedOrders)
                {
                    if (ord.OrderRegistrationCode.Value.Equals(regCode.Value) && ord.OrderAmount.Amount >= order.Amount)
                    {
                        flag = true;
                    }
                }
                return flag;
            };
            return TryAsync(func);
        }


        private static TryAsync<bool> CheckOrderByAddress(OrderAddress order)
        {
            Func<Task<bool>> func = async () =>
            {
                bool flag = false;
                foreach (var ord in listOfValidatedOrders)
                {
                    if (ord.OrderAddress.Address.Equals(order.Address))
                    {
                        flag = true;
                    }
                }
                return flag;
            };

            return TryAsync(func);
        }

        private static TryAsync<float> CheckPriceForOrder(OrderRegistrationCode order)
        {
            Func<Task<float>> func = async () =>
            {
                float price = 0;
                foreach (var ord in listOfValidatedOrders)
                {
                    if (ord.OrderRegistrationCode.Value.Equals(order.Value))
                    {
                        price = ord.OrderPrice.Price;
                    }
                }
                return price;
            };

            return TryAsync(func);
        }

        private static void printThecart(List<ValidatedCustomerOrder> ordersList)
        {
            foreach (var order in ordersList)
            {
                Console.WriteLine(order.toStringOrder());
                Console.WriteLine("-----------------------------------");
            }
        }

        private static void menu()
        {

            Console.WriteLine("1.Print list of orders:");
            Console.WriteLine("2.Check order by its reg code:");
            Console.WriteLine("3.Check address of order:");
            Console.WriteLine("4.Check stock for your order:");
            Console.WriteLine("5.Get price for your order:");
            Console.WriteLine("q - exit");
            Console.WriteLine("Your option is ...");
            Console.WriteLine("-----------------------------------");

        }
    }
}
