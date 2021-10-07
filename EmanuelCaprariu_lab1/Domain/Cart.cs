using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Text;


namespace Task2_EmanuelCaprariu.Domain
{
    [AsChoice]
    public static partial class Cart
    {
       
        public interface ICart { }

        public record EmptyCart() : ICart;

        public record UnvalidatedCartProducts(IReadOnlyCollection<UnvalidatedProducts> ProductsList) : ICart;

        public record InvalidatedCartProducts(IReadOnlyCollection<UnvalidatedProducts> ProductsList, string reason) : ICart;
        public record ValidatedCartProducts(IReadOnlyCollection<ValidatedProducts> ProductsList) : ICart;

        public record PaidCartProducts(IReadOnlyCollection<ValidatedProducts> ProductsList,CustomerRegistrationNumber Customer, DateTime PaidDate) : ICart;
    }
}
