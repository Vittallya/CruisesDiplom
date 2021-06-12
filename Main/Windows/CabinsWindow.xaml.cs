using Main.ViewModels;
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
using System.Windows.Shapes;

namespace Main.Windows
{
    /// <summary>
    /// Логика взаимодействия для CabinsWindow.xaml
    /// </summary>
    public partial class CabinsWindow : Window
    {
        public CabinsWindow()
        {
            InitializeComponent();
            Loaded += CabinsWindow_Loaded;
        }

        private void CabinsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as CabinsViewModel).SetupWindow(this);
        }
    }
}
