namespace SpaceBattle.Lib
{
    public class Angle
    {
        public int X { get; set; }

        public Angle(int x)
        {
            X = x % 360;
        }

        public static Angle operator +(Angle v1, Angle v2)
        {
            return new Angle(v1.X + v2.X);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Angle newAngle)
            {
                return false;
            }

            return X == newAngle.X;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X);
        }
    }
}
