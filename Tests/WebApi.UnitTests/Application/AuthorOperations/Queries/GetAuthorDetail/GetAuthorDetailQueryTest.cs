using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using WebApi.Application.AuthorOperations.Queries.GetAuthorDetail;
using WebApi.DBOperations;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorDetailQueryTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetAuthorDetailQueryTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistAuthorIdIsGiven_InvalidOperationException_ShouldReturn()
        {
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, _mapper);
            query.AuthorId = -1;

            FluentActions
                .Invoking(() => query.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar Bulunamadı!");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Author_ShouldBeBrought()
        {
            //arrange
            Random random = new Random();
            var randomAuthor = _context.Authors.OrderBy(x => random.Next()).FirstOrDefault();
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, _mapper);
            query.AuthorId = randomAuthor.Id;

            //act
            FluentActions.Invoking(() => query.Handle()).Invoke();

            //assert
            var author = _context.Authors.SingleOrDefault(author => author.Id == randomAuthor.Id);
            author.Should().NotBeNull();
            author.Name.Should().Be(randomAuthor.Name);
            author.Surname.Should().Be(randomAuthor.Surname);
        }
    }
}