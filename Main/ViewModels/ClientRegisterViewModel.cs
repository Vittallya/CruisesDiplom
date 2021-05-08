using BL;
using DAL.Dto;
using MVVM_Core;
using MVVM_Core.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class ClientRegisterViewModel : BasePageViewModel
    {
        private readonly RegisterService registerService;
        private readonly EventBus eventBus;
        private readonly Validator validator;
        private readonly OrderService orderService;

        public ClientDto ClientDto { get; set; }
        public ProfileDto ProfileDto { get; set; } = new ProfileDto();

        public PasswordBox PasswordBox { get; set; } = new PasswordBox();

        public bool IsRegisterRequiered { get; set; }

        public ClientRegisterViewModel(PageService pageservice,
                                       RegisterService registerService,
                                       EventBus eventBus,
                                       Validator validator,
                                       OrderService orderService) : base(pageservice)
        {
            this.registerService = registerService;
            this.eventBus = eventBus;
            this.validator = validator;
            this.orderService = orderService;

            validator.ForProperty(() => ClientDto.Name, "ФИО").NotEmpty();
            validator.ForProperty(() => ClientDto.Pasport, "Паспорт").NotEmpty();
            validator.ForProperty(() => ClientDto.Phone, "Номер телефона").NotEmpty();
            validator.ForProperty(() => ClientDto.Email, "Адр. эл. почты").NotEmpty();
            validator.ForProperty(() => ProfileDto.Login, "Логин").LengthMoreThan(2, "Длина логина должна быть больше 2 символов");

            validator.ForProperty(() => PasswordBox.Password, "Пароль").LengthMoreThan(2, "Длина пароля должна быть больше 2").
                Predicate(x => x.Any(char.IsDigit) && x.Any(char.IsLetter), "Пароль должен содержать цифры и буквы");

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

            if (validator.IsCorrect)
            {

                ProfileDto.Password = PasswordBox.Password;
                registerService.SetupClient(ClientDto);

                if (!await registerService.SetupProfile(ProfileDto))
                {
                    MessageBox.Show(registerService.ErrorMessage);
                    return;
                }

                pageservice.Next(PoolIndex);
            }
            else
            {
                MessageBox.Show(validator.ErrorMessage);
            }
        }

        public override int PoolIndex => Rules.Pages.MainPool;

        public string Message { get; private set; }

        public bool IsErrorVisible { get; set; }
    }
}