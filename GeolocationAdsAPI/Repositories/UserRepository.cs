using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class UserRepository : BaseRepositoryImplementation<User>, IUserRepository
    {
        public UserRepository(GeolocationContext context) : base(context)
        {
        }

        public async Task<ResponseTool<Login>> AddLoginCredential(Login credential)
        {
            ResponseTool<Login> response;

            try
            {
                credential.CreateDate = DateTime.Now;

                await _context.AddAsync(credential);

                await _context.SaveChangesAsync();

                response = ResponseFactory<Login>.BuildSuccess("Entity created successfully.", credential, ToolsLibrary.Tools.Type.Added);

                return response;
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Login>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return response;
            }
        }

        public async Task<ResponseTool<User>> ChangeStatus(int userId, UserStatus status)
        {
            ResponseTool<User> response;

            try
            {
                var _user = await _context.Users.FindAsync(userId);

                if (!_user.IsObjectNull())
                {
                    _user.UserStatus = status;

                    await _context.SaveChangesAsync();

                    response = ResponseFactory<User>.BuildSuccess("Setting Added.", _user, ToolsLibrary.Tools.Type.Added);
                }
                else
                {
                    response = ResponseFactory<User>.BuildFail("Entity Not Found.", null, ToolsLibrary.Tools.Type.NotFound);
                }

                return response;
            }
            catch (Exception ex)
            {
                response = ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return response;
            }
        }

        public async Task<ResponseTool<User>> GetUserByEmail(string email)
        {
            try
            {
                var _user = await _context.Users.Include("Login").Where(v => v.Email == email).FirstOrDefaultAsync();

                if (_user.IsObjectNull())
                    return ResponseFactory<User>.BuildFail("Entity Not Found.", null, ToolsLibrary.Tools.Type.NotFound);

                if (_user.Login.IsExternaUser())
                    return ResponseFactory<User>.BuildFail("Could Not Change Password To this User.", null, ToolsLibrary.Tools.Type.ExternalUser);

                return ResponseFactory<User>.BuildSuccess("Entity Found successfully.", _user, ToolsLibrary.Tools.Type.Found);
            }
            catch (Exception ex)
            {
                return ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<bool>> IsEmailRegistered(string email)
        {
            ResponseTool<bool> response;

            try
            {
                var _isRegistered = await _context.Users.AnyAsync(v => v.Email == email);

                if (!_isRegistered)
                {
                    response = ResponseFactory<bool>.BuildFail("Email is not registered.", _isRegistered, ToolsLibrary.Tools.Type.NotExist);

                    return response;
                }

                response = ResponseFactory<bool>.BuildSuccess("Email is registered.", _isRegistered, ToolsLibrary.Tools.Type.Exist);

                return response;
            }
            catch (Exception ex)
            {
                response = ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);

                return response;
            }
        }
    }
}