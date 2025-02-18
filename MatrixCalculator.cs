using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCalc
{
    public class MatrixCalculator
    {
        public double CalcDeterminant(Matrix determinant, out List<Matrix> steps, out List<MultiplicationPairNumbers> usedMultiNums)
        {
            steps = null;
            usedMultiNums = null;
            double res = 1;

            if (determinant.IsSquareMatrix)
            {
                Matrix newMatrix = CalcZeroes(determinant, out steps, out usedMultiNums);

                double numMultiplicatedOnMatrix = 1;

                for (int i = 0; i < usedMultiNums.Count; i++)
                {
                    numMultiplicatedOnMatrix *= usedMultiNums[i].ForWorkLine;
                    Double.Round(numMultiplicatedOnMatrix, 4);
                }

                for (int i = 0; i < determinant.M; i++)
                {
                    res *= newMatrix.Data[i][i];
                }

                res = Double.Round(res / numMultiplicatedOnMatrix, 4);
            }

            return res;
        }

        public Matrix MultiplicationMatrixes(Matrix m1, Matrix m2)
        {
            Matrix res = new Matrix(m1.M, m2.N);

            for (int m = 0; m < res.M; m++)
            {
                for (int n = 0; n < res.N; n++)
                {
                    Double val = 0;

                    for (int k = 0; k < m1.N; k++)
                    {
                        val += Double.Round(m1.Data[m][k] * m2.Data[k][n], 4);
                    }

                    res.Data[m][n] = val;
                }
            }
            return res;
        }

        public Matrix MultiplicationOnNum(int num, Matrix matrix)
        {
            Matrix newM = matrix.Copy();
            for (int m = 0; m < matrix.M; m++)
            {
                for (int n = 0; n < matrix.N; n++)
                {
                    newM.Data[m][n] *= num;
                }
            }
            return newM;
        }

        public Matrix Sum(Matrix m1, Matrix m2)
        {
            Matrix newM = m1.Copy();
            for (int m = 0; m < m1.M; m++)
            {
                for (int n = 0; n < m1.N; n++)
                {
                    newM.Data[m][n] += m2.Data[m][n];
                }
            }
            return newM;
        }

        public Matrix Substract(Matrix m1, Matrix m2)
        {
            return Sum(m1, MultiplicationOnNum(-1, m2));
        }

        public Matrix CalcZeroes(Matrix matrix, out List<Matrix> steps, out List<MultiplicationPairNumbers> usedMultiNums)
        {
            steps = new List<Matrix>();
            usedMultiNums = new List<MultiplicationPairNumbers>();

            if (matrix is ExtendedMatrix extendedMatrix)
            {
                ExtendedMatrix newExtendedMatrix = extendedMatrix.Copy();

                for (int k = 1; k < extendedMatrix.M; k++)
                {
                    List<Double> tempList = extendedMatrix.Data[k - 1].ToList();
                    tempList.Add(extendedMatrix.ExtendData[k - 1]);
                    Double[] curLine = tempList.ToArray();


                    for (int i = k; i < extendedMatrix.M; i++)
                    {
                        tempList = extendedMatrix.Data[i].ToList();
                        tempList.Add(extendedMatrix.ExtendData[i]);
                        Double[] workLine = tempList.ToArray();

                        Double secondValue = workLine[k - 1];
                        Double firstValue = curLine[k - 1];

                        if (secondValue == 0 || firstValue == 0) continue;

                        if (Math.Sign(firstValue) == Math.Sign(secondValue))
                        {
                            firstValue = -firstValue;
                        }
                        else
                        {
                            firstValue = Math.Abs(firstValue);
                            secondValue = Math.Abs(secondValue);
                        }

                        decimal sud = SUD(firstValue, secondValue);

                        firstValue = (Double)(sud / (decimal)firstValue);
                        secondValue = (Double)(sud / (decimal)secondValue);

                        usedMultiNums.Add(new MultiplicationPairNumbers() { ForCurrentLine = firstValue, ForWorkLine = secondValue });

                        tempList = SumLines(curLine, workLine, firstValue, secondValue).ToList();

                        newExtendedMatrix.ExtendData[i] = tempList[tempList.Count - 1];
                        tempList.RemoveAt(tempList.Count - 1);
                        newExtendedMatrix.Data[i] = tempList.ToArray();

                        steps.Add(newExtendedMatrix.Copy());



                        //Matrix.PrintExtendedMatrixOnConsole(newExtendedMatrix);

                        

                    }
                    extendedMatrix = newExtendedMatrix.Copy();
                }

                return extendedMatrix;
            }
            else
            {
                
                return CalcZeroes(matrix.ToExtended(), out steps, out usedMultiNums);
            }
        }

        private decimal SUD(Double v1, Double v2)
        {
            decimal t1 = Math.Abs((decimal)v1);
            decimal t2 = Math.Abs((decimal)v2);

            if (t1 == t2)
                return Math.Abs(t1);

            decimal biggest = t1 * t2;

            for (decimal sud = Math.Min(t1, t2); sud < biggest; sud++)
            {
                if ((sud >= t1) && (sud >= t2) && (sud % t1 == 0) && (sud % t2 == 0))
                {
                    return sud;
                }
            }

            return (t1 * t2);
        }

        private Double[] SumLines(Double[] firstLine, Double[] secondLine, Double firstMulty, Double secondMulty)
        {
            Double[] res = new double[firstLine.Length];

            for (int i = 0; i < firstLine.Length; i++)
            {
                res[i] = Double.Round((firstLine[i] * firstMulty) + (secondLine[i] * secondMulty), 6);
            }

            return res;
        }

        public Matrix Calc(Matrix matrix)
        {
            return matrix;
        }
    
        public class MultiplicationPairNumbers
        {
            public double ForCurrentLine = 0;
            public double ForWorkLine = 0;
        }
    }
}
