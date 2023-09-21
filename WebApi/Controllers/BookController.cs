using System;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application;
using WebApi.Application.BookOperations.CreateBook;
using WebApi.Application.BookOperations.DeleteBook;
using WebApi.Application.BookOperations.GetBookDetail;
using WebApi.Application.BookOperations.GetBooks;
using WebApi.Application.BookOperations.UpdateBook;
using WebApi.DBOperations;
using static WebApi.Application.BookOperations.CreateBook.CreateBookCommand;
using static WebApi.Application.BookOperations.GetBookDetail.GetBookDetailQuery;
using static WebApi.Application.BookOperations.UpdateBook.UpdateBookCommand;

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
      GetBooksQuery query = new GetBooksQuery(_context, _mapper);
      var result = query.Handle();
      return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult getById(int id)
    {
      BookDetailViewModel result;

      GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
      GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();
      query.BookId = id;
      validator.ValidateAndThrow(query);
      result = query.Handle();

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
      CreateBookCommand command = new CreateBookCommand(_context, _mapper);

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

      return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult updateBook(int id, [FromBody] UpdateBookModel updatedBook)
    {

      UpdateBookCommand command = new UpdateBookCommand(_context, _mapper);
      UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
      command.BookId = id;
      command.Model = updatedBook;
      validator.ValidateAndThrow(command);
      command.Handle();

      return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult deleteBook(int id)
    {

      DeleteBookCommand command = new DeleteBookCommand(_context);
      DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
      command.BookId = id;
      validator.ValidateAndThrow(command);
      command.Handle();

      return Ok();

    }
  }


}