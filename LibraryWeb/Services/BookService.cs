using LibraryWeb.Models;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


namespace LibraryWeb.Services;

public class BookService
{
	private readonly HttpClient _httpClient;
	private readonly string _baseUrl = "https://localhost:7003/api/Book";

	public BookService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<List<Book>> getAllBooksAsync()
	{
		var response = await _httpClient.GetAsync(_baseUrl);
		return await response.Content.ReadFromJsonAsync<List<Book>>();
	}

	public async Task<Book> getBookByIdAsync(long Id)
	{
		var response = await _httpClient.GetAsync($"{_baseUrl}/{Id}");
		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadFromJsonAsync<Book>();
		}
		else
		{
			throw new Exception($"Failed : {response.StatusCode}");
		}
	}

	public async Task DeleteBook(long id)
	{
		var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
		if (!response.IsSuccessStatusCode)
		{
			throw new Exception($"Failed to delete : {response.StatusCode}");
		}
	}

	public async Task EditBookById(Book book)
	{
		book.Author = null;
		var json = System.Text.Json.JsonSerializer.Serialize(book);
		//throw new Exception(json);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		var response = await _httpClient.PutAsync($"{_baseUrl}/{book.Id}/json", content);

		if (!response.IsSuccessStatusCode)
		{
			throw new Exception($"{response.StatusCode}");
			throw new Exception("Failed to edit book");
		}
	}

	public async Task<List<string>> CreateBookAsync(Book book)
	{

		var content = JsonContent.Create(book, MediaTypeHeaderValue.Parse("application/json"));
		var response = await _httpClient.PostAsync(_baseUrl, content);
		if (!response.IsSuccessStatusCode)
		{
			if (response.StatusCode == HttpStatusCode.BadRequest)
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				var errors = JsonSerializer.Deserialize<List<string>>(errorContent, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				if (errors != null && errors.Count > 0)
				{
					return errors;
				}
			}

			throw new Exception($"Failed to create book: {response.StatusCode}");
		}

		return new List<string>();
	}
}
