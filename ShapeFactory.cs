using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Simple_Clicking_Game_WPF
{
    internal class ShapeFactory
    {
        public static Shape CreateCircle(Brush brush)
        {
            Ellipse circle = new Ellipse
            {
                Tag = "circle",
                Height = 10,
                Width = 10,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = brush
            };
            return circle;
        }

        public static Shape CreateRectangle(Brush brush)
        {
            Rectangle rectangle = new Rectangle
            {
                Tag = "cub",
                Name = "game",
                Height = 10,
                Width = 10,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = brush
            };
            return rectangle;
        }
    }
}
