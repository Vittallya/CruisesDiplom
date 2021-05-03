using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MVVM_Core
{
    public abstract class BaseSliderViewModel: BaseViewModel
    {
        public abstract Page CurrentPage { get; set; }

        protected virtual ISliderAnimation defaultAnim { get; } = DisappearAnimation.Default;

        protected async void PageService_PageChanged(Page page, ISliderAnimation animation)
        {

            ISliderAnimation anim = animation ?? defaultAnim;

            if (CurrentPage != null)
            {
                await anim.AnimateOldPage(CurrentPage.Content as UIElement);
            }

            //(page.Content as UIElement).Opacity = 0;
            CurrentPage = page;

            await anim.AnimateNewPage(CurrentPage.Content as UIElement);
        }
    }
}
