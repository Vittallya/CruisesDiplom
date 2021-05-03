using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_Core
{
    public interface ISliderAnimation
    {
        Task AnimateOldPage(UIElement obj);
        Task AnimateNewPage(UIElement obj);
    }
}
