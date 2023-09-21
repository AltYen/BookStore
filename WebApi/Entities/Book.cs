using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto Increase ID tanımlaması
        public int Id {get;set;}
        public string Title { get; set; }
        public int GenreId { get; set; }
        public Genre Genre {get;set;} // book ve genre entities arasındaki relationu belirtme
        public int PageCount { get; set; }
        public DateTime PublishDate { get; set; }

        public Book(){

        }
    }
}
