using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto Increase ID tanımlaması
        public int Id { get; set; }
        public string Name {get;set;}
        public string Surname {get;set;}
        public DateTime BirthDate {get;set;}

    }
}