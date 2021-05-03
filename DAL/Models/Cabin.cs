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
        Place_1,
        Place_1_IncludeChild,
        Place_2,
        Place_2_IncludeChild,
        Place_3,
        Place_3_IncludeChild,
        Place_4,
        Place_4_IncludeChild,
    }

    public class Cabin
    {
        [Key]
        [Column("Id", Order = 1)]
        public int Number { get; set; }

        //[Key]
        //[Column( Order = 2)]
        public int Deck { get; set; }
        public int LaynerId { get; set; }
        public Layner Layner { get; set; }
        public CabinType CabinType { get; set; }

        /// <summary>
        /// Палуба
        /// </summary>

        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
    }
}
