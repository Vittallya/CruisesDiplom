using BL;
using DAL.Dto;
using Main.Services;
using Microsoft.Extensions.Configuration;
using MVVM_Core;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class ProfileViewModel : BasePageViewModel
    {
        private readonly ValuteGetterService valuteGetter;
        private readonly WordService wordService;
        private readonly SplashScreenService splashScreen;
        private readonly IConfiguration config;
        private readonly UserService userService;
        private readonly OrderService orderService;
        private OrderDto selectedOrder;

        public bool DetailsVis { get; set; }

        public OrderDto SelectedOrder 
        { 
            get => selectedOrder; 
            set 
            {
                DetailsVis = value != null;

                if (DetailsVis)
                {
                    selectedOrder = value;                  
                    OnPropertyChanged();
                    SetupService(value.TourId);
                }
            }
        }

        async void SetupService(int serviceId)
        {
        }

        public TourDto Tour { get; set; }



        public ProfileViewModel(PageService pageservice,
            ValuteGetterService valuteGetter,
            WordService wordService,
            SplashScreenService splashScreen,
            IConfiguration config,
            UserService userService, OrderService orderService) : base(pageservice)
        {
            this.valuteGetter = valuteGetter;
            this.wordService = wordService;
            this.splashScreen = splashScreen;
            this.config = config;
            this.userService = userService;
            this.orderService = orderService;
            Init();
        }

        async void Init()
        {
            splashScreen.OnOverlapScreen("Получение акутального курса валют...");

            bool res = await valuteGetter.ReloadAsync();
            await Task.Delay(250);

            if (res)
            {
                splashScreen.OnClearScreen();
            }
            else
            {
                splashScreen.OnShowPromtBtn(valuteGetter.Message);
            }

            Orders = new ObservableCollection<OrderDto>(await orderService.GetAllOrders(userService.CurrentUser.Id, valuteGetter));
        }

        public ICommand CancelOrder => new CommandAsync(async x =>
        {
            if (MessageBox.Show("Удалить заказ?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await orderService.CancelOrder(SelectedOrder.Id);
                Orders.Remove(SelectedOrder);
            }
        });

        public ICommand ShowContract => new CommandAsync(async x =>
        {
            if(x is OrderDto dto)
            {
                dto.PlacementDtos = (await orderService.GetPlacements(dto.Id)).ToList();

                wordService.ShowOrderContract(dto, valuteGetter, config["WordFile"]);
            }
        });

        public bool IsLoadingVisible { get; set; }


        protected override void Back(object p)
        {
            pageservice.ChangePage<Pages.HomePage>(DisappearAnimation.Default);
        }

        public ObservableCollection<OrderDto> Orders { get; set; }

        public override int PoolIndex => Rules.Pages.MainPool;
    }
}