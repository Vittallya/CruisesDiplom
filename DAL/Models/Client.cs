using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pasport { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public virtual Profile Profile { get; set; }
    }
}
