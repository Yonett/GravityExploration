namespace GEPre_Post
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            preField = new Panel();
            postField = new Panel();
            panel5 = new Panel();
            panel1 = new Panel();
            panel6 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            groupBox4 = new GroupBox();
            numericUpDown4 = new NumericUpDown();
            groupBox3 = new GroupBox();
            numericUpDown3 = new NumericUpDown();
            groupBox2 = new GroupBox();
            numericUpDown2 = new NumericUpDown();
            groupBox1 = new GroupBox();
            numericUpDown1 = new NumericUpDown();
            button1 = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // preField
            // 
            preField.Location = new Point(33, 26);
            preField.Name = "preField";
            preField.Size = new Size(1089, 386);
            preField.TabIndex = 0;
            preField.Paint += panel1_Paint;
            preField.MouseClick += panel1_MouseClick;
            // 
            // postField
            // 
            postField.Location = new Point(33, 26);
            postField.Name = "postField";
            postField.Size = new Size(1089, 386);
            postField.TabIndex = 1;
            postField.Paint += panel2_Paint;
            // 
            // panel5
            // 
            panel5.Location = new Point(418, 434);
            panel5.Name = "panel5";
            panel5.Size = new Size(26, 415);
            panel5.TabIndex = 2;
            panel5.Paint += panel5_Paint;
            // 
            // panel1
            // 
            panel1.Controls.Add(preField);
            panel1.Location = new Point(447, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(1125, 415);
            panel1.TabIndex = 2;
            panel1.Paint += panel2_Paint_1;
            // 
            // panel6
            // 
            panel6.Location = new Point(418, 12);
            panel6.Name = "panel6";
            panel6.Size = new Size(26, 415);
            panel6.TabIndex = 1;
            panel6.Paint += panel6_Paint;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panel2.Controls.Add(postField);
            panel2.Location = new Point(447, 434);
            panel2.Name = "panel2";
            panel2.Size = new Size(1125, 415);
            panel2.TabIndex = 3;
            panel2.Paint += panel2_Paint_1;
            // 
            // panel3
            // 
            panel3.Location = new Point(12, 12);
            panel3.Name = "panel3";
            panel3.Size = new Size(400, 250);
            panel3.TabIndex = 4;
            // 
            // panel4
            // 
            panel4.Controls.Add(groupBox4);
            panel4.Controls.Add(groupBox3);
            panel4.Controls.Add(groupBox2);
            panel4.Controls.Add(groupBox1);
            panel4.Location = new Point(12, 268);
            panel4.Name = "panel4";
            panel4.Size = new Size(400, 552);
            panel4.TabIndex = 5;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(numericUpDown4);
            groupBox4.Location = new Point(3, 192);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(388, 60);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Коэффициент Beta";
            // 
            // numericUpDown4
            // 
            numericUpDown4.DecimalPlaces = 2;
            numericUpDown4.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numericUpDown4.Location = new Point(6, 22);
            numericUpDown4.Name = "numericUpDown4";
            numericUpDown4.Size = new Size(382, 23);
            numericUpDown4.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(numericUpDown3);
            groupBox3.Location = new Point(3, 125);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(388, 61);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Коэффициент Alpha";
            // 
            // numericUpDown3
            // 
            numericUpDown3.DecimalPlaces = 2;
            numericUpDown3.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numericUpDown3.Location = new Point(6, 22);
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(382, 23);
            numericUpDown3.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(numericUpDown2);
            groupBox2.Location = new Point(3, 62);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(394, 57);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Высота ячейки";
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(6, 22);
            numericUpDown2.Maximum = new decimal(new int[] { 1111, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(382, 23);
            numericUpDown2.TabIndex = 0;
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(numericUpDown1);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(394, 53);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Ширина ячейки";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(6, 22);
            numericUpDown1.Maximum = new decimal(new int[] { 1111, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(382, 23);
            numericUpDown1.TabIndex = 0;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // button1
            // 
            button1.Location = new Point(12, 826);
            button1.Name = "button1";
            button1.Size = new Size(400, 23);
            button1.TabIndex = 6;
            button1.Text = "Произвести расчеты";
            button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1584, 861);
            Controls.Add(panel5);
            Controls.Add(panel6);
            Controls.Add(button1);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gravity Exploration";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).EndInit();
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel preField;
        private Panel postField;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private GroupBox groupBox1;
        private NumericUpDown numericUpDown1;
        private GroupBox groupBox2;
        private NumericUpDown numericUpDown2;
        private Button button1;
        private GroupBox groupBox4;
        private NumericUpDown numericUpDown4;
        private GroupBox groupBox3;
        private NumericUpDown numericUpDown3;
        private Panel panel5;
        private Panel panel6;
    }
}
