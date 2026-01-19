using Library.Domain.Models;
using Library.Logic.DTOs;
using Library.Logic.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.Repository.Implementation
{
	public class BookService : IBookService
	{
		private readonly IRepository<Books> _book;
		public BookService(IRepository<Books> book)
		{
				_book = book;
		}

		public async Task <AddBookResponse> AddBooks(BooksDTO req)
		{
			try
			{
				Books book = new Books();
				book.Id = req.Id;
				book.Title = req.Title;
				book.Author = req.Author;
				book.ISBN = req.ISBN;
				book.PublishedDate = DateTime.Now;
				await _book.InsertAsync(book);
				return new AddBookResponse { message = "Successful", responsecode = "00", responedata = new() };
			}

			catch (Exception ex) 
			{
				return new AddBookResponse { message = "UnSuccessful", responsecode = "04", responedata = new() };
			}
		}


		public async Task<List<AddBookResponse>> GetAllBooks()
		{
			try
			{
				AddBookResponse response = new AddBookResponse();


				var allbooks = await _book.GetAllAsync(); //gets all books from the Db
				var ListOfbook = allbooks.ToList();      //
				if (ListOfbook.Count > 0)
				{
					var mappedBooks = ListOfbook.Select(b => new BooksDTO
					{
						Id = b.Id,
						Title = b.Title,
						Author = b.Author,
						ISBN = b.ISBN
			
						// Map properties returned books here
					}).ToList();

					// Wrap the entire list in one AddBookResponse, then put that in a List
					response = new AddBookResponse
					{
						message = "Successful",
						responsecode = "00",
						responedata = mappedBooks
					};

					return new List<AddBookResponse> { response };
				}
				return new List<AddBookResponse>
				{
					new AddBookResponse
					{
						message = "No books found",
						responsecode = "01",
						responedata = new List<BooksDTO>()
					}
				};
			}
			catch (Exception ex)
			{

				return new List<AddBookResponse>
				{
				new AddBookResponse
				{
				message = "Unsuccessful: " + ex.Message,
				responsecode = "04",
				responedata = new List<BooksDTO>()
				}
			    };
			}
		}


		public async Task<AddBookResponse> GetBookbyId(int Id)
		{
			try
			{
				var book = await _book.GetByConditionAsync(s => s.Id == Id);
				var res = book.FirstOrDefaultAsync();
				if(res == null)
				{
					return new AddBookResponse { message = "Book not found", responsecode = "06", responedata = new () };
				}

				return new AddBookResponse { message = "Successful", responsecode = "00", responedata = res };
			}

			catch (Exception ex)
			{
				return new AddBookResponse { message = "UnSuccessful", responsecode = "04", responedata = new() };
			}
		}

		public async Task<AddBookResponse> DeleteBookbyId(int Id)
		{
			try
			{
				var book = _book.GetByConditionAsync(s => s.Id == Id);
				if (book == null)
				{
					return new AddBookResponse { message = "Book not found", responsecode = "06", responedata = new() };
				}
				else 
				{ 
				await _book.DeleteAsync(s => s.Id == Id);

				return new AddBookResponse { message = "Successful", responsecode = "00", responedata = book };
		     	}
			}

			catch (Exception ex)
			{
				return new AddBookResponse { message = "UnSuccessful", responsecode = "04", responedata = new() };
			}
		}

		public async Task<FilterResponseDTO> FilterBooksAsync(BookFilterRequestDTO filter)
		{
			try
			{
				// Get books based on search term
				var books = await _book.SearchAsync(filter.search);

				// Apply additional filters
				var filteredBooks = books
					.Where(b => string.IsNullOrWhiteSpace(filter.Author) || b.Author.Contains(filter.Author))
					.Where(b => string.IsNullOrWhiteSpace(filter.Title) || b.Title.Contains(filter.Title))
					.ToList();

				var bookDTOs = filteredBooks.Select(book => new BookResponseDTO
				{
					Id = book.Id,
					Title = book.Title,
					Author = book.Author,
					ISBN = book.ISBN,
					// Map other properties
				}).ToList();

				return new FilterResponseDTO
				{
					Success = true,
					Message = bookDTOs.Count > 0 ? $"Found {bookDTOs.Count} book(s)" : "No books found",
					Books = bookDTOs,
					TotalCount = bookDTOs.Count
				};
			}
			catch (Exception ex)
			{
				return new FilterResponseDTO
				{
					Success = false,
					Message = $"Error: {ex.Message}",
					Books = new List<BookResponseDTO>(),
					TotalCount = 0
				};
			}
		}

		public async Task<AddBookResponse> GetAllBooksWithPagination(ViewAllBooksDTO res)
		{
			try
			{
				// Get all books
				var allbooks = await _book.GetAllAsync();
				var ListOfbook = allbooks.ToList();

				// Check if pagination is requested
				if (res.Pagination)
				{
					// Calculate pagination
					var totalCount = ListOfbook.Count;
					var totalPages = (int)Math.Ceiling((double)totalCount / res.Pagesize);
					var currentPage = res.pagenumber < 1 ? 1 : res.pagenumber;

					// Apply pagination
					var paginatedBooks = ListOfbook
						.Skip((currentPage - 1) * res.Pagesize)
						.Take(res.Pagesize)
						.ToList();

					// Update list to show paginated results
					ListOfbook = paginatedBooks;
				}

				
				var mappedBooks = ListOfbook.Select(b => new BooksDTO
				{
					Id = b.Id,
					Title = b.Title,
					Author = b.Author,
					ISBN = b.ISBN,
				}).ToList();

				return new AddBookResponse
				{
					message = mappedBooks.Count > 0 ? "Successful" : "No books found",
					responsecode = mappedBooks.Count > 0 ? "00" : "01",
					responedata = mappedBooks
				};
			}
			catch (Exception ex)
			{
				return new AddBookResponse
				{
					message = $"Error: {ex.Message}",
					responsecode = "04",
					responedata = new List<BooksDTO>()
				};
			}
		}

	}
}
