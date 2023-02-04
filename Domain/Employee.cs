using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain
{
	public class Employee
	{
		public int Id { get; set; }
		[Required]
		[MinLength(1)]
		[MaxLength(64)]
		public string FirstName { get; set; }
		[Required]
		[MinLength(1)]
		[MaxLength(64)]
		public string LastName { get; set; }

		public ICollection<MealDay> MealDays { get; set; } = new List<MealDay>();

		public string LastFirstName => $"{LastName}, {FirstName}";

		public IEnumerable<MealDay> CurrentMealDays(DateTime start, DateTime end) =>
			MealDays.Where(day => day.Date >= start && day.Date <= end);
	}
}
