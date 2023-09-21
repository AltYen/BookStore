using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WebApi.Common;
using WebApi.DBOperations;
using WebApi.Entities;

namespace WebApi.BookOperations.GetBooks
{
  public class GetBooksQuery{

    private readonly BookStoreDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetBooksQuery(BookStoreDbContext dbContext,IMapper mapper){
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public List<BooksViewModel> Handle(){
      var bookList = _dbContext.Books.OrderBy(x => x.Id).ToList<Book>();
      
      //with mapper

      List<BooksViewModel> vm = _mapper.Map<List<BooksViewModel>>(bookList); // bookList'i BooksViewModel'e dönüştür

      //without mapper
      // List<BooksViewModel> vm = new List<BooksViewModel>();
      // foreach(var book in bookList)
      // {
      //   vm.Add(new BooksViewModel(){
      //     Title = book.Title,
      //     Genre = ((GenreEnum)book.GenreId).ToString(),
      //     PublishDate = book.PublishDate.Date.ToString("dd/MM/yyy"),
      //     PageCount = book.PageCount,
      //   });
      // }
      return vm;
    }


  }

  public class BooksViewModel
  {
    // UI'ya dönecek model.
    public string Title { get; set;}
    public int PageCount { get; set;}
    public string PublishDate { get; set;} 
    public string Genre { get; set;}

  }
}