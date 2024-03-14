using LibraryWeb.Models;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryWeb.Services
{
	public class AuthorService
	{
		private readonly HttpClient _httpClient;
		private readonly string _baseUrl = "https://localhost:7003/api/Author";

		public AuthorService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<Author>> getAuthorsAsync()
		{
			var response = await _httpClient.GetAsync(_baseUrl);
			return await response.Content.ReadFromJsonAsync<List<Author>>();
		}

		public async Task<List<string>> CreateAuthorAsync(Author author)
		{
			var content = JsonContent.Create(author, MediaTypeHeaderValue.Parse("application/json"));
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

				throw new Exception($"Failed to create author: {response.StatusCode}");
			}


			return new List<string>();
		}
		public async Task<Author> GetAuthorByIdAsync(long Id)
		{
			var response = await _httpClient.GetAsync($"{_baseUrl}/{Id}");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<Author>();
			}
			else
			{
				throw new Exception($"Failed : {response.StatusCode}");
			}
		}

		public async Task DeleteAuthor(long id)
		{
			var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"Could not delete {id} author");
			}
		}

		public async Task EditAuthorById(Author author)
		{
			author.Books = null;
			var json = System.Text.Json.JsonSerializer.Serialize(author);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync($"{_baseUrl}/{author.Id}/json", content);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"{response.StatusCode}");
				throw new Exception($"Failed to update author with Id: {author.Id}");
			}
		}
	}
}
