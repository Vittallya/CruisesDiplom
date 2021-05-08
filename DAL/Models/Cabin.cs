using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public enum CabinType
    {
        A, B, C, D
    }
    public class Cabin
    {
        public int Id { get; set; }

        public int Deck { get; set; }
        public int LaynerId { get; set; }
        public Layner Layner { get; set; }
        public CabinType CabinType { get; set; }

        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
    }
}
