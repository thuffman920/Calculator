using System;
using System.Collections;
using System.Linq;

namespace Calculator
{
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
                else if (parCo == 0 && line.ElementAt(i) == '+')
                {
                    list.Add(negative + line.Substring(count, i - count));
                    negative = "";
                    count = i + 1;
                }
                else if (parCo == 0 && line.ElementAt(i) == '−')
                {
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
            for (int i = 0; i < parts.Length; i++)
            {
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
            if (result.IndexOf("x^2") != -1)
            {
                coefficients[coefficients.Length - 4] += Coefficient(side, "x^2", result.Substring(result.IndexOf(text) + text.Length + 1, result.IndexOf("x^2") + 2 - result.IndexOf(text) - text.Length));
                coefficients[coefficients.Length - 3] += Coefficient(side, "x", result.Substring(result.IndexOf("x^2") + 4, result.LastIndexOf("x") - result.IndexOf("x^2") - 3));
                if (result.ElementAt(result.IndexOf("x^2") + 3) == '−')
                    coefficients[coefficients.Length - 3] -= 2.0 * coefficients[coefficients.Length - 3];
            }
            else
            {
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
            if (pieces[pieces.Length - 4] != 0 && count == 1 && pieces[pieces.Length - 5] % 2 == 0)
            {
                double a = Simple.Pow((0 - pieces[pieces.Length - 1]) / pieces[index], 1.0 / pieces[pieces.Length - 5]);
                double c1 = pieces[pieces.Length - 2] - f(a);
                double c2 = pieces[pieces.Length - 2] - f(-a);
                double d1 = -1.0;
                if (Simple.Pow(pieces[pieces.Length - 3], 2) - 4.0 * pieces[pieces.Length - 4] * c1 >= 0)
                    d1 = Simple.Sqrt(Simple.Pow(pieces[pieces.Length - 3], 2) - 4.0 * pieces[pieces.Length - 4] * c1);
                double d2 = -1.0;
                if (Simple.Pow(pieces[pieces.Length - 3], 2) - 4.0 * pieces[pieces.Length - 4] * c2 >= 0)
                    d2 = Simple.Sqrt(Simple.Pow(pieces[pieces.Length - 3], 2) - 4.0 * pieces[pieces.Length - 4] * c2);
                if (d1 >= 0 && d2 >= 0)
                {
                    double[] results = { (0 - pieces[pieces.Length - 3] + d1) / (2.0 * pieces[pieces.Length - 4]),
                                (0 - pieces[pieces.Length - 3] - d1) / (2.0 * pieces[pieces.Length - 4]),
                                (0 - pieces[pieces.Length - 3] + d2) / (2.0 * pieces[pieces.Length - 4]),
                                (0 - pieces[pieces.Length - 3] - d2) / (2.0 * pieces[pieces.Length - 4])};
                    return results;
                }
                else if (d1 >= 0)
                {
                    double[] results = {(0 - pieces[pieces.Length - 3] + d1) / (2.0 * pieces[pieces.Length - 4]),
                                (0 - pieces[pieces.Length - 3] - d1) / (2.0 * pieces[pieces.Length - 4])};
                    return results;
                }
                else if (d2 >= 0)
                {
                    double[] results = {(0 - pieces[pieces.Length - 3] + d2) / (2.0 * pieces[pieces.Length - 4]),
                                (0 - pieces[pieces.Length - 3] - d2) / (2.0 * pieces[pieces.Length - 4])};
                    return results;
                }
                throw new ArgumentException("Imaginary number!");
            }
            else if (pieces[pieces.Length - 4] != 0)
            {
                if (pieces[pieces.Length - 5] == 0)
                    pieces[pieces.Length - 5] = 1.0;
                double c = pieces[pieces.Length - 2] - f(Simple.Pow((0 - pieces[pieces.Length - 1]) / pieces[index], 1.0 / pieces[pieces.Length - 5]));
                double d = Simple.Sqrt(Simple.Pow(pieces[pieces.Length - 3], 2) - 4.0 * pieces[pieces.Length - 4] * c);
                double[] results = { (0 - pieces[pieces.Length - 3] + d) / (2.0 * pieces[pieces.Length - 4]),
                        (0 - pieces[pieces.Length - 3] - d) / (2.0 * pieces[pieces.Length - 4])};
                return results;
            }
            else if (pieces[pieces.Length - 4] == 0 && count == 1 && (1.0 / pieces[pieces.Length - 5]) % 2 == 0)
            {
                double a = Simple.Pow(((0 - pieces[pieces.Length - 1]) / pieces[index]), 1.0 / pieces[pieces.Length - 5]);
                double[] results = { (f(a) - pieces[pieces.Length - 2]) / pieces[pieces.Length - 3],
                            (f(-a) - pieces[pieces.Length - 2]) / pieces[pieces.Length - 3] };
                return results;
            }
            else
            {
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
            if (left.Length == 1 && right[0].Equals("0"))
            {
                int count = 0;
                for (int i = 0; i < left[0].Length; i++)
                    if (left[0].ElementAt(i) == '(')
                        count++;
                double[] aResults = new double[count];
                string full = left[0];
                int j = 0;
                while (j <= count)
                {
                    string next = full.Substring(1, full.IndexOf(")") - 1);
                    int max = 0;
                    if (next.IndexOf("x") != -1)
                        max = 1;
                    for (int i = 0; i < next.Length; i++)
                    {
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
                    else
                    {
                        double[] pResults = new double[aResults.Length + max];
                        for (int i = 0; i < aResults.Length; i++)
                            pResults[i] = aResults[i];
                        aResults = pResults;
                    }
                    if (max == 1)
                    {
                        pieces = new double[15];
                        pieces = GetCoefficients(GetParts(next), pieces, 1);
                        if (pieces[pieces.Length - 2] == 0.0)
                        {
                            double[] newResults = new double[count - 1];
                            for (int i = 0; i < aResults.Length - 1; i++)
                                if (i < j)
                                    newResults[i] = aResults[i];
                                else if (i >= j)
                                    newResults[i] = aResults[i + 1];
                            aResults = newResults;
                        }
                        else
                            aResults[j] = (0 - pieces[pieces.Length - 1]) / pieces[pieces.Length - 2];
                    }
                    else if (max == 2)
                    {
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
}
