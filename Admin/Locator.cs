using System;
using Admin.ViewModels;
using Admin.ViewModels.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MVVM_Core;

namespace Admin
{
    public class Locator
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public ViewModels.MainViewModel MainViewModel => ServiceProvider.GetRequiredService<MainViewModel>();
        public IEditItemViewModel EditItemViewModel => _detailViewModelTypeGetter?.Invoke();
        public IItemsViewModel ItemsViewModel => _itemsViewModelTypeGetter?.Invoke();

        private static Func<IItemsViewModel> _itemsViewModelTypeGetter;
        private static Func<IEditItemViewModel> _detailViewModelTypeGetter;



        public static void SetItemsViewModel<TViewModel>() 
        {
            _itemsViewModelTypeGetter = () => ServiceProvider.GetRequiredService<TViewModel>() as IItemsViewModel;            
        }

        public static void SetItemsViewModel(Type vmType)
        {
            _itemsViewModelTypeGetter = () => ServiceProvider.GetRequiredService(vmType) as IItemsViewModel;
        }

        public static void SetDetailsViewModel<TModel>() where TModel: class
        {
            _detailViewModelTypeGetter = () => ServiceProvider.GetService<EditItemViewModel<TModel>>();            
        }

        public static void SetupLocator(IServiceProvider provider)
        {
            ServiceProvider = provider;
        }
    }
}
