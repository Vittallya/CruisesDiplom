using BL;
using DAL.Dto;
using Main.Components;
using Main.Services;
using MVVM_Core;
using System;
using System.Collections.Generic;
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
        private readonly WindowsService windowsService;
        private readonly RegisterService registerService;
        private readonly OrderService orderService;

        public override int PoolIndex => Rules.Pages.MainPool;
        
        public PlacementViewModel(PageService pageservice,
            UserService userService,
            ToursService toursService, 
            PassengersService cabinsService,
            WindowsService windowsService,
            RegisterService registerService,            
            OrderService orderService) : base(pageservice)
        {
            this.userService = userService;
            this.toursService = toursService;
            this.cabinsService = cabinsService;
            this.windowsService = windowsService;
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

                if(list.Count() == 0)
                {
                    MessageBox.Show("Необходимо выбрать туриста(-ов)");
                    return;
                }


                if(list.All(z => !z.IsChildBefore14 ? 
                z.Pasport != null : z.BirthDocument != null && z.FIO != null))
                {
                    var other = Passengers.Except(list, new PassComparer());

                    cabinsService.SetupPassengers(list, other);
                    if (windowsService.ShowDialog<Windows.CabinsWindow>() == true)
                    {
                        Passengers = new ObservableCollection<Passenger>(
                            other.Union(list).OrderByDescending(a => a.IsCabinSelected));
                    }
                }
                else
                {
                    MessageBox.Show("ФИО и паспортные данные/св-во о рождении должны быть заполнены");
                }
            }
        });

        protected override void Back(object param)
        {
            if (userService.IsAutorized)
            {
                pageservice.Back<Pages.OrderDataPage>(PoolIndex, DisappearAnimation.Default);
            }
            else
            {
                pageservice.Back<Pages.ClientRegisterPage>(PoolIndex, DisappearAnimation.Default);
            }

        }

        protected override void Next(object param)
        {
            if(Passengers.All(x => x.IsCabinSelected))
            {
                orderService.SetupPlacements(
                    Passengers.Select(x => new PlacementDto
                    {
                        BirthDoc = x.BirthDocument,
                        CabinId = x.SelectedCabin,
                        Fio = x.FIO,
                        IsChild = x.IsChild,
                        IsChildBefore14 = x.IsChildBefore14,
                        Pasport = x.Pasport,
                        IsClient = x.IsClient,
                    }));
                pageservice.ChangePage<Pages.OrderResultPage>(PoolIndex, DisappearAnimation.Default);
            }
            else
            {
                MessageBox.Show("Для всех туристов должны быть выбраны каюты");
            }
        }

        public ObservableCollection<Passenger> Passengers { get; set; }
    }
}