using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApi.DBOperations;

namespace WebApi.Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorDetailQueryValidator : AbstractValidator<GetAuthorDetailQuery>
    {
        public GetAuthorDetailQueryValidator(IBookStoreDbContext context)
        {
            RuleFor(x => x.AuthorId).Must(authorId=>context.Authors.Any(a=>a.Id == authorId)).WithMessage("Geçersiz AuthorId. Lütfen mevcut bir AuthorId girin.");
        }
    }
}