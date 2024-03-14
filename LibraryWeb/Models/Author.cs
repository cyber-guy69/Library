
namespace LibraryWeb.Models
{
	public class Author
	{
		public long Id { get; set; }
		private string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				_name = !string.IsNullOrEmpty(value) ? char.ToUpper(value[0]) + value.Substring(1).ToLower() : value;
			}
		}
		private string _surname;
		public string? Surname
		{
			get { return _surname; }
			set
			{
				_surname = !string.IsNullOrEmpty(value) ? char.ToUpper(value[0]) + value.Substring(1).ToLower() : value;
			}
		}
		public ICollection<Book>? Books { get; set; }
	}
}
