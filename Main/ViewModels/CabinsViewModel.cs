using BL;
using DAL.Dto;
using DAL.Models;
using Main.Components;
using Main.Services;
using Microsoft.Extensions.Configuration;
using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class CabinsViewModel: BaseViewModel
    {
        private readonly OrderService orderService;
        private readonly ToursService toursService;
        private readonly IConfiguration config;
        private readonly WindowsService windowsService;
        private readonly PlacementService placementService;
        private readonly PassengersService passengersService;
        private IEnumerable<Passenger> _passengers;

        public ObservableCollection<PlacementDto> Placements { get; set; }


        public ObservableCollection<CabinDto> Cabins { get; set; }


        public CabinsViewModel(OrderService orderService,
                               ToursService toursService,
                               IConfiguration config,
                               WindowsService windowsService,
                               PlacementService placementService,
                               PassengersService cabinsService)
        {
            this.orderService = orderService;
            this.toursService = toursService;
            this.config = config;
            this.windowsService = windowsService;
            this.placementService = placementService;
            this.passengersService = cabinsService;
            Init();
        }

        private async void Init()
        {
            await placementService.ReloadAsync(orderService.TourId, config["DefaultImagePath"]);

            _passengers = passengersService.GetPassengers();

            int adults = _passengers.Count(x => !x.IsChild);
            int child = _passengers.Count(x => x.IsChild);
            int layner = toursService.GetTour(orderService.TourId).LaynerId;

            Cabins = new ObservableCollection<CabinDto>(placementService.GetCabins(layner, adults, child));
            //Получили не занятые каюты

            foreach(var o in passengersService.GetOtherPassengers().Where(x => x.IsCabinSelected))
            {
                var busy = Cabins.FirstOrDefault(x => x.Id == o.SelectedCabin);
                Cabins.Remove(busy);
            }
        }

        public ICommand SelectCommand => new Command(x =>
        {
            if(x is CabinDto dto)
            {
                foreach(var p in _passengers)
                {
                    p.IsCabinSelected = true;
                    p.SelectedCabin = dto.Id;
                }
                windowsService.CloseWindow();
            }
        });

    }
}
