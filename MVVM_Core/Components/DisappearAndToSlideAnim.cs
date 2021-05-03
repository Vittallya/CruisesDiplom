using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MVVM_Core
{
    public class DisappearAndToSlideAnim : DisappearAnimation
    {
        public float TimeSlideSec { get; set; } = 0.3f;
        public int FromX { get; set; } = 800;

        public override async Task AnimateNewPage(UIElement obj)
        {
            int from = FromX;

            obj.RenderTransform = new TranslateTransform(from, 0);

            if (obj.HasAnimatedProperties)
            {

                obj.BeginAnimation(UIElement.OpacityProperty, GetOpacityAnim());
            }

            DoubleAnimation anim = new DoubleAnimation(from, 0, TimeSpan.FromSeconds(TimeSlideSec));

            anim.EasingFunction = new ElasticEase() { Oscillations = 0 };
            obj.RenderTransform.BeginAnimation(TranslateTransform.XProperty, anim);

            await Task.Delay((int)(TimeSlideSec * 1000));

        }


        public static DisappearAndToSlideAnim ToLeft => new DisappearAndToSlideAnim();
        public static DisappearAndToSlideAnim ToRight => new DisappearAndToSlideAnim() { FromX = -800 };
    }
}
