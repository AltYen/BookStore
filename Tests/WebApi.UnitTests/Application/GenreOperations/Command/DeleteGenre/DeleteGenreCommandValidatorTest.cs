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
    public class DeleteGenreCommandValidatorTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteGenreCommandValidatorTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-11)]
        public void WhenInvalidIdInputIsGiven_Validator_ShouldBeReturnErrors(int id)
        {
            DeleteGenreCommand command = new DeleteGenreCommand(null);
            command.GenreId = id;

            DeleteGenreCommandValidator validator = new DeleteGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidIdInputIsGiven_Validator_ShouldNotBeReturnError()
        {
            //arrange (Hazırlık)
            Random random = new Random();
            var randomGenre = _context.Genres.OrderBy(x => random.Next()).FirstOrDefault(); // var olan bir genre idsi almak için, random bir genre seçme;
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = randomGenre.Id;

            //act
            DeleteGenreCommandValidator validator = new DeleteGenreCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Equals(0);
        }
    }
}