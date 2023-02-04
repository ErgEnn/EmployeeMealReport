using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace Domain
{
	public class NonWorkday
	{
		[Required] 
		[Key] 
		public DateTime Date { get; set; }
	}
}