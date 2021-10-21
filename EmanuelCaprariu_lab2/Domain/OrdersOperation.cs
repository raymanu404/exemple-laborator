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
    }
}
