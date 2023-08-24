using ToolsLibrary.Models;

namespace GeolocationAds.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        public static string _apiSuffix = nameof(UserService);

        public UserService()
        { }

        public UserService(string _apiSuffix) : base(_apiSuffix)
        {
        }

        public UserService(HttpClient htppClient, string _apiSuffix) : base(htppClient, _apiSuffix)
        {
        }
    }
}
