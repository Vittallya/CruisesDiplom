using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MVVM_Core;
using Microsoft.Extensions.DependencyInjection;

namespace Admin
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ServiceProviderBuilder();
            Locator.SetupLocator(builder.UseStartup<Startup>().BuidSeriveProvider());
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            var s = Locator.ServiceProvider.GetService<BL.ServerPipeHandler>();
            s.Dispose();
            base.OnExit(e);
        }
    }
}
