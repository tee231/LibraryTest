using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.DTOs
{
	public class BookResponseDTO
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
		public string ISBN { get; set; } = string.Empty;
		public DateTime PublishedDate { get; set; }
		public DateTime ModifiedDate { get; set; }
	}

	public class FilterResponseDTO
	{
		public bool Success { get; set; }
		public string Message { get; set; }
		public List<BookResponseDTO> Books { get; set; }
		public int TotalCount { get; set; }
	}
}
