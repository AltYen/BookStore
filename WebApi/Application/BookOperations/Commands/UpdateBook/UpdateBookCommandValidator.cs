using System;
using System.Data;
using System.Linq;
using FluentValidation;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.UpdateBook
{
  public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
  {
    public UpdateBookCommandValidator(IBookStoreDbContext context)
    {
      RuleFor(command=>command.BookId).GreaterThan(0);
      RuleFor(command=>command.Model.Title).NotEmpty().MinimumLength(4);
      RuleFor(command=>command.Model.GenreId).Must(genreId => context.Genres.Any(g => g.Id == genreId)).WithMessage("Geçersiz GenreId. Lütfen mevcut bir GenreId girin.");
      RuleFor(command => command.Model.AuthorId).Must(authorId => context.Authors.Any(a => a.Id == authorId)).WithMessage("Geçersiz AuthorId. Lütfen mevcut bir AuthorId girin.");
    }
  }
}