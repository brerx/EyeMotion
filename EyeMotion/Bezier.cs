using System;

namespace EyeMotion
{
    public class Bezier:Curve
    {
        public static double B(int n, int k, double t)
        {
            double tnk, tk, num = 1.0, den = 1.0;
            int b, j;
            n = n - 1;
            tk = Math.Pow(t, k);
            tnk = Math.Pow(1.0 - t, n - k);
            if (n - k > k)
                b = n - k;
            else
                b = k;
            //prefactor part of the combination out, so that the numbers aren't so large
            for (j = b + 1; j <= n; j++)
                num *= j;
            for (j = 1; j <= n - b; j++)
                den *= j;
            return tk*tnk*num/den;
        }

        public override Vector evaluate(Vector[] p, double t)
        {
            Vector r=new Vector();
            int n = p.GetLength(0);
            for (int i = 0; i < n; i++)
                r += B(n, i, t)*p[i];
            return r;
        }
    }
}
