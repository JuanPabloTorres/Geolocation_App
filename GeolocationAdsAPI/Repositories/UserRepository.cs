using GeolocationAdsAPI.Context;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Repositories
{
    public class UserRepository : BaseRepositoryImplementation<User>, IUserRepository
    {
        public UserRepository(GeolocationContext context) : base(context)
        {
        }


    }
}
