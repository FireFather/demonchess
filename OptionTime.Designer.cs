namespace DemonChess
{
    partial class OptionTime
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.setTimeCmd = new System.Windows.Forms.Button();
            this.numericTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericTimeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // setTimeCmd
            // 
            this.setTimeCmd.Location = new System.Drawing.Point(55, 43);
            this.setTimeCmd.Name = "setTimeCmd";
            this.setTimeCmd.Size = new System.Drawing.Size(45, 25);
            this.setTimeCmd.TabIndex = 1;
            this.setTimeCmd.Text = "OK";
            this.setTimeCmd.UseVisualStyleBackColor = true;
            this.setTimeCmd.Click += new System.EventHandler(this.SetTimeCmd_Click);
            // 
            // numericTimeUpDown
            // 
            this.numericTimeUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericTimeUpDown.Location = new System.Drawing.Point(16, 12);
            this.numericTimeUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericTimeUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericTimeUpDown.Name = "numericTimeUpDown";
            this.numericTimeUpDown.Size = new System.Drawing.Size(50, 20);
            this.numericTimeUpDown.TabIndex = 2;
            this.numericTimeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericTimeUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "mins/game";
            // 
            // OptionTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(144, 81);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericTimeUpDown);
            this.Controls.Add(this.setTimeCmd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OptionTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Time";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.numericTimeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button setTimeCmd;
        private System.Windows.Forms.NumericUpDown numericTimeUpDown;
        private System.Windows.Forms.Label label2;
    }
}