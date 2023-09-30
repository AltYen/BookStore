using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using WebApi.Application.BookOperations.UpdateBook;
using WebApi.DBOperations;
using WebApi.UnitTests.TestsSetup;
using Xunit;
using static WebApi.Application.BookOperations.UpdateBook.UpdateBookCommand;

namespace WebApi.UnitTests.Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public UpdateBookCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context; // common oluşturduğumuz CommonTestFixture'daki Contexti bu sınıftaki context'e atıyoruz.
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistBookIdIsGiven_InvalidOperationException_ShouldReturn()
        {
            UpdateBookCommand command = new UpdateBookCommand(_context,_mapper);
            command.BookId = -1;

            FluentActions
                .Invoking(() => command.Handle()) // command içerisindeki 'Handle' methodunu çalıştır.
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Böyle Bir ID'ye sahip kitap yok!");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeUpdated()
        {
            //arrange
            Random random = new Random();
            var randomBook = _context.Books.OrderBy(x => random.Next()).FirstOrDefault(); // var olan bir kitap idsi almak için, random bir kitap seçme;
            UpdateBookCommand command = new UpdateBookCommand(_context, _mapper);
            UpdateBookModel model = new UpdateBookModel() { Title = "Hobbit 2",  AuthorId = 2, GenreId = 2 };
            command.Model = model;
            command.BookId = randomBook.Id;
            
            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var book = _context.Books.SingleOrDefault(book=>book.Id == randomBook.Id);
            book.Should().NotBeNull();
            book.Title.Should().Be(model.Title);
            book.GenreId.Should().Be(model.GenreId);
            book.AuthorId.Should().Be(model.AuthorId);
        }

    }
}