using System;

namespace EyeMotion
{
    public class Matrix
    {
        private double[,] mat;

        //constructor
        public Matrix()
        {
            //create identity matrix
            int row, col;
            mat = new double[4, 4];
            for(row=0;row<4;row++)
                for(col=0;col<4;col++)
                    if (row == col)
                        mat[row, col] = 1.0;
                    else
                        mat[row, col] = 0.0;
        }

        public object Clone()
        {
            Matrix m=new Matrix();
            int row, col;
            for(row=0;row<4;row++)
                for (col = 0; col < 4; col++)
                    m[row, col] = mat[row, col];
            return (object) m;
        }

        public override bool Equals(object b)
        {
            Matrix m = (Matrix) b;
            int row, col;
            for (row = 0; row < 4; row++)
                for (col = 0; col < 4; col++)
                    if (m[row, col] != mat[row, col])
                        return false;
            return true;
        }

        //index operator, to retrieve and set values in the matrix
        public double this[int row, int col]
        {
            get { return mat[row, col]; }
            set { mat[row, col] = value; }
        }

        //arithmetic operators
        public static Matrix operator +(Matrix a, Matrix b)
        {
            Matrix r=new Matrix();
            int row, col;
            for (row = 0; row < 4; row++)
                for (col = 0; col < 4; col++)
                    r[row, col] = a[row, col] + b[row, col];
            return r;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            Matrix r = new Matrix();
            int row, col;
            for (row = 0; row < 4; row++)
                for (col = 0; col < 4; col++)
                    r[row, col] = a[row, col] - b[row, col];
            return r;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            Matrix r = new Matrix();
            int row, col;
            for (row = 0; row < 4; row++)
                for (col = 0; col < 4; col++)
                {
                    r[row, col] = 0;
                    for (int i = 0; i < 4; i++)
                        r[row, col] += a[row, i]*b[i, col];
                }
            return r;
        }

        public static Vector operator *(Matrix a, Vector v)
        {
            Vector r = new Vector();
            r.x = v.x * a[0, 0] + v.y * a[0, 1] + v.z * a[0, 2] + v.w * a[0, 3];
            r.y = v.x * a[1, 0] + v.y * a[1, 1] + v.z * a[1, 2] + v.w * a[1, 3];
            r.z = v.x * a[2, 0] + v.y * a[2, 1] + v.z * a[2, 2] + v.w * a[2, 3];
            r.w = v.x * a[3, 0] + v.y * a[3, 1] + v.z * a[3, 2] + v.w * a[3, 3];
            return r;
        }

        public Matrix Transpose()
        {
            Matrix m = new Matrix();
            int row, col;
            for (row = 0; row < 4; row++)
                for (col = 0; col < 4; col++)
                    m[row, col] = mat[col, row];
            return m;
        }

        public static Matrix Translate(Vector t)
        {
            Matrix m = new Matrix();
            m[0, 3] = t.x;
            m[1, 3] = t.y;
            m[2, 3] = t.z;
            return m;
        }

        public static Matrix Scale(double s)
        {
            Matrix m = new Matrix();
            m[0, 0] = s;
            m[1, 1] = s;
            m[2, 2] = s;
            return m;
        }

        public static Matrix Scale(double sx, double sy, double sz)
        {
            Matrix m = new Matrix();
            m[0, 0] = sx;
            m[1, 1] = sy;
            m[2, 2] = sz;
            return m;
        }

        //general rotation around the vector axis
        public static Matrix Rotate(Vector axis, double angle)
        {
            Matrix r = new Matrix();
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);
            axis.normalize();
            r[0, 0] = (1.0 - c)*axis.x*axis.x + c;
            r[1, 0] = (1.0 - c)*axis.x*axis.y + s*axis.z;
            r[2, 0] = (1.0 - c)*axis.x*axis.z - s*axis.y;
            r[3, 0] = 0.0;
            r[0, 1] = (1.0 - c)*axis.x*axis.y - s*axis.z;
            r[1, 1] = (1.0 - c)*axis.y*axis.y + c;
            r[2, 1] = (1.0 - c)*axis.y*axis.z + s*axis.x;
            r[3, 1] = 0.0;
            r[0, 2] = (1.0 - c)*axis.x*axis.z + s*axis.y;
            r[1, 2] = (1.0 - c)*axis.y*axis.z - s*axis.x;
            r[2, 2] = (1.0 - c)*axis.z*axis.z + c;
            r[3, 2] = 0.0;
            r[0, 3] = 0.0;
            r[1, 3] = 0.0;
            r[2, 3] = 0.0;
            r[3, 3] = 1.0;
            return r;
        }

        public static Matrix Projection(double n, double f, 
            double t, double b, double l, double r)
        {
            Matrix m = new Matrix();
            m[0, 0] = 2.0*n/(r - 1);
            m[1, 0] = 0.0;
            m[2, 0] = 0.0;
            m[3, 0] = 0.0;
            m[0, 1] = 0.0;
            m[1, 1] = 2.0*n/(t - b);
            m[2, 1] = 0.0;
            m[3, 1] = 0.0;
            m[0, 2] = 0.0;
            m[1, 2] = 2.0*n/(t - b);
            m[2, 2] = -(f + n)/(f - n);
            m[3, 2] = -1.0;
            m[0, 3] = (r + 1)/(r - 1);
            m[1, 3] = (t + b)/(t - b);
            m[2, 3] = -2.0*f*n/(f - n);
            m[3, 3] = 0.0;
            return m;
        }

        //method to display the matrix
        public override string ToString()
        {
            String s = "\n";
            for (int i = 0; i < 4; i++)
                s += String.Format("[{0},{1},{2},{3}]\n",
                    mat[i, 0], mat[i, 1], mat[i, 2], mat[i, 3]);
            return s;
        }
    }
}
