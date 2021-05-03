using BL;
using DAL.Dto;
using MVVM_Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Main.ViewModels
{
    public class OrderResultViewModel : BasePageViewModel
    {
        private readonly ValuteGetterService valuteGetter;
        private readonly ToursService toursService;
        private readonly RegisterService registerService;
        private readonly OrderService orderService;
        private readonly UserService userService;

        public bool IsValuteVisible { get; set; }

        public double CostUSD { get; private set; }
        public double CostEuro { get; private set; }

        public OrderResultViewModel(PageService pageservice, ValuteGetterService valuteGetter, ToursService toursService, RegisterService registerService, 
            OrderService orderService, UserService userService) : base(pageservice)
        {
            this.valuteGetter = valuteGetter;
            this.toursService = toursService;
            this.registerService = registerService;
            this.orderService = orderService;
            this.userService = userService;
            Init();
        }

        public string Message { get; set; }

        public bool IsAnimVisible { get; set; }

        public OrderDto OrderDto { get; set; }

        public TourDto TourDto { get; set; }

        public ObservableCollection<InsuranceDto> Insurances { get; private set; }

        public bool HasIns => orderService.HasInsurances;

        void Init()
        {
            OrderDto = orderService.GetOrder();
            TourDto = toursService.GetTour(orderService.TourId);
            Insurances = new ObservableCollection<InsuranceDto>(orderService.GetUsedInsurances());
            CostUSD = valuteGetter.GetUSDValue(OrderDto.FullCost);
            CostEuro = valuteGetter.GetEuroValue(OrderDto.FullCost);
        }


        protected async override void Next(object p)
        {
            IsAnimVisible = true;


            if (!userService.IsAutorized)
            {
                var res1 = await registerService.RegisterAsync();

                if (!res1.Item1)
                {
                    MessageBox.Show(registerService.ErrorMessage, "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                registerService.Clear();

                orderService.SetupClient(res1.Item2);
            }
            else
            {
                orderService.SetupClient(userService.CurrentUser.Id);
            }

            bool res = await orderService.ApplyOrder();
            Message = res ? "Оформлено!" : orderService.ErrorMessage;
            orderService.Clear();
            pageservice.ClearHistoryByPool(PoolIndex);
            IsAnimVisible = false;

            pageservice.ChangePage<Pages.HomePage>(PoolIndex, DisappearAnimation.Default);
        }

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
        public override int PoolIndex => Rules.Pages.MainPool;
    }
}