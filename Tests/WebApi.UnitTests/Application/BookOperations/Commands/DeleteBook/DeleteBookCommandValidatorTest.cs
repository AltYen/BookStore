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
    public class DeleteBookCommandValidatorTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteBookCommandValidatorTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }
        
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-4242)]
        public void WhenInvalidIdInputIsGiven_Validator_ShouldBeReturnErrors(int id)
        {
            DeleteBookCommand command = new DeleteBookCommand(null);
            command.BookId = id;

            DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidIdInputIsGiven_Validator_ShouldNotBeReturnError()
        {
            //arrange (Hazırlık)
            Random random = new Random();
            var randomBook = _context.Books.OrderBy(x => random.Next()).FirstOrDefault(); // var olan bir kitap idsi almak için, random bir kitap seçme;
            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = randomBook.Id;



            //act
            DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Equals(0);
        }
    }
}