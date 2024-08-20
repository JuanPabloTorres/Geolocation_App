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
                // Intentar encontrar el login con el username coincidente
                var foundLogin = await _context.Logins
                                               .AsNoTracking()  // Evitar el tracking si no se necesita modificar la entidad
                                               .FirstOrDefaultAsync(v => v.Username == credential.Username);

                // Manejar casos donde no se encuentra el login o la verificación de contraseña falla
                if (foundLogin == null || !BCrypt.Net.BCrypt.Verify(credential.Password, foundLogin.HashPassword))
                {
                    return ResponseFactory<User>.BuildFail("Invalid Credential", null, ToolsLibrary.Tools.Type.NotFound);
                }

                // Obtener los detalles del usuario asociado si la verificación de login fue exitosa
                var foundUser = await _context.Users
                                              .Include(u => u.Login)
                                              .AsNoTracking()  // Evitar el tracking si no se necesita modificar la entidad
                                              .FirstOrDefaultAsync(v => v.ID == foundLogin.UserId);

                if (foundUser == null)
                {
                    return ResponseFactory<User>.BuildFail("User not found", null, ToolsLibrary.Tools.Type.NotFound);
                }

                // Verificar el estado del usuario
                if (foundUser.UserStatus == UserStatus.ResetPassword)
                {
                    return ResponseFactory<User>.BuildFail("User needs to reset password", null, ToolsLibrary.Tools.Type.Fail);
                }

                // Si todas las verificaciones son exitosas, devolver una respuesta exitosa
                return ResponseFactory<User>.BuildSuccess("Valid Credential", foundUser, ToolsLibrary.Tools.Type.Found);
            }
            catch (Exception ex)
            {
                // Manejo genérico de excepciones
                return ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<User>> VerifyCredentialByProvider(Login credential)
        {
            try
            {
                User foundUser = null;

                if (credential.Provider == ToolsLibrary.Enums.Providers.Google)
                {
                    // Intentar encontrar el login con GoogleId coincidente
                    var foundLogin = await _context.Logins.Include(u => u.User).FirstOrDefaultAsync(v => v.GoogleId == credential.GoogleId);

                    if (!foundLogin.IsObjectNull())
                    {
                        foundUser = foundLogin.User;
                    }
                }

                // Verifica si se encontró el usuario
                if (foundUser.IsObjectNull())
                {
                    return ResponseFactory<User>.BuildFail("User not found", null, ToolsLibrary.Tools.Type.NotFound);
                }

                // Si todas las verificaciones pasan, devuelve una respuesta exitosa
                return ResponseFactory<User>.BuildSuccess("Valid Credential", foundUser, ToolsLibrary.Tools.Type.Found);
            }
            catch (Exception ex)
            {
                // Manejo genérico de excepciones
                return ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}