using System;
using FluentValidation;
using FluentValidation.Validators;
using WebApi.Common;

namespace  WebApi.Application.BookOperations.CreateBook
{
  public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand> // CreateBookCommand'in objelerini,nesnelerini validate eder.
  {
    public CreateBookCommandValidator(){
      RuleFor(command=> command.Model.GenreId).GreaterThan(0).LessThanOrEqualTo(Enum.GetNames(typeof(GenreEnum)).Length);;
      RuleFor(command=> command.Model.PageCount).GreaterThan(0);
      RuleFor(command=> command.Model.PublishDate.Date).NotEmpty().LessThan(DateTime.Now.Date); // bugunden daha küçük olmalı.
      RuleFor(command=> command.Model.Title).NotEmpty().MinimumLength(4);
    }
  }
}