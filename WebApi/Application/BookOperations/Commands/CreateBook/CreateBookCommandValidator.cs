using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.CreateBook
{
  public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand> // CreateBookCommand'in objelerini,nesnelerini validate eder.
  {
    public CreateBookCommandValidator(IBookStoreDbContext context)
    {
      RuleFor(command => command.Model.GenreId).Must(genreId => context.Genres.Any(g => g.Id == genreId)).WithMessage("Geçersiz GenreId. Lütfen mevcut bir GenreId girin.");
      RuleFor(command => command.Model.AuthorId).Must(authorId => context.Authors.Any(a => a.Id == authorId)).WithMessage("Geçersiz AuthorId. Lütfen mevcut bir AuthorId girin.");
      RuleFor(command => command.Model.PageCount).GreaterThan(0);
      RuleFor(command => command.Model.PublishDate.Date).NotEmpty().LessThan(DateTime.Now.Date); // bugunden daha küçük olmalı.
      RuleFor(command => command.Model.Title).NotEmpty().MinimumLength(4);

    }
  }
}