using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Math
{
    public struct Vector2
    {
        private static Random random = new();

        #region Defaults
        public static Vector2 Zero = new Vector2(0, 0);
        public static Vector2 One = new Vector2(1, 1);
        public static Vector2 Right = new Vector2(1, 0);
        public static Vector2 Up = new Vector2(0, 1);
        #endregion

        public float x { get; set; }
        public float y { get; set; }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Random <see cref="Vector2"/> with values greater or equal to 0.0 and less than 1.0
        /// </summary>
        public static Vector2 Random()
        {
            return new Vector2(random.NextSingle(), random.NextSingle());
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector2 vector &&
                   x == vector.x &&
                   y == vector.y;
        }

        #region Operators
        public static Vector2 operator *(Vector2 left, float right)
            => new Vector2(right * left.x, right * left.y);
        public static Vector2 operator +(Vector2 left, Vector2 right)
            => new Vector2(left.x + right.x, left.y + right.y);
        public static Vector2 operator -(Vector2 left, Vector2 right)
            => new Vector2(left.x - right.x, left.y - right.y);
        #endregion
    }
}
