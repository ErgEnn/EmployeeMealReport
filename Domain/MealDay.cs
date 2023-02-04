using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
	public class MealDay
	{
		public int Id { get; set; }
		[Required]
		public DateTime Date { get; set; }
		public int EmployeeId { get; set; }
	}
}
