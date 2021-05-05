using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_Core
{
    public class WindowsService
    {

        List<Window> windows = new List<Window>();
        Window _currentWindow;

        public bool? ShowDialog<TWindow>() where TWindow: Window, new()
        {

            _currentWindow = new TWindow();
            windows.Add(_currentWindow);
            return _currentWindow.ShowDialog();
        }

        public void CloseWindow<TWindow>(bool dialogResult = true) where TWindow : Window, new()
        {
            var w = windows.OfType<TWindow>().FirstOrDefault();
            if (w != null) 
            { 
                w.DialogResult = dialogResult;
                windows.Remove(w);
            }           
        }
        public void CloseWindow(bool dialogResult = true)
        {
            if (_currentWindow != null) 
            {
                _currentWindow.DialogResult = dialogResult;
                windows.Remove(_currentWindow);
                _currentWindow = null;
            }           
        }
    }
}
