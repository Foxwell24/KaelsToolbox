using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KaelsToolbox.Math
{
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public Vector3(Vector2 x_y, float z)
        {
            X = x_y.X;
            Y = x_y.Y;
            Z = z;
        }
        public Vector3(Vector2 x_y, int z)
        {
            X = x_y.X;
            Y = x_y.Y;
            Z = z;
        }
        public Vector3(Vector2 x_y)
        {
            X = x_y.X;
            Y = x_y.Y;
            Z = 0;
        }

        public static Vector3 Zero()
        {
            return new Vector3(0, 0, 0);
        }
    }
}
