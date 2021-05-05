using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dto
{
    public class CabinDto
    {
        public int Id { get; set; }

        //[Key]
        //[Column( Order = 2)]
        public int Deck { get; set; }
        public int LaynerId { get; set; }
        public CabinType CabinType { get; set; }

        /// <summary>
        /// Палуба
        /// </summary>

        public int AdultCount { get; set; }
        public int ChildCount { get; set; }

        public string ImagePath { get; set; }
    }
}
