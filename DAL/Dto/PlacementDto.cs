using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dto
{
    public class PlacementDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderDto OrderDto { get; set; }

        /// <summary>
        /// Клиент, на которого офорлмен заказ
        /// </summary>
        public bool IsClient { get; set; }

        public string Fio { get; set; }
        public string Pasport { get; set; }
        public string BirthDoc { get; set; }
        public bool IsChild { get; set; }
        public bool IsChildBefore14 { get; set; }

        public int CabinNumber { get; set; }
        public CabinDto CabinDto { get; set; }
    }
}
