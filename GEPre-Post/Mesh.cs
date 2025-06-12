using System.Collections.Immutable;

namespace GEPre_Post
{
    public class Mesh
    {

        public int MeshSize { get; set; }
        public IReadOnlyList<Point2D> Points { get; }
        public IReadOnlyList<FiniteElement<Point2D, Rect>> Elements { get; }
        public IReadOnlyList<FiniteElement<Point2D, Rect>> ElementsExtended { get; }
        public MeshParameters MeshParameters { get; }

        public Mesh(MeshParameters? parameters, MeshBuilder meshBuilder)
        {
            MeshParameters = parameters;
            (Points, Elements, ElementsExtended) = meshBuilder.Build(parameters);
        }
    }

    public readonly record struct AreaAdvance2D(int Material, int nxb, int nxe, int nyb, int nye)
    {
        public static AreaAdvance2D Parse(string line)
        {
            if (!TryParse(line, out var area))
                throw new FormatException("Cant parse Area!");
            return area;
        }

        public static bool TryParse(string line, out AreaAdvance2D area)
        {
            var data = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length != 5 || !int.TryParse(data[0], out var number) ||
                !int.TryParse(data[1], out var x1) || !int.TryParse(data[2], out var x2) ||
                !int.TryParse(data[3], out var y1) || !int.TryParse(data[4], out var y2))
            {
                area = default;
                return false;
            }
            area = new(number, x1, x2, y1, y2);
            return true;
        }
    }

    public class MeshParameters
    {
        protected int _no;
        protected int _kx, _ky;
        protected AreaAdvance2D[] _areas;
        protected double[,] _X;
        protected double[,] _Y;
        protected (int, double)[] _splitsX;
        protected (int, double)[] _splitsY;

        public int Nx => SplitsX.Select(s => s.Item1).Sum() + 1;
        public int Ny => SplitsY.Select(s => s.Item1).Sum() + 1;
        public ImmutableArray<AreaAdvance2D> Areas => _areas.ToImmutableArray();
        public double[,] X => _X;
        public double[,] Y => _Y;

        public ImmutableArray<(int, double)> SplitsX => _splitsX.ToImmutableArray();
        public ImmutableArray<(int, double)> SplitsY => _splitsY.ToImmutableArray();

        public MeshParameters(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException("File does not exist", path);

            using var sr = new StreamReader(path);

            // Чтение Kx, Ky
            int[]? line_params = sr.ReadLine()!.Split().Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();
            _kx = line_params[0];
            _ky = line_params[1];

            // Чтение координат X и Y
            _X = new double[_ky, _kx];
            _Y = new double[_ky, _kx];
            for (int i = 0; i < _ky; i++)
            {
                var linec = sr.ReadLine()!.Split().Where(s => !string.IsNullOrWhiteSpace(s)).Select(double.Parse).ToArray();

                for (int j = 0; j < _kx; j++)
                {
                    _X[i, j] = linec[j * 2];
                    _Y[i, j] = linec[j * 2 + 1];
                }
            }

            // Чтение No
            _no = int.Parse(sr.ReadLine()!.Trim());

            // Чтение областей
            _areas = new AreaAdvance2D[_no];
            for (int i = 0; i < _no; i++)
            {
                _areas[i] = AreaAdvance2D.Parse(sr.ReadLine()!);
            }

            // Чтение разбиений по X
            var line = sr.ReadLine()!.Split().Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            _splitsX = new (int, double)[_kx - 1];
            for (int i = 0; i < _kx - 1; i++)
                _splitsX[i] = (int.Parse(line[i * 2]), double.Parse(line[i * 2 + 1]));

            // Чтение разбиений по Y
            line = sr.ReadLine()!.Split().Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            _splitsY = new (int, double)[_ky - 1];
            for (int i = 0; i < _ky - 1; i++)
                _splitsY[i] = (int.Parse(line[i * 2]), double.Parse(line[i * 2 + 1]));
        }
    }

    public class MeshBuilder
    {
        public int ElementSize => 4;

        public (IReadOnlyList<Point2D>, FiniteElement<Point2D, Rect>[], FiniteElement<Point2D, Rect>?[]) Build(MeshParameters? meshParameters)
        {
            int ElementSize = 4;

            int Nx = meshParameters.SplitsX.Select(s => s.Item1).Sum() + 1;
            int Ny = meshParameters.SplitsY.Select(s => s.Item1).Sum() + 1;
            int N = Nx * Ny;

            var nodes = new Point2D[N];

            int baseNy = meshParameters.X.GetLength(0);
            int baseNx = meshParameters.X.GetLength(1);

            double[,] tempX_coords = BuildCoordsX(baseNx, baseNy, Nx, meshParameters.SplitsX, meshParameters.X);
            double[,] tempY_coords = BuildCoordsY(baseNx, baseNy, Ny, meshParameters.SplitsY, meshParameters.Y);

            double[,] x_coords = ConcatCoordX(Nx, Ny, tempX_coords, meshParameters.SplitsY);
            double[,] y_coords = ConcatCoordY(Nx, Ny, tempY_coords, meshParameters.SplitsX);

            int nodeIndex = 0;
            for (int j = 0; j < Ny; j++)
            {
                for (int i = 0; i < Nx; i++)
                {
                    nodes[nodeIndex++] = new Point2D(x_coords[j, i], y_coords[j, i]);
                }
            }

            int numElements = (Nx - 1) * (Ny - 1);
            var elements = new FiniteElement<Point2D, Rect>[numElements];
            int elementIndex = 0;

            for (int j = 0; j < Ny - 1; j++)
            {
                for (int i = 0; i < Nx - 1; i++)
                {
                    int[] elementNodes = new int[ElementSize];
                    elementNodes[0] = i + j * Nx;
                    elementNodes[1] = (i + 1) + j * Nx;
                    elementNodes[2] = i + (j + 1) * Nx;
                    elementNodes[3] = (i + 1) + (j + 1) * Nx;

                    Point2D[] elementPoints = new Point2D[ElementSize];
                    for (int p = 0; p < ElementSize; p++)
                    {
                        elementPoints[p] = nodes[elementNodes[p]];
                    }

                    Rect area = new Rect(elementPoints);

                    int subAreaNumber = -1;
                    for (int a = 0; a < meshParameters.Areas.Length; a++)
                    {
                        int xb = (meshParameters.Areas[a].nxb - 1);
                        int xe = (meshParameters.Areas[a].nxe - 1);
                        int yb = (meshParameters.Areas[a].nyb - 1);
                        int ye = (meshParameters.Areas[a].nye - 1);
                        int nxb = meshParameters.SplitsX.Select((s, index) => index < xb ? s.Item1 : 0).Sum();
                        int nxe = meshParameters.SplitsX.Select((s, index) => index < xe ? s.Item1 : 0).Sum();

                        int nyb = meshParameters.SplitsY.Select((s, index) => index < yb ? s.Item1 : 0).Sum();
                        int nye = meshParameters.SplitsY.Select((s, index) => index < ye ? s.Item1 : 0).Sum();

                        if (nxb <= i && i < nxe &&
                            nyb <= j && j < nye)
                        {
                            subAreaNumber = meshParameters.Areas[a].Material;
                            break;
                        }
                    }

                    // 2.5 Создание конечного элемента
                    elements[elementIndex++] = new FiniteElement<Point2D, Rect>(
                        elementNodes,
                        elementPoints,
                        area,
                        subAreaNumber,
                        null,
                        null
                    );
                }
            }
            return (nodes, elements, null);
        }

        static double[,] BuildCoordsY(int kX, int kY, int numY, ImmutableArray<(int, double)> splits, double[,] lines)
        {
            double[,] coords = new double[numY, kX];
            int currentIndex = 0;

            for (int i = 0; i < splits.Length; i++)
            {
                int n = splits[i].Item1;
                double c = splits[i].Item2;

                for (int j = 0; j < kX; j++)
                {
                    double y1 = lines[i, j];
                    double y2 = lines[i + 1, j];
                    double h = (y2 - y1) / (c == 1.0 ? n : (Math.Pow(c, n) - 1) / (c - 1));

                    for (int k = 0; k < n; k++)
                    {
                        coords[currentIndex + k, j] = y1 + (c == 1.0 ? k * h : h * (Math.Pow(c, k) - 1) / (c - 1));
                    }
                }
                currentIndex += n;
            }
            for (int j = 0; j < kX; j++)
            {
                coords[currentIndex, j] = lines[kY - 1, j];
            }
            return coords;
        }
        static double[,] BuildCoordsX(int kX, int kY, int numX, ImmutableArray<(int, double)> splits, double[,] lines)
        {
            double[,] coords = new double[kY, numX];
            int currentIndex = 0;

            for (int i = 0; i < kY; i++)
            {
                currentIndex = 0;
                for (int j = 0; j < splits.Length; j++)
                {
                    int n = splits[j].Item1;
                    double c = splits[j].Item2;

                    double x1 = lines[i, j];
                    double x2 = lines[i, j + 1];
                    double h = (x2 - x1) / (c == 1.0 ? n : (Math.Pow(c, n) - 1) / (c - 1));

                    for (int k = 0; k < n; k++)
                    {
                        coords[i, currentIndex + k] = x1 + (c == 1.0 ? k * h : h * (Math.Pow(c, k) - 1) / (c - 1));
                    }
                    currentIndex += n;
                }
            }
            for (int j = 0; j < kY; j++)
            {
                coords[j, currentIndex] = lines[j, kX - 1];
            }
            return coords;
        }
        static double[,] ConcatCoordY(int Nx, int Ny, double[,] Y, ImmutableArray<(int, double)> splitsX)
        {
            double[,] coords = new double[Ny, Nx];
            int currentIndex = 0;
            for (int i = 0; i < Ny; i++)
            {
                currentIndex = 0;
                for (int j = 0; j < splitsX.Length; j++)
                {
                    int n = splitsX[j].Item1;
                    double c = splitsX[j].Item2;
                    double y1 = Y[i, j];
                    double y2 = Y[i, j + 1];
                    double h = (y2 - y1) / (c == 1.0 ? n : (Math.Pow(c, n) - 1) / (c - 1));
                    for (int q = 0; q < n; q++)
                    {
                        //coords[i, currentIndex + q] = Y[i, j];
                        coords[i, currentIndex + q] = y1 + (c == 1.0 ? q * h : h * (Math.Pow(c, q) - 1) / (c - 1));
                    }
                    currentIndex += n;
                }
            }
            for (int j = 0; j < Ny; j++)
            {
                coords[j, currentIndex] = Y[j, Y.GetLength(1) - 1];
            }
            return coords;
        }
        static double[,] ConcatCoordX(int Nx, int Ny, double[,] X, ImmutableArray<(int, double)> splitsY)
        {
            double[,] coords = new double[Ny, Nx];
            int currentIndex = 0;
            for (int i = 0; i < splitsY.Length; i++)
            {
                int n = splitsY[i].Item1;
                double c = splitsY[i].Item2;
                for (int j = 0; j < Nx; j++)
                {
                    for (int q = 0; q < n; q++)
                    {
                        double x1 = X[i, j];
                        double x2 = X[i + 1, j];
                        double h = (x2 - x1) / (c == 1.0 ? n : (Math.Pow(c, n) - 1) / (c - 1));
                        //coords[currentIndex + q, j] = X[i, j];
                        coords[currentIndex + q, j] = x1 + (c == 1.0 ? q * h : h * (Math.Pow(c, q) - 1) / (c - 1));
                    }
                }
                currentIndex += n;
            }
            for (int j = 0; j < Nx; j++)
            {
                coords[currentIndex, j] = X[X.GetLength(0) - 1, j];
            }
            return coords;
        }
    }

    public readonly record struct Point2D(double X, double Y)
    {
        public static implicit operator Point2D((double, double) value) => new(value.Item1, value.Item2);
        public static Point2D operator +(Point2D a, Point2D b) => new(a.X + b.X, a.Y + b.Y);
        public static Point2D operator -(Point2D a, Point2D b) => new(a.X - b.X, a.Y - b.Y);
        public static Point2D operator *(Point2D p, double value) => new(p.X * value, p.Y * value);
        public static Point2D operator /(Point2D p, double value) => new(p.X / value, p.Y / value);
        public static Point2D operator +(Point2D p, (double, double) value) => new(p.X + value.Item1, p.Y + value.Item2);
        public static Point2D operator -(Point2D p, (double, double) value) => new(p.X - value.Item1, p.Y - value.Item2);
        public static double Distance(Point2D a, Point2D b) =>
            Math.Sqrt((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));

        public static Point2D Parse(string line)
        {
            var words = line.Split(new[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length != 2 ||
                !double.TryParse(words[0], out var x) ||
                !double.TryParse(words[1], out var y))
                return default;
            return new(x, y);
        }

        public static bool TryParse(string line, out Point2D point)
        {
            var words = line.Split(new[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length != 2 || !float.TryParse(words[1], out var x) || !float.TryParse(words[2], out var y))
            {
                point = default;
                return false;
            }

            point = new(x, y);
            return true;
        }
    }
    public readonly record struct Rect
    {
        public Point2D LeftBottom { get; }
        public Point2D RightTop { get; }

        public Point2D LeftTop { get; }
        public Point2D RightBottom { get; }

        public Rect(Point2D leftBottom, Point2D rightTop)
        {
            LeftBottom = leftBottom;
            RightTop = rightTop;
            LeftTop = new Point2D(LeftBottom.X, RightTop.Y);
            RightBottom = new Point2D(RightTop.X, LeftBottom.Y);
        }

        public Rect(Point2D[] points)
        {
            LeftBottom = points[0];
            RightBottom = points[1];
            LeftTop = points[2];
            RightTop = points[3];
        }

        public Rect(Point2D leftBottom, Point2D rightBottom, Point2D leftTop, Point2D rightTop)
        {
            LeftBottom = leftBottom;
            RightBottom = rightBottom;
            LeftTop = leftTop;
            RightTop = rightTop;
        }

        public Point2D Center => ((LeftBottom.X + RightBottom.X) / 2.0, (RightBottom.Y + RightTop.Y) / 2.0);
        public double Square => Math.Abs((RightTop.X - LeftBottom.X) * (RightTop.Y - LeftBottom.Y));
        public double LengthX => Math.Abs(LeftBottom.X - RightBottom.X);
        public double LengthY => Math.Abs(RightBottom.Y - RightTop.Y);
        public bool IsContain(Point2D point) => point.X >= LeftBottom.X && point.X <= RightBottom.X &&
                                                point.Y >= RightBottom.Y && point.Y <= RightTop.Y;
        public static bool TryParse(string line, out Rect rect)
        {
            var words = line.Split(new[] { ' ', ',', '[', ']', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length != 4 || !float.TryParse(words[0], out var x1) || !float.TryParse(words[1], out var y1) ||
                !float.TryParse(words[2], out var x2) || !float.TryParse(words[3], out var y2))
            {
                rect = default;
                return false;
            }

            rect = new(new(x1, y1), new(x2, y2));
            return true;
        }
    }

    public class FiniteElement<TypePoint, TypeArea>
    {
        // Нумерация каждой подобласти
        public int SubAreaNumber { get; }

        public TypeArea Area;

        //public ImmutableArray<int> Nodes { get; }
        public List<(int, (int, int))>? Edges { get; }
        public List<(int, (int, int, int, int))>? Faces { get; }
        public IReadOnlyList<int>? Nodes { get; }
        public IReadOnlyList<TypePoint> Points { get; }

        public FiniteElement(IReadOnlyList<int> nodes, IReadOnlyList<TypePoint> points,
                             TypeArea area, int area_num,
                             List<(int, (int, int))> edges,
                             List<(int, (int, int, int, int))> faces)
            => (Nodes, Points, Area, SubAreaNumber, Edges, Faces) = (nodes, points, area, area_num, edges, faces);
        public FiniteElement(IReadOnlyList<int> nodes, IReadOnlyList<TypePoint> points, TypeArea area, int area_num)
            => (Nodes, Points, Area, SubAreaNumber) = (nodes, points, area, area_num);
        public FiniteElement(IReadOnlyList<int> nodes, IReadOnlyList<TypePoint> points, int area_num)
            => (Nodes, Points, Area, SubAreaNumber) = (nodes, points, default!, area_num);
        public FiniteElement(IReadOnlyList<int> nodes, IReadOnlyList<TypePoint> points)
            => (Nodes, Points, Area, SubAreaNumber) = (nodes, points, default!, -1);
        public FiniteElement(IReadOnlyList<int> nodes, int area_num)
            => (Nodes, Points, Area, SubAreaNumber) = (nodes, default!, default!, area_num);
        public FiniteElement(int[] nodes, int area_num)
            => (Nodes, Points, Area, SubAreaNumber) = (nodes.ToImmutableArray(), default!, default!, area_num);
    }

}
