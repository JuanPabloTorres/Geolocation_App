using Microsoft.Extensions.Configuration;
using ToolsLibrary.Models;

namespace GeolocationAds.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        public static string _apiSuffix = nameof(UserService);

        public UserService(HttpClient htppClient, IConfiguration configuration) : base(htppClient, configuration)
        {
        }
    }
}