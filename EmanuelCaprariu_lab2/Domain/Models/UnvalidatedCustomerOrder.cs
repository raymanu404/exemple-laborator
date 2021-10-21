using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public record UnvalidatedCustomerOrder(string OrderRegistrationCode,string OrderDescription,string OrderAmount,string OrderAddress,string OrderPrice)
    {
        public string toStringOrder()
        {
            return $"Registration Code: {OrderRegistrationCode} \nDescription of Order: {OrderDescription} \nAmount: {OrderAmount}\nAddress: {OrderAddress} \nPrice per item: {OrderPrice}";
        }
    }
   
    
}
