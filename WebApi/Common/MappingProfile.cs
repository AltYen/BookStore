using AutoMapper;
using WebApi.Application.BookOperations.GetBooks;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;
using WebApi.Application.GenreOperations.Queries.GetGenres;
using WebApi.Entities;
using static WebApi.Application.BookOperations.CreateBook.CreateBookCommand;
using static WebApi.Application.BookOperations.GetBookDetail.GetBookDetailQuery;
using static WebApi.Application.BookOperations.UpdateBook.UpdateBookCommand;

namespace WebApi.Common
{
  public class MappingProfile : Profile // ": Profile" eklersek AutoMapper artık burayı bir config olarak görecek.
  {
    public MappingProfile()
    {
      CreateMap<CreateBookModel, Book>(); // ilk parametre source, 2. parametre targettir. CreateBookModel objesi Book objesine maplenebilir olsun.
      CreateMap<Book,BookDetailViewModel>().ForMember(dest => dest.Genre, opt=>opt.MapFrom(src=>src.Genre.Name)).ForMember(dest => dest.Author, opt=>opt.MapFrom(src=>src.Author.Name+" "+src.Author.Surname)); // Booku, BookDetailViewModel'e dönüştürür.
      CreateMap<Book,BooksViewModel>().ForMember(dest => dest.Genre, opt=>opt.MapFrom(src=>src.Genre.Name)).ForMember(dest => dest.Author, opt=>opt.MapFrom(src=>src.Author.Name+" "+src.Author.Surname)); //destination içerisindeki dest.Genreyı şu şekilde maple, source üzerindeki GenreIdyi GenreEnumdan cast ederek bu idyi genreEnuma dönüştür, sornasında GenreEnum'ın string karşığını getir.
      CreateMap<UpdateBookModel,Book>();
      
      CreateMap<Genre,GenresViewModel>(); // Genre'yi GenresViewModel'e dönüştür.
      CreateMap<Genre,GenreDetailViewModel>();
    }
  }
}