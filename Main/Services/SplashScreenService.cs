using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Services
{
    public class SplashScreenService
    {
        public event Action<string> OverlapScreen;
        public event Action<string> ShowPromtBtn;
        public event Action ClearScreen;

        public void OnOverlapScreen(string message)
        {
            OverlapScreen?.Invoke(message);
        }

        public void OnShowPromtBtn(string message)
        {
            ShowPromtBtn?.Invoke(message);
        }

        public void OnClearScreen()
        {
            ClearScreen?.Invoke();
        }
    }
}
