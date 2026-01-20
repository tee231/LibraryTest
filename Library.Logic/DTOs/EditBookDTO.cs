using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.DTOs
{
	public class EditBookDTO
	{
		public int Id { get; set; }
		public string? Title { get; set; } = string.Empty;
		public string? Author { get; set; } = string.Empty;
		public string? ISBN { get; set; } = string.Empty;
		public DateTime? PublishedDate { get; set; }
		public DateTime ModifieddDate { get; set; }

	}
}
