using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using WebApi.Application.BookOperations.GetBookDetail;
using WebApi.DBOperations;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.BookOperations.Queries.GetBookDetail
{
    public class GetBookDetailQueryTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetBookDetailQueryTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistBookIdIsGiven_InvalidOperationException_ShouldReturn()
        {
            GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
            query.BookId = -1;

            FluentActions
                .Invoking(() => query.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Bu Id'ye ait Kitap Bulunamadı");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeBrought()
        {
            //arrange
            Random random = new Random();
            var randomBook = _context.Books.OrderBy(x => random.Next()).FirstOrDefault(); // var olan bir kitap idsi almak için, random bir kitap seçme;
            GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
            query.BookId = randomBook.Id;
            
            //act
            FluentActions.Invoking(() => query.Handle()).Invoke();

            //assert
            var book = _context.Books.SingleOrDefault(book=>book.Id == randomBook.Id);
            book.Should().NotBeNull();
            book.PageCount.Should().Be(randomBook.PageCount);
            book.PublishDate.Should().Be(randomBook.PublishDate);
            book.Title.Should().Be(randomBook.Title);
            book.GenreId.Should().Be(randomBook.GenreId);
            book.AuthorId.Should().Be(randomBook.AuthorId);
        }
    }
}