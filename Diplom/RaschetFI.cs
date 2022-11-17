using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    class RaschetFI
    {
        public List<double[,]> fi;
        List<double[,]> E;
        public static List<int[]> coords;
        public static List<double> charge;
        int Width, Height; 
        public static byte ind = 0;

        public RaschetFI()
        {
            
        }

        public RaschetFI(int width, int height)
        {
            Width = width;
            Height = height;
            fi = new List<double[,]>();
            E = new List<double[,]>();
            coords = new List<int[]>();
            charge = new List<double>();
        }

        public void Raschet(double q, int x, int y, bool key, int id)
        {
            if(key)
                fi.Add(new double[Width, Height]); E.Add(new double[Width, Height]);
            List<double[,]> fi1 = new List<double[,]>();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    fi[id][i, j] = Charge(q * Math.Pow(10, -9), Distance(x, y, i, j));
                }
            }
            if (key)
                ind += 1;
        }
        public void Raschet(double q, int x, int y, double R, bool key, int id)
        {
            if (key)
            {
                fi.Add(new double[Width, Height]);
                E.Add(new double[Width, Height]);
            }
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    fi[id][i, j] = (Sphere(Distance(x, y, i, j), R * 10, q * Math.Pow(10, -9)));
                }
            }
            if (key)
                ind += 1;
        }

        public void Raschet(double q, int x, int y, double R, double h, bool key, int id)
        {
            if (key)
            {
                fi.Add(new double[Width, Height]);
                E.Add(new double[Width, Height]);
            }
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    fi[id][i, j] = Cylinder(Distance(x, y, i, j), R * 10, q * Math.Pow(10, -9), h * 10);
                }
            }
            if (key)
                ind += 1;
        }

        int Distance(int x0, int y0, int x1, int y1)
        {
            return Convert.ToInt32(Math.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1)));
        }

        double Charge(double q, double r)
        {
            return 1 / (4 * (8.85 * Math.Pow(10, -12))) * (q / r);
        }

        double Sphere(double r, double R, double q)
        {
            double S = 4 * Math.PI * R * R;
            double ro = q / S;
            if (r > R)
            {
                return ro * R * R * R / r / (8.85 * Math.Pow(10, -12));
            }
            else
            {
                return -ro * r * r / 6 / (8.85 * Math.Pow(10, -12));
            }
        }

        double Cylinder(double r, double R, double q, double h)
        {
            double S = (2 * Math.PI * R * h) + (2 * Math.PI * R * R);
            double ro = q / S;
            if (r > R)
            {
                return -((ro * R * R) / (2 * (8.85 * Math.Pow(10, -12)))) * Math.Log(r);
            }
            else
            {
                return -(ro * r * r) / (4 * (8.85 * Math.Pow(10, -12)));
            }
        }

    }
}
