using Microsoft.Extensions.DependencyInjection;
using System;
using Main.ViewModels;
using DAL;
using BL;
using System.Threading.Tasks;
using Main.Pages;

namespace Main
{
    public class Locator
    {
        public static IServiceProvider Services { get; private set; }
        public static void InitServices(IServiceProvider provider)
        {
            Services = provider;
        }

        public  MainViewModel MainViewModel => Services.GetRequiredService<MainViewModel>();
        public  LoginViewModel LoginViewModel => Services.GetRequiredService<LoginViewModel>();
        public  ViewModels.ClientRegisterViewModel ClientRegisterViewModel => Services.GetRequiredService<ClientRegisterViewModel>();
        public  ViewModels.OrderResultViewModel OrderResultViewModel => Services.GetRequiredService<OrderResultViewModel>();
        public  ViewModels.ProfileViewModel ClientViewModel => Services.GetRequiredService<ProfileViewModel>();
        public  ViewModels.HomeViewModel HomeViewModel => Services.GetRequiredService<HomeViewModel>();
        public  ViewModels.OrderDataViewModel OrderDataViewModel => Services.GetRequiredService<OrderDataViewModel>();
        public  ViewModels.PicturesPage SertsViewModel => Services.GetRequiredService<PicturesPage>();
        public  ViewModels.ProfileRegisterViewModel ProfileRegisterViewModel => Services.GetRequiredService<ProfileRegisterViewModel>();
        public  ViewModels.ToursViewModel ToursViewModel => Services.GetRequiredService<ToursViewModel>();
        public  ViewModels.TourDetailsViewModel TourDetailsViewModel => Services.GetRequiredService<TourDetailsViewModel>();
        public  ViewModels.PlacementViewModel PlacementViewModel => Services.GetRequiredService<PlacementViewModel>();
    }
}

