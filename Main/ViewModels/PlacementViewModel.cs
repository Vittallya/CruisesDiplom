using BL;
using DAL.Dto;
using Main.Components;
using Main.Services;
using MVVM_Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class PlacementViewModel : BasePageViewModel
    {
        private readonly UserService userService;
        private readonly ToursService toursService;
        private readonly PassengersService cabinsService;
        private readonly RegisterService registerService;
        private readonly OrderService orderService;

        public override int PoolIndex => Rules.Pages.MainPool;
        
        public PlacementViewModel(PageService pageservice,
            UserService userService,
            ToursService toursService, 
            PassengersService cabinsService,
            RegisterService registerService,            
            OrderService orderService) : base(pageservice)
        {
            this.userService = userService;
            this.toursService = toursService;
            this.cabinsService = cabinsService;
            this.registerService = registerService;
            this.orderService = orderService;

            Init();
        }


        public TourDto TourDto { get; private set; }
        public OrderDto OrderDto { get; private set; }

        private void Init()
        {
            TourDto = toursService.GetTour(orderService.TourId);
            OrderDto = orderService.GetOrder();

            Passengers = new ObservableCollection<Passenger>();

            ClientDto cl = null;

            if (userService.IsAutorized)
                cl = userService.CurrentUser;
            else
                cl = registerService.GetClient();

            Passengers.Add(new Passenger
            {
                FIO = cl.Name,
                Pasport = cl.Pasport,
                IsClient = true,
                Number = 1,
            });

            for (int i = 2; i <= OrderDto.PeopleCount; i++)
            {
                var pass = new Passenger
                {
                    IsChild = i > OrderDto.PeopleCount - OrderDto.ChildCount,
                    Number = i > OrderDto.PeopleCount - OrderDto.ChildCount ?
                    i - OrderDto.PeopleCount + OrderDto.ChildCount : i,
                };
                Passengers.Add(pass);
            }
        }

        public ICommand PlaceCommand => new Command(x =>
        {
            if(x is ListBox lb)
            {
                var list = lb.SelectedItems.Cast<Passenger>();

                if(list.All(z => !z.IsChildBefore14 ? 
                z.Pasport != null : z.BirthDocument != null && z.FIO != null))
                {

                }
                else
                {
                    MessageBox.Show("ФИО и паспортные данные/св-во о рождении должны быть заполнены");
                }
            }
        });

        protected override void Next(object param)
        {
            
        }

        public ObservableCollection<Passenger> Passengers { get; set; }
    }
}