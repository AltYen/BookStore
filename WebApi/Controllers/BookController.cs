using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.BookOperations.CreateBook;
using WebApi.BookOperations.DeleteBook;
using WebApi.BookOperations.GetBookDetail;
using WebApi.BookOperations.GetBooks;
using WebApi.BookOperations.UpdateBook;
using WebApi.DBOperations;
using static WebApi.BookOperations.CreateBook.CreateBookCommand;
using static WebApi.BookOperations.GetBookDetail.GetBookDetailQuery;
using static WebApi.BookOperations.UpdateBook.UpdateBookCommand;

namespace WebApi.AddControllers
{

  [ApiController]
  [Route("[controller]s")]
  public class BookController : ControllerBase
  {
    //readonly dosyalar sadece constructor içerisinde set edilebilirler.
    private readonly BookStoreDbContext _context;
    public BookController (BookStoreDbContext context){
      _context = context; // inject edilen instance'i atadık.
    }  

    [HttpGet]
    public IActionResult getBooks()
    {
      GetBooksQuery query = new GetBooksQuery(_context);
      var result = query.Handle();
      return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult getById(int id)
    {
      BookDetailViewModel result;
      try
      {
        GetBookDetailQuery query = new GetBookDetailQuery(_context);
        query.BookId = id;
        result=query.Handle();
      }
      catch(Exception ex)
      {
        return BadRequest(ex.Message);
      }
      return Ok(result);
    }

    // [HttpGet]
    // public Book get([FromQuery] string id)
    // {
    //   var book = BookList.Where(book=> book.Id == Convert.ToInt32(id)).SingleOrDefault();
    //   return book;
    // }

    [HttpPost]
    public IActionResult addBook([FromBody] CreateBookModel newBook)
    {
      CreateBookCommand command = new CreateBookCommand(_context);
      try
      {
        command.Model = newBook;
        command.Handle();
      }
      catch(Exception ex)
      {
        return BadRequest(ex.Message);
      }
     
      return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult updateBook(int id, [FromBody] UpdateBookModel updatedBook)
    {
      try
      {
        UpdateBookCommand command = new UpdateBookCommand(_context);
        command.BookId = id;
        command.Model = updatedBook;
        command.Handle();
      }catch(Exception ex){
        return BadRequest(ex.Message);
      }
      return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult deleteBook(int id){
      try
      {
        DeleteBookCommand command = new DeleteBookCommand(_context);
        command.BookId = id;
        command.Handle();
      }
      catch(Exception ex)
      {
        return BadRequest(ex.Message);
      }
     
      return Ok();

    }
  }


}