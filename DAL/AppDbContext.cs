using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
		{
			
		}
		public DbSet<Employee> Employees { get; set; }
		public DbSet<MealDay> MealDays { get; set; }
		public DbSet<NonWorkday> NonWorkdays { get; set; }
	}
}
