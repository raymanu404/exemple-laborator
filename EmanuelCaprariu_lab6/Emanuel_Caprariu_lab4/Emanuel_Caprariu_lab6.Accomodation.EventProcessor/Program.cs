using System;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Emanuel_Caprariu_lab6.Events.ServiceBus;
using Emanuel_Caprariu_lab6.Events;

namespace Emanuel_Caprariu_lab6.Accomodation.EventProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {
               services.AddAzureClients(builder =>
               {
                   builder.AddServiceBusClient(hostContext.Configuration.GetConnectionString("ServiceBus"));
               });
               
               services.AddSingleton<IEventListener, ServiceBusTopicEventListener>();
               services.AddSingleton<IEventHandler, OrdersPlacedEventHandler>();

               services.AddHostedService<Worker>();
           });
    }
}
