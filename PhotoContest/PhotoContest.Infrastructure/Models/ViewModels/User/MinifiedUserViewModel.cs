namespace PhotoContest.Infrastructure.Models.ViewModels.User
{
    using PhotoContest.Infrastructure.Mapping;

    public class MinifiedUserViewModel : IMapFrom<PhotoContest.Models.User>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
        
        public string ProfileImageUrl { get; set; }
    }
}