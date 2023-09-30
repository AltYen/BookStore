using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApi.Application.GenreOperations.Commands.CreateGenre;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.GenreOperations.Command.CreateGenre
{
    public class CreateGenreCommandValidatorTest : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("Tes")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string name)
        {
            //arrange
            CreateGenreCommand command = new CreateGenreCommand(null);
            command.Model = new CreateGenreModel()
            {
                Name = name
            };

            //act
            CreateGenreCommandValidator validator = new CreateGenreCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("Love")]
        [InlineData("History")]
        [InlineData("Literature")]
        public void WhenValidInputAreGiven_Validator_ShouldNotBeReturnError(string name)
        {
            CreateGenreCommand command = new CreateGenreCommand(null);
            command.Model = new CreateGenreModel()
            {
                Name = name
            };

            //act
            CreateGenreCommandValidator validator = new CreateGenreCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Equals(0);
        }
    }
}