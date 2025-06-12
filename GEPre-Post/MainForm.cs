using System.Drawing;
using Microsoft.VisualBasic;

namespace GEPre_Post
{
    public partial class MainForm : Form
    {
        Cell[,] field;
        int cols = 13;
        int rows = 9;
        int width = 80;
        int height = 40;
        int problemCellWidth = 80;
        int problemCellHeight = 40;

        FontFamily LucidaFontFamily = new FontFamily("Lucida Console");
        Font LucidaFont;

        StringFormat drawFormat = new StringFormat();
        StringFormat verticalFormat = new StringFormat();

        public MainForm()
        {
            field = new Cell[cols, rows];
            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                    field[i, j] = new Cell(row: i, col: j, width: width, height: height);

            drawFormat.Alignment = StringAlignment.Center;
            verticalFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            LucidaFont = new Font(
               LucidaFontFamily,
               14,
               FontStyle.Regular,
               GraphicsUnit.Point);

            InitializeComponent();

            numericUpDown1.Value = problemCellWidth;
            numericUpDown2.Value = problemCellHeight;
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
                        g.FillRectangle(brush, c.rect);
                        if (!c.empty)
                            g.DrawString($"{c.value:e2}", this.Font, Brushes.Black, c.rect, drawFormat);
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

                    this.preField.Invalidate(cell.rect);
                    //this.postField.Invalidate(cell.rect);

                    break;
                }
            }
        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int i = 0; i < cols + 1; i++)
            {
                g.DrawString($"{(problemCellWidth * i):d}", this.Font, Brushes.Black, (i > 0 ? 20 : 0) + width * i, 0);
            }

            for (int i = 1; i < rows + 1; i++)
            {
                g.DrawString($"{(problemCellHeight * i):d}", this.Font, Brushes.Black, 0, 15 + height * i);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            problemCellWidth = (int)numericUpDown1.Value;
            panel1.Invalidate();
            panel2.Invalidate();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            problemCellHeight = (int)numericUpDown2.Value;
            panel1.Invalidate();
            panel2.Invalidate();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString("Input", LucidaFont, Brushes.Black, 0, 185, verticalFormat);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString("Output", LucidaFont, Brushes.Black, 0, 185, verticalFormat);
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
