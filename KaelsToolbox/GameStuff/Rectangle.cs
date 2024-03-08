using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaelsToolbox.GameStuff
{
    public class Rectangle
    {
        public int X, Y, Width, Height;

        public Rectangle()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }

        public Rectangle(int width, int height)
        {
            X = 0;
            Y = 0;
            Width = width;
            Height = height;
        }

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
