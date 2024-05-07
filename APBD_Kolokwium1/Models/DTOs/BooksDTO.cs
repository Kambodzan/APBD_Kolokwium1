namespace APBD_Kolokwium1.Models.DTOs;

public class BooksDTO
{
    public int id { get; set; }
    public string title { get; set; }
    public List<AuthorsDTO>? authors { get; set; }
}

public class AuthorsDTO
{
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; }
}