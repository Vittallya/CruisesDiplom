using BL;
using DAL.Dto;
using DAL.Models;
using Main.Events;
using Microsoft.Extensions.Configuration;
using MVVM_Core;
using System;
using System.Threading.Tasks;
using Main.Services;

namespace Main.ViewModels
{
    public class TourDetailsViewModel : BasePageViewModel
    {
        private readonly IConfiguration config;
        private readonly EventBus eventBus;
        private readonly ToursService toursService;
        private readonly SplashScreenService splashScreen;
        private readonly OrderService orderService;
        private readonly ValuteGetterService valuteGetter;

        public override int PoolIndex => Rules.Pages.MainPool;

        public TourDto Tour { get; private set; }

        public double CostUsd { get; private set; }
        public double CostEuro { get; private set; }
        public double CostChildUsd { get; private set; }
        public double CostChildEuro { get; private set; }

        public TourDetailsViewModel(PageService pageservice, 
            IConfiguration config,
            EventBus eventBus,
            ToursService toursService,
            SplashScreenService splashScreen,
            OrderService orderService, ValuteGetterService valuteGetter) : base(pageservice)
        {
            this.config = config;
            this.eventBus = eventBus;
            this.toursService = toursService;
            this.splashScreen = splashScreen;
            this.orderService = orderService;
            this.valuteGetter = valuteGetter;
            Init();
        }

        protected override void Next(object p)
        {
            
            pageservice.ChangePage<Pages.OrderDataPage>(PoolIndex, DisappearAnimation.Default);
        }



        private async void Init()
        {
            eventBus.Subscribe<Events.DataUpdated<Tour>, TourDetailsViewModel>(OnTourUpdated, false);
            splashScreen.OnOverlapScreen("Получение акутального курса валют...");

            bool res = await valuteGetter.ReloadAsync();
            await Task.Delay(950);

            if (res)
            {
                splashScreen.OnClearScreen();
            }
            else
            {
                splashScreen.OnShowPromtBtn(valuteGetter.Message);
            }
            Reload();

        }

        void Reload()
        {
            Tour = toursService.GetTour(orderService.TourId);

            CostUsd = valuteGetter.GetUSDValue(Tour.Cost);
            CostEuro = valuteGetter.GetEuroValue(Tour.Cost);
            CostChildUsd = valuteGetter.GetUSDValue(Tour.ChildCost);
            CostChildEuro = valuteGetter.GetEuroValue(Tour.ChildCost);
        }

        private async Task OnTourUpdated(DataUpdated<Tour> arg)
        {
            await Task.Delay(400);
            Reload();
        }
    }
}