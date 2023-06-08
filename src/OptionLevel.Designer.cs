namespace DemonChess
{
    partial class OptionLevel
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
            this.cmdSetLevel = new System.Windows.Forms.Button();
            this.nLevel = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdSetLevel
            // 
            this.cmdSetLevel.Location = new System.Drawing.Point(48, 44);
            this.cmdSetLevel.Name = "cmdSetLevel";
            this.cmdSetLevel.Size = new System.Drawing.Size(45, 25);
            this.cmdSetLevel.TabIndex = 0;
            this.cmdSetLevel.Text = "Ok";
            this.cmdSetLevel.UseVisualStyleBackColor = true;
            this.cmdSetLevel.Click += new System.EventHandler(this.CmdSetLevel_Click);
            // 
            // nLevel
            // 
            this.nLevel.Location = new System.Drawing.Point(47, 13);
            this.nLevel.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.nLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nLevel.Name = "nLevel";
            this.nLevel.Size = new System.Drawing.Size(50, 20);
            this.nLevel.TabIndex = 2;
            this.nLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nLevel.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nLevel.ValueChanged += new System.EventHandler(this.NLevel_ValueChanged);
            // 
            // OptionLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(144, 81);
            this.Controls.Add(this.nLevel);
            this.Controls.Add(this.cmdSetLevel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OptionLevel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Level";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.nLevel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdSetLevel;
        private System.Windows.Forms.NumericUpDown nLevel;
    }
}