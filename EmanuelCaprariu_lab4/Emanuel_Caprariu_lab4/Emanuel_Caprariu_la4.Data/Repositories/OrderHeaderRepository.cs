﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emanuel_Caprariu_lab4.Domain.Repositories;
using LanguageExt;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Emanuel_Caprariu_lab4.Data.Repositories
{
    public class OrderHeaderRepository :IOrderHeaderRepository
    {

        private readonly OrdersContext ordersContext;

        public OrderHeaderRepository(OrdersContext ordersContext)
        {
            this.ordersContext = ordersContext;
        }

        public TryAsync<List<OrderAddress>> TryGetExistingOrders(IEnumerable<string> orderToCheck) => async () =>
        {
            var orders = await ordersContext.OrdersHeader
                                              .Where(order => orderToCheck.Contains(order.Address))
                                              .AsNoTracking()
                                              .ToListAsync();
            return orders.Select(order => new OrderAddress(order.Address))
                           .ToList();
        };
    }
}