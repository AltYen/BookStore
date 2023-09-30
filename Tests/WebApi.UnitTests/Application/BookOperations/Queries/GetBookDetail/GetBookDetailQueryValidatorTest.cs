using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApi.Application.BookOperations.GetBookDetail;
using WebApi.DBOperations;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.BookOperations.Queries.GetBookDetail
{
    public class GetBookDetailQueryValidatorTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public GetBookDetailQueryValidatorTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-4242)]
        public void WhenInvalidIdInputIsGiven_Validator_ShouldBeReturnErrors(int id)
        {
            GetBookDetailQuery command = new GetBookDetailQuery(null, null);
            command.BookId = id;

            GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidIdInputIsGiven_Validator_ShouldNotBeReturnError()
        {
            //arrange (Hazırlık)
            Random random = new Random();
            var randomBook = _context.Books.OrderBy(x => random.Next()).FirstOrDefault();
            GetBookDetailQuery command = new GetBookDetailQuery(null, null);
            command.BookId = randomBook.Id;

            //act
            GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Equals(0);
        }
    }
}