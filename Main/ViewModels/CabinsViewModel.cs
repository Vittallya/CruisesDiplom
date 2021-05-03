using BL;
using DAL.Dto;
using DAL.Models;
using Main.Components;
using Main.Services;
using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class CabinsViewModel: BaseViewModel
    {
        private readonly OrderService orderService;
        private readonly ToursService toursService;
        private readonly PlacementService placementService;
        private readonly PassengersService passengersService;
        private IEnumerable<Passenger> _passengers;

        public ObservableCollection<PlacementDto> Placements { get; set; }


        public ObservableCollection<CabinDto> Cabins { get; set; }

        public CabinsViewModel(OrderService orderService, 
            ToursService toursService,
            PlacementService placementService, 
            PassengersService cabinsService)
        {
            this.orderService = orderService;
            this.toursService = toursService;
            this.placementService = placementService;
            this.passengersService = cabinsService;
            Init();
        }

        private async void Init()
        {
            await placementService.ReloadAsync();
            Placements = new ObservableCollection<PlacementDto>(placementService.GetAllPlacements());

            _passengers = passengersService.GetPassengers();

            int adults = _passengers.Count(x => !x.IsChild);
            int child = _passengers.Count(x => x.IsChild);
            int layner = toursService.GetTour(orderService.TourId).LaynerId;

            Cabins = new ObservableCollection<CabinDto>(placementService.GetCabins(layner, adults, child));
        }



    }
}
