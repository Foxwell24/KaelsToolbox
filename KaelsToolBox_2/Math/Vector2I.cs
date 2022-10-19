namespace KaelsToolBox_2.Math
{
    public struct Vector2I
    {
        #region Defaults
        public static Vector2I Zero = new Vector2I(0, 0);
        public static Vector2I One = new Vector2I(1, 1);
        public static Vector2I Right = new Vector2I(1, 0);
        public static Vector2I Up = new Vector2I(0, 1);
        #endregion

        public int x { get; set; }
        public int y { get; set; }

        public Vector2I(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Random <see cref="Vector2I"/> with values greater or equal to 0 and less than <see cref="int.MaxValue"/>
        /// </summary>
        public static Vector2I Random(Random random) => new Vector2I(random.Next(), random.Next());
        public static Vector2I Random(Random random, int max) => new Vector2I(random.Next(max), random.Next(max));

        public override bool Equals(object? obj)
        {
            return obj is Vector2I i &&
                   x == i.x &&
                   y == i.y;
        }

        #region Operators
        public static implicit operator Vector2f(Vector2I origin)
            => new Vector2I(origin.x, origin.y);

        public static bool operator ==(Vector2I left, Vector2I right)
            => right.x == left.x && right.y == left.y;
        public static bool operator !=(Vector2I left, Vector2I right)
            => right.x != left.x || right.y != left.y;
        public static Vector2I operator *(Vector2I left, int right)
            => new Vector2I(right * left.x, right * left.y);
        public static Vector2I operator +(Vector2I left, Vector2I right)
            => new Vector2I(left.x + right.x, left.y + right.y);
        public static Vector2I operator -(Vector2I left, Vector2I right)
            => new Vector2I(left.x - right.x, left.y - right.y);
        #endregion
    }
}
