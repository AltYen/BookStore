using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.GetBookDetail
{
  public class GetBookDetailQuery
  {
    public int BookId { get; set; }
    private readonly IBookStoreDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetBookDetailQuery(IBookStoreDbContext dbContext,IMapper mapper){
      _dbContext = dbContext; 
      _mapper = mapper;
    }

    public BookDetailViewModel Handle(){
      var book = _dbContext.Books.Include(x=>x.Genre).Include(x=>x.Author).Where(book=> book.Id == BookId).SingleOrDefault();
      if(book is null){
        throw new InvalidOperationException("Bu Id'ye ait Kitap Bulunamadı");
      }

      //withmapper
      BookDetailViewModel vm = _mapper.Map<BookDetailViewModel>(book);

      //without mapper
      // BookDetailViewModel vm = new BookDetailViewModel();
      // vm.Title = book.Title;
      // vm.PageCount = book.PageCount;
      // vm.PublishDate = book.PublishDate.Date.ToString("dd/MM/yyyy");
      // vm.Genre = ((GenreEnum)book.GenreId).ToString();
      return vm;
    }

    public class BookDetailViewModel
    {
      public string Title { get; set;}
      public int PageCount { get; set; }
      public string Genre { get; set; }
      public string Author {get;set;}
      public string PublishDate { get; set; }
    }
  }
}