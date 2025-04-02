using ToolsLibrary.Enums;
using ToolsLibrary.Models;

namespace GeolocationAds.Factories
{
    public class LoginFactory : ILoginFactory
    {
        public ToolsLibrary.Models.Login CreateLogin(string email, string? googleId = null, string? facebookId = null,
                                 Providers provider = Providers.App)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty", nameof(email));

            return CreateLoginInternal(email, googleId, facebookId, provider);
        }

        public ToolsLibrary.Models.Login CreateCredential(string providerId, Providers provider)
        {
            if (string.IsNullOrWhiteSpace(providerId))
                throw new ArgumentException("Provider ID cannot be null or empty", nameof(providerId));

            return provider switch
            {
                Providers.Google => CreateLoginInternal("googleuser", providerId, null, Providers.Google),
                Providers.Facebook => CreateLoginInternal("facebook", null, providerId, Providers.Facebook),
                _ => throw new NotSupportedException($"Provider {provider} is not supported.")
            };
        }

        private static ToolsLibrary.Models.Login CreateLoginInternal(string email, string? googleId, string? facebookId, Providers provider)
        {
            return new ToolsLibrary.Models.Login
            {
                CreateBy = 0,
                CreateDate = DateTime.UtcNow,
                GoogleId = googleId,
                FacebookId = facebookId,
                Password = provider switch
                {
                    Providers.Google => "google",
                    Providers.Facebook => "facebook",
                    _ => "default_password"
                },
                Username = email,
                Provider = provider
            };
        }
    }
}