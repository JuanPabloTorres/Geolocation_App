using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<ResponseTool<User>> GetUserByEmail(string email);

        public Task<ResponseTool<User>> ChangeStatus(int userId, UserStatus status);
    }
}