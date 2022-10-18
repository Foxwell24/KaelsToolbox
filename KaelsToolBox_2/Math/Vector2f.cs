using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Math
{
    public struct Vector2f
    {
        private static Random random = new();

        #region Defaults
        public static Vector2f Zero = new Vector2f(0, 0);
        public static Vector2f One = new Vector2f(1, 1);
        public static Vector2f Right = new Vector2f(1, 0);
        public static Vector2f Up = new Vector2f(0, 1);
        #endregion

        public float x { get; set; }
        public float y { get; set; }

        public Vector2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Random <see cref="Vector2f"/> with values greater or equal to 0.0 and less than 1.0
        /// </summary>
        public static Vector2f Random()
        {
            return new Vector2f(random.NextSingle(), random.NextSingle());
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector2f vector &&
                   x == vector.x &&
                   y == vector.y;
        }

        #region Operators
        public static implicit operator Vector2I(Vector2f origin)
            => new Vector2I((int)origin.x, (int)origin.y);

        public static bool operator ==(Vector2f left, Vector2f right)
            => right.x == left.x && right.y == left.y;
        public static bool operator !=(Vector2f left, Vector2f right)
            => right.x != left.x || right.y != left.y;
        public static Vector2f operator *(Vector2f left, float right)
            => new Vector2f(right * left.x, right * left.y);
        public static Vector2f operator +(Vector2f left, Vector2f right)
            => new Vector2f(left.x + right.x, left.y + right.y);
        public static Vector2f operator -(Vector2f left, Vector2f right)
            => new Vector2f(left.x - right.x, left.y - right.y);
        #endregion
    }
}
