using DataAccess.Entities;
using DataAccess.Repositories;
using LibraryConsoleApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookController : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAllBooks()
		{
			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			List<Book> books = await bookRepository.ReadAllAsync();
			return Ok(books);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetOneBook(long id)
		{
			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			Book book = await bookRepository.ReadOneAsync(id);
			return Ok(book);
		}

		[HttpPost]
		public async Task<IActionResult> AddBook(Book book)
		{
			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			await bookRepository.CreateAsync(book);
			return Ok(book);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBook(int id, string title, string genre, int authorId, string Image)
		{
			var book = new Book
			{
				Id = id,
				Title = title,
				Genre = genre,
				AuthorId = authorId

			};
			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			await bookRepository.UpdateAsync(book);
			return NoContent();

		}



		[HttpPut("{id}/json")]
		public async Task<IActionResult> UpdateBook(Book book)
		{

			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			await bookRepository.UpdateAsync(book);
			return NoContent();

		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBook(int id)
		{
			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			await bookRepository.DeleteAsync(id);
			return Ok(id);
		}
	}
}
