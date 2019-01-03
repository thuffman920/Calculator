using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class CalculatorUI
    {
        public String[] parts;

        private double answer;

        public String[] delimit = {",", "gcd", "(", ")", "lcm", "min", "max", "solve", "sin",
                "cos", "tan", "tan\u02C9\u00B9", "cos\u02C9\u00B9", "sin\u02C9\u00B9", "+", "-",
                "\u2212", "\u00D7", "\u00F7", "\u036F\u221A", "e \u036F", "10 \u036F", "\u221A",
                "abs", "\u03C0", "\u02C4"};

        public CalculatorUI(String calculatorLine)
        {
            toParts(calculatorLine);
        }

        private void toParts(String calculatorLine)
        {
            int count = 0;
            String[] newParts = new String[calculatorLine.Length];
            int j = 0;
            while (j < calculatorLine.Length) {
                for (int i = 0; i < delimit.Length; i++) {
                    if (calculatorLine.Substring(0, j + 1).Equals(delimit[i])) {
                        parts[count] = calculatorLine.Substring(0, j + 1);
                        count++;
                        calculatorLine = calculatorLine.Substring(j + 1);
                        j = -1;
                        break;
                    } else if (Char.IsNumber(Char.Parse(calculatorLine.Substring(j - 1, j)))
                         && Char.IsLetter(Char.Parse(calculatorLine.Substring(j, j + 1)))) {
                        parts[count] = calculatorLine.Substring(0, j);
                        count++;
                        calculatorLine = calculatorLine.Substring(j);
                        j = -1;
                        break;
                    }
                }
                j++;
            }
            parts[count] = calculatorLine;
            count++;
            parts = new String[count];
            for (int i = 0; i < count; i++)
                parts[i] = newParts[i];
        }

        private void ordOfOperations()
        {
            for (int i = 0; i < parts.Length; i++)
            {


                if (parts[i].Equals("sin"))
                {

                }
            }
        }

        public double getAnswer()
        {
            return answer;
        }
    }
}
