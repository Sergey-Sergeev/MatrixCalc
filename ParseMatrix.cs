using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCalc
{
    public static class ParseMatrix
    {
        static public Matrix Parse()
        {
            int m, n;

            do Console.Write("M:   ");
            while (!int.TryParse(Console.ReadLine(), out m));
            do Console.Write("N:   ");
            while (!int.TryParse(Console.ReadLine(), out n));

            Matrix matrix = new Matrix(m, n);

            for (int i = 0; i < m; i++)
            {
                Console.Write("|\t");
                for (int k = 0; k < n; k++)
                {
                    matrix.Data[i][k] = InputValue();
                    Console.Write("\t");
                }
                Console.WriteLine("|");
            }

            return matrix;
        }

        static public ExtendedMatrix ParseExtended()
        {
            int m, n;

            do Console.Write("M:   ");
            while (!int.TryParse(Console.ReadLine(), out m));
            do Console.Write("N:   ");
            while (!int.TryParse(Console.ReadLine(), out n));

            ExtendedMatrix extended = new ExtendedMatrix(m, n);

            for (int i = 0; i < m; i++)
            {
                Console.Write("|\t");
                for (int k = 0; k < n; k++)
                {
                    extended.Data[i][k] = InputValue();
                    Console.Write("\t");
                }

                Console.Write(" | ");
                extended.ExtendData[i] = InputValue();
                Console.Write("\t");

                Console.WriteLine("|");
            }

            return extended;

        }

        static private Double InputValue()
        {
            string str = "";
            while (true)
            {
                char ch = Console.ReadKey(true).KeyChar;

                if ((ConsoleKey)ch == ConsoleKey.Backspace || (ConsoleKey)ch == ConsoleKey.Delete)
                {
                    Console.Write(new String('\b', str.Length));
                    Console.Write(new String(' ', str.Length));
                    Console.Write(new String('\b', str.Length));
                    str = "";
                }
                else if ((ConsoleKey)ch != ConsoleKey.Enter)
                {
                    str += ch;
                    Console.Write(ch);
                }
                else
                {
                    if (Double.TryParse(str, out Double val))
                    {
                        return val;
                    }
                    else
                    {
                        Console.Write(new String('\b', str.Length));
                        Console.Write(new String(' ', str.Length));
                        Console.Write(new String('\b', str.Length));
                        str = "";
                    }
                }

            }
        }
    }
}
