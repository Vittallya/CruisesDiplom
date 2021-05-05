using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dto
{
    public class OrderDto: IDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public ClientDto ClientDto { get; set; }
        public int TourId { get; set; }

        public double FullCost { get; set; }
        public double FullCostUSD { get; set; }
        public double FullCostEUR { get; set; }

        public DateTimeOffset CreationDate { get; set; }
        public int PeopleCount { get; set; }
        public int ChildCount { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public TourDto TourDto { get; set; }
        public ICollection<InsuranceDto> InsuranceDtos { get; set; }
        public ICollection<PlacementDto> PlacementDtos { get; set; }

        public bool HasIns => InsuranceDtos != null && InsuranceDtos.Count > 0;
    }
}
