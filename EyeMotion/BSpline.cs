using System;

namespace EyeMotion
{
    public class BSpline:Curve 
    {
        public static double B(int n, double t)
        {
            if (n == 0)
                return 1.0/6.0*Math.Pow(1.0 - t, 3.0);
            if (n == 1)
                return 1.0/6.0*(3.0*t*t*t - 6.0*t*t + 4.0);
            if (n == 2)
                return 1.0/6.0*(-3.0*t*t*t + 3.0*t*t + 3.0*t + 1);
            if (n == 3)
                return 1.0/6.0*Math.Pow(t, 3.0);
            return 0.0;
        }

        public static double N(int n, int i, double t)
        {
            double tj, T = t*(n - 3);
            int b = -1;
            if ((i - 3.0 <= T) && (T < i - 2.0)) b = 3;
            if ((i - 2.0 <= T) && (T < i - 1.0)) b = 2;
            if ((i - 1.0 <= T) && (T < i)) b = 1;
            if ((i <= T) && (T < i + 1.0)) b = 0;
            if (b == -1) return 0.0;
            tj = T - i + b;
            return B(b, tj);
        }

        public override Vector evaluate(Vector[] p, double t)
        {
            Vector r=new Vector();
            int n = p.GetLength(0);
            if (n < 4) return r;
            int j = (int) (t*(n - 3));
            if (j > n - 4) j = n - 4;
            for (int i = j; i <= j + 3; i++)
                r += N(n, i, t)*p[i];
            return r;
        }
    }
}
