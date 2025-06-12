using System.Collections;
using System.Collections.Immutable;

namespace GEPre_Post
{
    public abstract class Tensor
    {
        protected readonly double[,] _storage;
        public bool IsSquareMatrix = false;
        public bool IsTransposed = false;
        public int Rows { get; protected set; }
        public int Columns { get; protected set; }

        protected Tensor(IEnumerable<double> list)
        {
            Rows = list.Count();
            Columns = 1;
            _storage = new double[Rows, Columns];
        }

        protected Tensor(int rows, int columns)
        {
            _storage = new double[rows, columns];
            Rows = rows;
            Columns = columns;
        }
        public void Clear() => Array.Clear(_storage, 0, _storage.Length);

        public static bool IsSquare(Tensor m1)
        {
            if (m1.Rows == m1.Columns) return true;
            return false;
        }

        protected static bool AreProportional(Tensor m1, Tensor m2)
        {
            if (m1.Rows == m2.Rows && m1.Columns == m2.Columns)
                return true;
            return false;
        }

        protected static bool AreMultiplied(Tensor m1, Tensor m2)
        {
            if (m1.Columns == m2.Rows)
                return true;
            return false;
        }

        public double this[int i, int j]
        {
            get
            {
                return _storage[i, j];
                // Здесь без проверки
                //if (i < 0 || i >= Rows || j < 0 || j >= Columns)
                //    throw new IndexOutOfRangeException($"Invalid indices: ({i}, {j}). Matrix dimensions are {Rows}x{Columns}.");
            }
            set
            {
                //if (i < 0 || i >= Rows || j < 0 || j >= Columns)
                //    throw new IndexOutOfRangeException($"Invalid indices: ({i}, {j}). Matrix dimensions are {Rows}x{Columns}.");
                _storage[i, j] = value;
            }
        }
        public double this[int i]
        {
            get
            {
                return _storage[i, 0];
                //
                // Для проверки, но чтобы не было проверки при взятии,
                // нужно зараннее знать как будет повёрнут вектор
                // он всегда будет как (n, 1),
                // а транспонированное состояние определяется через переменную.
                //
                //if (Columns == 1 && i >= 0 && i < Rows)
                //{
                //    return _storage[i, 0];
                //}
                //else if (Rows == 1 && i >= 0 && i < Columns)
                //{
                //    IsTransposed = true;
                //    return _storage[1, 0];
                //}
                //else
                //    throw new IndexOutOfRangeException($"Invalid indicex: ({i}). Vector dimensions are {Rows}x{Columns}.");
            }
            set
            {
                _storage[i, 0] = value;
                //if (Columns == 1 && i >= 0 && i < Rows)
                //{
                //    _storage[i, 0] = value;
                //}
                //else if (Rows == 1 && i >= 0 && i < Columns)
                //{
                //    IsTransposed = true;
                //    _storage[1, 0] = value;
                //}
                //else
                //    throw new IndexOutOfRangeException($"Invalid index: ({i}). Vector dimensions are {Rows}x{Columns}.");
            }
        }
        public abstract Tensor Copy();
        //public abstract Tensor Transpose();
    }

    public class Vector : Tensor, IEnumerable<double>
    {
        public int Size { get; }
        public Vector(int rows) : base(rows, 1)
        {
            IsTransposed = false;
            Size = rows;
        }
        public Vector(IEnumerable<double> list) : base(list)
        {
            IsTransposed = false;
            Size = Rows;
        }

        public override Vector Copy()
        {
            var vec = new Vector(Rows);
            vec.IsTransposed = IsTransposed;
            for (int i = 0; i < Rows; i++)
                vec[i] = _storage[i, 0];
            return vec;
        }

        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < Size; i++)
                yield return this[i];
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ImmutableArray<double> ToImmutableArray()
        {
            return ImmutableArray.Create(MathConverter.FromVectorToArray(this));
        }

        public void Add(IEnumerable<double> collection)
        {
            var enumerable = collection as double[] ?? collection.ToArray();

            if (Size != enumerable.Length)
                throw new ArgumentOutOfRangeException(nameof(collection), "Sizes of vector and collection not equal");

            for (int i = 0; i < Size; i++)
                this[i] = enumerable[i];
        }

        //public static Vector Zero(int size)
        //{
        //    var vector = new Vector(size);
        //    return vector;
        //}

        public static Vector operator -(Vector left)
        {
            var vector = new Vector(left.Size);
            for (int i = 0; i < left.Size; i++)
                vector[i] = -left[i];
            return vector;
        }
        public static Vector operator -(Vector left, Vector right)
        {
            var vector = new Vector(left.Size);
            for (int i = 0; i < left.Size; i++)
                vector[i] = left[i] - right[i];
            return vector;
        }
        public static Vector operator +(Vector left, Vector right)
        {
            var vector = new Vector(left.Size);
            for (int i = 0; i < left.Size; i++)
                vector[i] = left[i] + right[i];
            return vector;
        }
        public static Vector operator *(Vector left, double value)
        {
            var result = new Vector(left.Size);
            for (int i = 0; i < left.Size; ++i)
                result[i] = value * left[i];
            return result;
        }
        public static Vector operator *(double value, Vector left)
        {
            var result = new Vector(left.Size);
            for (int i = 0; i < left.Size; ++i)
                result[i] = value * left[i];
            return result;
        }
    }

    public class Matrix : Tensor, IEnumerable<double>
    {
        public int Size { get; protected set; }
        public Matrix(int size) : base(size, size)
        {
            Size = size;
            IsSquareMatrix = true;
            IsTransposed = false;
        }
        public Matrix(int rows, int columns) : base(rows, columns)
        {
            if (rows == columns)
            {
                IsSquareMatrix = true;
                Size = rows;
            }
            IsTransposed = false;
        }

        public static Matrix IdentityMatrix(int size)
        {
            Matrix matrix = new(size);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (i == j)
                        matrix[i, j] = 1.0;
            return matrix;
        }

        public static Matrix operator +(Matrix l, Matrix r)
        {
            if (!AreProportional(l, r)) throw new Exception($"Invalid sum {l.Rows}-{l.Columns} {r.Rows}-{r.Columns}");
            var result = new Matrix(l.Rows, l.Columns);
            for (int i = 0; i < l.Rows; i++)
                for (int j = 0; j < l.Columns; j++)
                    result[i, j] = l[i, j] + r[i, j];
            return result;
        }

        public static Matrix operator *(double value, Matrix matrix)
        {
            var resultMatrix = new Matrix(matrix.Rows, matrix.Columns);
            for (int i = 0; i < resultMatrix.Rows; i++)
                for (int j = 0; j < resultMatrix.Columns; j++)
                    resultMatrix[i, j] = value * matrix[i, j];
            return resultMatrix;
        }
        public static Vector? operator *(Matrix matrix, Vector vector)
        {
            if (matrix.Columns != vector.Size) return null;
            var result = new Vector(matrix.Rows);
            for (int i = 0; i < matrix.Rows; i++)
            {
                double sum = 0;
                for (int j = 0; j < matrix.Columns; j++)
                    sum += matrix[i, j] * vector[j];
                result[i] = sum;
            }
            return result;
        }

        public static Matrix operator *(Matrix one, Matrix two)
        {
            if (!AreMultiplied(one, two)) throw new Exception($"Invalid multiply {one.Columns}x{two.Rows}");

            Matrix resultMatrix = new(one.Rows, two.Columns);
            double sum = 0;
            for (int i = 0; i < one.Rows; i++)
                for (int k = 0; k < two.Columns; k++)
                {
                    sum = 0;
                    for (int j = 0; j < one.Columns; j++)
                        sum += one[i, j] * two[j, k];
                    resultMatrix[i, k] = sum;
                }
            return resultMatrix;
        }

        //public void Copy(Tensor destination)
        //{
        //    if (!Tensor.AreProportional(this, destination)) return;

        //    for (int i = 0; i < destination.Rows; i++)
        //    {
        //        for (int j = 0; j < destination.Columns; j++)
        //        {
        //            destination[i, j] = _storage[i, j];
        //        }
        //    }
        //}

        public Matrix Transpose()
        {
            IsTransposed = true;
            double tmp;
            for (int i = 0; i < Size; i++)
            {
                for (int j = i + 1; j < Size; j++)
                {
                    tmp = this[j, i];
                    this[j, i] = this[i, j];
                    this[i, j] = tmp;
                }
            }
            return this;
        }

        public override Matrix Copy()
        {
            var mat = new Matrix(Rows, Columns);
            mat.IsTransposed = IsTransposed;
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    mat[i, j] = _storage[i, j];
            return mat;
        }

        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    yield return this[i, j];
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
