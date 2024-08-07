using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class LoginRepository : BaseRepositoryImplementation<Login>, ILoginRespository
    {
        public LoginRepository(GeolocationContext context) : base(context)
        {
        }

        public override async Task<ResponseTool<Login>> UpdateAsync(int id, Login entity)
        {
            ResponseTool<Login> response;

            try
            {
                var _toUpdate = await _context.Logins.FindAsync(id);

                if (_toUpdate.IsObjectNull())
                {
                    response = ResponseFactory<Login>.BuildFail("Not Found.", null, ToolsLibrary.Tools.Type.NotFound);

                    return response;
                }

                _toUpdate.SetUpdateInformation(entity.UpdateBy);

                _toUpdate.HashPassword = CommonsTool.GenerateHashPassword(entity.Password);

                await _context.SaveChangesAsync();

                return ResponseFactory<Login>.BuildSuccess("Updated successfully.", _toUpdate, ToolsLibrary.Tools.Type.Updated);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Login>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return response;
            }
        }

        //public async Task<ResponseTool<User>> VerifyCredential(Login credential)
        //{
        //    ResponseTool<User> response = null;

        //    try
        //    {
        //        // Primero obtenemos el usuario que coincide con el nombre de usuario proporcionado
        //        var foundLogin = await _context.Logins
        //                                       .Where(v => v.Username == credential.Username)
        //                                       .FirstOrDefaultAsync();

        //        // Si no se encuentra el usuario, devolvemos una respuesta de credenciales inválidas
        //        if (foundLogin.IsObjectNull() || !BCrypt.Net.BCrypt.Verify(credential.Password, foundLogin.HashPassword))
        //        {
        //            response = ResponseFactory<User>.BuildFail("Invalid Credential", null, ToolsLibrary.Tools.Type.NotFound);

        //            return response;
        //        }

        //        // Si las credenciales son válidas, obtenemos el usuario asociado
        //        var foundUser = await _context.Users
        //            .Where(v => v.ID == foundLogin.UserId)
        //            .Include(u => u.Login)
        //            .FirstOrDefaultAsync();

        //        if (!foundUser.IsObjectNull())
        //        {
        //            // Verificamos el estado del usuario
        //            if (foundUser.UserStatus == UserStatus.ResetPassword)
        //            {
        //                response = ResponseFactory<User>.BuildFail("User Reset Password", null, ToolsLibrary.Tools.Type.Fail);

        //                return response;
        //            }

        //            response = ResponseFactory<User>.BuildSuccess("Valid Credential", foundUser, ToolsLibrary.Tools.Type.Found);
        //        }
        //        else
        //        {
        //            response = ResponseFactory<User>.BuildFail("Invalid Credential", null, ToolsLibrary.Tools.Type.NotFound);
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response = ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

        //        return response;
        //    }
        //}

        public async Task<ResponseTool<User>> VerifyCredential(Login credential)
        {
            try
            {
                // Attempt to find login with matching username
                var foundLogin = await _context.Logins
                                               .FirstOrDefaultAsync(v => v.Username == credential.Username);

                // Handle cases where either login is not found or password verification fails
                if (foundLogin == null || !BCrypt.Net.BCrypt.Verify(credential.Password, foundLogin.HashPassword))
                {
                    return ResponseFactory<User>.BuildFail("Invalid Credential", null, ToolsLibrary.Tools.Type.NotFound);
                }

                // User is found, proceed to get associated user details
                var foundUser = await _context.Users
                                              .Include(u => u.Login)
                                              .FirstOrDefaultAsync(v => v.ID == foundLogin.UserId);

                if (foundUser.IsObjectNull())
                {
                    return ResponseFactory<User>.BuildFail("User not found", null, ToolsLibrary.Tools.Type.NotFound);
                }

                // Check user status
                if (foundUser.UserStatus == UserStatus.ResetPassword)
                {
                    return ResponseFactory<User>.BuildFail("User Reset Password", null, ToolsLibrary.Tools.Type.Fail);
                }

                // If all checks pass, return successful response
                return ResponseFactory<User>.BuildSuccess("Valid Credential", foundUser, ToolsLibrary.Tools.Type.Found);
            }
            catch (Exception ex)
            {
                // Generic exception handling
                return ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

    }
}