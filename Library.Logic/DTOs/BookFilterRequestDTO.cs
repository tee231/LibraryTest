using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.DTOs
{
	public class BookFilterRequestDTO
	{
		public string search {  get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;

	}
}
