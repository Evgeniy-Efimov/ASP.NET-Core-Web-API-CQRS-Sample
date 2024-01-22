using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
	public class Employee
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Login { get; set; }

		[Required]
		public string Email { get; set; }

		public string Department { get; set; }

		public string Location { get; set; }

		public ICollection<Role> Roles { get; set; }
	}
}
