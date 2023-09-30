using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DBOperations;
using WebApi.Entities;

namespace WebApi.UnitTests.TestsSetup
{
    public static class Authors
    {
        // fake inmemory db için fake author verisi oluşturma;
        public static void AddAuthors(this BookStoreDbContext context)
        {
            context.Authors.AddRange(
            new Author
            {
                Name = "Frank",
                Surname = "Herbert",
                BirthDate = new DateTime(1920, 10, 08)
            },
            new Author
            {
                Name = "Charlotte Perkins",
                Surname = "Gilman",
                BirthDate = new DateTime(1860, 07, 03)
            },
            new Author
            {
                Name = "Eric",
                Surname = "Ries",
                BirthDate = new DateTime(1978, 09, 22)
            }
          );
        }
    }
}