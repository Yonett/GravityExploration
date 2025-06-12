namespace GEPre_Post
{
    public class IntegrationFEM
    {
        private readonly IEnumerable<QuadratureNode<double>> _quadratures;
        public IntegrationFEM(IEnumerable<QuadratureNode<double>> quadratures) => _quadratures = quadratures;

        public double Gauss(Func<Point2D, double> psi, Rect element)
        {
            double hx = (element.RightTop.X - element.LeftTop.X) / 2.0;
            double hy = (element.RightTop.Y - element.RightBottom.Y) / 2.0;
            double x = (element.LeftBottom.X + element.RightBottom.X) / 2.0;
            double y = (element.RightBottom.Y + element.RightTop.Y) / 2.0;

            double result = (from qi in _quadratures
                             from qj in _quadratures
                             let point = new Point2D(qi.Node * hx + x,
                                                     qj.Node * hy + y)
                             select psi(point) * qi.Weight * qj.Weight
                             ).Sum();

            //double sum = 0.0;
            //foreach (var qi in _quadratures)
            //    foreach (var qj in _quadratures)
            //        sum += qi.Weight * qj.Weight * psi((qi.Node * hx + x,
            //                                            qj.Node * hy + y));

            // h делится на 4
            return result * hx * hy;
        }
    }

    public class QuadratureNode<T> where T : notnull
    {
        public T Node { get; }
        public double Weight { get; }
        public QuadratureNode(T node, double weight)
        {
            Node = node;
            Weight = weight;
        }
    }

    public static class Quadratures
    {
        public static IEnumerable<QuadratureNode<double>> SegmentGaussOrder5()
        {
            const int n = 3;
            double[] points =
            {
                0,
                -Math.Sqrt(3.0 / 5.0),
                Math.Sqrt(3.0 / 5.0)
            };
            double[] weights =
            {
                8.0 / 9.0,
                5.0 / 9.0,
                5.0 / 9.0
            };
            for (int i = 0; i < n; i++)
            {
                yield return new(points[i], weights[i]);
            }
        }

        public static IEnumerable<QuadratureNode<double>> SegmentGaussOrder7()
        {
            const int n = 4;

            double[] points =
            {
                Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                -Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                -Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0))
            };

            double[] weights =
            {
                18.0 + Math.Sqrt(30.0) / 36.0,
                18.0 + Math.Sqrt(30.0) / 36.0,
                18.0 - Math.Sqrt(30.0) / 36.0,
                18.0 - Math.Sqrt(30.0) / 36.0,
            };

            for (int i = 0; i < n; i++)
            {
                yield return new QuadratureNode<double>(points[i], weights[i]);
            }
        }

        public static IEnumerable<QuadratureNode<double>> SegmentGaussOrder9()
        {
            const int n = 5;
            double[] points =
            {
                0.0,
                1.0 / 3.0 * Math.Sqrt(5 - 2 * Math.Sqrt(10.0 / 7.0)),
                -1.0 / 3.0 * Math.Sqrt(5 - 2 * Math.Sqrt(10.0 / 7.0)),
                1.0 / 3.0 * Math.Sqrt(5 + 2 * Math.Sqrt(10.0 / 7.0)),
                -1.0 / 3.0 * Math.Sqrt(5 + 2 * Math.Sqrt(10.0 / 7.0))
            };

            double[] weights =
            {
                128.0 / 225.0,
                (322.0 + 13.0 * Math.Sqrt(70.0)) / 900.0,
                (322.0 + 13.0 * Math.Sqrt(70.0)) / 900.0,
                (322.0 - 13.0 * Math.Sqrt(70.0)) / 900.0,
                (322.0 - 13.0 * Math.Sqrt(70.0)) / 900.0
            };

            for (int i = 0; i < n; i++)
            {
                yield return new(points[i], weights[i]);
            }
        }
    }
}
