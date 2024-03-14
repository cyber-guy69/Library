using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories;
public class AuthorRepository
{
	private readonly LibraryDbContext _context;

	public AuthorRepository(LibraryDbContext context)
	{
		_context = context;
	}

	public async Task<Author> ReadOneAsync(long id)
	{
		return await _context.Authors
			 .Select(a => new Author
			 {
				 Id = a.Id,
				 Name = a.Name,
				 Surname = a.Surname,

				 Books = a.Books.Select(b => new Book { Id = b.Id, Title = b.Title, Genre = b.Genre }).ToList()
			 })
			.FirstOrDefaultAsync(a => a.Id == id);

	}



	public async Task<List<Author>> ReadAllAsync()
	{
		return await _context.Authors
	 .Select(a => new Author
	 {
		 Id = a.Id,
		 Name = a.Name,
		 Surname = a.Surname,
		 Books = a.Books.Select(b => new Book { Id = b.Id, Title = b.Title, Genre = b.Genre }).ToList()
	 })
	 .ToListAsync();
	}



	public async Task UpdateAsync(Author author)
	{

		var eAuthor = await _context.Authors.FindAsync(author.Id);
		if (eAuthor == null)
		{
			throw new KeyNotFoundException($"Author with Id {author.Id} not found.");
		}
		if (string.IsNullOrWhiteSpace(author.Name) || ContainsNumbersOrSymbols(author.Name) || ContainsNumbersOrSymbols(author.Surname))
		{
			throw new Exception("Invalid Author Or Surname format");
		}

		_context.Authors.Entry(eAuthor).CurrentValues.SetValues(author);
		await _context.SaveChangesAsync();

	}

	public async Task CreateAsync(Author author)
	{

		if (string.IsNullOrWhiteSpace(author.Name) || ContainsNumbersOrSymbols(author.Name) || ContainsNumbersOrSymbols(author.Surname))
		{
			throw new Exception("Invalid name format: Invalid Name or Authors Surname");
		}
		_context.Authors.Add(author);
		await _context.SaveChangesAsync();

	}

	public async Task DeleteAsync(long id)
	{
		Author author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
		_context.Authors.Remove(author);
		await _context.SaveChangesAsync();


	}


	public async Task<List<Author>> FindAuthorsByName(string searchKeyword)
	{
		return await _context.Authors
			.Include(a => a.Books)
			.Where(a => a.Name.Contains(searchKeyword) || a.Surname.Contains(searchKeyword))
			.ToListAsync();
	}


	private bool ContainsNumbersOrSymbols(string input)
	{
		return System.Text.RegularExpressions.Regex.IsMatch(input, @"[^a-zA-Z\s]");
	}
}

