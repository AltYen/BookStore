using System;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
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
    private readonly IMapper _mapper;
    public BookController(BookStoreDbContext context, IMapper mapper)
    {
        _context = context; // inject edilen instance'i atadık.
        _mapper = mapper;
    }

        [HttpGet]
    public IActionResult getBooks()
    {
      GetBooksQuery query = new GetBooksQuery(_context,_mapper);
      var result = query.Handle();
      return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult getById(int id)
    {
      BookDetailViewModel result;
      try
      {
        GetBookDetailQuery query = new GetBookDetailQuery(_context,_mapper);
        GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();
        query.BookId = id;
        validator.ValidateAndThrow(query);
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
      CreateBookCommand command = new CreateBookCommand(_context,_mapper);
      try
      {
        command.Model = newBook;
        CreateBookCommandValidator validator = new CreateBookCommandValidator();
        validator.ValidateAndThrow(command);
        command.Handle();

        // if(!result.IsValid)
        // {
        //   foreach(var item in result.Errors){
        //     Console.WriteLine("Property : " + item.PropertyName + " - Error Message : " + item.ErrorMessage);
        //   }
        // }else{
        //   command.Handle();
        // }
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
        UpdateBookCommand command = new UpdateBookCommand(_context,_mapper);
        UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
        command.BookId = id;
        command.Model = updatedBook;
        validator.ValidateAndThrow(command);
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
        DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
        command.BookId = id;
        validator.ValidateAndThrow(command);
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