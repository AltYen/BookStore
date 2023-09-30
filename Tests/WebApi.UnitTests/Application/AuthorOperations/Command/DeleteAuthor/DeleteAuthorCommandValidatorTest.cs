using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApi.Application.AuthorOperations.Command.DeleteAuthor;
using WebApi.DBOperations;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.AuthorOperations.Command.DeleteAuthor
{
    public class DeleteAuthorCommandValidatorTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteAuthorCommandValidatorTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Theory]
        [InlineData(-521)]
        [InlineData(0)]
        [InlineData(-2421)]
        public void WhenInvalidIdInputIsGiven_Validator_ShouldBeReturnErrors(int id)
        {
            DeleteAuthorCommand command = new DeleteAuthorCommand(null);
            command.AuthorId = id;

            DeleteAuthorCommandValidator validator = new DeleteAuthorCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidIdInputIsGiven_Validator_ShouldNotBeReturnError()
        {
            //arrange (Hazırlık)
            Random random = new Random();
            var randomAuthor = _context.Authors.OrderBy(x => random.Next()).FirstOrDefault(); // var olan bir author idsi almak için, random bir author seçme;
            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = randomAuthor.Id;

            //act
            DeleteAuthorCommandValidator validator = new DeleteAuthorCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Equals(0);
        }
    }
}