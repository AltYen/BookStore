using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using WebApi.Application.AuthorOperations.Command.UpdateAuthor;
using WebApi.DBOperations;
using WebApi.Entities;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.AuthorOperations.Command.UpdateAuthor
{
    public class UpdateAuthorCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public UpdateAuthorCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistAuthorIdIsGiven_InvalidOperationException_ShouldReturn()
        {
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
            command.AuthorId = -1;

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar Bulunamadı!");
        }

        [Fact]
        public void WhenAlreadyExistAuthorIsGiven_InvalidOperationException_ShouldReturn()
        {
            var author = new Author() { Name = "Victor", Surname = "Hugo", BirthDate = new DateTime(1802, 02, 26) };
            _context.Authors.Add(author);
            _context.SaveChanges();

            var alreadyExistAuthor = _context.Authors.SingleOrDefault(a => a.Name == "Victor" && a.Surname == "Hugo");
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
            command.AuthorId = alreadyExistAuthor.Id - 1;
            command.Model = new UpdateAuthorModel() { Name = "Victor", Surname = "Hugo", BirthDate = new DateTime(1802, 02, 26) };

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar zaten mevcut !");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Author_ShouldBeUpdated()
        {
            //arrange
            Random random = new Random();
            var randomAuthor = _context.Authors.OrderBy(x => random.Next()).FirstOrDefault(); // var olan bir author idsi almak için, random bir author seçme;
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
            UpdateAuthorModel model = new UpdateAuthorModel() { Name = "Hans", Surname = "Andersen" };
            command.Model = model;
            command.AuthorId = randomAuthor.Id;

            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var author = _context.Authors.SingleOrDefault(author => author.Id == randomAuthor.Id);
            author.Should().NotBeNull();
            author.Name.Should().Be(model.Name);
            author.Surname.Should().Be(model.Surname);
        }
    }
}