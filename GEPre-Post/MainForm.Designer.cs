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
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // preField
            // 
            preField.Location = new Point(68, 34);
            preField.Name = "preField";
            preField.Size = new Size(171, 90);
            preField.TabIndex = 0;
            preField.Paint += panel1_Paint;
            preField.MouseClick += panel1_MouseClick;
            // 
            // postField
            // 
            postField.Location = new Point(68, 86);
            postField.Name = "postField";
            postField.Size = new Size(171, 90);
            postField.TabIndex = 1;
            postField.Paint += panel2_Paint;
            // 
            // panel1
            // 
            panel1.Controls.Add(preField);
            panel1.Location = new Point(418, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(1154, 415);
            panel1.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panel2.Controls.Add(postField);
            panel2.Location = new Point(418, 434);
            panel2.Name = "panel2";
            panel2.Size = new Size(1154, 415);
            panel2.TabIndex = 3;
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
            panel4.Location = new Point(12, 268);
            panel4.Name = "panel4";
            panel4.Size = new Size(400, 581);
            panel4.TabIndex = 5;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1584, 861);
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
            ResumeLayout(false);
        }

        #endregion

        private Panel preField;
        private Panel postField;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
    }
}
