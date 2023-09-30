using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApi.Application.BookOperations.DeleteBook;
using WebApi.DBOperations;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.BookOperations.Commands.DeleteBook
{
    public class DeleteBookCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteBookCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistBookIdIsGiven_InvalidOperationException_ShouldReturn()
        {
            //arrange (Hazırlık)
            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = -1;

            //act & assert (çalıştırma - Doğrulama)
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Böyle bir idye ait kitap yok");
        }

        [Fact]
        public void WhenExistBookIdIsGiven_Book_ShouldBeDeleted()
        {
            //arrange (Hazırlık)
            Random random = new Random();
            var randomBook = _context.Books.OrderBy(x => random.Next()).FirstOrDefault(); // var olan bir kitap idsi almak için, random bir kitap seçme;
            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = randomBook.Id;

            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var book = _context.Books.SingleOrDefault(book=>book.Id == command.BookId);
            book.Should().BeNull();
        }

    }
}