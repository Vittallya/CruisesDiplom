using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public enum OrderStatus
    {
        Active, Completed, Canceled
    }

    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int TourId { get; set; }

        public double FullCost { get; set; }

        public DateTimeOffset CreationDate { get; set; }
        public int PeopleCount { get; set; }
        public int ChildCount { get; set; }
        public virtual Client Client { get; set; }
        public virtual Tour Tour { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public ICollection<Insurance> Insurances { get; set; }
        public ICollection<Placement> Placements { get; set; }

    }
}
