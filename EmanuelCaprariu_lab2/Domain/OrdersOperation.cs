using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.PlacingOrderEvent;
using Domain.Models;
using LanguageExt;
using static Domain.Models.OrdersCart;

namespace Domain
{
    public static class OrdersOperation
    {
      
        public static IOrdersCart ValidatedOrdersCartOP(Func<OrderRegistrationCode, bool> checkOrderExist, UnvalidatedOrdersCart orders)
        {
            List<ValidatedCustomerOrder> validatedOrders = new();
            bool isValidList = true;
            string invalidReason = string.Empty;
            foreach (var unvalidatedOrder in orders.OrderList)
            {
                if (!OrderRegistrationCode.TryParse(unvalidatedOrder.OrderRegistrationCode, out OrderRegistrationCode orderReg))
                {
                    invalidReason = $"Invalid Order Registration Code ({unvalidatedOrder.OrderRegistrationCode})";
                    isValidList = false;
                    break;
                }
                if (!OrderDescription.TryParse(unvalidatedOrder.OrderDescription,out OrderDescription orderDes)){
                    invalidReason = $"Invalid Order Description ({unvalidatedOrder.OrderDescription})";
                    isValidList = false;
                    break;
                }
                if(!OrderAmount.TryParse(unvalidatedOrder.OrderAmount,out OrderAmount orderAm))
                {
                    invalidReason = $"Invalid Amount of Order ({unvalidatedOrder.OrderAmount})";
                    isValidList = false;
                    break;
                }

                if (!OrderAddress.TryParse(unvalidatedOrder.OrderAddress, out OrderAddress orderAddress))
                {
                    invalidReason = $"Invalid Address  ({unvalidatedOrder.OrderAddress})";
                    isValidList = false;
                    break;
                }
                if (!OrderPrice.TryParse(unvalidatedOrder.OrderPrice, out OrderPrice orderPrice))
                {
                    invalidReason = $"Invalid Price of Order:  ({unvalidatedOrder.OrderPrice})";
                    isValidList = false;
                    break;
                }

                ValidatedCustomerOrder validOrder = new(orderReg, orderDes,orderAm, orderAddress, orderPrice);
                validatedOrders.Add(validOrder);
            }

            if (isValidList)
            {
                return new ValidatedOrdersCart(validatedOrders);
            }
            else
            {
                return new InvalidatedOrdersCart(orders.OrderList, invalidReason);
            }

        }

        public static IOrdersCart CalculatePriceOfCart(IOrdersCart orders) => orders.Match(
             whenUnvalidatedOrdersCart: unvalidatedCustomerOrder => unvalidatedCustomerOrder,
             whenInvalidatedOrdersCart: invalidatedCustomerOrder => invalidatedCustomerOrder,
             whenCalculatedOrder: calculatedOrder =>calculatedOrder,
             whenPlacedOrder: placedOrder => placedOrder,
             whenCheckedOrderByCode: checkedOrderByCode => checkedOrderByCode,
             whenValidatedOrdersCart: validatedOrdersCart =>
             {
                
                 var calculatePrice = validatedOrdersCart.OrdersList.Select(validOrder =>
                 new CalculateCustomerOrder(validOrder.OrderRegistrationCode, validOrder.OrderDescription, validOrder.OrderAmount, validOrder.OrderAddress, validOrder.OrderPrice, validOrder.OrderPrice * validOrder.OrderAmount));

                 return new CalculatedOrder(calculatePrice.ToList().AsReadOnly());
             }
          );

        public static IOrdersCart PlacedOrder(IOrdersCart orders) => orders.Match(
             whenUnvalidatedOrdersCart: unvalidatedCustomerOrder => unvalidatedCustomerOrder,
             whenInvalidatedOrdersCart: invalidatedCustomerOrder => invalidatedCustomerOrder,           
             whenPlacedOrder: placedOrder => placedOrder,
             whenCheckedOrderByCode: checkedOrderByCode => checkedOrderByCode,
             whenValidatedOrdersCart: validatedOrdersCart => validatedOrdersCart,
              whenCalculatedOrder: calculatedOrder => 
             {
                 StringBuilder csv = new();
                 DateTime currentTime = DateTime.Now;
                 Random random = new Random();
                 int numberOfOrder = random.Next(1000, 9999);
                 calculatedOrder.OrdersList.Aggregate(csv, (export, order) => export.AppendLine($"{order.OrderRegistrationCode.Value}, {order.OrderDescription.Description}, {order.OrderAmount.Amount}, {order.OrderAddress.Address}, {order.OrderPrice.Price}, {order.FinalPrice.Price}"));

                 PlacedOrder placedOrder = new(calculatedOrder.OrdersList, numberOfOrder, currentTime);

                 return placedOrder;
            });
    }
}
