using AutoMapper;
using WebApi.BookOperations.GetBooks;
using WebApi.Entities;
using static WebApi.BookOperations.CreateBook.CreateBookCommand;
using static WebApi.BookOperations.GetBookDetail.GetBookDetailQuery;
using static WebApi.BookOperations.UpdateBook.UpdateBookCommand;

namespace WebApi.Common
{
  public class MappingProfile : Profile // ": Profile" eklersek AutoMapper artık burayı bir config olarak görecek.
  {
    public MappingProfile()
    {
      CreateMap<CreateBookModel, Book>(); // ilk parametre source, 2. parametre targettir. CreateBookModel objesi Book objesine maplenebilir olsun.
      CreateMap<Book,BookDetailViewModel>().ForMember(dest => dest.Genre, opt=>opt.MapFrom(src=>((GenreEnum)src.GenreId).ToString())); // Booku, BookDetailViewModel'e dönüştürür.
      CreateMap<Book,BooksViewModel>().ForMember(dest => dest.Genre, opt=>opt.MapFrom(src=>((GenreEnum)src.GenreId).ToString())); //destination içerisindeki dest.Genreyı şu şekilde maple, source üzerindeki GenreIdyi GenreEnumdan cast ederek bu idyi genreEnuma dönüştür, sornasında GenreEnum'ın string karşığını getir.
      CreateMap<UpdateBookModel,Book>();
    }
  }
}