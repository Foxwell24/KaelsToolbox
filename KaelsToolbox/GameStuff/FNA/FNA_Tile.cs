using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KaelsToolbox.Math;
using Microsoft.Xna.Framework.Graphics;

namespace KaelsToolbox.GameStuff.FNA
{
    public class FNA_Tile
    {
        public string Name;
        public Texture2D Texture;
        public Rectangle RectangeToolbox;
        public Microsoft.Xna.Framework.Rectangle RectangleFNA;

        public void Move(Microsoft.Xna.Framework.Vector2 direction)
        {
            RectangeToolbox.X += (int)direction.X;
            RectangeToolbox.Y += (int)direction.Y;
            RectangleFNA.X += RectangeToolbox.X;
            RectangleFNA.Y += RectangeToolbox.Y;
        }
    }
}
