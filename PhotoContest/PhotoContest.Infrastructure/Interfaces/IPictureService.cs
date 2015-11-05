namespace PhotoContest.Infrastructure.Interfaces
{
    public interface IPictureService
    {
        int Vote(int id, string userId);
    }
}