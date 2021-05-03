using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Insurance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
