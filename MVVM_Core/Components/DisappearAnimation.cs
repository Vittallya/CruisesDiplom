using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace MVVM_Core
{
    public class DisappearAnimation : ISliderAnimation
    {

        public float TimeOpacityOldSec { get; set; } = 0.2f;
        public float TimeOpacityNewSec { get; set; } = 0.2f;

        public virtual async Task AnimateNewPage(UIElement obj)
        {
            obj.Opacity = 0;

            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(TimeOpacityNewSec));

            obj.BeginAnimation(UIElement.OpacityProperty, anim);

            await Task.Delay((int)(TimeOpacityNewSec * 1000));

            obj.Opacity = 1;
        }

        protected DoubleAnimation GetOpacityAnim(bool fadeIn = true)
        {
            int to = fadeIn ? 1 : 0;

            DoubleAnimation anim = new DoubleAnimation(to, TimeSpan.FromSeconds(0));
            return anim;
        }

        public virtual async Task AnimateOldPage(UIElement obj)
        {
            obj.Opacity = 1;

            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(TimeOpacityNewSec));

            obj.BeginAnimation(UIElement.OpacityProperty, anim);

            await Task.Delay((int)(TimeOpacityNewSec * 1000));

            obj.Opacity = 0;

        }

        public static DisappearAnimation Default => new DisappearAnimation();
    }
}
