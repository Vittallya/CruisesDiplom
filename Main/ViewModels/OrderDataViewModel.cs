using BL;
using DAL.Dto;
using DAL.Models;
using Main.Events;
using MVVM_Core;
using MVVM_Core.Validation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class OrderDataViewModel : BasePageViewModel
    {
        private readonly OrderService orderService;
        private readonly UserService userService;
        private readonly InsurancesService insuranceService;
        private readonly ToursService toursService;
        private readonly ValuteGetterService valuteGetter;
        private readonly Validator validator;
        private readonly RegisterService registerService;
        private readonly EventBus eventBus;

        public bool IsAutorized { get; set; }

        TourDto _tour;

        public OrderDto OrderDto { get; set; }

        public ClientDto ClientDto { get; set; } = new ClientDto();

        public double FullCost { get; set; }
        public double FullCostUSD { get; set; }
        public double FullCostEuro { get; set; }

        public int Old { get; set; }
        public int Child { get; set; }

        double cost;
        double costChild;

        public OrderDataViewModel(PageService pageservice, 
            OrderService orderService, UserService userService, 
            InsurancesService insuranceService,
            ToursService toursService,
            ValuteGetterService valuteGetter,
            Validator validator,
            RegisterService registerService, EventBus eventBus) : base(pageservice)
        {
            this.orderService = orderService;
            this.userService = userService;
            this.insuranceService = insuranceService;
            this.toursService = toursService;
            this.valuteGetter = valuteGetter;
            this.validator = validator;
            this.registerService = registerService;
            this.eventBus = eventBus;
            PropertyChanged += OrderDataViewModel_PropertyChanged;
            Init();
        }

        private void OrderDataViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Old) || e.PropertyName == nameof(Child))
            {
                Calculate();
            }
        }

        private void Calculate()
        {
            FullCost = 0;
            foreach(var i in UsedIns)
            {
                FullCost += i.Cost;
            }

            FullCost += GetCost(cost, costChild, Old, Child, _tour.DaysCount);
            FullCostUSD = valuteGetter.GetUSDValue(FullCost);
            FullCostEuro = valuteGetter.GetEuroValue(FullCost);
        }

        async Task ReloadInsurance()
        {
            await insuranceService.ReloadAsync(valuteGetter);
            var allIns = insuranceService.GetInsurances();

            if (orderService.HasInsurances)
            {
                UsedIns = new ObservableCollection<InsuranceDto>(
                   orderService.GetUsedInsurances().Where(x => allIns.Contains(x, new InsComparer())));

                NotUsedIns = new ObservableCollection<InsuranceDto>(
                    allIns.Except(UsedIns, new InsComparer()));
            }
            else
            {
                UsedIns = new ObservableCollection<InsuranceDto>();
                NotUsedIns = new ObservableCollection<InsuranceDto>(allIns);
            }

            IsInsAdditionVisible = NotUsedIns.Count > 0;

            if (IsInsAdditionVisible)
            {
                InsuranceIndex = 0;
            }
            orderService.SetupInsurances(UsedIns);
        }

        async void Init()
        {
            eventBus.Subscribe<Events.DataUpdated<Insurance>, OrderDataViewModel>(OnDbUpdated, false);
            await ReloadInsurance();

            _tour = toursService.GetTour(orderService.TourId);

            cost = _tour.Cost;
            costChild = _tour.ChildCost;
            OrderDto = orderService.GetOrder();
            Old = OrderDto.PeopleCount - OrderDto.ChildCount;
            Child = OrderDto.ChildCount;

            IsAutorized = userService.IsAutorized;
        }

        private async Task OnDbUpdated(DataUpdated<Insurance> arg)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    await ReloadInsurance();
                    break;
                }
                catch (NotSupportedException) when(i < 3)
                {
                    await Task.Delay(450);
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                    break;
                }
            }
        }

        double GetCost(double cost, double costChild, int old, int child, int daysCount)
        {
            return (cost * old + costChild * child) * daysCount;
        }

        bool Check()
        {

            return true;
        }

        public ObservableCollection<InsuranceDto> NotUsedIns { get; set; }
        public ObservableCollection<InsuranceDto> UsedIns { get; set; }

        public bool IsInsAdditionVisible { get; set; } = true;

        public ICommand RemoveIns => new Command(x =>
        {
            if(x is InsuranceDto dto)
            {
                UsedIns.Remove(dto);
                NotUsedIns.Add(dto);
                Calculate();
                IsInsAdditionVisible = NotUsedIns.Count > 0;

                if(IsInsAdditionVisible)
                    InsuranceIndex = 0;
            }
        });

        public int InsuranceIndex { get; set; }

        public ICommand AddIns => new Command(x =>
        {
            if (InsuranceIndex >= 0)
            {
                var dto = NotUsedIns[InsuranceIndex];

                NotUsedIns.Remove(dto);
                UsedIns.Add(dto);
                Calculate();

                IsInsAdditionVisible = NotUsedIns.Count > 0;
            }

        });

        public ICommand Accept => new Command(x =>
        {
            Next(x);
        }, y => Old > 0);


        protected override void Next(object p)
        {
            if (!Check())
            {
                return;
            }
            Calculate();

            OrderDto.PeopleCount = Child + Old;
            OrderDto.ChildCount = Child;
            OrderDto.FullCost = FullCost;
            orderService.SetupFilledOrder(OrderDto);

            //pageservice.ChangePage<Pages.PlacementPage>(PoolIndex, DisappearAnimation.Default);

            if (userService.IsAutorized)
            {
                orderService.SetupClient(userService.CurrentUser.Id);
                pageservice.ChangePage<Pages.PlacementPage>(PoolIndex, DisappearAnimation.Default);
            }
            else
            {
                pageservice.SetupNext<Pages.PlacementPage>(DisappearAnimation.Default);
                pageservice.ChangePage<Pages.ClientRegisterPage>(PoolIndex, DisappearAnimation.Default);
            }

        }
        public override int PoolIndex => Rules.Pages.MainPool;
    }
}