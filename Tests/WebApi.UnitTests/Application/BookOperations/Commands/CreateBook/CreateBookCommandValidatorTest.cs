using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebApi.Application.BookOperations.CreateBook;
using WebApi.DBOperations;
using WebApi.UnitTests.TestsSetup;
using Xunit;

namespace WebApi.UnitTests.Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommandValidatorTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public CreateBookCommandValidatorTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        //Theory birden fazla veri için çalışması için
        [Theory]
        [InlineData("Lord Of The Rings", 0, 0, 0)]
        [InlineData("Lord Of The Rings", 0, 1, 0)]
        [InlineData("Lord Of The Rings", 100, 0, 0)]
        [InlineData("", 0, 0, 0)]
        [InlineData("", 100, 1, 0)]
        [InlineData("", 0, 0, 1)]
        [InlineData("Lor", 100, 1, 1)]
        [InlineData("Lord", 100, 0, 1)]
        [InlineData("Lord", 0, 1, 1)]
        [InlineData(" ", 100, 1, 1)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string title, int pageCount, int genreId, int authorId)
        {
            //arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookCommand.CreateBookModel()
            {
                Title = title,
                PageCount = pageCount,
                PublishDate = DateTime.Now.Date.AddYears(-1),
                GenreId = genreId,
                AuthorId = authorId
            };

            //act
            CreateBookCommandValidator validator = new CreateBookCommandValidator(_context);
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0); // doğrulama işlemi sırasında oluşan hataları içeren bir koleksiyondur. Eğer bu koleksiyonun eleman sayısı 0'dan büyükse (yani hata varsa), test başarılı kabul edilir.
        }

        [Fact]
        public void WhenDateTimeEqualNowIsGiven_Validator_ShouldBeReturnError()
        {
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookCommand.CreateBookModel()
            {
                Title = "Lord of the Rings",
                PageCount = 200,
                PublishDate = DateTime.Now.Date,
                GenreId = 1,
                AuthorId = 1
            };

            //act
            CreateBookCommandValidator validator = new CreateBookCommandValidator(_context);
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputAreGiven_Validator_ShouldNotBeReturnError()
        {
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookCommand.CreateBookModel()
            {
                Title = "Lord of the Rings",
                PageCount = 200,
                PublishDate = DateTime.Now.Date.AddYears(-2),
                GenreId = 1,
                AuthorId = 1
            };

            //act
            CreateBookCommandValidator validator = new CreateBookCommandValidator(_context);
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Equals(0); // doğrulama işlemi
            //result.Errors.Count.Should().Be(0);

        }



        // [Fact]
        // public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors()
        // {
        //     //arrange
        //     CreateBookCommand command = new CreateBookCommand(null, null);
        //     command.Model = new CreateBookCommand.CreateBookModel()
        //     {
        //         Title = "",
        //         PageCount = 0,
        //         PublishDate = DateTime.Now.Date,
        //         GenreId = 0,
        //         AuthorId = 0
        //     };

        //     //act
        //     CreateBookCommandValidator validator = new CreateBookCommandValidator(_context);
        //     var result = validator.Validate(command);

        //     //assert
        //     result.Errors.Count.Should().BeGreaterThan(0);
        // }
    }
}