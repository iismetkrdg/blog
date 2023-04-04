using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Yazi { get; set; }
        public DateTime Tarih { get; set; }
        public string yazar { get; set; }
        public string KeyWords { get; set; }
        public string slugfield { get; set; }
        public bool yayinda { get; set; }
        public int görüntülenmeSayisi { get; set; }
    }
}