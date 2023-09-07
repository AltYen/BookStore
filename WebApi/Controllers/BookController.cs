using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.AddControllers
{

  [ApiController]
  [Route("[controller]s")]
  public class BookController : ControllerBase
  {
    private static List<Book> BookList = new List<Book>
     {
        new Book {
          Id = 1,
          Title = "Lean Startup",
          GenreId = 1, // Personel Growth,
          PageCount = 200,
          PublishDate = new DateTime(2001,06,12)
        },
        new Book {
          Id = 2,
          Title = "Herland",
          GenreId = 2, // Science Fiction,
          PageCount = 250,
          PublishDate = new DateTime(2010,05,23)
        },
        new Book {
          Id = 3,
          Title = "Dune",
          GenreId = 2, // Personel Growth,
          PageCount = 540,
          PublishDate = new DateTime(2001,12,21)
        }
     };

    [HttpGet]
    public List<Book> getBooks()
    {
      var bookList = BookList.OrderBy(x => x.Id).ToList<Book>(); // OrderBy LINQ yapısı sql sorgusuna yakin sorgular ile entityi yönetmeyi yarar. Bu yapıda idye göre sıralar.
      return bookList;
    }

    [HttpGet("{id}")]
    public Book getById(int id)
    {
      var book = BookList.Where(book=> book.Id == id).SingleOrDefault();
      return book;
    }

    // [HttpGet]
    // public Book get([FromQuery] string id)
    // {
    //   var book = BookList.Where(book=> book.Id == Convert.ToInt32(id)).SingleOrDefault();
    //   return book;
    // }

    [HttpPost]
    public IActionResult addBook([FromBody] Book newBook)
    {
      var book = BookList.SingleOrDefault(x=>x.Title == newBook.Title);
      if(book is not null)
        return BadRequest();
      
      BookList.Add(newBook);

      return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult updateBook(int id, [FromBody] Book updatedBook)
    {
      var book = BookList.SingleOrDefault(x=> x.Id == id);
      if(book is null)
        return BadRequest();
      
      book.GenreId = updatedBook.GenreId != default ? updatedBook.GenreId : book.GenreId;
      book.PageCount = updatedBook.PageCount != default ? updatedBook.PageCount : book.PageCount;
      book.PublishDate = updatedBook.PublishDate != default ? updatedBook.PublishDate : book.PublishDate;
      book.Title = updatedBook.Title != default ? updatedBook.Title : book.Title;
      
      return Ok();
    }
  }


}