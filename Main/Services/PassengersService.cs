using Main.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Services
{
    public class PassengersService
    {
        private IEnumerable<Passenger> _passengers;

        public void SetupPassengers(IEnumerable<Passenger> passengers)
        {
            _passengers = passengers;
        }
        public IEnumerable<Passenger> GetPassengers()
        {
            return _passengers;
        }
    }
}
