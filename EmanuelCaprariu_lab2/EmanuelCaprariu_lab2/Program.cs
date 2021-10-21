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

            foreach(var order in listOfOrders)
            {
                Console.WriteLine(order.toStringOrder());
            }
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
    }
}
