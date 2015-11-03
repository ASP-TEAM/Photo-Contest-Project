using PhotoContest.Infrastructure.Enumerations;

namespace PhotoContest.Infrastructure.Interfaces
{
    public interface IServiceResult
    {
         ServiceResultType ResultType { get; }
    }
}