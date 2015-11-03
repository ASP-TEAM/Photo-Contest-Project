using System;
using System.Collections.Generic;
using PhotoContest.Infrastructure.Enumerations;
using PhotoContest.Infrastructure.Interfaces;

namespace PhotoContest.Infrastructure.ServiceResult
{
    public class ErrorServiceResult : IServiceResult
    {
        private List<Exception> _exceptions;

        public ErrorServiceResult()
        {
            this.Exceptions = new List<Exception>();
        }

        public List<Exception> Exceptions
        {
            get { return this._exceptions; }
            private set { this._exceptions = value; }
        }

        public bool AddException(Exception exception)
        {
            this._exceptions.Add(exception);

            return true;
        }

        public ServiceResultType ResultType
        {
            get { return ServiceResultType.UnSuccessful; }
        }
    }
}