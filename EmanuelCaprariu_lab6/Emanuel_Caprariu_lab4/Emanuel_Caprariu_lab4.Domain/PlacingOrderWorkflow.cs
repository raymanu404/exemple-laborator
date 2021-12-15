using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.PlacingOrderEvent;
using Domain.Models;
using LanguageExt;
using static Domain.Models.OrdersCart;
using static Emanuel_Caprariu_lab4.OrdersOperation;
using Microsoft.Extensions.Logging;
using Emanuel_Caprariu_lab4.Domain.Repositories;
using static LanguageExt.Prelude;
using Emanuel_Caprariu_lab6.Events;
using Emanuel_Caprariu_lab6.Dto.Events;
using Emanuel_Caprariu_lab6.Dto.Models;

namespace Emanuel_Caprariu_lab4
{
    public class PlacingOrderWorkflow
    {
        private readonly IOrderHeaderRepository orderHeaderRepository;
        private readonly IOrderLineRepository orderLineRepository;
        private readonly IProductRepository productRepository;
        private readonly ILogger<PlacingOrderWorkflow> logger;

        private readonly IEventSender eventSender;

        public PlacingOrderWorkflow(IOrderHeaderRepository orderHeaderRepository, IProductRepository productRepository, IOrderLineRepository orderLineRepository, ILogger<PlacingOrderWorkflow> logger, IEventSender eventSender)
        {
            this.orderHeaderRepository = orderHeaderRepository;
            this.productRepository = productRepository;
            this.orderLineRepository = orderLineRepository;
            this.logger = logger;
            this.eventSender = eventSender;

        }
        public async Task<IPlacingOrderEvent> ExecuteAsync(PlacingOrdersCommand command)
        {
            UnvalidatedOrdersCart unvalidatedOrders = new UnvalidatedOrdersCart(command.InputOrder);

            var result = from product in productRepository.TryGetExistingOrders(unvalidatedOrders.OrderList.Select(order => order.OrderRegistrationCode))
                                                        .ToEither(ex => new FailedCart(unvalidatedOrders.OrderList, ex.Message) as IOrdersCart)
                         from existingOrder in orderLineRepository.TryGetExistingOrders()
                                          .ToEither(ex => new FailedCart(unvalidatedOrders.OrderList,ex.Message) as IOrdersCart)
                         let checkOrdersExists = (Func<OrderRegistrationCode, Option<OrderRegistrationCode>>)(order => CheckOrderExists(product, order))
                         from placedOrders in ExecuteWorkFlowAsync(unvalidatedOrders, existingOrder, checkOrdersExists).ToAsync()
                         from _ in orderLineRepository.TrySaveOrders(placedOrders)
                                          .ToEither(ex => new FailedCart(unvalidatedOrders.OrderList, ex.Message) as IOrdersCart)
                         let orders = placedOrders.CalculateCustomerOrders.Select(order => new PlacedCartOrder(
                                                                            order.OrderRegistrationCode,
                                                                            OrderDescription:order.OrderDescription,
                                                                            OrderAmount:order.OrderAmount,
                                                                            OrderAddress:order.OrderAddress,
                                                                            OrderPrice:order.OrderPrice,
                                                                            FinalPrice:order.FinalPrice
                                                                            ))
                        let sucsessfulEvent = new PlacingOrderSuccedeedEvent(orders,placedOrders.Csv,placedOrders.NumberOfOrder,placedOrders.PlacedDate)
                        let eventToPlace = new OrdersPlacingEvent()
                        {
                            Orders = orders.Select(o =>new CartOrderDto()
                            {
                               Name = o.OrderRegistrationCode.Value,
                               OrderRegistrationCode = o.OrderRegistrationCode.Value,
                               Description = o.OrderDescription.Description,
                               Amount = o.OrderAmount.Amount,
                               Address = o.OrderAddress.Address,
                               Price = o.OrderPrice.Price,
                               FinalPrice = o.FinalPrice.Price

                            }).ToList()
                        }
                        from placeEventResult in eventSender.SendAsync("orders", eventToPlace)
                                                    .ToEither(ex => new FailedCart(unvalidatedOrders.OrderList, ex.Message) as IOrdersCart)
                        select sucsessfulEvent;

            return await result.Match(
                    Left: orders => GenerateFailedEvent(orders) as IPlacingOrderEvent,
                    Right: placedOrders => placedOrders
                );
        }

        private async Task<Either<IOrdersCart, PlacedOrder>> ExecuteWorkFlowAsync(UnvalidatedOrdersCart unvalidatedOrder,
                                                                                  IEnumerable<CalculateCustomerOrder> existingOrder, 
                                                                                  Func<OrderRegistrationCode, Option<OrderRegistrationCode>> checkOrderExist)
        {
           
            IOrdersCart orders = await ValidatedOrdersCartOP(checkOrderExist, unvalidatedOrder);
            orders = CalculatePriceOfCart(orders);
            orders = PlacedOrder(orders);

            return orders.Match<Either<IOrdersCart, PlacedOrder>>(
                whenUnvalidatedOrdersCart: unvalidatedOrdersCart => Left(unvalidatedOrdersCart as IOrdersCart),
                whenInvalidatedOrdersCart: InvalidatedCustomerOrder => Left(InvalidatedCustomerOrder as IOrdersCart),
                whenValidatedOrdersCart: validateOrdersCart => Left(validateOrdersCart as IOrdersCart),
                whenCalculatedOrder: calculateOrder => Left(calculateOrder as IOrdersCart),
                whenCheckedOrderByCode: checkedOrderByCode => Left(checkedOrderByCode as IOrdersCart),
                whenFailedCart: failed => Left( failed as IOrdersCart),
                whenPlacedOrder: placedOrder => Right(placedOrder)
                );

        }

        private Option<OrderRegistrationCode> CheckOrderExists(IEnumerable<OrderRegistrationCode> orders, OrderRegistrationCode orderRegistrationNumber)
        {
            if (orders.Any(s => s == orderRegistrationNumber))
            {
                return Some(orderRegistrationNumber);
            }
            else
            {
                return None;
            }
        }

        private PlacingOrderFailedEvent GenerateFailedEvent(IOrdersCart orders) =>
            orders.Match<PlacingOrderFailedEvent>(
                whenUnvalidatedOrdersCart: unvalidatedOrdersCart => new($"Invalid state {nameof(UnvalidatedCustomerOrder)}"),
                whenInvalidatedOrdersCart: invalidatedCustomerOrder => new(invalidatedCustomerOrder.Reason),
                whenValidatedOrdersCart: validatedCustomerOrder => new($"Invalid state {nameof(validatedCustomerOrder)}"),           
                whenFailedCart: failed =>
                {
                    logger.LogError(failed.Reason, failed.Reason);
                    return new(failed.Reason);
                },
                whenCheckedOrderByCode: checkedOrderByCode => new($"Invalid state {nameof(CheckedOrderByCode)}"),
                whenCalculatedOrder: calculateCustomerOrder => new($"Invalid state {nameof(CalculateCustomerOrder)}"),
                whenPlacedOrder: placedOrder => new($"Invalid state {nameof(PlacedOrder)}")
             );

    }
}
