namespace PhotoContest.Infrastructure.Exceptions
{
    using System;

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message)
        : base(message)
        {
        }
    }
}