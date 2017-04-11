using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MadWizard.Kinect2D.Tools
{
    public static class Maths
    {
        public static System.Drawing.PointF ToPointF(this Point p)
        {
            return new System.Drawing.PointF { X = (float)p.X, Y = (float)p.Y };
        }

        internal static bool IsReal(this Point point)
        {
            if (double.IsNaN(point.X) || double.IsInfinity(point.X))
                return false;
            if (double.IsNaN(point.Y) || double.IsInfinity(point.Y))
                return false;
            return true;
        }

        internal static double Distance(Point p1, Point p2)
        {
            return Point.Subtract(p2, p1).Length;
        }

        internal static bool CircleCircleIntersection(Point p1, double r1, Point p2, double r2, out Point i1, out Point i2)
        {
            Point invalid = new Point(double.NaN, double.NaN);

            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            double d = Math.Sqrt(dx * dx + dy * dy); // d = |P1-P2|

            if (d > r1 + r2 || d < Math.Abs(r1 - r2) || d == 0 && r1 == r2)
            {
                i1 = i2 = invalid;

                return false;
            }

            double a = (Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(d, 2)) / (2.0 * d);
            double h = Math.Sqrt(Math.Pow(r1, 2) - Math.Pow(a, 2));

            Point p3 = default(Point);
            p3.X = p1.X + a * (p2.X - p1.X) / d;
            p3.Y = p1.Y + a * (p2.Y - p1.Y) / d;

            i1 = new Point(p3.X + h * (p2.Y - p1.Y) / d, p3.Y - h * (p2.X - p1.X) / d);
            i2 = new Point(p3.X - h * (p2.Y - p1.Y) / d, p3.Y + h * (p2.X - p1.X) / d);

            return true;
        }
    }
}
