
namespace LibraryWeb.Models;

public class Book
{
	public long Id { get; set; }
	private string _title;
	public string Title
	{
		get { return _title; }
		set
		{
			_title = !string.IsNullOrEmpty(value) ? char.ToUpper(value[0]) + value.Substring(1).ToLower() : value;
		}
	}
	private string _genre;
	public string Genre
	{
		get { return _genre; }
		set
		{
			_genre = !string.IsNullOrEmpty(value) ? char.ToUpper(value[0]) + value.Substring(1).ToLower() : value;
		}
	}
	public long AuthorId { get; set; }

	public string? ImageFileName { get; set; }
	public string? ImagePath { get; set; }
	public Author? Author { get; set; }



}
