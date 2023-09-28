using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.DBOperations;

namespace WebApi.Application.AuthorOperations.Command.DeleteAuthor
{
    public class DeleteAuthorCommand
    {
        public int AuthorId { get; set; }
        private readonly BookStoreDbContext _context;

        public DeleteAuthorCommand(BookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var author = _context.Authors.SingleOrDefault(x => x.Id == AuthorId);
            if (author is null)
                throw new InvalidOperationException("Yazar Bulunamadı!");

            var authorsBook = _context.Books.Include(x=>x.Author).Any(x=>x.AuthorId == AuthorId);
            if (authorsBook)
                throw new InvalidOperationException("Yazarı silmeden önce yazarın kitaplarını silmelisiniz!");

            _context.Authors.Remove(author);
            _context.SaveChanges();
        }
    }
}