using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public BooksController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets a list of all books.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Book>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        return await _dbContext.Books.ToListAsync();
    }

    /// <summary>
    /// Gets a specific book by ID.
    /// </summary>
    /// <param name="id">The ID of the book.</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await _dbContext.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }

    /// <summary>
    /// Creates a new book.
    /// </summary>
    /// <param name="book">The book to create.</param>
    [HttpPost]
    [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
    public async Task<ActionResult<Book>> CreateBook(Book book)
    {
        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    /// <summary>
    /// Updates an existing book by ID.
    /// </summary>
    /// <param name="id">The ID of the book to update.</param>
    /// <param name="book">The updated book data.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBook(int id, Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(book).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_dbContext.Books.Any(b => b.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a book by ID.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _dbContext.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
