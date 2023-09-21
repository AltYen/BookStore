using System;
using System.Linq;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.DeleteBook
{
  public class DeleteBookCommand
  {
    public int BookId { get; set; }
    private readonly BookStoreDbContext _dbContext;

    public DeleteBookCommand(BookStoreDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public void Handle(){
       var book = _dbContext.Books.SingleOrDefault(x=>x.Id == BookId);
       if(book is null)
         throw new InvalidOperationException("Böyle bir idye ait kitap yok");

       _dbContext.Books.Remove(book); //Remove kendi tipinde yani bir book nesnesi alır.
       _dbContext.SaveChanges();
    }
  }
}