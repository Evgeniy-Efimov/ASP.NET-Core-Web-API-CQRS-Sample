namespace Application.Providers.JwtTokenProvider.Models
{
	public class JwtUserData
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Login { get; set; }

		public string Email { get; set; }

		public string Department { get; set; }

		public string Location { get; set; }

		public IEnumerable<string> Roles { get; set; }
	}
}
