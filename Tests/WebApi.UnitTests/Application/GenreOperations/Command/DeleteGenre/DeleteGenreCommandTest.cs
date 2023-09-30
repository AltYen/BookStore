using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApi.Application.GenreOperations.Commands.DeleteGenre;
using WebApi.DBOperations;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.GenreOperations.Command.DeleteGenre
{
    public class DeleteGenreCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteGenreCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistBookIdIsGiven_InvalidOperationException_ShouldReturn()
        {
            //arrange (Hazırlık)
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = -1;

            //act & assert (çalıştırma - Doğrulama)
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap Türü Bulunamadı!");
        }

        [Fact]
        public void WhenExistBookIdIsGiven_Genre_ShouldBeDeleted()
        {
            //arrange (Hazırlık)
            Random random = new Random();
            var randomGenre = _context.Genres.OrderBy(x => random.Next()).FirstOrDefault(); // var olan bir genre idsi almak için random bir genre seçme;
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = randomGenre.Id;

            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var book = _context.Genres.SingleOrDefault(genre => genre.Id == command.GenreId);
            book.Should().BeNull();
        }

    }
}