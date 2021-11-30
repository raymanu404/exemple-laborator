using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using LanguageExt;

namespace Emanuel_Caprariu_lab4.Domain.Repositories
{
    public interface IOrderHeaderRepository
    {
        TryAsync<List<int>> TryGetExistingOrders(IEnumerable<int> ordertsToCheck);
    }
}
