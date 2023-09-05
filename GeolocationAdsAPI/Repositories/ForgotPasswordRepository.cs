using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Dto;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class ForgotPasswordRepository : BaseRepositoryImplementation<ForgotPassword>, IForgotPasswordRepository
    {
        public ForgotPasswordRepository(GeolocationContext context) : base(context)
        {
        }

        public async Task<ResponseTool<ForgotPassword>> UserHaveForgotPassword(int userId)
        {
            try
            {
                var entity = await _context.ForgotPasswords.Where(v => v.UserId == userId && v.IsValid == true && v.ExpTime >= DateTime.Now).FirstOrDefaultAsync();

                if (!entity.IsObjectNull())
                {
                    return ResponseFactory<ForgotPassword>.BuildSusccess("Reset Password.", entity, ToolsLibrary.Tools.Type.IsRecoveryPassword);
                }
                else
                {
                    return ResponseFactory<ForgotPassword>.BuildFail("Not have recovery password.", null, ToolsLibrary.Tools.Type.NotRecoveryPassword);
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<ForgotPassword>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<ForgotPassword>> ConfirmCode(string code)
        {
            try
            {
                var entity = await _context.ForgotPasswords.Where(v => v.Code == code && v.ExpTime >= DateTime.Now).FirstOrDefaultAsync();

                if (!entity.IsObjectNull())
                {
                    return ResponseFactory<ForgotPassword>.BuildSusccess("Code Confirmed.", entity, ToolsLibrary.Tools.Type.Found);
                }
                else
                {
                    return ResponseFactory<ForgotPassword>.BuildFail("Invalid Code.Try again.", null, ToolsLibrary.Tools.Type.NotFound);
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<ForgotPassword>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<ForgotPassword>>> InvalidUserForgotPassword(int userId)
        {
            try
            {
                var entity = await _context.ForgotPasswords.Where(v => v.UserId == userId && v.IsValid == true).ToListAsync();

                if (!entity.IsObjectNull())
                {
                    foreach (var item in entity)
                    {
                        item.IsValid = false;
                    }

                    await _context.SaveChangesAsync();

                    return ResponseFactory<IEnumerable<ForgotPassword>>.BuildSusccess("Reset Password.", entity, ToolsLibrary.Tools.Type.IsRecoveryPassword);
                }
                else
                {
                    return ResponseFactory<IEnumerable<ForgotPassword>>.BuildFail("Not have recovery password.", null, ToolsLibrary.Tools.Type.NotRecoveryPassword);
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<ForgotPassword>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<ForgotPassword>> ChangePassword(NewPasswordDto newPasswordDto)
        {
            try
            {
                var entity = await _context.ForgotPasswords.Where(v => v.Code == newPasswordDto.Code).FirstOrDefaultAsync();

                if (!entity.IsObjectNull())
                {
                    var _user = await _context.Users.Include(l => l.Login).Where(u => u.ID == entity.UserId).FirstOrDefaultAsync();

                    if (_user.IsObjectNull())
                    {
                        return ResponseFactory<ForgotPassword>.BuildFail("Fail on change password.", null, ToolsLibrary.Tools.Type.Fail);
                    }

                    _user.Login.Password = newPasswordDto.NewPassword;

                    _context.Entry(_user).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    return ResponseFactory<ForgotPassword>.BuildSusccess("Succesffully.", entity, ToolsLibrary.Tools.Type.IsRecoveryPassword);
                }
                else
                {
                    return ResponseFactory<ForgotPassword>.BuildFail("Fail on change password.", null, ToolsLibrary.Tools.Type.Fail);
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<ForgotPassword>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}