using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCalc
{
    public class MatrixMemory
    {
        private Dictionary<string, Matrix> _matrixData;
        private KeyValuePair<string, Matrix>? _currentMatrix;

        public Dictionary<string, Matrix> MatrixData { get => _matrixData; private set => _matrixData = value; }

        public KeyValuePair<string, Matrix>? CurrentMatrix { get => _currentMatrix; private set => _currentMatrix = value; }


        public MatrixMemory()
        {
            _matrixData = new Dictionary<string, Matrix>();
            _currentMatrix = new KeyValuePair<string, Matrix>("0", new Matrix(1, 1, new Double[1][] { new Double[1] { 1 } }));
        }

        public int Add(string name)
        {
            if (_matrixData.ContainsKey(name)) return -1;

            Matrix newMatrix = ParseMatrix.Parse();
            _matrixData.Add(name, newMatrix);
            return 0;
        }

        public int Add(Matrix m, string name, bool makeCheckOnBaseClass = false)
        {
            if (makeCheckOnBaseClass && (m is ExtendedMatrix exm))
                return Add(exm, name);

            if (_matrixData.ContainsKey(name)) return -1;
            _matrixData.Add(name, m);
            return 0;
        }

        public int Add(ExtendedMatrix exm, string name)
        {
            if (_matrixData.ContainsKey(name)) return -1;
            _matrixData.Add(name, exm);
            return 0;
        }

        public int AddExtended(string name)
        {
            if (_matrixData.ContainsKey(name)) return -1;

            ExtendedMatrix newMatrix = ParseMatrix.ParseExtended();
            _matrixData.Add(name, newMatrix);
            return 0;
        }

        public void ClearListOfMatrixes()
        {
            _matrixData.Clear();
        }

        public int Delete(string name)
        {
            if (_matrixData.ContainsKey(name))
            {
                if (_currentMatrix?.Key == name)
                    _currentMatrix = null;

                _matrixData.Remove(name);
                return 0;
            }

            return -1;
        }

        public int ChooseMatrix(string name)
        {
            if (_matrixData.TryGetValue(name, out Matrix m))
            {
                _currentMatrix = new KeyValuePair<string, Matrix>(name, m);
                return 0;
            }
            return -1;
        }

        public Matrix? GetMatrix(string name)
        {
            if (MatrixData.TryGetValue(name, out Matrix res))
            {
                return res;
            }

            return null;
        }

    }
}
