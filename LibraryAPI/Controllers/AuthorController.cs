using DataAccess.Entities;
using DataAccess.Repositories;
using LibraryConsoleApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAllAuthors()
	{
		var dbcf = new LibraryDbContextFactory();
		var context = dbcf.CreateDbContext([]);
		var authorRepository = new AuthorRepository(context);
		List<Author> authors = await authorRepository.ReadAllAsync();
		return Ok(authors);
	}
	[HttpGet("{id}")]
	public async Task<IActionResult> GetOneAuthor(long id)
	{
		var dbcf = new LibraryDbContextFactory();
		var context = dbcf.CreateDbContext([]);
		var authorRepository = new AuthorRepository(context);
		Author author = await authorRepository.ReadOneAsync(id);
		return Ok(author);
	}



	[HttpPost]
	public async Task<IActionResult> AddAuthor(Author author)
	{
		var dbcf = new LibraryDbContextFactory();
		var context = dbcf.CreateDbContext([]);
		var authorRepository = new AuthorRepository(context);
		await authorRepository.CreateAsync(author);
		return Ok(author);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateAuthor(int id, string name, string surname)
	{
		var dbcf = new LibraryDbContextFactory();
		var context = dbcf.CreateDbContext([]);
		var authorRepository = new AuthorRepository(context);
		var author = new Author
		{
			Id = id,
			Name = name,
			Surname = surname,
		};
		await authorRepository.UpdateAsync(author);
		return NoContent();

	}
	[HttpPut("{id}/json")]
	public async Task<IActionResult> UpdateAuthor(Author author, long id)
	{
		author.Id = id;
		var dbcf = new LibraryDbContextFactory();
		var context = dbcf.CreateDbContext([]);
		AuthorRepository authorRepository = new AuthorRepository(context);
		await authorRepository.UpdateAsync(author);
		return Ok(author);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteAuthor(int id)
	{
		var dbcf = new LibraryDbContextFactory();
		var context = dbcf.CreateDbContext([]);
		var authorRepository = new AuthorRepository(context);
		await authorRepository.DeleteAsync(id);
		return NoContent();
	}
}
