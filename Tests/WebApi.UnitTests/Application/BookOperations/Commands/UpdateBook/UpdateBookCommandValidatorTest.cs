using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApi.Application.BookOperations.UpdateBook;
using WebApi.DBOperations;
using WebApi.UnitTests.TestsSetup;
using Xunit;
using static WebApi.Application.BookOperations.UpdateBook.UpdateBookCommand;

namespace WebApi.UnitTests.Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandValidatorTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public UpdateBookCommandValidatorTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Theory]
        [InlineData(-1, "test1", 1, 1)] // invalid id
        [InlineData(1, "test2", 0, 1)] // invalid genreId
        [InlineData(1, "test2", 1, 0)] //invalid authorId
        [InlineData(-1, "", 0, 1)] //invalid title
        [InlineData(-1, "", 0, 0)] // all invalid
        [InlineData(1, "", 0, 1)]  //invalid title and genreId
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(int id, string title, int genreId, int authorId)
        {
            //arrange
            UpdateBookCommand command = new UpdateBookCommand(null, null);
            command.BookId = id;
            command.Model = new UpdateBookModel()
            {
                Title = title,
                GenreId = genreId,
                AuthorId = authorId
            };

            //act
            UpdateBookCommandValidator validator = new UpdateBookCommandValidator(_context);
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0); 
        }

        [Fact]
        public void WhenValidInputAreGiven_Validator_ShouldNotBeReturnError()
        {
            Random random = new Random();
            var randomBook = _context.Books.OrderBy(x => random.Next()).FirstOrDefault();
            UpdateBookCommand command = new UpdateBookCommand(null, null);
            command.Model = new UpdateBookModel()
            {
                Title = "Lord of the Rings",
                GenreId = 1,
                AuthorId = 1
            };
            command.BookId = randomBook.Id;

            //act
            UpdateBookCommandValidator validator = new UpdateBookCommandValidator(_context);
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Equals(0); // doğrulama işlemi

        }
    }
}