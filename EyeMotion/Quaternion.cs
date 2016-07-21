using System;

namespace EyeMotion
{
    public class Quaternion
    {
        //w - real, x, y, z - imaginary
        public double x, y, z, w;

        public const double eps = 1e-7;
        public const double pi = Math.PI;
        public const double halfpi = (pi/2.0);

        public Quaternion()
        {
            x = 0.0;
            y = 0.0;
            z = 0.0;
            w = 1.0;
        }

        public Quaternion(Vector axis, double theta)
        {
            Vector a = axis;
            double s, c;
            a.normalize();
            s = Math.Sin(pi*theta/180.0/2.0);
            c = Math.Cos(pi*theta/180.0/2.0);
            x = a.x*s;
            y = a.y*s;
            z = a.z*s;
            w = c;
        }

        public Quaternion(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public object Clone()
        {
            object o = new Quaternion(x, y, z, w);
            return o;
        }

        public override bool Equals(object b)
        {
            if ((((Quaternion) b).x == x) && (((Quaternion) b).y == y) &&
                (((Quaternion) b).z == z) && (((Quaternion) b).w == w))
                return true;
            else return false;
        }

        public static Quaternion operator +(Quaternion a, Quaternion v)
        {
            return new Quaternion(a.x + v.x, a.y + v.y, a.z + v.z, a.w + v.w);
        }

        public static Quaternion operator -(Quaternion a, Quaternion v)
        {
            return new Quaternion(a.x - v.x, a.y - v.y, a.z - v.z, a.w - v.w);
        }

        public static Quaternion operator *(Quaternion a, Quaternion r)
        {
            Quaternion m = new Quaternion();
            m.x = a.y*r.z - a.z*r.y + r.w*a.x + a.w*r.x;
            m.y = a.z*r.x - a.x*r.z + r.w*a.y + a.w*r.y;
            m.z = a.x*r.y - a.y*r.x + r.w*a.z + a.w*r.z;
            m.w = a.w*r.w - a.x*r.x - a.y*r.y - a.z*r.z;
            return m;
        }

        public static Quaternion operator +(Quaternion a, double c)
        {
            return new Quaternion(a.x, a.y, a.z, a.w + c);
        }

        public static Quaternion operator -(Quaternion a, double c)
        {
            return new Quaternion(a.x, a.y, a.z, a.w - c);
        }

        public static Quaternion operator *(Quaternion a, double c)
        {
            return new Quaternion(c*a.x, c*a.y, c*a.z, c*a.w);

        }

        public static Quaternion operator *(double c, Quaternion v)
        {
            return new Quaternion(v.x*c, v.y*c, v.z*c, v.w*c);
        }

        public static Quaternion operator +(double x, Quaternion v)
        {
            return new Quaternion(v.x, v.y, v.z, v.w + x);
        }

        public static Quaternion operator -(double x, Quaternion v)
        {
            return new Quaternion(-v.x, -v.y, -v.z, x - v.w);
        }

        public static Quaternion operator /(Quaternion a, double c)
        {
            return new Quaternion(a.x/c, a.y/c, a.z/c, a.w/c);
        }

        //unary minus
        public static Quaternion operator -(Quaternion a)
        {
            return new Quaternion(-a.x, -a.y, -a.z, -a.w);
        }

        //unary plus
        public static Quaternion operator +(Quaternion a)
        {
            return a;
        }

        public Matrix toMatrix()
        {
            double s, xs, ys, zs, wx, wy, wz, xx, xy, xz, yy, yz, zz;
            double div = x*x + y*y + z*z + w*w;
            Matrix mat=new Matrix();
            if (div != 0.0)
                s = 2.0/(div);
            else
                s = 0.0;
            xs = x*s;
            ys = y*s;
            zs = z*s;
            wx = w*xs;
            wy = w*ys;
            wz = w*zs;
            xx = x*xs;
            xy = x*ys;
            xz = x*zs;
            yy = y*ys;
            yz = y*zs;
            zz = z*zs;
            mat[0, 0] = 1.0 - (yy + zz);
            mat[1, 0] = xy + wz;
            mat[2, 0] = xz - wy;
            mat[0, 1] = xy - wz;
            mat[1, 1] = 1.0 - (xx + zz);
            mat[2, 1] = yz + wx;
            mat[0, 2] = xz + wy;
            mat[1, 2] = yz - wx;
            mat[2, 2] = 1.0 - (xx + yy);
            mat[0, 3] = 0.0;
            mat[1, 3] = 0.0;
            mat[2, 3] = 0.0;
            mat[3, 3] = 1.0;
            mat[3, 0] = 0.0;
            mat[3, 1] = 0.0;
            mat[3, 2] = 0.0;
            return mat;
        }

        public Quaternion conj()
        {
            return new Quaternion(-x, -y, -z, w);
        }

        public static Quaternion slerp(Quaternion p, Quaternion q, double t)
        {
            double omega, cosom, sinom, sclp, sclq;
            Quaternion qt = new Quaternion();
            if (p.dist(q) > p.dist(-q))
                q = -q;
            cosom = p.x*q.x + p.y*q.y + p.z*q.z + p.w*q.w;
            if ((1.0 + cosom) > eps)
            {
                if ((1.0 - cosom) > eps)
                {
                    omega = Math.Acos(cosom);
                    sinom = Math.Sin(omega);
                    sclp = Math.Sin((1.0 - t)*omega)/sinom;
                    sclq = Math.Sin(t*omega)/sinom;
                }
                else
                {
                    sclp = 1.0 - t;
                    sclq = t;
                }
                qt.x = sclp*p.x + sclq*q.x;
                qt.y = sclp*p.y + sclq*q.y;
                qt.z = sclp*p.z + sclq*q.z;
                qt.w = sclp*p.w + sclq*q.w;
            }
            else
            {
                qt.x = -p.y;
                qt.y = p.x;
                qt.z = -p.w;
                qt.w = p.z;
                sclp = Math.Sin((1.0 - t)*halfpi);
                sclq = Math.Sin(t*halfpi);
                qt.x = sclp*p.x + sclq*qt.x;
                qt.y = sclp*p.y + sclq*qt.y;
                qt.z = sclp*p.z + sclq*qt.z;
            }
            return qt;
        }

        public double dist(Quaternion q)
        {
            return (this - q).norm();
        }

        public double norm()
        {
            return Math.Sqrt(x*x + y*y + z*z + w*w);
        }

        public void normalize()
        {
            double s = norm();
            x /= s;
            y /= s;
            z /= s;
            w /= s;
        }

        public override string ToString()
        {
            return String.Format("({0},{1},{2},{3})", x, y, z, w);
        }
    }
}
