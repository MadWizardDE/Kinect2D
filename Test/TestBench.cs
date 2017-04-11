using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class TestBench
    {
        public static bool Test()
        {
            //Console.WriteLine(Calc(591, 354, 6));
            //Console.WriteLine(Calc(1024, 613, 4));
            //Console.WriteLine(Calc(1024, 595, 4));
            //Console.WriteLine(Calc(682, 1024, 7, 2));

            return true;
        }

        static Size Calc(int width, int height, int minCount, int maxLoss)
        {
            Size size = new Size(1, 1);

            int solutions = 0;
            for (int w = 1; w <= width; w++)
                for (int h = 1; h <= height; h++)
                    if (h != height && w != width)
                        if (width % w <= maxLoss && height % h <= maxLoss)
                        {
                            solutions++;

                            Size check = new Size(w, h);

                            int count = (width / w) * (height / h);

                            Console.WriteLine("[w={0} x h={1}]: c={2}, s={3:0.00}", w, h, count, Score(check));

                            if (count >= minCount)
                            {
                                if (Score(check) > Score(size))
                                    size = check;
                            }
                        }

            Console.WriteLine("solutions: " + solutions);

            return size;
        }

        static double Score(Size size)
        {
            double dist = Math.Abs(size.Width - size.Height);
            double area = size.Width * size.Height;

            return area / (dist + 1.0);
        }
    }
}
