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
        private IEnumerable<Passenger> _oPassengers;

        public void SetupPassengers(IEnumerable<Passenger> selected, IEnumerable<Passenger> otherPassengers)
        {
            _passengers = selected;
            _oPassengers = otherPassengers;
        }

        public IEnumerable<Passenger> GetPassengers()
        {
            return _passengers;
        }
        public IEnumerable<Passenger> GetOtherPassengers()
        {
            return _oPassengers;
        }
    }
}
