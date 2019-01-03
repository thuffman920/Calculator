using System;
using System.Diagnostics;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
    class Program
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
                for (int i = 0; i < 65; i++) {
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
                if (x > 2.0) {
                    for (int i = bytes.Length - 1; i > 0; i--)
                        if (x / (1.0 * bytes[i]) >= bytes[i - 1])
                            return Ln(x / (1.0 * bytes[i])) + ((int)(i) + 1.0) * Ln(2.0);
                    return Ln(x / 8.0) + 3.0 * Ln(2.0);
                } else if (x == 2.0)
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
                while (value > 0) {
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
                if (x_n.IndexOf(".") != -1) {
                    x_d = x_n.Length - x_n.IndexOf(".") - 1;
                    x_n = x_n.Remove(x_n.IndexOf("."), 1);
                    hasDecX = true;
                }
                if (y_n.IndexOf(".") != -1) {
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
                    if (gcd.Length < Max(x_d, y_d) && (hasDecX || hasDecY)) {
                        int t = (int)(Max(x_d, y_d)) - gcd.Length;
                        for (int i = 0; i < t; i++)
                            gcd = "0" + gcd;
                    }
                    if (hasDecY || hasDecX)
                        gcd = gcd.Insert(gcd.Length - (int)(Max(x_d, y_d)), ".");
                    return gcd;
                } catch(OverflowException)
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
                    if (gcd.Length < Max(x_d, y_d) && (hasDecX || hasDecY)) {
                        int t = (int)(Max(x_d, y_d)) - gcd.Length;
                        for (int i = 0; i < t; i++)
                            gcd = "0" + gcd;
                    }
                    if (hasDecY || hasDecX) {
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
                for (int i = 0; i < bottom.Length; i++) {
                    String result = "";
                    for (int j = 0; j < top.Length; j++)
                        if (!(result = GCD("" + top[j], "" + bottom[i])).Equals("1")) {
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
                for (int i = 308; i > -15; i -= 2) {
                    double b = x / PowI(10, i);
                    if (b >= 1 && b < 10) {
                        count = i / 2;
                        break;
                    } else if (b >= 1 && b >= 10) {
                        count = i / 2;
                        a = 6;
                        break;
                    }
                }
                double y = a * PowI(10, count);
                double x_n = (y + x / y) / 2.0;
                double tol = PowI(10, -12);
                while (1000 * Simple.Abs(y - x_n) >= tol) {
                    y = x_n;
                    x_n = (y + x / y) / 2.0;
                }
                return y;
            }
        }

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
                for (int i = 1; i < 19; i += 2) {
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
                for (int i = 1; i < 17; i += 2) {
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
                while (y2 > y + 0.000000000000001 ||  y2 < y - 0.000000000000001) {
                    double y1 = Sin(x1);
                    y2 = Sin(x2);
                    double ym = Sin(xm);
                    double m1 = (xm - x1) / (ym - y1);
                    double m2 = (x2 - xm) / (y2 - ym);
                    if (y > y1 && y < ym)
                        x2 = m1 * (y - y1) + x1;
                    else if (y > ym)
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
                while (y2 > Simple.Abs(x) + 0.000000000000001 || y2 < Simple.Abs(x) - 0.000000000000001) {
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
                for (int i = 0; i < 150; i++) {
                    answer += Simple.Pow(-1, (int)(i)) * t / (2 * i + 1);
                    t *= y * y;
                }
                if (x > 1)
                    return pi / 2.0 - answer;
                return answer;
            }
        }

        class Solve
        {
            private string[] left;

            private string[] right;

            public Solve(string equation)
            {
                int index = equation.IndexOf("=");
                if (index == -1)
                    throw new ArgumentException("Can't be computed!");
                left = GetParts(equation.Substring(0, index));
                right = GetParts(equation.Substring(index + 1));
            }

            private string[] GetParts(string line)
            {
                ArrayList list = new ArrayList();
                string[] parts = { "+", "−" };
                int count = 0;
                int parCo = 0;
                string negative = "";
                for (int i = 0; i < line.Length; i++)
                    if (line.ElementAt(i) == ')')
                        parCo--;
                    else if (line.ElementAt(i) == '(')
                        parCo++;
                    else if (parCo == 0 && line.ElementAt(i) == '+') {
                        list.Add(negative + line.Substring(count, i - count));
                        negative = "";
                        count = i + 1;
                    } else if (parCo == 0 && line.ElementAt(i) == '−') {
                        list.Add(negative + line.Substring(count, i - count));
                        negative = "-";
                        count = i + 1;
                    }
                list.Add(negative + line.Substring(count, line.Length - count));
                string[] results = new string[list.Count];
                for (int i = 0; i < list.Count; i++)
                    results[i] = (string)(list[i]);
                return results;
            } 

            private double[] GetCoefficients(string[] parts, double[] coefficients, int side)
            {
                for (int i = 0; i < parts.Length; i++) {
                    string result = parts[i];
                    if (result.IndexOf("arcsin") != -1 && result.IndexOf(")^") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 3, "arcsin", result);
                    else if (result.IndexOf("arccos") != -1 && result.IndexOf(")^") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 4, "arccos", result);
                    else if (result.IndexOf("arctan") != -1 && result.IndexOf(")^") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 5, "arctan", result);
                    else if (result.IndexOf("sin") != -1 && result.IndexOf(")^") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 0, "sin", result);
                    else if (result.IndexOf("cos") != -1 && result.IndexOf(")^") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 1, "cos", result);
                    else if (result.IndexOf("tan") != -1 && result.IndexOf(")^") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 2, "tan", result);
                    else if (result.IndexOf("arcsin") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 9, "arcsin", result);
                    else if (result.IndexOf("arccos") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 10, "arccos", result);
                    else if (result.IndexOf("arctan") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 11, "arctan", result);
                    else if (result.IndexOf("sin") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 6, "sin", result);
                    else if (result.IndexOf("cos") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 7, "cos", result);
                    else if (result.IndexOf("tan") != -1 && coefficients[12] == 0 && coefficients[13] == 0)
                        coefficients = Inside(coefficients, side, 8, "tan", result);
                    else if (result.IndexOf("(x") != -1 && result.IndexOf(")^2") != -1)
                    {
                        double c = 1.0;
                        if (result.IndexOf("(") > 0 && result.ElementAt(0) == '-')
                            c = -1.0;
                        else if (result.IndexOf("(") > 0)
                            c = Double.Parse(result.Substring(0, result.IndexOf("(")));
                        double b = Coefficient(side, "x", result.Substring(result.IndexOf("("), result.IndexOf("x") - result.IndexOf("(")));
                        coefficients[12] += side * c * Simple.Pow(b, 2);
                        double a = Double.Parse(result.Substring(result.IndexOf("x") + 2, result.IndexOf(")^2") - result.IndexOf("x") - 2)); ;
                        if (result.IndexOf("−") != -1)
                            a = 0 - a;
                        coefficients[13] += 2.0 * c * b * a;
                        coefficients[14] += side * c * Simple.Pow(a, 2);
                    }
                    else if (result.IndexOf("x^2") != -1)
                        coefficients[12] += Coefficient(side, "x^2", result);
                    else if (result.IndexOf("x") != -1)
                        coefficients[13] += Coefficient(side, "x", result);
                    else
                        coefficients[14] += side * Double.Parse(result);
                }
                return coefficients;
            }

            private double[] Inside(double[] coefficients, double side, int index, string text, string result)
            {
                coefficients[index] += Coefficient(side, text, result);
                if (result.IndexOf("x^2") != -1) {
                    coefficients[coefficients.Length - 4] += Coefficient(side, "x^2", result.Substring(result.IndexOf(text) + text.Length + 1, result.IndexOf("x^2") + 2 - result.IndexOf(text) - text.Length));
                    coefficients[coefficients.Length - 3] += Coefficient(side, "x", result.Substring(result.IndexOf("x^2") + 4, result.LastIndexOf("x") - result.IndexOf("x^2") - 3));
                    if (result.ElementAt(result.IndexOf("x^2") + 3) == '−')
                        coefficients[coefficients.Length - 3] -= 2.0 * coefficients[coefficients.Length - 3];
                } else {
                    coefficients[coefficients.Length - 3] += Coefficient(side, "x", result.Substring(result.IndexOf(text) + text.Length, result.IndexOf("x") - result.IndexOf(text) - text.Length));
                }
                coefficients[coefficients.Length - 2] = Double.Parse(result.Substring(result.LastIndexOf("x") + 2, result.IndexOf(")") - result.LastIndexOf("x") - 2));
                if (result.ElementAt(result.LastIndexOf("x") + 1) == '−')
                    coefficients[coefficients.Length - 2] -= 2.0 * coefficients[coefficients.Length - 2];
                if (result.IndexOf(")^") != -1)
                    coefficients[coefficients.Length - 5] = Double.Parse(result.Substring(result.IndexOf(")^") + 2));
                return coefficients;
            }

            private double Coefficient(double side, string next, string result)
            {
                if (result.IndexOf(next) == 1 && result.ElementAt(0) == '-')
                    return 0 - side * 1.0;
                else if (result.IndexOf(next) >= 1)
                    return side * Double.Parse(result.Substring(0, result.IndexOf(next)));
                return side * 1.0;
            }

            private delegate double MyFunction(double x);

            public double[] TrigSol()
            {
                double[] pieces = new double[15];
                pieces = GetCoefficients(left, pieces, 1);
                pieces = GetCoefficients(right, pieces, -1);
                for (int i = 0; i < 15; i++)
                    Debug.Print("" + pieces[i]);
                if (pieces[0] != 0 && pieces[1] == 0 && pieces[2] == 0)
                    return TrigAnswers(Trig.Arcsin, pieces, 0, 1);
                else if (pieces[1] != 0 && pieces[0] == 0 && pieces[2] == 0)
                    return TrigAnswers(Trig.Arccos, pieces, 1, 1);
                else if (pieces[2] != 0 && pieces[0] == 0 && pieces[1] == 0)
                    return TrigAnswers(Trig.Arctan, pieces, 2, 1);
                else if (pieces[3] != 0 && pieces[4] == 0 && pieces[5] == 0)
                    return TrigAnswers(Trig.Sin, pieces, 3, 1);
                else if (pieces[4] != 0 && pieces[3] == 0 && pieces[5] == 0)
                    return TrigAnswers(Trig.Cos, pieces, 4, 1);
                else if (pieces[5] != 0 && pieces[3] == 0 && pieces[4] == 0)
                    return TrigAnswers(Trig.Tan, pieces, 5, 1);
                else if (pieces[6] != 0 && pieces[7] == 0 && pieces[8] == 0)
                    return TrigAnswers(Trig.Arcsin, pieces, 6, 0);
                else if (pieces[7] != 0 && pieces[6] == 0 && pieces[8] == 0)
                    return TrigAnswers(Trig.Arccos, pieces, 7, 0);
                else if (pieces[8] != 0 && pieces[6] == 0 && pieces[7] == 0)
                    return TrigAnswers(Trig.Arctan, pieces, 8, 0);
                else if (pieces[9] != 0 && pieces[10] == 0 && pieces[11] == 0)
                    return TrigAnswers(Trig.Sin, pieces, 9, 0);
                else if (pieces[10] != 0 && pieces[9] == 0 && pieces[11] == 0)
                    return TrigAnswers(Trig.Cos, pieces, 10, 0);
                else if (pieces[11] != 0 && pieces[9] == 0 && pieces[10] == 0)
                    return TrigAnswers(Trig.Tan, pieces, 11, 0);

                return null;
            }

            private double[] TrigAnswers(MyFunction f, double[] pieces, int index, int count)
            {
                if (pieces[pieces.Length - 4] != 0 && count == 1 && pieces[pieces.Length - 5] % 2 == 0) {
                    double a = Simple.Pow((0 - pieces[pieces.Length - 1]) / pieces[index], 1.0 / pieces[pieces.Length - 5]);
                    double c1 = pieces[pieces.Length - 2] - f(a);
                    double c2 = pieces[pieces.Length - 2] - f(-a);
                    double d1 = -1.0;
                    if (Simple.Pow(pieces[pieces.Length - 3], 2) - 4.0 * pieces[pieces.Length - 4] * c1 >= 0)
                        d1 = Simple.Sqrt(Simple.Pow(pieces[pieces.Length - 3], 2) - 4.0 * pieces[pieces.Length - 4] * c1);
                    double d2 = -1.0;
                    if (Simple.Pow(pieces[pieces.Length - 3], 2) - 4.0 * pieces[pieces.Length - 4] * c2 >= 0)
                        d2 = Simple.Sqrt(Simple.Pow(pieces[pieces.Length - 3], 2) - 4.0 * pieces[pieces.Length - 4] * c2);
                    if (d1 >= 0 && d2 >= 0) {
                        double[] results = { (0 - pieces[pieces.Length - 3] + d1) / (2.0 * pieces[pieces.Length - 4]),
                                (0 - pieces[pieces.Length - 3] - d1) / (2.0 * pieces[pieces.Length - 4]),
                                (0 - pieces[pieces.Length - 3] + d2) / (2.0 * pieces[pieces.Length - 4]),
                                (0 - pieces[pieces.Length - 3] - d2) / (2.0 * pieces[pieces.Length - 4])};
                        return results;
                    } else if (d1 >= 0) {
                        double[] results = {(0 - pieces[pieces.Length - 3] + d1) / (2.0 * pieces[pieces.Length - 4]),
                                (0 - pieces[pieces.Length - 3] - d1) / (2.0 * pieces[pieces.Length - 4])};
                        return results;
                    } else if (d2 >= 0) {
                        double[] results = {(0 - pieces[pieces.Length - 3] + d2) / (2.0 * pieces[pieces.Length - 4]),
                                (0 - pieces[pieces.Length - 3] - d2) / (2.0 * pieces[pieces.Length - 4])};
                        return results;
                    }
                    throw new ArgumentException("Imaginary number!");
                } else if (pieces[pieces.Length - 4] != 0) {
                    if (pieces[pieces.Length - 5] == 0)
                        pieces[pieces.Length - 5] = 1.0;
                    double c = pieces[pieces.Length - 2] - f(Simple.Pow((0 - pieces[pieces.Length - 1]) / pieces[index], 1.0 / pieces[pieces.Length - 5]));
                    double d = Simple.Sqrt(Simple.Pow(pieces[pieces.Length - 3], 2) - 4.0 * pieces[pieces.Length - 4] * c);
                    double[] results = { (0 - pieces[pieces.Length - 3] + d) / (2.0 * pieces[pieces.Length - 4]),
                        (0 - pieces[pieces.Length - 3] - d) / (2.0 * pieces[pieces.Length - 4])};
                    return results;
                } else if (pieces[pieces.Length - 4] == 0 && count == 1 && (1.0 / pieces[pieces.Length - 5]) % 2 == 0) {
                    double a = Simple.Pow(((0 - pieces[pieces.Length - 1]) / pieces[index]), 1.0 / pieces[pieces.Length - 5]);
                    double[] results = { (f(a) - pieces[pieces.Length - 2]) / pieces[pieces.Length - 3],
                            (f(-a) - pieces[pieces.Length - 2]) / pieces[pieces.Length - 3] };
                    return results;
                } else {
                    if (pieces[pieces.Length - 5] == 0)
                        pieces[pieces.Length - 5] = 1.0;
                    double[] results = { (f(Simple.Pow((0 - pieces[pieces.Length - 1]) / pieces[index], 1.0 / pieces[pieces.Length - 5])) - 
                            pieces[pieces.Length - 2]) / pieces[pieces.Length - 3] };
                    return results;
                }
            }

            private double Maximum()
            {
                double answer = 0.0;
                CalculatorUI ui = null;

                return answer;
            }

            public double Linear()
            {
                double[] pieces = new double[9];
                pieces = GetCoefficients(left, pieces, 1);
                pieces = GetCoefficients(right, pieces, -1);
                if (pieces[pieces.Length - 1] == 0.0 && pieces[pieces.Length - 2] == 0.0)
                    throw new ArgumentException("Infinite many points!");
                else if (pieces[pieces.Length - 2] == 0.0)
                    throw new ArgumentException("Parallel lines!");
                return (0 - pieces[pieces.Length - 1]) / pieces[pieces.Length - 2];
            }

            public double[] Quadratic()
            {
                double[] pieces = new double[9];
                pieces = GetCoefficients(left, pieces, 1);
                pieces = GetCoefficients(right, pieces, -1);
                double[] answer = { 0.0, 0.0 };
                double d = Simple.Pow(pieces[pieces.Length - 2], 2) - 4.0 * pieces[pieces.Length - 3] * pieces[pieces.Length - 1];
                if (d < 0)
                    throw new ArgumentException("Imaginary number!");
                else if (d == 0)
                    d = 0.0;
                else
                    d = Simple.Sqrt(d);
                answer[0] = (0 - pieces[pieces.Length - 2] + d) / (2.0 * pieces[pieces.Length - 3]);
                answer[1] = (0 - pieces[pieces.Length - 2] - d) / (2.0 * pieces[pieces.Length - 3]);
                return answer;
            }

            public double[] PolySolve()
            {
                double[] pieces = new double[15];
                if (left.Length == 1 && right[0].Equals("0")) {
                    int count = 0;
                    for (int i = 0; i < left[0].Length; i++)
                        if (left[0].ElementAt(i) == '(')
                            count++;
                    double[] aResults = new double[count];
                    string full = left[0];
                    int j = 0;
                    while (j <= count) {
                        string next = full.Substring(1, full.IndexOf(")") - 1);
                        int max = 0;
                        if (next.IndexOf("x") != -1)
                            max = 1;
                        for (int i = 0; i < next.Length; i++) {
                            int min = next.Substring(i).IndexOf("+");
                            if (next.Substring(i).IndexOf("−") > -1 && next.Substring(i).IndexOf("−") < min)
                                min = next.Substring(i).IndexOf("−");
                            else if (min == -1)
                                min = next.Substring(i).IndexOf("−");
                            if (next[i] == '^' && max < Int32.Parse("" + next.Substring(i + 1, min - i)))
                                max = Int32.Parse("" + next.Substring(i + 1, min - i));
                        }
                        if (j == 0)
                            aResults = new double[max];
                        else {
                            double[] pResults = new double[aResults.Length + max];
                            for (int i = 0; i < aResults.Length; i++)
                                pResults[i] = aResults[i];
                            aResults = pResults;
                        }
                        if (max == 1) {
                            pieces = new double[15];
                            pieces = GetCoefficients(GetParts(next), pieces, 1);
                            if (pieces[pieces.Length - 2] == 0.0) {
                                double[] newResults = new double[count - 1];
                                for (int i = 0; i < aResults.Length - 1; i++)
                                    if (i < j)
                                        newResults[i] = aResults[i];
                                    else if (i >= j)
                                        newResults[i] = aResults[i + 1];
                                aResults = newResults;
                            } else
                                aResults[j] = (0 - pieces[pieces.Length - 1]) / pieces[pieces.Length - 2];
                        } else if (max == 2) {
                            pieces = new double[15];
                            pieces = GetCoefficients(GetParts(next), pieces, 1);
                            double d = Simple.Sqrt(Simple.Pow(pieces[pieces.Length - 2], 2) - 4.0 * pieces[pieces.Length - 1] * pieces[pieces.Length - 3]);
                            aResults[j++] = (0 - pieces[pieces.Length - 2] + d) / (2.0 * pieces[pieces.Length - 3]);
                            aResults[j] = (0 - pieces[pieces.Length - 2] - d) / (2.0 * pieces[pieces.Length - 3]);
                        }
                        full = full.Substring(full.IndexOf(")^") + 3);
                        j++;
                    }
                    return aResults;
                }
                double[] results = { 0.0 };
                return results;
            }
        }

        class CalculatorUI
        {
            private ArrayList parts;

            private double answer;

            public String[] delimit = {",", "gcd", "(", ")", "lcm", "min", "max", "solve", "sin",
                "cos", "tan", "tan\u02C9\u00B9", "cos\u02C9\u00B9", "sin\u02C9\u00B9", "+", "-",
                "\u2212", "\u00D7", "\u00F7", "\u036F\u221A", "e \u036F", "10 \u036F", "\u221A",
                "abs", "\u03C0", "\u02C4"};

            public CalculatorUI(String calculatorLine)
            {
                parts = new ArrayList();
                answer = 0.0;
                ToParts(calculatorLine);
                OrdOfOperations();
            }

            private void ToParts(String calculatorLine)
            {
                int j = 0;
                while (j < calculatorLine.Length) {
                    for (int i = 0; i < delimit.Length; i++) {
                        if (calculatorLine.Substring(0, j + 1).Equals(delimit[i])) {
                            parts.Add(calculatorLine.Substring(0, j + 1));
                            calculatorLine = calculatorLine.Substring(j + 1);
                            j = -1;
                            break;
                        } else if (j != 0 && Char.IsNumber(Char.Parse(calculatorLine.Substring(j - 1, 1)))
                           && !Char.IsNumber(Char.Parse(calculatorLine.Substring(j, 1)))
                           && !calculatorLine.Substring(j, 1).Equals(".")) {
                            if (calculatorLine.Substring(0, j).IndexOf(".") != -1)
                                parts.Add(Double.Parse(calculatorLine.Substring(0, j)));
                            else
                                parts.Add(Int32.Parse(calculatorLine.Substring(0, j)));
                            calculatorLine = calculatorLine.Substring(j);
                            j = -1;
                            break;
                        }
                    }
                    j++;
                }
                parts.Add(calculatorLine);
            }

            private void OrdOfOperations()
            {
                int o = 0;
                int o_max = 0;
                int c = 0;
                for (int i = 0; i < parts.Count; i++) {
                    if (parts[i].Equals("(")) {
                        o++;
                        c--;
                        o_max = (int)(Simple.Max((double)(o_max), (double)(o)));
                    } else if (parts[i].Equals(")")) {
                        o--;
                        c++;
                        o_max = (int)(Simple.Max((double)(o_max), (double)(o)));
                    }
                }
                for (int i = 0; i < parts.Count; i++) {
                    if (parts[i].Equals("(") && o == o_max && !parts[i - 1].Equals("max")
                        && !parts[i - 1].Equals("min")) {
                        int count = i;
                        double ans2 = 1.0;
                        ArrayList subAns = new ArrayList(); ;
                        while (!parts[count].Equals(")")) {
                            if (parts[count].GetType() == typeof(double) ||
                                parts[count].GetType() == typeof(int)) {
                                if (parts[count - 1].Equals(delimit[14]) || parts[count - 1].Equals(delimit[16])) {
                                    if (ans2 > 0.0 && ans2 != 1.0)
                                        subAns.Add(ans2);
                                    else if (ans2 < 0.0)
                                    ans2 = 1.0;
                                    if (parts[count - 2].Equals(delimit[15])) {
                                        subAns.Add(parts[count - 2]);
                                        subAns.Add(parts[count - 1]);
                                    } else
                                        subAns.Add(parts[count - 1]);
                                    subAns.Add((double)(parts[count]));
                                } else if (parts[count - 2].Equals(delimit[15]) && parts[count - 1].Equals(delimit[17]))
                                    ans2 *= 0 - (double)(parts[count]);
                                else if (parts[count - 2].Equals(delimit[15]) && parts[count - 1].Equals(delimit[18]))
                                    ans2 /= 0 - (double)(parts[count]);
                                else if (parts[count - 1].Equals(delimit[17]))
                                    ans2 *= (double)(parts[count]);
                                else if (parts[count - 1].Equals(delimit[18]))
                                    ans2 /= (double)(parts[count]);
                            }
                        }
                        ans2 = 0.0;
//                        for (int j = 0; j < subAns.Count; j++)
//                            if (subAns[j].Equals(delimit[14]))
                    } else if (parts[i].Equals("("))
                        o++;
                }
//                for (int i = 0; i < parts.Length; i++) {
//                    
//
 //                   if (parts[i].Equals("sin")) {
//
//                    }
//                }
            }

            public double GetAnswer()
            {
                return answer;
            }

            public ArrayList GetParts()
            {
                return this.parts;
            }
        }

        static void Main(string[] args)
        {
            CalculatorUI ui = new CalculatorUI("1+2.4-9.8\u00D7(sin((0.9\u00D7\u03C0)\u00F72.4))");
            ArrayList parts = ui.GetParts();
            Solve solver = new Solve("(x^2−3x−2)^6(x^2+x−6)=0");
            for (int i = 0; i < parts.Count; i++)
                Debug.Print("" + parts[i] + "\n");
            try
            {
                Debug.Print("4^(1/4): " + Simple.Pow(-4, 1.0 / 3.0) + "\n");
                Debug.Print("sqrt(.0225): " + Simple.Sqrt(.0225) + "\n");
                Debug.Print("" + Trig.Arccos(-.560) + "\n");
                int start_time = DateTime.Now.Millisecond;
                double[] solve = solver.PolySolve();
                int end_time = DateTime.Now.Millisecond;
                for (int i = 0; i < solve.Length; i++)
                    Debug.Print("" + solve[i]);
                Debug.Print("Elapsed: " + (end_time - start_time) + " ms");
            }
            catch (ArgumentException g)
            {
                Debug.Print("" + g.Message);
            }
        }
    }
}
