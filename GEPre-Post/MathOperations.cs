using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEPre_Post
{

    public static class  MathOperations
    {
        public static double Norm(double[] vector) => Math.Sqrt(vector.Select(x => x * x).Sum());
    }
    public static class MathConverter
    {
        public static Vector FromArrayToVector(ImmutableArray<double>? vec)
        {
            if (vec == null) throw new ArgumentNullException(nameof(vec));
            Vector result = new(vec.Value.Length);
            for (int i = 0; i < vec.Value.Length; i++)
                result[i] = vec.Value[i];
            return result;
        }
        public static double[] FromVectorToArray(Vector vec)
        {
            double[] result = new double[vec.Size];
            for (int i = 0; i < vec.Size; i++)
                result[i] = vec[i];
            return result;
        }
    }
}
