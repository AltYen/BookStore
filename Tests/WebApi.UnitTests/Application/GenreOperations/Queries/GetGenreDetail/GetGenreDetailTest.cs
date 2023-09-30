using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;
using WebApi.DBOperations;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.GenreOperations.Queries.GetGenreDetail
{
    public class GetGenreDetailTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetGenreDetailTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistGenreIdIsGiven_InvalidOperationException_ShouldReturn()
        {
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            query.GenreId = -1;

            FluentActions
                .Invoking(() => query.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap türü bulunamadı!");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Genre_ShouldBeBrought()
        {
            //arrange
            Random random = new Random();
            var randomGenre = _context.Genres.OrderBy(x => random.Next()).FirstOrDefault(); // var olan bir genre idsi almak için, random bir genre seçme;
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            query.GenreId = randomGenre.Id;
            
            //act
            FluentActions.Invoking(() => query.Handle()).Invoke();

            //assert
            var genre = _context.Genres.SingleOrDefault(genre=>genre.Id == randomGenre.Id);
            genre.Should().NotBeNull();
            genre.Name.Should().Be(randomGenre.Name);
        }
    }
}