using System;

namespace Calculator
{
    class Trig
    {
        public static double pi = 3.14159265358979;

        public static double Sin(double x)
        {
            if (x == 0)
                return 0;
            else if (Simple.Abs(2 * x / pi - 3) % 4 == 0)
                return -1;
            else if (Simple.Abs(2 * x / pi - 3) % 4 == 2)
                return 1;
            else if (Simple.Abs(x) % pi == 0)
                return 0;
            double y = x;
            if (x < 0)
                y = Math.Round(0 - x / (2.0 * pi)) * 2.0 * pi + x;
            double mod = y % (2.0 * pi);
            if (mod > pi / 2.0 && mod < pi)
                y = pi - y;
            else if (mod > pi && mod < 3.0 * pi / 2.0)
                y -= pi;
            else if (mod > 3.0 * pi / 2.0)
                y = 2.0 * pi - y;
            double x_n = y;
            UInt64 fact = 1;
            double answer = y / (1.0 * fact);
            for (int i = 1; i < 19; i += 2)
            {
                x_n *= y * y;
                fact = fact * (UInt64)((i + 1) * (i + 2));
                answer += Simple.Pow(-1, (int)((i + 1) / 2)) * x_n / (1.0 * fact);
            }
            if (mod > pi)
                return 0 - answer;
            return answer;
        }

        public static double Cos(double x)
        {
            if (x == 0 || Simple.Abs(x / pi) % 2 == 0)
                return 1;
            else if (Simple.Abs(x) % (pi / 2.0) == 0)
                return 0;
            else if (Simple.Abs(x / pi) % 2 == 1)
                return -1;
            double y = x;
            if (x < 0)
                y = Math.Round(0 - x / (2.0 * pi)) * 2.0 * pi + x;
            double mod = y % (2.0 * pi);
            if (mod > pi / 2.0 && mod < pi)
                y = pi - y;
            else if (mod > pi && mod < 3.0 * pi / 2.0)
                y -= pi;
            else if (mod > 3.0 * pi / 2.0)
                y = 2.0 * pi - y;
            double x_n = 1;
            UInt64 fact = 1;
            double answer = x_n / (1.0 * fact);
            for (int i = 1; i < 17; i += 2)
            {
                x_n *= y * y;
                fact = fact * (UInt64)((i) * (i + 1));
                answer += Simple.Pow(-1, (int)((i + 1) / 2)) * x_n / (1.0 * fact);
            }
            if (mod > pi)
                return 0 - answer;
            return answer;
        }

        public static double Tan(double x)
        {
            if ((2.0 * x / pi) % 2 == 1)
                throw new ArgumentException("Can't compute!");
            else if (x % pi == 0)
                return 0.0;
            else if (x > 0)
                return Simple.Sqrt(Simple.Pow(1.0 / Cos(x), 2) - 1);
            return 0 - Simple.Sqrt(Simple.Pow(1.0 / Cos(x), 2) - 1);
        }

        public static double Arcsin(double x)
        {
            if (x < -1 || x > 1)
                throw new ArgumentException("Can't compute!");
            else if (x == -1)
                return pi;
            else if (x == 1)
                return 0.0;
            else if (x == 0)
                return pi / 2.0;
            double y = Simple.Abs(x);
            double x1 = 0.0;
            double x2 = 1.0;
            double xm = .5;
            double y2 = 0.0;
            while (y2 > Simple.Abs(x) + 0.000000000000001 || y2 < Simple.Abs(x) - 0.000000000000001)
            {
                double y1 = Sin(x1);
                y2 = Sin(x2);
                double ym = Sin(xm);
                double m1 = (xm - x1) / (ym - y1);
                double m2 = (x2 - xm) / (y2 - ym);
                if (y > y1 && y < ym)
                    x2 = m1 * (y - y1) + x1;
                else if (y > ym && y < y2)
                    x2 = m2 * (y - ym) + xm;
                xm = (x1 - x2) / 2.0 + x2;
            }
            if (x < 0)
                return 0 - x2;
            return x2;
        }

        public static double Arccos(double x)
        {
            if (x < -1 || x > 1)
                throw new ArgumentException("Can't compute!");
            else if (x == -1)
                return 0 - pi / 2.0;
            else if (x == 1)
                return pi / 2.0;
            else if (x == 0)
                return 0.0;
            double y = Simple.Abs(x);
            double x1 = pi / 2.0;
            double x2 = 0;
            double xm = pi / 4.0;
            double y2 = 0.0;
            while (y2 > Simple.Abs(x) + 0.000000000000001 || y2 < Simple.Abs(x) - 0.000000000000001)
            {
                double y1 = Cos(x1);
                y2 = Cos(x2);
                double ym = Cos(xm);
                double m1 = (xm - x1) / (ym - y1);
                double m2 = (x2 - xm) / (y2 - ym);
                if (y > y1 && y < ym)
                    x2 = m1 * (y - y1) + x1;
                else if (y > ym && y < y2)
                    x2 = m2 * (y - ym) + xm;
                xm = (x1 - x2) / 2.0 + x2;
            }
            if (x < 0)
                return pi - x2;
            return x2;
        }

        public static double Arctan(double x)
        {
            if (x == 0)
                return 0.0;
            double answer = 0.0;
            double y = x;
            if (x > 1)
                y = 1.0 / x;
            double t = y;
            for (int i = 0; i < 30; i++)
            {
                answer += Simple.Pow(-1, (int)(i)) * t / (2 * i + 1);
                t *= y * y;
            }
            if (x > 1)
                return pi / 2.0 - answer;
            return answer;
        }
    }
}
