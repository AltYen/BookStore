using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.UnitTests.TestsSetup
{
    // testlerimiz için tanımlayacağımız ayarlar.
    public class CommonTestFixture
    {
        public BookStoreDbContext Context { get; set; }
        public IMapper Mapper { get; set; }

        public CommonTestFixture()
        {
            // sadece testler için kullanacağımız ayrı bir fake inmemory database oluşturmalıyız.
            var options = new DbContextOptionsBuilder<BookStoreDbContext>().UseInMemoryDatabase(databaseName: "BookStoreTestDB").Options;
            Context = new BookStoreDbContext(options);

            Context.Database.EnsureCreated(); // oluşturulduğundan emin ol
            Context.AddBooks(); // oluşan fake verileri ekleme.
            Context.AddGenres();
            Context.AddAuthors();
            Context.SaveChanges();

            Mapper = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); }).CreateMapper();// Direk olarak WebApi içerisindeki mapper configlerini kullanmasını gösteriyoruz.
            

        }
    }
}