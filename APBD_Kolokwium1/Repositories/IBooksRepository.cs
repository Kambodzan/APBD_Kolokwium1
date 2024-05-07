using APBD_Kolokwium1.Models.DTOs;

namespace APBD_Kolokwium1.Repositories;

public interface IBooksRepository
{
    Task<BooksDTO> getBooksFromDB(int id);

    Task<int> createNewBook(BooksDTO booksDto);
}