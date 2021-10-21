using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Choices;

namespace Domain.Models
{
    [AsChoice]
    public static partial class OrdersCart
    {
        public interface IOrdersCart { }

        public record UnvalidatedOrdersCart : IOrdersCart
        {
            public UnvalidatedOrdersCart(IReadOnlyCollection<UnvalidatedCustomerOrder> orderList)
            {
                OrderList = orderList;
            }
            public IReadOnlyCollection<UnvalidatedCustomerOrder> OrderList { get; }
        }

        public record InvalidatedOrdersCart : IOrdersCart
        {
            internal InvalidatedOrdersCart(IReadOnlyCollection<UnvalidatedCustomerOrder> orderList,string reason)
            {
                OrderList = orderList;
                Reason = reason;
            }
            public IReadOnlyCollection<UnvalidatedCustomerOrder> OrderList { get; }
            public string Reason { get; }
        }

        public record ValidatedOrdersCart : IOrdersCart
        {
            internal ValidatedOrdersCart(IReadOnlyCollection<ValidatedCustomerOrder> ordersList)
            {
                OrdersList = ordersList;
            }
            public IReadOnlyCollection<ValidatedCustomerOrder> OrdersList { get; }
        }

        public record CalculatedOrder : IOrdersCart
        {
            internal CalculatedOrder(IReadOnlyCollection<CalculateCustomerOrder> ordersList)
            {
                OrdersList = ordersList;
            }
            public IReadOnlyCollection<CalculateCustomerOrder> OrdersList { get; }
        }

        public record CheckedOrderByCode : IOrdersCart
        {
            internal CheckedOrderByCode(IReadOnlyCollection<CheckOrder> checkOrder)
            {
                CheckOrder = checkOrder;
            }
            public IReadOnlyCollection<CheckOrder> CheckOrder { get; }
        }
        public record PlacedOrder : IOrdersCart
        {
            internal PlacedOrder(IReadOnlyCollection<CheckOrder> checkOrder,decimal numberOfOrder, DateTime placedDate)
            {
                CheckOrder = checkOrder;
                NumberOfOrder = numberOfOrder;
                PlacedDate = placedDate;
            }
            public IReadOnlyCollection<CheckOrder> CheckOrder { get; }
            public decimal NumberOfOrder { get; }
            public DateTime PlacedDate { get; }

        }


    }
}
