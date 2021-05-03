using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVM_Core;
using BL;
using DAL;
using System.Windows;
using DAL.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Admin.ViewModels
{
    [Singleton]
    public class MainViewModel: BaseSliderViewModel
    {
        private readonly DbContextLoader loader;
        private readonly PageService pageService;
        private readonly EventBus eventBus;
        private readonly ServerPipeHandler serverPipeHandler;

        public bool LoadingContext { get; set; } = true;

        public ObservableCollection<Components.TableComponent> Tables { get; set; } = new ObservableCollection<Components.TableComponent>()
        {
            new Components.TableComponent{Name = "Туры", Type = typeof(ToursViewModel)},
            new Components.TableComponent{Name = "Лайнеры", Type = typeof(LaynersViewModel)},
        };

        public MainViewModel(DbContextLoader loader, PageService pageService, EventBus eventBus, 
            BL.ServerPipeHandler serverPipeHandler)
        {
            this.loader = loader;
            this.pageService = pageService;
            this.eventBus = eventBus;
            this.serverPipeHandler = serverPipeHandler;
            pageService.PageChanged += PageService_PageChanged;

            Console.WriteLine(1.CompareTo(2));

            init();
        }

        public override Page CurrentPage { get; set; }

        async Task Update(Events.UpdatePipe updatePipe)
        {
            serverPipeHandler.Send(updatePipe.GetString());
        }


        public ICommand SelectTable => new Command(x =>
        {
            if(x is Components.TableComponent comp)
            {
                Locator.SetItemsViewModel(comp.Type);
                pageService.ClearHistoryByPool(1);
                pageService.ChangePage<Pages.ItemsPage>(1, DisappearAndToSlideAnim.Default);

            }
        });

        async void init()
        {
            eventBus.Subscribe<Events.UpdatePipe, MainViewModel>(Update, false);

            serverPipeHandler.Init("CruisesPipe");
            try
            {
                await loader.LoadAsync<Tour>();
                LoadingContext = false;
            }
            catch(Exception ex)
            {
                MessageBoxResult res = MessageBox.Show(ex.Message, "", MessageBoxButton.OK);

                if (res == MessageBoxResult.OK || res == MessageBoxResult.Cancel)
                {
                    
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
