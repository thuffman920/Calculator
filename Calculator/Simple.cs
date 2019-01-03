using System;
using System.Diagnostics;

namespace Calculator
{
    class Simple
    {
        private static double e = 2.718281828459045;

        private static int[] bytes = { 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };

        public static double Max(double x, double y)
        {
            if (x >= y)
                return x;
            return y;
        }

        public static double Min(double x, double y)
        {
            if (x <= y)
                return x;
            return y;
        }

        private static double PowI(double x, int y)
        {
            if (y == 0)
                return 1;
            else if (y == 1)
                return x;
            else if (y == -1)
                return 1 / x;
            return PowI(x, (int)(y / 2)) * PowI(x, (int)(y - y / 2));
        }

        public static double Exp(double x)
        {
            if (x > 2.0)
                return PowI(Exp(x / 2.0), 2);
            else if (x == 2.0)
                return PowI(e, 2);
            double answer = 0.0;
            for (int i = 0; i < 65; i++)
            {
                double iFac = 1.0;
                for (int j = 0; j < i; j++)
                    iFac *= 1.0 / (1.0 * (j + 1));
                answer += iFac * PowI(x, i);
            }
            return answer;
        }

        public static double Ln(double x)
        {
            if (x <= 0)
                throw new ArithmeticException("Undefined!");
            if (x > 2.0)
            {
                for (int i = bytes.Length - 1; i > 0; i--)
                    if (x / (1.0 * bytes[i]) >= bytes[i - 1])
                        return Ln(x / (1.0 * bytes[i])) + ((int)(i) + 1.0) * Ln(2.0);
                return Ln(x / 8.0) + 3.0 * Ln(2.0);
            }
            else if (x == 2.0)
                return 0.6931471805599453;
            else if (x == 1.0)
                return 0;
            double answer = 0.0;
            for (int i = 1; i < 75; i++)
                answer += PowI(-1, i - 1) * PowI(x - 1, i) / (1.0 * i);
            return answer;
        }

        public static double Abs(double x)
        {
            if (x < 0)
                return 0 - x;
            return x;
        }

        public static double Pow(double x, double y)
        {
            if (y - (int)(y) == 0)
                return PowI(x, (int)(y));
            else if (x < 0 && (1 / y) % 2 == 0)
                throw new ArgumentException("Imaginary number!");
            double answer = PowI(Simple.Abs(x), (int)(y));
            double value = (Abs(y) - (int)(Abs(y))) * 10;
            int count = 1;
            while (value > 0)
            {
                answer *= Exp((int)(value) * Ln(Simple.Abs(x)) / PowI(10, count));
                value -= (int)(value);
                value *= 10;
                count += 1;
            }
            if (x < 0)
                return 0 - answer;
            return answer;
        }

        public static string GCD(string x, string y)
        {
            string x_n = x;
            string y_n = y;
            if (x_n[0] == '-')
                x_n = x_n.Remove(0, 1);
            if (y_n[0] == '-')
                y_n = y_n.Remove(0, 1);
            while (x_n[0] == '0')
                x_n = x_n.Remove(0, 1);
            while (y_n[0] == '0')
                y_n = y_n.Remove(0, 1);
            int x_d = 0;
            int y_d = 0;
            bool hasDecX = false;
            bool hasDecY = false;
            if (x_n.IndexOf(".") != -1)
            {
                x_d = x_n.Length - x_n.IndexOf(".") - 1;
                x_n = x_n.Remove(x_n.IndexOf("."), 1);
                hasDecX = true;
            }
            if (y_n.IndexOf(".") != -1)
            {
                y_d = y_n.Length - y_n.IndexOf(".") - 1;
                y_n = y_n.Remove(y_n.IndexOf("."), 1);
                hasDecY = true;
            }
            if (x_d > y_d && hasDecX)
                for (int i = 0; i < x_d - y_d; i++)
                    y_n += "0";
            else if (y_d > x_d && hasDecY)
                for (int i = 0; i < y_d - x_d; i++)
                    x_n += "0";
            try
            {
                UInt64 x_i = UInt64.Parse(x_n);
                UInt64 y_i = UInt64.Parse(y_n);
                String gcd = GCDp(x_i, y_i).ToString();
                if (gcd.Length < Max(x_d, y_d) && (hasDecX || hasDecY))
                {
                    int t = (int)(Max(x_d, y_d)) - gcd.Length;
                    for (int i = 0; i < t; i++)
                        gcd = "0" + gcd;
                }
                if (hasDecY || hasDecX)
                    gcd = gcd.Insert(gcd.Length - (int)(Max(x_d, y_d)), ".");
                return gcd;
            }
            catch (OverflowException)
            {
                throw new ArgumentException("Can't compute gcd!");
            }
        }

        private static UInt64 GCDp(UInt64 x, UInt64 y)
        {
            if (y == 0)
                return x;
            return GCDp(y, x % y);
        }

        public static string Lcm(string x, string y)
        {
            string x_n = x;
            string y_n = y;
            if (x_n[0] == '-')
                x_n = x_n.Remove(0, 1);
            if (y_n[0] == '-')
                y_n = y_n.Remove(0, 1);
            while (x_n[0] == '0')
                x_n = x_n.Remove(0, 1);
            while (y_n[0] == '0')
                y_n = y_n.Remove(0, 1);
            int x_d = 0;
            int y_d = 0;
            bool hasDecX = false;
            bool hasDecY = false;
            if (x_n.IndexOf(".") != -1)
            {
                x_d = x_n.Length - x_n.IndexOf(".") - 1;
                x_n = x_n.Remove(x_n.IndexOf("."), 1);
                hasDecX = true;
            }
            if (y_n.IndexOf(".") != -1)
            {
                y_d = y_n.Length - y_n.IndexOf(".") - 1;
                y_n = y_n.Remove(y_n.IndexOf("."), 1);
                hasDecY = true;
            }
            if (x_d > y_d && hasDecX)
                for (int i = 0; i < x_d - y_d; i++)
                    y_n += "0";
            else if (y_d > x_d && hasDecY)
                for (int i = 0; i < y_d - x_d; i++)
                    x_n += "0";
            try
            {
                UInt64 x_i = UInt64.Parse(x_n);
                UInt64 y_i = UInt64.Parse(y_n);
                String gcd = "" + (x_i * y_i / GCDp(x_i, y_i));
                if (gcd.Length < Max(x_d, y_d) && (hasDecX || hasDecY))
                {
                    int t = (int)(Max(x_d, y_d)) - gcd.Length;
                    for (int i = 0; i < t; i++)
                        gcd = "0" + gcd;
                }
                if (hasDecY || hasDecX)
                {
                    gcd = gcd.Insert(gcd.Length - (int)(Max(x_d, y_d)), ".");
                    while (gcd[gcd.Length - 1] == '0')
                        gcd = gcd.Substring(0, gcd.Length - 1);
                    if (gcd[gcd.Length - 1] == '.')
                        gcd = gcd.Substring(0, gcd.Length - 1);
                }
                return gcd;
            }
            catch (OverflowException)
            {
                throw new ArgumentException("Can't compute gcd!");
            }
        }

        public static UInt64 P(int x, int y)
        {
            if (y > x || y < 0)
                throw new ArgumentException("This is doesn't work!");
            else if (x > 19)
                throw new ArgumentException("This can't be computed!");
            int d = x - y;
            Debug.Print("" + d + "\n");
            UInt64 ex = 1;
            for (int i = d + 1; i <= x; i++)
                ex = ex * (UInt64)(i);
            return ex;
        }

        public static UInt64 C(int x, int y)
        {
            if (y > x || y < 0)
                throw new ArgumentException("This is doesn't work!");
            else if (x > 68)
                throw new ArgumentException("This can't be computed!");
            int d = x - y;
            if (y > d)
                d = y;
            int[] top = new int[x - d];
            for (int i = d + 1; i <= x; i++)
                top[i - d - 1] = i;
            int[] bottom = new int[x - d - 1];
            for (int i = 0; i < x - d - 1; i++)
                bottom[i] = i + 2;
            for (int i = 0; i < bottom.Length; i++)
            {
                String result = "";
                for (int j = 0; j < top.Length; j++)
                    if (!(result = GCD("" + top[j], "" + bottom[i])).Equals("1"))
                    {
                        int t = Int16.Parse(result);
                        top[j] /= t;
                        bottom[i] /= t;
                        if (bottom[i] == 1)
                            break;
                    }
            }
            UInt64 ex = 1;
            for (int i = 0; i < top.Length; i++)
                ex = ex * (UInt64)(top[i]);
            return ex;
        }

        public static double Sqrt(double x)
        {
            if (x < 0)
                throw new ArgumentException("");
            int count = 0;
            int a = 2;
            for (int i = 308; i > -15; i -= 2)
            {
                double b = x / PowI(10, i);
                if (b >= 1 && b < 10)
                {
                    count = i / 2;
                    break;
                }
                else if (b >= 1 && b >= 10)
                {
                    count = i / 2;
                    a = 6;
                    break;
                }
            }
            double y = a * PowI(10, count);
            double x_n = (y + x / y) / 2.0;
            double tol = PowI(10, -12);
            while (1000 * Simple.Abs(y - x_n) >= tol)
            {
                y = x_n;
                x_n = (y + x / y) / 2.0;
            }
            return y;
        }
    }
}
