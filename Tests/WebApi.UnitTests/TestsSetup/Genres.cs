using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DBOperations;
using WebApi.Entities;

namespace WebApi.UnitTests.TestsSetup
{
    public static class Genres
    {
        // fake inmemory db için fake genre verisi oluşturma;
        public static void AddGenres(this BookStoreDbContext context)
        {
            context.Genres.AddRange(
            new Genre
            {
                Name = "Personel Growth"
            },
            new Genre
            {
                Name = "Science Fiction"
            },
            new Genre
            {
                Name = "Romance"
            }
          );
        }
    }
}