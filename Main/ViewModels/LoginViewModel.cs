using System.Threading.Tasks;
using BL;
using System.Windows.Input;
using MVVM_Core;
using System.Windows.Controls;
using System.Windows;

namespace Main.ViewModels
{
    public class LoginViewModel : BasePageViewModel
    {
        private readonly LoginService loginService;
        private readonly PageService pageService;
        private readonly EventBus eventBus;
        private readonly UserService userService;

        public LoginViewModel(LoginService loginService, 
            PageService pageService, EventBus eventBus, UserService userService) : base(pageService)
        {
            this.loginService = loginService;
            this.pageService = pageService;
            this.eventBus = eventBus;
            this.userService = userService;
        }
        public bool IsErrorVisible { get; set; }
        public string ErrorMessage { get; set; }

        public bool IsAnimationVisible { get; set; }

        public string Login { get; set; }
        public PasswordBox PasswordBox { get; set; } = new PasswordBox();



        public ICommand LoginCommand => new CommandAsync(async x =>
        {
            IsErrorVisible = false;

            if(await loginService.Login(Login, PasswordBox.Password))
            {
                //await eventBus.Publish(new Events.AccountEntered(userService.CurrentUser));
                pageservice.Next(PoolIndex);
            }
            else
            {
                MessageBox.Show(loginService.ErrorMessage, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }, y => Login != null && Login.Length > 1 && PasswordBox.Password != null && PasswordBox.Password.Length > 1);

        public override int PoolIndex => Rules.Pages.MainPool;
    }
}
