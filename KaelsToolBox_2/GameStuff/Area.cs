using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.GameStuff
{
    public class Area
    {
        List<IShape> shapes = new();

        public void AddShape(IShape shape) => shapes.Add(shape);
        public void RemoveShape(IShape shape) => shapes.Remove(shape);

        public bool Inside(Point cordinate)
        {
            bool inside = false;
            foreach (IShape shape in shapes)
            {
                if (shape is Circle circle)
                {
                    inside = Circle.Contains(cordinate, circle);
                }
                else if (shape is Rectangle rectangle)
                {
                    inside = Rectangle.Contains(cordinate, rectangle);
                }
                if (inside) break;
            }
            return inside;
        }
    }

    public class Rectangle : IShape
    {
        public Point center { get; set; }

        public Size size { get; set; }

        public static bool Contains(Point cordinate, Rectangle rectangle)
        {
            bool xInside = MathF.Abs(cordinate.X - rectangle.center.X) <= rectangle.size.Width/2;
            bool yInside = MathF.Abs(cordinate.Y - rectangle.center.Y) <= rectangle.size.Height/2;

            return xInside && yInside;
        }
    }

    public class Circle : IShape
    {
        public Point center { get; set; }

        public Size size { get; private set; }

        public float radius { get => radius; set { size.Height = value; size.Width = value; radius = value; } }

        public static bool Contains(Point cordinate, Circle circle)
        {
            return Point.Distance(circle.center, cordinate) <= circle.radius;
        }
    }
}
