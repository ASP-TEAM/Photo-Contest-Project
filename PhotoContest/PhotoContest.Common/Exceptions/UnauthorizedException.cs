namespace PhotoContest.Common.Exceptions
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