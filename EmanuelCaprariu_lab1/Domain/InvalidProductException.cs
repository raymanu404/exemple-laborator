using System;
using System.Runtime.Serialization;

namespace Task2_EmanuelCaprariu.Domain
{
    [Serializable]
    internal class InvalidProductException : Exception
    {
        public InvalidProductException() { }
        public InvalidProductException(string? message) : base(message) {
            Console.WriteLine(message);
        }
        public InvalidProductException(string? message, Exception? innerException) : base(message, innerException) {
            Console.WriteLine(message + "\n" + innerException);
        }
        protected InvalidProductException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
