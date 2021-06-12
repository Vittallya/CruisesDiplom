using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            Init();            
        }

        async void Init()
        {
            InitializeComponent();
            var c = Locator.Services.GetService(typeof(BL.ConnectorService)) as BL.ConnectorService;

            var res = await c.GetValue(5);

            if (res)
            {
                App.Current.Shutdown();
            }
        }

    }
}
