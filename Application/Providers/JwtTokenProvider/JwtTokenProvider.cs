using Application.Providers.JwtTokenProvider.Interfaces;
using Application.Providers.JwtTokenProvider.Models;
using Application.Providers.JwtTokenProvider.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Providers.JwtTokenProvider
{
	public class JwtTokenProvider : IJwtTokenProvider
	{
		private readonly JwtTokenSettings _jwtTokenSettings;

		public JwtTokenProvider(IOptions<JwtTokenSettings> jwtTokenOptions)
		{
			_jwtTokenSettings = jwtTokenOptions.Value;
		}

		public string GetToken(JwtUserData userData)
		{
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Expires = DateTime.UtcNow.AddMinutes(_jwtTokenSettings.Lifetime),
				Issuer = _jwtTokenSettings.Issuer,
				Audience = _jwtTokenSettings.Audience,
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSettings.SecretKey)),
					SecurityAlgorithms.HmacSha256),
				Subject = GetClaimsIdentity(userData)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		private static ClaimsIdentity GetClaimsIdentity(JwtUserData userData)
		{
			var claimsIdentity = new ClaimsIdentity(new[]
			{
				new Claim(JwtClaims.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtClaims.Sub, userData.Id.ToString()),
				new Claim(JwtClaims.Email, userData.Email),
				new Claim(JwtClaims.UserPrid, userData.Login),
				new Claim(JwtClaims.Department, userData.Department),
				new Claim(JwtClaims.Location, userData.Location)
			});

			foreach (var role in userData.Roles)
			{
				claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
			}

			return claimsIdentity;
		}
	}
}
