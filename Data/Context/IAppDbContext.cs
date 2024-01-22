using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
	public interface IAppDbContext
	{
		DbSet<Employee> Employees { get; set; }

		DbSet<Role> Roles { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
