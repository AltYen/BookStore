using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApi.Application.GenreOperations.Commands.UpdateGenre;
using WebApi.DBOperations;
using WebApi.Entities;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.GenreOperations.Command.UpdateGenre
{
    public class UpdateGenreCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public UpdateGenreCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistGenreIdIsGiven_InvalidOperationException_ShouldReturn()
        {
            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = -1;

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap Türü Bulunamadı!");
        }

        [Fact]
        public void WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldReturn()
        {
            var genre = new Genre() { Name = "Education" };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            var alreadyExistGenre = _context.Genres.SingleOrDefault(g => g.Name == "Education");
            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = alreadyExistGenre.Id - 1;
            command.Model = new UpdateGenreModel() { Name = "Education" };

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Aynı isimli bir kitap türü zaten mevcut !");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Genre_ShouldBeUpdated()
        {
            //arrange
            Random random = new Random();
            var randomGenre = _context.Genres.OrderBy(x => random.Next()).FirstOrDefault(); // var olan bir genre idsi almak için, random bir genre seçme;
            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            UpdateGenreModel model = new UpdateGenreModel() { Name = "Theatre"};
            command.Model = model;
            command.GenreId = randomGenre.Id;

            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var genre = _context.Genres.SingleOrDefault(genre => genre.Id == randomGenre.Id);
            genre.Should().NotBeNull();
            genre.Name.Should().Be(model.Name);
        }
    }
}