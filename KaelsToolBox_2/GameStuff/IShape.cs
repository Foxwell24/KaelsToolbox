using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.GameStuff
{
    public interface IShape
    {
        Point center { get; }
        Size size { get; }
    }

    public class Point
    {
        public float X;
        public float Y;

        public static float Distance(Point point1, Point point2)
        {
            float disX = point1.X - point2.X;
            float disY = point1.Y - point2.Y;
            return MathF.Sqrt(disX * disX + disY * disY);
        }
    }

    public class Size
    {
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
