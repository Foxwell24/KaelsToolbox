using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KaelsToolbox.Math
{
    public class Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        public Point GetPoint()
        {
            return new Point((int)X, (int)Y);
        }

        public static Vector2 Zero()
        {
            return new Vector2(0, 0);
        }

        public static float Distance(Vector2 value1, Vector2 value2)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            return (float)System.Math.Sqrt(num * num + num2 * num2);
        }
        public static Vector2 Divide(Vector2 value1, Vector2 value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }
    }
}
