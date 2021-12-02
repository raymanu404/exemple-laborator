using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using LanguageExt;

namespace Emanuel_Caprariu_lab4.Domain.Repositories
{
    public interface IProductRepository
    {
        TryAsync<List<OrderRegistrationCode>> TryGetExistingOrders(IEnumerable<string> ordersToCheck);
    }
}
