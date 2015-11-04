namespace PhotoContest.Infrastructure.Services
{
    using PhotoContest.Data.Interfaces;

    public class HomeService : BaseService
    {
        public HomeService(IPhotoContestData data) : base(data)
        {

        }
    }
}