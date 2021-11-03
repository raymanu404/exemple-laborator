using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.PlacingOrderEvent;
using Domain.Models;
using LanguageExt;
using static Domain.Models.OrdersCart;
using static Domain.OrdersOperation;

namespace Domain
{
    public class PlacingOrderWorkflow
    {
        public async Task<IPlacingOrderEvent> ExecuteAsync(PlacingOrdersCommand command, Func<OrderRegistrationCode, TryAsync<bool>> checkOrderExist)
        {
            UnvalidatedOrdersCart unvalidatedOrders = new UnvalidatedOrdersCart(command.InputOrder);
            IOrdersCart orders = await ValidatedOrdersCartOP(checkOrderExist, unvalidatedOrders);
            orders = CalculatePriceOfCart(orders);
            orders = PlacedOrder(orders);

            return orders.Match(

                whenUnvalidatedOrdersCart: unvalidatedOrdersCart => new PlacingOrderEventFailedEvent("") as IPlacingOrderEvent,
                whenInvalidatedOrdersCart: InvalidatedCustomerOrder => new PlacingOrderEventFailedEvent(InvalidatedCustomerOrder.Reason),
                whenValidatedOrdersCart: validateOrdersCart => new PlacingOrderEventFailedEvent(""),
                whenCalculatedOrder: calculateOrder => new PlacingOrderEventFailedEvent(""),
                whenCheckedOrderByCode: checkedOrderByCode => new PlacingOrderEventFailedEvent(""),
                whenPlacedOrder: placedOrder => new PlacingOrderEventSuccedeedEvent(placedOrder.CalculateCustomerOrders,placedOrder.NumberOfOrder, placedOrder.PlacedDate)
                );

        }
      
    }
}
