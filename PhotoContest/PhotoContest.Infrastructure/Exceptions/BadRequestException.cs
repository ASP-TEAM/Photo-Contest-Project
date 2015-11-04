namespace PhotoContest.Infrastructure.Exceptions
{
    using System;

    public class BadRequestException : Exception
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message)
        : base(message)
        {
        }
    }
}