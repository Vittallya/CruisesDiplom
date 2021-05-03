using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Admin.ViewModels
{
    public interface IItemsViewModel
    {
        ICommand RemoveCommand { get; }
        ICommand EditCommand { get; }
        ICommand AddCommand { get; }
        ICommand UpdateCommand { get; }
        GridView GridView { get; set; }

        StackPanel StackPanel { get; }
    }
}
