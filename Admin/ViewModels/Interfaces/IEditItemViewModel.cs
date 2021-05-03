using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Admin.ViewModels.Interfaces
{
    public interface IEditItemViewModel
    {
        StackPanel Stack { get; set; }

        ICommand Accept { get; }
    }
}
