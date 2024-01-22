namespace Application.Providers.JwtTokenProvider.Settings
{
	public class JwtTokenSettings
	{
		public string SecretKey { get; set; }
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public int Lifetime { get; set; }
	}
}
