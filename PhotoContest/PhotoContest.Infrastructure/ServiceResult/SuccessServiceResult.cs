using System.Collections.Generic;
using PhotoContest.Infrastructure.Enumerations;
using PhotoContest.Infrastructure.Interfaces;

namespace PhotoContest.Infrastructure.ServiceResult
{
    public class SuccessServiceResult : IServiceResult
    {
        private List<string> _messages;

        public SuccessServiceResult()
        {
            this._messages = new List<string>();
        }

        public List<string> Messages
        {
            get { return this._messages; }
            private set { this._messages = value; }
        }

        public bool AddMessage(string message)
        {
            this._messages.Add(message);

            return true;
        }

        public ServiceResultType ResultType
        {
            get
            {
                return ServiceResultType.Successful;
            }
        }
    }
}