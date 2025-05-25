using System.Drawing;
using Microsoft.VisualBasic;

namespace GEPre_Post
{
    public partial class Form1 : Form
    {
        Cell[,] field = new Cell[10, 10];
        int cols = 10;
        int rows = 10;
        int width = 80;
        int height = 40;

        StringFormat drawFormat = new StringFormat();

        public Form1()
        {
            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                    field[i, j] = new Cell(row: i, col: j, width: width, height: height);

            drawFormat.Alignment = StringAlignment.Center;

            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (Cell c in field)
            {
                if (c.active)
                {
                    using (var brush = new SolidBrush(c.color))
                    {
                        e.Graphics.FillRectangle(brush, c.rect);
                        if (!c.empty)
                            e.Graphics.DrawString($"{c.value:e2}", this.Font, Brushes.Black, c.rect, drawFormat);
                    }
                }
                else
                {
                    using (var pen = new Pen(c.color, 2))
                    {
                        g.DrawRectangle(pen, c.rect);
                    }
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (Cell c in field)
            {
                if (c.active)
                {
                    using (var brush = new SolidBrush(c.color))
                    {
                        e.Graphics.FillRectangle(brush, c.rect);
                        if (!c.empty)
                            e.Graphics.DrawString($"{c.value:e2}", this.Font, Brushes.Black, c.rect, drawFormat);
                    }
                }
                else
                {
                    using (var pen = new Pen(c.color, 2))
                    {
                        g.DrawRectangle(pen, c.rect);
                    }
                }
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var cell in field)
            {
                if (cell.rect.Contains(e.Location))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        cell.active = !cell.active;
                        cell.empty = true;
                    }
                    if (e.Button == MouseButtons.Right)
                    {
                        if (cell.active)
                        {
                            cell.empty = !cell.empty;
                            if (!cell.empty)
                            {
                                string input = Interaction.InputBox("Введите число:", "Числовой ввод", "");

                                if (input == String.Empty)
                                {
                                    ;
                                }
                                else if (double.TryParse(input, out double number))
                                {
                                    cell.value = number;
                                }
                                else
                                {
                                    MessageBox.Show("Некорректный ввод! Пожалуйста, введите число.", "Ошибка");
                                }
                            }
                        }
                                
                    }

                    this.panel1.Invalidate(cell.rect);
                    this.panel2.Invalidate(cell.rect);

                    break;
                }
            }
        }
    }

    public class Cell
    {
        public int row;
        public int col;

        public Rectangle rect;

        public Color color = Color.Gray;

        public bool active = false;

        public bool empty = true;

        public double value = 0.0;

        public Cell(int row, int col, int width, int height)
        {
            this.row = row;
            this.col = col;
            this.rect = new Rectangle(row * width, col * height, width, height);
        }
    }
}
