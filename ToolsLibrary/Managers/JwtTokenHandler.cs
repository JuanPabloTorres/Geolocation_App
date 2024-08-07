using System.Net.Http.Headers;

namespace ToolsLibrary.Managers
{
    public class JwtTokenHandler : DelegatingHandler
    {
        private readonly string _token;

        public JwtTokenHandler(string token)
        {
            _token = token;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}