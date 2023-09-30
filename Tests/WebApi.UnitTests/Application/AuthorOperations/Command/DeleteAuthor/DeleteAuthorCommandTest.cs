using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApi.Application.AuthorOperations.Command.DeleteAuthor;
using WebApi.DBOperations;
using WebApi.Entities;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.AuthorOperations.Command.DeleteAuthor
{
    public class DeleteAuthorCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteAuthorCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistAuthorIdIsGiven_InvalidOperationException_ShouldReturn()
        {
            //arrange (Hazırlık)
            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = -1;

            //act & assert (çalıştırma - Doğrulama)
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar Bulunamadı!");
        }

        [Fact]
        public void WhenTheAuthorStillHasTheBook_InvalidOperationException_ShouldReturn()
        {
            var author = new Author() { Name = "Richard", Surname = "Bach", BirthDate = new DateTime(1936, 06, 23) };
            _context.Authors.Add(author);
            _context.SaveChanges();

            var authorId = _context.Authors.SingleOrDefault(a => a.Name == author.Name && a.Surname == author.Surname).Id;

            var book = new Book() { Title = "Martı", PageCount = 147, PublishDate = DateTime.Now.Date.AddYears(-50), GenreId = 1, AuthorId = authorId };
            _context.Books.Add(book);
            _context.SaveChanges();

            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = authorId;

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazarı silmeden önce yazarın kitaplarını silmelisiniz!");
        }

        [Fact]
        public void WhenAuthorWhoExistAndHasNoBooks_Author_ShouldBeDeleted()
        {
            //arrange (Hazırlık)
            var author = new Author() { Name = "Antoine", Surname = "Saint-Exupéry", BirthDate = new DateTime(1900, 06, 29) };
            _context.Authors.Add(author);
            _context.SaveChanges();

            var authorId = _context.Authors.SingleOrDefault(a => a.Name == author.Name && a.Surname == author.Surname).Id;

            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = authorId;

            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var book = _context.Books.SingleOrDefault(author => author.Id == authorId);
            book.Should().BeNull();
        }
    }
}