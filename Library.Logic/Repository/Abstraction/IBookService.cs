using Library.Domain.Models;
using Library.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.Repository.Abstraction
{
	public interface IBookService
	{
		Task<AddBookResponse> AddBooks(BooksDTO req);
		Task<AddBookResponse> DeleteBookbyId(int Id);
		Task<FilterResponseDTO> FilterBooksAsync(BookFilterRequestDTO filter);
		Task<List<AddBookResponse>> GetAllBooks();
		Task<AddBookResponse> GetAllBooksWithPagination(ViewAllBooksDTO res);
		Task<AddBookResponse> GetBookbyId(int Id);

		//Task<List<AddBookResponse>> SearchBooksAsync(string searchTerm); // Add this
	}
}
