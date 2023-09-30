using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using WebApi.Application.BookOperations.CreateBook;
using WebApi.DBOperations;
using WebApi.Entities;
using WebApi.UnitTests.TestsSetup;
using Xunit;
using static WebApi.Application.BookOperations.CreateBook.CreateBookCommand;

namespace WebApi.UnitTests.Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommandTest : IClassFixture<CommonTestFixture> //IClassFixture Xunit'den gelen bir özellik.
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public CreateBookCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context; // common oluşturduğumuz CommonTestFixture'daki Contexti bu sınıftaki context'e atıyoruz.
            _mapper = testFixture.Mapper;
        }

        //testler genel olarak void tipindedir.
        //WhenKontrol edilecek durum_nesne_beklenen sonuc
        // [Fact] test methodu olduğunu belirtir !
        [Fact]
        public void WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldReturn()
        {
            //arrange (Hazırlık)
            var book = new Book() { Title = "Test_WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldReturn", PageCount = 100, PublishDate = new System.DateTime(1990, 01, 10), GenreId = 1, AuthorId = 1 };
            _context.Books.Add(book);
            _context.SaveChanges();

            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            command.Model = new CreateBookModel() { Title = "Test_WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldReturn" };

            //act & assert (çalıştırma - Doğrulama)
            FluentActions
                .Invoking(() => command.Handle()) // command içerisindeki 'Handle' methodunu çalıştır.
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap Zaten Mevcut!");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeCreated()
        {
            //arrange
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            CreateBookModel model = new CreateBookModel() { Title = "Hobbit", PageCount = 100, PublishDate = DateTime.Now.Date.AddYears(-10), GenreId = 1, AuthorId = 1 };
            command.Model = model;
            
            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var book = _context.Books.SingleOrDefault(book=>book.Title == model.Title);
            book.Should().NotBeNull();
            book.PageCount.Should().Be(model.PageCount); // yaratmış olduğumuz book objesinin page count'u, modelimizdeki pagecount ile aynı olmalı
            book.PublishDate.Should().Be(model.PublishDate);
            book.GenreId.Should().Be(model.GenreId);
            book.AuthorId.Should().Be(model.AuthorId);
        }
    }
}