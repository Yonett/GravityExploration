using System.Collections.Immutable;

namespace GEPre_Post
{
    public abstract class DirectSolver
    {
        public Matrix A => _matrix;
        public Vector? xk { get => _xk; set { _xk = value; } }
        public Vector F => _vector;

        protected Vector _vector = default!;
        protected Matrix _matrix = default!;
        protected Vector _xk = default!;
        public ImmutableArray<double>? Solution => _solution?.ToImmutableArray();
        protected Vector? _solution;

        public void SetVector(Vector vector)
            => _vector = vector;

        public void SetMatrix(Matrix matrix)
            => _matrix = matrix;

        protected DirectSolver(Matrix matrix, Vector vector)
            => (_matrix, _vector) = (matrix, vector);

        protected DirectSolver() { }
        public abstract void Compute();
        public bool IsSolved() => !(Solution is null);
    }
    public static class SMatrixDirect
    {
        public class Gauss : DirectSolver
        {
            public Gauss(Matrix matrix, Vector vector) : base(matrix, vector) { }
            public Gauss() { }

            public override void Compute()
            {
                xk = null;
                try
                {
                    ArgumentNullException.ThrowIfNull(A, $"{nameof(A)} cannot be null, set the Matrix");
                    ArgumentNullException.ThrowIfNull(F, $"{nameof(F)} cannot be null, set the Vector");
                    if (A.Rows != A.Columns)
                        throw new NotSupportedException("The Gaussian method will not be able to solve this system");

                    double eps = 1E-15;

                    for (int k = 0; k < A.Rows; k++)
                    {
                        var max = Math.Abs(A[k, k]);
                        int index = k;

                        for (int i = k + 1; i < A.Rows; i++)
                        {
                            if (Math.Abs(A[i, k]) > max)
                            {
                                max = Math.Abs(A[i, k]);
                                index = i;
                            }
                        }

                        for (int j = 0; j < A.Rows; j++)
                        {
                            (A[k, j], A[index, j]) =
                                (A[index, j], A[k, j]);
                        }

                        (F[k], F[index]) = (F[index], F[k]);

                        for (int i = k; i < A.Rows; i++)
                        {
                            double temp = A[i, k];

                            if (Math.Abs(temp) < eps)
                            {
                                throw new Exception("Zero element of the column");
                            }

                            for (int j = 0; j < A.Rows; j++)
                            {
                                A[i, j] /= temp;
                            }

                            F[i] /= temp;

                            if (i != k)
                            {
                                for (int j = 0; j < A.Rows; j++)
                                {
                                    A[i, j] -= A[k, j];
                                }

                                F[i] -= F[k];
                            }
                        }
                    }
                    xk = new(F.Size);
                    for (int k = A.Rows - 1; k >= 0; k--)
                    {
                        xk![k] = F[k];

                        for (int i = 0; i < k; i++)
                        {
                            F[i] -= A[i, k] * xk[k];
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                _solution = xk;
            }

            // A = LL^T ; A = LU
            // LL^Tx = b ; LUx = b
            // L^Tx = y ; Ux = y
            // Ly = b ; Ly = b
            //public static Vector Compute(Matrix matrix, Vector f) where T : INumber<T>, IRootFunctions<T>
            public static Vector Direct(Matrix matrix, Vector f)
            {
                Vector x = f.Copy();
                double sum = 0.0;
                //T sum = T.Zero;
                int size = f.Size;

                // Ly = b
                for (int i = 0; i < size; i++)
                {
                    sum = 0.0;
                    for (int k = 0; k < i; k++)
                        sum += matrix[i, k] * x[k];
                    //sum += T.CreateChecked(matrix[i, k]) * x[k];
                    //x[i] = (f[i] - sum) / T.CreateChecked(matrix[i, i]);
                    x[i] = (f[i] - sum) / matrix[i, i];
                }

                // Ux = y
                for (int i = size - 1; i >= 0; i--)
                {
                    sum = 0.0;
                    for (int k = i + 1; k < size; k++)
                        sum += matrix[i, k] * x[k];
                    //sum += T.CreateChecked(matrix[i, k]) * x[k];
                    x[i] = (x[i] - sum) / matrix[i, i];
                }
                return x;
            }
        }
    }
}
