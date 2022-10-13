using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KaelsToolbox.Input
{
    public static class Mouse
    {
        static MouseState mouse;

        static void Update()
        {
            mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }
        public static Vector2 GetVector2XNA()
        {
            Update();
            return new Vector2(mouse.X, mouse.Y);
        }
        public static Math.Vector2 GetVector2()
        {
            Update();
            return new Math.Vector2(mouse.X, mouse.Y);
        }

        public static Point GetPoint()
        {
            Update();
            return new Point(mouse.X, mouse.Y);
        }

        ///<summary>
        ///Returns [LeftButton, RightButton].
        ///</summary>
        public static bool[] IsPressed()
        {
            Update();
            bool left = false, right = false;

            if (mouse.LeftButton == ButtonState.Pressed)
                left = true;
            if (mouse.RightButton == ButtonState.Pressed)
                right = true;

            return new bool[] { left, right };
        }
    }
}
