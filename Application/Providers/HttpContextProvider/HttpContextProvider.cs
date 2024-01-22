using Application.Providers.HttpContextProvider.Interfaces;
using Application.Providers.JwtTokenProvider.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Providers.HttpContextProvider
{
    public class HttpContextProvider : IHttpContextProvider
	{
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserId()
        {
            var subClaim = _httpContextAccessor?.HttpContext?.User?.FindFirst(JwtClaims.Sub) ??
                throw new UnauthorizedAccessException("User is not authorized");

            return Guid.Parse(subClaim.Value);
        }

        public string GetUserLogin()
        {
            var loginWithDomain =
                _httpContextAccessor?.HttpContext?.User?.Identity?.Name ??
                _httpContextAccessor?.HttpContext?.User?.FindFirst(JwtClaims.UserPrid)?.Value ??
                    throw new UnauthorizedAccessException("User is not authorized");

            var loginWithDomainArray = loginWithDomain.Split('\\');

            return loginWithDomainArray.Length == 2 ? loginWithDomainArray.Last() : loginWithDomainArray.First();
        }
    }
}
