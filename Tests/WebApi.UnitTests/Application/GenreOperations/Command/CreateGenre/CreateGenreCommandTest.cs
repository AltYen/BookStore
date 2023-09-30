using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using WebApi.Application.GenreOperations.Commands.CreateGenre;
using WebApi.DBOperations;
using WebApi.Entities;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.GenreOperations.Command.CreateGenre
{
    public class CreateGenreCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public CreateGenreCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAlreadyExistGenreIsGiven_InvalidOperationException_ShouldReturn()
        {
            //arrange (Hazırlık)
            var genre = new Genre() { Name = "Psychology" };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            CreateGenreCommand command = new CreateGenreCommand(_context);
            command.Model = new CreateGenreModel() { Name = "Psychology" };

            //act & assert (çalıştırma - Doğrulama)
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap Türü Zaten Mevcut!");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Genre_ShouldBeCreated()
        {
            //arrange
            CreateGenreCommand command = new CreateGenreCommand(_context);
            CreateGenreModel model = new CreateGenreModel() { Name = "Biography" };
            command.Model = model;
            
            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var genre = _context.Genres.SingleOrDefault(genre=>genre.Name == model.Name);
            genre.Should().NotBeNull();
            genre.Name.Should().Be(model.Name);
        }
    }
}