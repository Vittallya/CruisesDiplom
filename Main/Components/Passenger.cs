using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Components
{
    public class Passenger
    {
        public int Number { get; set; }

        public bool IsClient { get; set; }

        public bool IsChild { get; set; }
        public bool IsChildBefore14 { get; set; }
        public string FIO { get; set; }

        public string Pasport { get; set; }
        public string BirthDocument { get; set; }

        public int SelectedCabin { get; set; }
        public bool IsCabinSelected { get; set; }


        public bool _IsSelected { get; set; }
    }

    public class PassComparer : IEqualityComparer<Passenger>
    {
        public bool Equals(Passenger x, Passenger y)
        {
            return x.Number == y.Number && x.IsChild == y.IsChild;
        }

        public int GetHashCode(Passenger obj)
        {
            return obj.Number;
        }
    }
}
