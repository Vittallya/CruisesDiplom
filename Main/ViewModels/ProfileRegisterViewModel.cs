using BL;
using DAL.Dto;
using MVVM_Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class ProfileRegisterViewModel : BasePageViewModel
    {
        private readonly RegisterService registerService;

        public PasswordBox PasswordBox { get; set; } = new PasswordBox();
        public PasswordBox PasswordBox1 { get; set; } = new PasswordBox();

        public ProfileRegisterViewModel(PageService pageservice, RegisterService registerService) : base(pageservice)
        {
            this.registerService = registerService;
        }

        bool Check()
        {

            if (string.IsNullOrEmpty(Profile.Login))
            {
                MessageBox.Show("Поле логина должно быть заполнено", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Поле пароля должно быть заполнено", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if(PasswordBox.Password != PasswordBox1.Password)
            {
                MessageBox.Show("Пароли должны быть одинаковыми", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        protected async override void Next(object p)
        {
            if (!Check())
                return;

            Profile.Password = PasswordBox.Password;

            if (!await registerService.SetupProfile(Profile))
            {
                MessageBox.Show(registerService.ErrorMessage, "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            pageservice.ChangePage<Pages.OrderResultPage>(PoolIndex, DisappearAnimation.Default);
        }

        public ICommand Decline => new Command(x =>
        {
            pageservice.ChangePage<Pages.OrderResultPage>(PoolIndex, DisappearAnimation.Default);
        });

        public override int PoolIndex => Rules.Pages.MainPool;

        public ProfileDto Profile { get; private set; } = new ProfileDto();
    }
}