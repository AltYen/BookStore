using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;
using WebApi.DBOperations;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.GenreOperations.Queries.GetGenreDetail
{
    public class GetGenreDetailValidatorTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public GetGenreDetailValidatorTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-512321)]
        public void WhenInvalidIdInputIsGiven_Validator_ShouldBeReturnErrors(int id)
        {
            GetGenreDetailQuery command = new GetGenreDetailQuery(null, null);
            command.GenreId = id;

            GetGenreDetailQueryValidator validator = new GetGenreDetailQueryValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidIdInputIsGiven_Validator_ShouldNotBeReturnError()
        {
            //arrange (Hazırlık)
            Random random = new Random();
            var randomGenre = _context.Genres.OrderBy(x => random.Next()).FirstOrDefault();
            GetGenreDetailQuery command = new GetGenreDetailQuery(null, null);
            command.GenreId = randomGenre.Id;

            //act
            GetGenreDetailQueryValidator validator = new GetGenreDetailQueryValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Equals(0);
        }
    }
}