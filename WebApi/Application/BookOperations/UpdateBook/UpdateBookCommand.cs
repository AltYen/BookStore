using System;
using System.Linq;
using AutoMapper;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.UpdateBook{
  public class UpdateBookCommand{
    public int BookId { get; set; }
    public UpdateBookModel Model { get; set; }

    private readonly BookStoreDbContext _dbContext;

    private readonly IMapper _mapper;

    public UpdateBookCommand(BookStoreDbContext dbContext, IMapper mapper){
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public void Handle(){
      var book = _dbContext.Books.SingleOrDefault(x=> x.Id == BookId);
      if(book is null)
        throw new InvalidOperationException("Böyle Bir ID'ye sahip kitap yok!");
      
      //withmapper
      book = _mapper.Map(Model,book);
      _dbContext.SaveChanges();

      // withoutmapper
      // book.Title = Model.Title != default ? Model.Title : book.Title;
      // book.GenreId = Model.GenreId != default ? Model.GenreId : book.GenreId;
      // book.PageCount = Model.PageCount != default ? Model.PageCount : book.PageCount;
      // book.PublishDate = Model.PublishDate != default ? Model.PublishDate : book.PublishDate;
       _dbContext.SaveChanges();
    }

    public class UpdateBookModel
    {
      public string Title { get; set;}
      public int GenreId{ get; set;}
      // public int PageCount{ get; set;}
      // public DateTime PublishDate{ get; set;}
    }

  }
}