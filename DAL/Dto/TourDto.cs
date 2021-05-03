using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dto
{
    public class TourDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desctiprion { get; set; }
        public double Cost { get; set; }
        public double ChildCost { get; set; }
        public string ImageName { get; set; }
        public int DaysCount { get; set; }

        public string ImagePath { get; set; }

        public LaynerDto LaynerDto { get; set; }
        public DateTime StartDate { get; set; }
        public string StartPlace { get; set; }
        public int LaynerId { get; set; }
    }
}
