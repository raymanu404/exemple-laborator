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
        public IPlacingOrderEvent Execute(PlacingOrdersCommand command, Func<OrderRegistrationCode, bool> checkOrderExist)
        {
            UnvalidatedOrdersCart unvalidatedOrders = new UnvalidatedOrdersCart(command.InputOrder);
            IOrdersCart orders = ValidatedOrdersCartOP(checkOrderExist, unvalidatedOrders);

            return orders.Match(

                whenUnvalidatedOrdersCart: unvalidatedOrdersCart => new PlacingOrderEventFailedEvent("ceva") as IPlacingOrderEvent,
                whenInvalidatedOrdersCart: InvalidatedCustomerOrder => new PlacingOrderEventFailedEvent(InvalidatedCustomerOrder.Reason),
                whenValidatedOrdersCart: validateOrdersCart => new PlacingOrderEventFailedEvent(""),
                whenCalculatedOrder: calculateOrder => new PlacingOrderEventFailedEvent(""),
                whenCheckedOrderByCode: checkedOrderByCode => new PlacingOrderEventFailedEvent(""),
                whenPlacedOrder: placedOrder => new PlacingOrderEventSuccedeedEvent(placedOrder.NumberOfOrder, placedOrder.PlacedDate)
                );

        }
        public PlacingOrderWorkflow()
        {

        }
    }
}
