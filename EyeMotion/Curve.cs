namespace EyeMotion
{
    public abstract class Curve
    {
        public abstract Vector evaluate(Vector[] p, double t);
    }

}
