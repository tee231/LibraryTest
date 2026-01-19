using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models
{
	public class Books
	{
		public int Id { get; set; }
       public string Title { get; set; } = string.Empty;
      public   string Author { get; set; } = string.Empty ;
		public string ISBN { get; set; } = string.Empty;
      public   DateTime PublishedDate { get; set; }

		// move this to a base class.
	public	DateTime ModifiedDate { get; set; }
	public	bool IsDeleted { get; set; }
	}
}
