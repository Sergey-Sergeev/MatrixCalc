using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCalc
{
    public class Matrix
    {
        protected int _m;
        protected int _n;
        protected Double[][] _matrix;
        protected readonly bool _isSquareMatrix;

        public Double[][] Data { get => _matrix; set { _matrix = value; } }
        public bool IsSquareMatrix => _isSquareMatrix;
        public int M => _m;
        public int N => _n;

        public Matrix(int m, int n, Double[][] data = null)
        {
            if (m <= 0) m = 1;
            if (n <= 0) n = 1;

            _m = m;
            _n = n;

            if (m == n)
                _isSquareMatrix = true;

            if (data != null && data.GetLength(0) == n && n * m == data.Length)
            {
                _matrix = data;
                return;
            }

            _matrix = new Double[m][];

            for (int i = 0; i < m; i++)
            {
                _matrix[i] = new Double[n];

                for (int j = 0; j < n; j++)
                {
                    if (data == null)
                        _matrix[i][j] = 0;
                    else _matrix[i][j] = data[i][j];
                }
            }
        }

        public static bool IsHasOneSize(Matrix? m1, Matrix? m2)
        {
            if (m1 == null || m2 == null) return false;
            return (m1.N == m2.N) && (m1.M == m2.M);
        }

        public static bool IsCanBeMultiplicationed(Matrix? m1, Matrix? m2)
        {
            if (m1 == null || m2 == null) return false;
            return m1.N == m2.M;
        }

        public static void PrintMatrixOnConsole(Matrix matrix)
        {
            for (int m = 0; m < matrix.M; m++)
            {
                Console.Write("|\t");
                for (int n = 0; n < matrix.N; n++)
                {
                    Console.Write($"{matrix.Data[m][n]}\t");
                }
                Console.WriteLine("|");
            }

            Console.WriteLine();
        }

        public static void PrintExtendedMatrixOnConsole(ExtendedMatrix matrix)
        {
            for (int m = 0; m < matrix.M; m++)
            {
                Console.Write("|\t");
                for (int n = 0; n < matrix.N; n++)
                {
                    Console.Write($"{matrix.Data[m][n]}\t");
                }
                Console.Write($" | {matrix.ExtendData[m]}\t");

                Console.WriteLine("|");
            }

            Console.WriteLine();
        }

        public virtual Matrix Copy()
        {
            return new Matrix(_m, _n, _matrix);
        }

        public ExtendedMatrix ToExtended()
        {
            if (this is ExtendedMatrix ex)
            {
                return ex;
            }

            Matrix copyM = Copy();
            ExtendedMatrix newExMatrix = new ExtendedMatrix(copyM.M, copyM.N, copyM.Data);
            newExMatrix.ExtendData = new Double[copyM.M];

            for (int i = 0; i < copyM.M; i++)            
                newExMatrix.ExtendData[i] = 0;            

            return newExMatrix;
        }

    }

    public class ExtendedMatrix : Matrix
    {
        private Double[] _extendData;

        public ExtendedMatrix(int m, int n, Double[][] data = null) : base(m, n, data)
        {
            _extendData = new Double[m];
        }

        public Double[] ExtendData { get => _extendData; set => _extendData = value; }

        public override ExtendedMatrix Copy()
        {
            ExtendedMatrix m = new ExtendedMatrix(_m, _n, _matrix);
            Array.Copy(_extendData, m._extendData, _extendData.Length);
            return m;
        }
    }
}
