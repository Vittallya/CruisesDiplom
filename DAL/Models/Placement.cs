using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Placement
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        /// <summary>
        /// Клиент, на которого офорлмен заказ
        /// </summary>
        public bool IsClient { get; set; }

        public string Fio { get; set; }
        public string Pasport { get; set; }
        public string BirthDoc { get; set; }
        public bool IsChild { get; set; }
        public bool IsChildBefore14 { get; set; }

        /// <summary>
        /// Номер каюты
        /// </summary>
        /// 
        //[Column("CabinId")]
        public int CabinId { get; set; }
        public Cabin Cabin { get; set; }
    }
}
