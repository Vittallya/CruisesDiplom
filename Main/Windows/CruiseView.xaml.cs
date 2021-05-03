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
    /// Логика взаимодействия для CruiseView.xaml
    /// </summary>
    public partial class CruiseView : Window
    {
        public CruiseView()
        {
            InitializeComponent();
            img.RenderTransform = new ScaleTransform(1, 1);
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(img.RenderTransform is ScaleTransform s)
            {
                

                var p = e.GetPosition(img);

                if (e.Delta > 0)
                {
                    var x = p.X / img.ActualWidth;
                    var y = p.Y / img.ActualHeight;
                    img.RenderTransformOrigin = new Point(x, y);
                }
                double newX = s.ScaleX + (double)e.Delta / 200;
                double newY = s.ScaleY + (double)e.Delta / 200;

                if (newX >= 1 && newY >= 1)
                {
                    s.ScaleX = newX;
                    s.ScaleY = newY;
                }
            }
        }

        bool mouseDown;
        Point startOffset;
        Point currPos;

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var canv = (sender as Canvas);
            mouseDown = true;

            var point = e.GetPosition(canv);

            startOffset.X = point.X - Canvas.GetLeft(img);
            startOffset.Y = point.Y - Canvas.GetTop(img);
        }


        double eps = 0.0001;
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var canv = (sender as Canvas);
            currPos = e.GetPosition(canv);

            if (mouseDown)
            {

                double newLeft = currPos.X - startOffset.X;
                double newTop = currPos.Y  - startOffset.Y;


                if (Math.Abs(newLeft) > eps && newLeft <= 0)
                    Canvas.SetLeft(img, newLeft);
                if(Math.Abs(newTop) > eps && newTop <= 0)
                    Canvas.SetTop(img, newTop);
                
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
        }
    }
}
