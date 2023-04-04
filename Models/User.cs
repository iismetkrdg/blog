using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Models
{
    public class User
    {
        public int UserId { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; }
        public string Aciklama { get; set; }
        public int BlogSayisi { get; set; }
        
    }
}