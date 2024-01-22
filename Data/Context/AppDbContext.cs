using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
	public class AppDbContext : DbContext, IAppDbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Employee> Employees { get; set; }

		public DbSet<Role> Roles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
