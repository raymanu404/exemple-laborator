using System;
using System.Runtime.Serialization;

namespace Task2_EmanuelCaprariu.Domain
{
    [Serializable]
    internal class InvalidCustomerRegistrationNumberExcepton : Exception
    {
        public InvalidCustomerRegistrationNumberExcepton(){}

        public InvalidCustomerRegistrationNumberExcepton(string? message) : base(message) {
            Console.WriteLine(message);
        }
        public InvalidCustomerRegistrationNumberExcepton(string? message, Exception? innerException) : base(message, innerException){
            Console.WriteLine(message+"\n"+ innerException);
        }

        protected InvalidCustomerRegistrationNumberExcepton(SerializationInfo info, StreamingContext context) : base(info, context){}

    }
}
