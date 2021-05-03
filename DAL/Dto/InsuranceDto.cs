using DAL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dto
{
    public class InsuranceDto: IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public double CostUSD { get; set; }
        public double CostEUR { get; set; }
    }


    public class InsComparer : IEqualityComparer<InsuranceDto>
    {

        public bool Equals(InsuranceDto x, InsuranceDto y)
        {
            return x.Id == y.Id;
        }


        public int GetHashCode(InsuranceDto obj)
        {
            return obj.Id;
        }
    }
}
