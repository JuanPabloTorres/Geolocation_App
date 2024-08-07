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
            ResponseTool<User> response;

            try
            {
                var _user = await _context.Users.Where(v => v.Email == email).FirstOrDefaultAsync();

                if (!_user.IsObjectNull())
                {
                    response = ResponseFactory<User>.BuildSuccess("Entity Found successfully.", _user, ToolsLibrary.Tools.Type.Added);
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

        public async Task<ResponseTool<bool>> IsEmailRegistered(string email)
        {
            ResponseTool<bool> response;

            try
            {
                var _isRegistered = await _context.Users.AnyAsync(v => v.Email == email);

                if (_isRegistered)
                {
                    response = ResponseFactory<bool>.BuildSuccess("Email is registered.", _isRegistered, ToolsLibrary.Tools.Type.Exist);

                    return response;
                }

                response = ResponseFactory<bool>.BuildFail("Email is not registered.", _isRegistered, ToolsLibrary.Tools.Type.NotExist);

                return response;
            }
            catch (Exception ex)
            {
                response = ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);

                return response;
            }
        }

        //public override async Task<ResponseTool<User>> UpdateAsync(int id, User entity)
        //{
        //    ResponseTool<User> response;

        //    try
        //    {
        //        var _userToUpdate = await _context.Users.Include(l => l.Login).FirstOrDefaultAsync(u => u.ID == id);

        //        if (_userToUpdate.IsObjectNull())
        //        {
        //            response = ResponseFactory<User>.BuildFail("Entity Not Found.", null, ToolsLibrary.Tools.Type.NotFound);

        //            return response;
        //        }

        //        _userToUpdate.SetUpdateInformation(_userToUpdate.ID);

        //        _userToUpdate.Login.Password = entity.Login.Password;

        //        var _newHash = CommonsTool.GenerateHashPassword(entity.Login.Password);

        //        _userToUpdate.Login.HashPassword = CommonsTool.GenerateHashPassword(entity.Login.Password);

        //        await _context.SaveChangesAsync();

        //        return ResponseFactory<User>.BuildSuccess("Entity Found successfully.", _userToUpdate, ToolsLibrary.Tools.Type.Updated);
        //    }
        //    catch (Exception ex)
        //    {
        //        response = ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

        //        return response;
        //    }
        //}
    }
}