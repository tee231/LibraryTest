using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models
{
	public class AddBookResponse
	{
		public string message { get; set; }
		public string responsecode { get; set; }

		public object responedata { get; set; } = new object();
	}
}
