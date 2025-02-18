using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCalc
{
    public class ComandManager
    {
        public static MatrixMemory _matrixMemory;
        private static MatrixCalculator _matrixCalculator;
        private static Matrix? _lastMatrix;
        private static Matrix? _lastExtendedMatrix;

        private Dictionary<string, Func<string[], int>> _comandList = new Dictionary<string, Func<string[], int>>()
        {
            { "addex",  AddExtendedMatrix},
            { "add",  AddMatrix},
            { "del",  DeleteMatrix},
            { "use",  ChooseMatrix},
            { "save",  SaveLastMatrix},

            { "mm", MultiplicationMatrixes},
            { "mn", MultiplicationNumberAndMatrix},
            { "sum", SumMatrixes},
            { "substr", SubstractMatrixes},


            { "det",  FindDetMatrix},                       //      y
            { "getzs",  GetZeroesUnderDiagonal},

            { "show",  ShowAllMatrix},
            { "clr",  ClearConsole},
            { "delall",  DeleteAllMatrixes},
        };


        public ComandManager()
        {
            _matrixMemory = new MatrixMemory();
            _matrixCalculator = new MatrixCalculator();
        }

        public int ProccessCmd(string str)
        {
            if (str == null || str == "")
                return 0;

            List<string> tempList;
            string cmd = null;
            string[] args = null;

            str.ToLower();

            if (str.Contains('/'))
            {
                string[] cmds = str.Split('/');

                for (int i = 0; i < cmds.Length; i++)
                {
                    str = cmds[i];
                    tempList = str.Split(' ').ToList();
                    cmd = tempList[0];
                    tempList.RemoveAt(0);
                    args = tempList.ToArray();
                    if (_comandList.TryGetValue(cmd.ToLower(), out Func<string[], int> func))
                    {
                        func(args);
                    }
                }
                return 0;
            }
            else
            {
                tempList = str.Split(' ').ToList();
                cmd = tempList[0];
                tempList.RemoveAt(0);
                args = tempList.ToArray();
                if (_comandList.TryGetValue(cmd.ToLower(), out Func<string[], int> func))
                {
                    func(args);
                    return 0;
                }
            }

            return -1;
        }

        private static int ClearConsole(string[] args)
        {
            Console.Clear();
            return 0;
        }

        private static int MultiplicationNumberAndMatrix(string[] args)
        {
            if (args.Length == 1 && int.TryParse(args[0], out int num) && _matrixMemory.CurrentMatrix != null)
            {
                Matrix res = _matrixCalculator.MultiplicationOnNum(num, _matrixMemory.CurrentMatrix.Value.Value);
                UpdateLastMatrix(res);
                Matrix.PrintMatrixOnConsole(res);
                return 0;
            }

            return -1;
        }

        private static int SubstractMatrixes(string[] args)
        {
            Matrix? m1 = _matrixMemory.GetMatrix(args[0]);
            Matrix? m2 = _matrixMemory.GetMatrix(args[1]);

            if (args.Length == 2 && Matrix.IsHasOneSize(m1, m2))
            {
                Matrix res = _matrixCalculator.Substract(m1, m2);
                UpdateLastMatrix(res);
                Matrix.PrintMatrixOnConsole(res);
                return 0;
            }
            return -1;
        }

        private static int SumMatrixes(string[] args)
        {
            Matrix? m1 = _matrixMemory.GetMatrix(args[0]);
            Matrix? m2 = _matrixMemory.GetMatrix(args[1]);

            if (args.Length == 2 && Matrix.IsHasOneSize(m1, m2))
            {
                Matrix res = _matrixCalculator.Sum(m1, m2);
                UpdateLastMatrix(res);
                Matrix.PrintMatrixOnConsole(res);
                return 0;
            }
            return -1;
        }

        private static int MultiplicationMatrixes(string[] args)
        {
            if (args.Length != 2) return -1;

            Matrix? m1 = _matrixMemory.GetMatrix(args[0]);
            Matrix? m2 = _matrixMemory.GetMatrix(args[1]);

            if (Matrix.IsCanBeMultiplicationed(m1, m2))
            {
                Matrix res = _matrixCalculator.MultiplicationMatrixes(m1, m2);
                UpdateLastMatrix(res);
                Matrix.PrintMatrixOnConsole(res);
                return 0;
            }

            return -1;
        }

        private static int AddExtendedMatrix(string[] args)
        {
            if (args.Length == 1)
                return _matrixMemory.AddExtended(args[0]);
            return -1;
        }

        private static int AddMatrix(string[] args)
        {
            if (args.Length == 1)
                return _matrixMemory.Add(args[0]);
            return -1;
        }

        private static int DeleteMatrix(string[] args)
        {
            if (args.Length == 1)
                return _matrixMemory.Delete(args[0]);
            return -1;
        }

        private static int DeleteAllMatrixes(string[] args)
        {
            _matrixMemory.ClearListOfMatrixes();
            return 0;
        }

        private static int ChooseMatrix(string[] args)
        {
            if (args.Length == 1)
                return _matrixMemory.ChooseMatrix(args[0]);
            return -1;
        }

        private static int SaveLastMatrix(string[] args)
        {
            if (args.Length == 1 && (_lastMatrix != null || _lastExtendedMatrix != null))
                return _matrixMemory.Add(GetLastMatrix(), args[0], true);
            return -1;
        }


        private static int GetZeroesUnderDiagonal(string[] args)
        {
            Matrix res;
            List<Matrix> steps;
            List<MatrixCalculator.MultiplicationPairNumbers> numsMultiplited;

            if (args.Length == 1)
            {
                if (_matrixMemory.MatrixData.TryGetValue(args[0], out Matrix m))
                {
                    res = _matrixCalculator.CalcZeroes(m, out steps, out numsMultiplited);
                }
                else return -1;
            }
            else if (_matrixMemory.CurrentMatrix != null)
            {
                res = _matrixCalculator.CalcZeroes(_matrixMemory.CurrentMatrix?.Value, out steps, out numsMultiplited);
            }
            else return -1;

            UpdateLastMatrix(res);

            PrintAllStepsWithNumsInConsole(ref steps, ref numsMultiplited);
            Matrix.PrintExtendedMatrixOnConsole(res.ToExtended());


            return 0;
        }

        private static void PrintAllStepsWithNumsInConsole(ref List<Matrix> steps, ref List<MatrixCalculator.MultiplicationPairNumbers> nums)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                ExtendedMatrix extendedMatrix = steps[i].ToExtended();
                Matrix.PrintExtendedMatrixOnConsole(extendedMatrix);
                Console.WriteLine($"cur: {nums[i].ForCurrentLine}, work: {nums[i].ForWorkLine}");
            }
        }

        private static int FindDetMatrix(string[] args)
        {
            double result = 0;
            string name = "";
            Matrix? curMatrix = null;

            if (args.Length == 1 || args.Length == 2)
            {
                if (_matrixMemory.MatrixData.TryGetValue(args[0], out Matrix m))
                {
                    name = args[0];
                    curMatrix = m;
                }
                else return -1;
            }
            else if (_matrixMemory.CurrentMatrix != null)
            {
                name = _matrixMemory.CurrentMatrix?.Key;
                curMatrix = _matrixMemory.CurrentMatrix?.Value;
            }
            else return -1;

            if (!curMatrix.IsSquareMatrix)
                return -1;

            result = _matrixCalculator.CalcDeterminant(curMatrix, out List<Matrix> steps, out List<MatrixCalculator.MultiplicationPairNumbers> nums);

            if (args.Length == 2 && args[1] == UserAdditionalComands.CONFIRM)
            {

            }

            Console.WriteLine($"det {name} = {result}");

            return 0;
        }

        private static int ShowAllMatrix(string[] args)
        {
            Console.WriteLine(new string('-', 40));

            List<KeyValuePair<string, Matrix>> pairs = _matrixMemory.MatrixData.ToList();

            for (int i = 0; i < pairs.Count; i++)
            {
                KeyValuePair<string, Matrix> pair = pairs[i];
                Matrix matrix = pair.Value;

                Console.WriteLine($"name:\t{pair.Key}");

                if (matrix is ExtendedMatrix extended)
                {
                    Matrix.PrintExtendedMatrixOnConsole(extended);
                }
                else
                {
                    Matrix.PrintMatrixOnConsole(matrix);
                }
            }

            Console.WriteLine(new string('-', 40));

            return 0;
        }

        private static void UpdateLastMatrix(Matrix newM)
        {
            _lastExtendedMatrix = null;
            _lastMatrix = null;

            if (newM is ExtendedMatrix newExM)
                _lastExtendedMatrix = newExM;
            else _lastMatrix = newM;
        }

        private static Matrix GetLastMatrix()
        {
            if (_lastMatrix == null)
                return _lastExtendedMatrix;
            return _lastMatrix;
        }

        private static class UserAdditionalComands
        {
            public const string CONFIRM = "y";
            public const string CANCEL = "n";
        }
    }

}
