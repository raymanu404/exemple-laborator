using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public record ValidatedCustomerOrder(OrderRegistrationCode OrderRegistrationCode, OrderDescription OrderDescription, OrderAmount OrderAmount,OrderAddress OrderAddress,OrderPrice OrderPrice);
}
