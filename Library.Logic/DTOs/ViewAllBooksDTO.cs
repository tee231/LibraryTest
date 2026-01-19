using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.DTOs
{
	public class ViewAllBooksDTO
	{
		
			[Required]
			public int Pagesize { get; set; }

			[Required]
			public int pagenumber { get; set; }
			public bool Pagination { get; set; }
	

	}
}
