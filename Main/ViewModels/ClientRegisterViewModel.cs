using BL;
using DAL.Dto;
using MVVM_Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class ClientRegisterViewModel : BasePageViewModel
    {
        private readonly RegisterService registerService;
        private readonly EventBus eventBus;
        private readonly OrderService orderService;

        public ClientDto ClientDto { get; set; }
        public ProfileDto ProfileDto { get; set; } = new ProfileDto();

        public PasswordBox PasswordBox { get; set; } = new PasswordBox();

        public bool IsRegisterRequiered { get; set; }

        public ClientRegisterViewModel(PageService pageservice, RegisterService registerService, EventBus eventBus, OrderService orderService) : base(pageservice)
        {
            this.registerService = registerService;
            this.eventBus = eventBus;
            this.orderService = orderService;
            Init();
        }

        public ICommand EnterCommand => new Command(x =>
        {
            pageservice.ChangePage<Pages.LoginPage>(PoolIndex, DisappearAnimation.Default);
        });

        public bool IsProfileRegister { get; set; }
        
        void Init()
        {
            ClientDto = registerService.GetClient();
            IsRegisterRequiered = registerService.IsRegisterRequiered;
            IsProfileRegister = IsRegisterRequiered;
        }

        protected override async void Next(object param)
        {
            IsErrorVisible = false;

            ProfileDto.Password = PasswordBox.Password;
            registerService.SetupClient(ClientDto);

            


            if (!await registerService.SetupProfile(ProfileDto))
            {
                MessageBox.Show(registerService.ErrorMessage);
                return;
            }

            pageservice.Next(PoolIndex);
        }

        public override int PoolIndex => Rules.Pages.MainPool;

        public string Message { get; private set; }

        public bool IsErrorVisible { get; set; }
    }
}