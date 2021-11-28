using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using static Domain.Models.OrdersCart;
using Microsoft.EntityFrameworkCore;
using Emanuel_Caprariu_lab4.Data.Models;
using static LanguageExt.Prelude;
using Emanuel_Caprariu_lab4.Domain.Repositories;

namespace Emanuel_Caprariu_lab4.Data.Repositories
{
    public class OrderLineRepository : IOrderLineRepository
    {
        private readonly OrdersContext ordersContext;

        public OrderLineRepository(OrdersContext ordersContext)
        {
            this.ordersContext = ordersContext;
        }


        public TryAsync<List<CalculateCustomerOrder>> TryGetExistingOrders() => async () => (await (
                         from o in ordersContext.OrdersLine
                         join p in ordersContext.Products on o.OrderLineId equals p.ProductId
                         select new { o.OrderLineId,p.RegistrationCode, p.Description, o.Amount, o.Address, o.Price,o.FinalPrice})
                         .AsNoTracking()
                         .ToListAsync())
                         .Select(result => new CalculateCustomerOrder(
                                                   OrderRegistrationCode: new(result.RegistrationCode),
                                                   OrderDescription: new(result.Description),
                                                   OrderAmount: new(result.Amount ?? 0f),
                                                   OrderAddress: new(result.Address),
                                                   OrderPrice: new(result.Price ?? 0f),
                                                   FinalPrice: new(result.FinalPrice ?? 0f))
                         {
                             OrderLineId = result.OrderLineId
                         })
                         .ToList();

        public TryAsync<Unit> TrySaveOrders(PlacedOrder orders) => async () =>
        {
            var products = (await ordersContext.Products.ToListAsync()).ToLookup(student => student.RegistrationCode);
            var newOrders = orders.CalculateCustomerOrders
                                    .Where(g => g.IsUpdated && g.OrderLineId == 0)
                                    .Select(g => new OrderLineDbo()
                                    {
                                        ProductId = products[g.OrderRegistrationCode.Value].Single().ProductId,
                                        Address = g.OrderAddress.Address,
                                        Amount = g.OrderAmount.Amount,
                                        Price = g.OrderPrice.Price,
                                        FinalPrice = g.FinalPrice.Price

                                    }) ;
            var updatedOrders = orders.CalculateCustomerOrders.Where(g => g.IsUpdated && g.OrderLineId > 0)
                                    .Select(g => new OrderLineDbo()
                                    {
                                        OrderLineId = g.OrderLineId,
                                        ProductId = products[g.OrderRegistrationCode.Value].Single().ProductId,
                                        Address = g.OrderAddress.Address,
                                        Amount = g.OrderAmount.Amount,
                                        Price = g.OrderPrice.Price,
                                        FinalPrice = g.FinalPrice.Price
                                    });

            ordersContext.AddRange(newOrders);
            foreach (var entity in updatedOrders)
            {
                ordersContext.Entry(entity).State = EntityState.Modified;
            }

            await ordersContext.SaveChangesAsync();

            return unit;
        };
    }
}
