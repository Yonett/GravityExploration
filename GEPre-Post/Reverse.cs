namespace GEPre_Post
{
    public class Reverse
    {
        public Mesh mesh;
        public MeshBuilder meshBuilder;
        public MeshParameters meshParameters;

        public IntegrationFEM integrator = default!;
        public DirectSolver directSolver = default!;

        // Solution
        public Point2D[] Epoints;
        public double[] initGz;
        public double[] initDensity;

        public double[] solveGz;
        public double[] solveDensity;
        //public SafeStorage density = new SafeStorage();

        private Point2D[] InitReceivers(int num)
        {
            int start, end, step, count;
            switch (num)
            {
                case 1:
                    //Grid 41,42,43
                    // 800 приёмников
                    // x = [-2000, 6000]
                    start = -2000;
                    end = 6000;
                    count = 800;
                    step = (end - start) / count;
                    break;
                case 2:
                    //Grid 44
                    // 1800 приёмников
                    // x = [-5000, 10000]
                    start = -5000;
                    end = 5000;
                    count = 1000;
                    step = (end - start) / count;
                    break;
                default:
                    //Grid 45
                    // 200 приёмников
                    // x = [-1000, 1000]
                    start = -1000;
                    end = 1000;
                    step = 5;
                    count = (end - start) / step;
                    break;
            }
            Point2D[] Epoints = new Point2D[count];
            for (int i = 0; i < count; i++)
            {
                double value = start + i * step;
                Epoints[i] = new Point2D(value, 0);
            }
            return Epoints;
        }

        public Reverse()
        {
            // 41, 42, 43
            //Epoints = InitReceivers(1);
            // 44
            Epoints = InitReceivers(2);
            // 45
            //Epoints = InitReceivers(3);

            meshParameters = new MeshParameters("input/DM/simpleGrid44.txt");
            //meshParameters = new DMFourthMeshParameters("input/DM/simpleGrid42.txt");
            //meshParameters = new DMFourthMeshParameters("input/DM/simpleGrid43.txt");
            //meshParameters = new DMFourthMeshParameters("input/DM/simpleGrid44.txt");
            //meshParameters = new DMFourthMeshParameters("input/DM/simpleGrid45.txt");

            directSolver = new SMatrixDirect.Gauss();
            integrator = new IntegrationFEM(Quadratures.SegmentGaussOrder5());
            meshBuilder = new MeshBuilder();
            mesh = new Mesh(meshParameters, meshBuilder);

            // Решаем задачу 5 раз:
            // 1. Простая ячеистая структура 41
            initDensity = new double[mesh.Elements.Count];
            initDensity[0] = 1;
            initDensity[1] = 1;
            initDensity[2] = 1;
            initDensity[3] = 1;

            int Nx = meshParameters.SplitsX.Select(s => s.Item1).Sum();
            int Nz = meshParameters.SplitsY.Select(s => s.Item1).Sum();
            int size = Nx * Nz;
            Matrix C = new Matrix(size);
            C.Clear();

            // Сгенерировали G по известному P
            initGz = Compute(initDensity);
            double gzMax = 0;
            for (int i = 0; i < initGz.Length; i++)
            {
                if (Math.Abs(initGz[i]) > gzMax)
                {
                    gzMax = Math.Abs(initGz[i]);
                }
            }
            PrintProfile(initGz, "output/data.txt");

            Console.WriteLine("Alpha: 0");
            ComputeSolver(size, 0, C, gzMax, "output/tableP.txt", "output/basic.txt");
            //Console.WriteLine("Alpha: 1e-1");
            //ComputeSolver(size, 1e-1, C, gzMax, "output/tableP1.txt", "output/reg1.txt");
            //Console.WriteLine("Alpha: 1e-3");
            //ComputeSolver(size, 1e-3, C, gzMax, "output/tableP2.txt", "output/reg2.txt");
            Console.WriteLine("Alpha: 1e-5");
            ComputeSolver(size, 1e-5, C, gzMax, "output/tableP3.txt", "output/reg3.txt");
            Console.WriteLine("Alpha: 1e-7");
            ComputeSolver(size, 1e-7, C, gzMax, "output/tableP4.txt", "output/reg4.txt");
            //Console.WriteLine("Alpha: 1e-10");
            //ComputeSolver(size, 1e-10, C, gzMax, "output/tableP5.txt", "output/reg5.txt");
            Console.WriteLine("Alpha: 1e-14");
            ComputeSolver(size, 1e-14, C, gzMax, "output/tableP6.txt", "output/reg6.txt");

            //Graphics.Render.FuncGraphic("Reverse", "residual.py");

            //Console.WriteLine("Gamma: 1e-4");
            //CMatrix43(C, 1e-4);
            //ComputeSolver(size, 0, C, gzMax, "output/GammaTableP0.txt", "output/GamaReg0.txt");
            //Console.WriteLine("Gamma: 1e-7");
            //CMatrix43(C, 1e-7);
            //ComputeSolver(size, 0, C, gzMax, "output/GammaTableP1.txt", "output/GamaReg1.txt");
            //Console.WriteLine("Gamma: 1e-9");
            //CMatrix43(C, 1e-9);
            //ComputeSolver(size, 0, C, gzMax, "output/GammaTableP2.txt", "output/GamaReg2.txt");
            //Console.WriteLine("Gamma: 1e-12");
            //CMatrix43(C, 1e-12);
            //ComputeSolver(size, 0, C, gzMax, "output/GammaTableP3.txt", "output/GammaReg3.txt");
            //Console.WriteLine("Gamma: 1e-14");
            //CMatrix43(C, 1e-14);
            //ComputeSolver(size, 0, C, gzMax, "output/GammaTableP4.txt", "output/GammaReg4.txt");
        }

        public void CMatrix41(Matrix C, double gamma)
        {
            C.Clear();
            C[0, 0] = gamma * 3 + gamma + gamma + gamma;
            C[0, 1] = C[1, 0] = -(gamma + gamma);
            C[0, 2] = C[2, 0] = -(gamma + gamma);
            C[0, 3] = C[3, 0] = -(gamma + gamma);

            C[1, 1] = gamma * 3 + gamma + gamma + gamma;
            C[1, 2] = C[2, 1] = -(gamma + gamma);
            C[1, 3] = C[3, 1] = -(gamma + gamma);

            C[2, 2] = gamma * 3 + gamma + gamma + gamma;
            C[2, 3] = C[3, 2] = -(gamma + gamma);
            C[3, 3] = gamma * 3 + gamma + gamma + gamma;
        }

        public void ComputeSolver(int size, double alpha, Matrix C, double gzMax, string str1, string str2)
        {
            solveDensity = ComputeReverse(size, alpha, C);
            PrintTableP(str1);
            solveGz = Compute(solveDensity);
            ComputeResidual();
            ComputeProcentResidual(gzMax, str2);
        }

        public void ComputeResidual()
        {
            double sum = 0;
            for (int i = 0; i < Epoints.Length; i++)
            {
                sum += Math.Pow(solveGz[i] - initGz[i], 2);
            }
            Console.WriteLine($"Функцинал невязки: {sum}");
        }

        public void ComputeProcentResidual(double gzMax, string str)
        {
            double[] residualPercent = new double[initGz.Length];
            for (int i = 0; i < initGz.Length; i++)
            {
                double absoluteResidual = solveGz[i] - initGz[i];
                residualPercent[i] = (absoluteResidual / gzMax) * 100.0;
            }
            PrintProfile(residualPercent, str);
        }

        public void PrintTableP(string str)
        {
            using StreamWriter sw = new StreamWriter(str);
            int Nx = meshParameters.SplitsX.Select(s => s.Item1).Sum();
            int Nz = meshParameters.SplitsY.Select(s => s.Item1).Sum();

            for (int j = 0; j < Nz; j++)
            {
                for (int i = 0; i < Nx; i++)
                {
                    sw.WriteLine($"{solveDensity[j * Nx + i]} ");
                }
                sw.WriteLine();
            }
        }

        public void PrintProfile(double[] gZ, string str)
        {
            using StreamWriter sw = new StreamWriter(str);
            for (int i = 0; i < Epoints.Length; i++)
            {
                sw.WriteLine($"{Epoints[i].X} {gZ[i]}");
            }
        }

        public double[] ComputeReverse(int size, double alpha, Matrix C)
        {
            Vector vector = new Vector(size);
            Matrix matrix = new Matrix(size);
            Matrix alphaMatrix = alpha * Matrix.IdentityMatrix(size);

            for (int i = 0; i < size; i++)
            {
                for (int j = i; j < size; j++)
                {
                    var elementI = mesh.Elements[i];
                    var elementJ = mesh.Elements[j];

                    Point2D eleIC = elementI.Area.Center;
                    Point2D eleJC = elementJ.Area.Center;

                    double sum = 0;

                    for (int q = 0; q < Epoints.Length; q++)
                    {
                        Point2D pCI = (Epoints[q].X - eleIC.X, Epoints[q].Y - eleIC.Y);
                        Point2D pCJ = (Epoints[q].X - eleJC.X, Epoints[q].Y - eleJC.Y);

                        var mat1 = (Point2D point) =>
                        {
                            double value1 = 1.0 / (4 * Math.PI);
                            double value2 = MathOperations.Norm([pCI.X, pCI.Y]);
                            double value3 = Math.Pow(value2, 3);
                            double value4 = pCI.Y / value3;
                            return value1 * value4;
                        };

                        var mat2 = (Point2D point) =>
                        {
                            double value1 = 1.0 / (4 * Math.PI);
                            double value2 = MathOperations.Norm([pCJ.X, pCJ.Y]);
                            double value3 = Math.Pow(value2, 3);
                            double value4 = pCJ.Y / value3;
                            return value1 * value4;
                        };

                        double integ1 = integrator.Gauss(mat1, elementI.Area);
                        double integ2 = integrator.Gauss(mat2, elementJ.Area);
                        double result = integ1 * integ2;
                        sum += result;
                    }

                    matrix[i, j] = matrix[j, i] = sum;
                }
            }

            for (int i = 0; i < size; i++)
            {
                var elementI = mesh.Elements[i];

                Point2D eleIC = elementI.Area.Center;
                double sum = 0;

                for (int q = 0; q < Epoints.Length; q++)
                {
                    Point2D pCI = (Epoints[q].X - eleIC.X, Epoints[q].Y - eleIC.Y);

                    var mat1 = (Point2D point) =>
                    {
                        double value1 = 1.0 / (4 * Math.PI);
                        double value2 = MathOperations.Norm([pCI.X, pCI.Y]);
                        double value3 = Math.Pow(value2, 3);
                        double value4 = pCI.Y / value3;
                        return value1 * value4;
                    };

                    double integ1 = integrator.Gauss(mat1, elementI.Area);
                    double integ2 = initGz[q];
                    double result = integ1 * integ2;
                    sum -= result;
                }
                vector[i] = sum;
            }

            directSolver.SetMatrix(matrix + alphaMatrix + C);
            directSolver.SetVector(vector);
            directSolver.Compute();
            return MathConverter.FromArrayToVector(directSolver.Solution).ToArray();
        }

        public double[] Compute(double[] den)
        {
            double[] resultGz = new double[Epoints.Length];

            for (int ielem = 0; ielem < mesh.Elements.Count; ielem++)
            {
                var element = mesh.Elements[ielem];
                Point2D p1 = element.Area.LeftTop;
                Point2D p2 = element.Area.RightTop;
                Point2D p3 = element.Area.RightBottom;
                Point2D p4 = element.Area.LeftBottom;
                double x_c = (p1.X + p2.X + p3.X + p4.X) / 4;
                double y_c = (p1.Y + p2.Y + p3.Y + p4.Y) / 4;

                for (int i = 0; i < Epoints.Length; i++)
                {
                    double xCenter = Epoints[i].X - x_c;
                    double yCenter = Epoints[i].Y - y_c;
                    double pow3 = Math.Pow(MathOperations.Norm([xCenter, yCenter]), 3);
                    double integ = (yCenter / pow3) * element.Area.Square;
                    double val = den[ielem] / (4 * Math.PI);
                    resultGz[i] += val * integ;
                }
            }
            return resultGz;
        }
    }
}

