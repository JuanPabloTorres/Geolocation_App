using GeolocationAdsAPI.Context;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Repositories
{
    public class ForgotPasswordRepository : BaseRepositoryImplementation<ForgotPassword>, IForgotPasswordRepository
    {
        public ForgotPasswordRepository(GeolocationContext context) : base(context)
        {
        }
    }
}
