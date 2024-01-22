using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
	public class Role
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public ICollection<Employee> Employees { get; set; }
	}
}
