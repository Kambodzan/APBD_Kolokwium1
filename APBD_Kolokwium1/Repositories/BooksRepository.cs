using System.Data.SqlClient;
using APBD_Kolokwium1.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Kolokwium1.Repositories;

public class BooksRepository : IBooksRepository
{
    private readonly IConfiguration _configuration;

    public BooksRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<BooksDTO> getBooksFromDB(int id)
    {
        var getBooksQuery = $"select b.PK as id, b.title from books b where PK = @ID;";
        var getAuthorsQuery =
            $"select a.first_name as firstName, a.last_name as lastName from authors a join books_authors ba on a.PK = ba.FK_author where ba.FK_book = @ID;";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        
        await connection.OpenAsync();
        
        command.CommandText = getAuthorsQuery;
        command.Parameters.AddWithValue("@ID", id);
        var authorsReader = await command.ExecuteReaderAsync();
        List<AuthorsDTO> allAuthors = new List<AuthorsDTO>();
        while (authorsReader.Read())
        {
            allAuthors.Add(new AuthorsDTO()
            {
                firstName = authorsReader.GetString(authorsReader.GetOrdinal("firstName")),
                lastName = authorsReader.GetString(authorsReader.GetOrdinal("lastName"))
            });
        }
        authorsReader.Close();
        
        command.Parameters.Clear();
        command.CommandText = getBooksQuery;
        command.Parameters.AddWithValue("@ID", id);
    
        var booksReader = await command.ExecuteReaderAsync();
        BooksDTO booksDto = new BooksDTO();
        if (booksReader.Read())
        {
            booksDto.id = booksReader.GetInt32(booksReader.GetOrdinal("id"));
            booksDto.title = booksReader.GetString(booksReader.GetOrdinal("title"));
            booksDto.authors = allAuthors;
        }
        booksReader.Close();
        
        connection.Close();

        return booksDto;
    }

    
    public async Task<int> createNewBook(BooksDTO booksDto)
    {
        var insertNewBook = $"insert into books(title) values ('@TITLE');";
        var selectNewBook = $"select PK from books where title = '@TITLE';";

        var insertNewAuthor = $"insert into authors(first_name, last_name) values ('@FIRSTNAME', '@LASTNAME');";
        var selectNewAuthor = $"select PK from authors where first_name = '@FIRSTNAME' and last_name = '@LASTNAME';";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        
        await connection.OpenAsync();
        
        // command.Parameters.AddWithValue("@TITLE", booksDto.title);
        
        var transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction as SqlTransaction;
	    
        try
        {
            // Cienko z czasem :(
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

        return 0;
    }
}