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

        //public async Task<ResponseTool<User>> VerifyCredential(Login credential)
        //{
        //    ResponseTool<User> response = null;

        //    try
        //    {
        //        Func<Login, bool> credentialCondition = v => v.Username == credential.Username && BCrypt.Net.BCrypt.Verify(credential.Password, v.HashPassword);

        //        var _foundLogin = _context.Logins.Where(credentialCondition).FirstOrDefault();

        //        if (_foundLogin.IsObjectNull())
        //        {
        //            response = ResponseFactory<User>.BuildFail("Invalid Credentail", null, ToolsLibrary.Tools.Type.NotFound);

        //            return response;
        //        }

        //        var _foundUser = await _context.Users.Include(u => u.Login).Where(v => v.ID == _foundLogin.UserId).FirstOrDefaultAsync();

        //        if (!_foundUser.IsObjectNull())
        //        {
        //            if (_foundUser.UserStatus == UserStatus.ResetPassword)
        //            {
        //                response = ResponseFactory<User>.BuildFail("User Reset Password", null, ToolsLibrary.Tools.Type.Fail);

        //                return response;
        //            }

        //            response = ResponseFactory<User>.BuildSuccess("Valid Credentail", _foundUser, ToolsLibrary.Tools.Type.Found);
        //        }
        //        else
        //        {
        //            response = ResponseFactory<User>.BuildFail("Invalid Credentail", null, ToolsLibrary.Tools.Type.NotFound);
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
            ResponseTool<User> response = null;

            try
            {
                // Primero obtenemos el usuario que coincide con el nombre de usuario proporcionado
                var foundLogin = await _context.Logins
                                               .Where(v => v.Username == credential.Username)
                                               .FirstOrDefaultAsync();

                // Si no se encuentra el usuario, devolvemos una respuesta de credenciales inválidas
                if (foundLogin.IsObjectNull() || !BCrypt.Net.BCrypt.Verify(credential.Password, foundLogin.HashPassword))
                {
                    response = ResponseFactory<User>.BuildFail("Invalid Credential", null, ToolsLibrary.Tools.Type.NotFound);

                    return response;
                }

                // Si las credenciales son válidas, obtenemos el usuario asociado
                var foundUser = await _context.Users
                                              .Include(u => u.Login)
                                              .Where(v => v.ID == foundLogin.UserId)
                                              .FirstOrDefaultAsync();

                if (!foundUser.IsObjectNull())
                {
                    // Verificamos el estado del usuario
                    if (foundUser.UserStatus == UserStatus.ResetPassword)
                    {
                        response = ResponseFactory<User>.BuildFail("User Reset Password", null, ToolsLibrary.Tools.Type.Fail);

                        return response;
                    }

                    response = ResponseFactory<User>.BuildSuccess("Valid Credential", foundUser, ToolsLibrary.Tools.Type.Found);
                }
                else
                {
                    response = ResponseFactory<User>.BuildFail("Invalid Credential", null, ToolsLibrary.Tools.Type.NotFound);
                }

                return response;
            }
            catch (Exception ex)
            {
                response = ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return response;
            }
        }

    }
}