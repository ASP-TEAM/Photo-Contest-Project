namespace PhotoContest.App.Models.ViewModels.User
{
    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.Models;

    public class MinifiedUserViewModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
        
        public string ProfileImageUrl { get; set; }
    }
}