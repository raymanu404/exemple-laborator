using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using LanguageExt;
using static Domain.Models.OrdersCart;

namespace Emanuel_Caprariu_lab4.Domain.Repositories
{
    public interface IOrderLineRepository
    {
        TryAsync<List<CalculateCustomerOrder>> TryGetExistingOrders();

        TryAsync<Unit> TrySaveOrders(PlacedOrder order);
    }
}
