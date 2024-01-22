using Application.Providers.JwtTokenProvider.Models;

namespace Application.Providers.JwtTokenProvider.Interfaces
{
	public interface IJwtTokenProvider
	{
		string GetToken(JwtUserData userData);
	}
}
