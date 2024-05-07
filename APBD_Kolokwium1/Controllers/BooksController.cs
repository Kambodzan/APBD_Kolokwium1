using APBD_Kolokwium1.Models.DTOs;
using APBD_Kolokwium1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Kolokwium1.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBooksRepository _booksRepository;

    public BooksController(IBooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }

    [HttpGet("{id}/authors")]
    public async Task<IActionResult> GetBooks(int id)
    {
        var test = await _booksRepository.getBooksFromDB(id);

        return Ok(test);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] BooksDTO newBook) {
        if (newBook == null || string.IsNullOrWhiteSpace(newBook.title) || !newBook.authors.Any()) {
            return BadRequest("Body is not correct");
        }


        return Created();
    }
}