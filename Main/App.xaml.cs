using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MVVM_Core;
using Microsoft.Extensions.DependencyInjection;

namespace Main
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var config = new ServiceProviderBuilder();
            var provider = config.UseStartup<Startup>().BuidSeriveProvider();

            Locator.InitServices(provider);
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            var disp = Locator.Services.GetRequiredService<BL.ClientPipeHanlder>();
            disp.Dispose();
            base.OnExit(e);
        }
    }
}
