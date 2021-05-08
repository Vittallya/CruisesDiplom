using BL;
using DAL.Dto;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using MVVM_Core;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class ToursViewModel : BasePageViewModel
    {
        private readonly EventBus eventBus;
        private readonly ToursService toursService;
        private readonly OrderService orderService;
        private readonly IConfiguration config;

        public override int PoolIndex => Rules.Pages.MainPool;
        public ToursViewModel(PageService pageservice, 
            EventBus eventBus,
            ToursService toursService, OrderService orderService, IConfiguration config ) : base(pageservice)
        {
            this.eventBus = eventBus;
            this.toursService = toursService;
            this.orderService = orderService;
            this.config = config;
            Init();
        }

        async void Init()
        {
            orderService.Clear();
            eventBus.Subscribe<Events.DataUpdated<Tour>, ToursViewModel>(OnUpdate, false);
            await Reload();
        }

        

        async Task Reload()
        {
            await toursService.ReloadAsync(x => $"{config["DefaultImagePath"]}\\{x}");
            Tours = new ObservableCollection<TourDto>(toursService.GetAllTours());

        }

        private async Task OnUpdate(IEvent arg)
        {
            await Reload();
        }

        public ObservableCollection<TourDto> Tours { get; set; }

        public ICommand SelectTour => new Command(x =>
        {
            if (x is TourDto tour) 
            {
                orderService.SetupTour(tour.Id);
                pageservice.ChangePage<Pages.TourDetails>(PoolIndex, DisappearAnimation.Default);
            }
        });
    }
}
