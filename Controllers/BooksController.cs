using Azure.Core;
using Library.Controllers;
using Library.Logic.DTOs;
using Library.Logic.Repository.Abstraction;
using Library.Logic.Repository.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{

	[Authorize]
	[ApiController]
	public class BooksController : ControllerBase
	{

		private readonly IBookService _bookService;
		private readonly ILogger<BooksController> _logger;


		public BooksController(IBookService book, ILogger<BooksController> logger )
		{
			_bookService = book; //bookservice 
			_logger = logger; // Implenented for logging
		}

		[HttpPost]
		[Route("api/Books")]
		public async Task<IActionResult> AddBook(BooksDTO request)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var newbook = _bookService.AddBooks(request);
					return Ok(newbook);
				}
				return BadRequest();
			}
			catch
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("api/ViewBooks")]
		public async Task<IActionResult> GetBooks()
		{
			try
			{
				var newbook = _bookService.GetAllBooks();
				return Ok(newbook);
			}
			catch (Exception ex)
			{
				return StatusCode(500);
			}

		}

		[HttpPut]
		[Route("api/EditBooks/{Id}")]
		public async Task<IActionResult> ModifyBook([FromBody] EditBookDTO request)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var newbook = _bookService.GetBookbyId(request);
					return Ok(newbook);
				}
				return StatusCode(400);

			}
			catch
			{
				return StatusCode(500);

			}
		}

		[HttpDelete]
		[Route("api/DeleteBooks/{Id}")]
		public async Task<IActionResult> DeleteBook([FromBody] BookRequestDTO request)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var newbook = _bookService.DeleteBookbyId(request.Id);
					return Ok(newbook);
				}
				return StatusCode(400);
			}
			catch (Exception ex)
			{
				return StatusCode(400);

			}
		}


		//Bonus

		[HttpGet]
		[Route("api/FilterBooks")]
		public async Task<IActionResult> FilterBooks([FromQuery] BookFilterRequestDTO filter)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = await _bookService.FilterBooksAsync(filter);
					return Ok(result); 
				}

				return BadRequest(new FilterResponseDTO
				{
					Success = false,
					Message = "Invalid filter parameters",
					Books = new List<BookResponseDTO>(),
					TotalCount = 0
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new FilterResponseDTO
				{
					Success = false,
					Message = $"Server error: {ex.Message}",
					Books = new List<BookResponseDTO>(),
					TotalCount = 0
				});
			}
		}

		//Pagination

		[HttpGet]
		[Route("api/GetAllBooksWithPagination")]
		public async Task<IActionResult> GetAccountReport([FromBody] ViewAllBooksDTO Request)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var result = await _bookService.GetAllBooksWithPagination(Request);
					return Ok(result);
				}
				else return BadRequest();
			}
			catch (Exception ex) 
			{
				return StatusCode(500);
			}
		}
	}
}
