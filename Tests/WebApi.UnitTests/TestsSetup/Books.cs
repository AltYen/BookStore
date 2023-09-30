using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DBOperations;
using WebApi.Entities;

namespace WebApi.UnitTests.TestsSetup
{
    //static classların altında statik method ve proplar bulunabilir.
    public static class Books
    {
        // fake inmemory db için fake book verisi oluşturma;
        public static void AddBooks(this BookStoreDbContext context)
        {
            context.Books.AddRange(new Book
            {
                Title = "Lean Startup",
                GenreId = 1,
                AuthorId = 3,
                PageCount = 200,
                PublishDate = new DateTime(2001, 06, 12)
            },
            new Book
            {
                Title = "Herland",
                GenreId = 2,
                AuthorId = 2,
                PageCount = 250,
                PublishDate = new DateTime(2010, 05, 23)
            },
            new Book
            {
                Title = "Dune",
                GenreId = 2,
                AuthorId = 1,
                PageCount = 540,
                PublishDate = new DateTime(2001, 12, 21)
            }
          );
        }
    }
}