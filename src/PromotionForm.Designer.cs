namespace DemonChess
{
    partial class PromotionForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.QWP = new System.Windows.Forms.PictureBox();
            this.RWP = new System.Windows.Forms.PictureBox();
            this.NWP = new System.Windows.Forms.PictureBox();
            this.BWP = new System.Windows.Forms.PictureBox();
            this.BBP = new System.Windows.Forms.PictureBox();
            this.NBP = new System.Windows.Forms.PictureBox();
            this.RBP = new System.Windows.Forms.PictureBox();
            this.QBP = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.QWP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RWP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NWP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BWP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BBP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NBP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RBP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QBP)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(53, -9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 8;
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // QWP
            // 
            this.QWP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.QWP.Location = new System.Drawing.Point(19, 20);
            this.QWP.Name = "QWP";
            this.QWP.Size = new System.Drawing.Size(52, 44);
            this.QWP.TabIndex = 9;
            this.QWP.TabStop = false;
            this.QWP.Click += new System.EventHandler(this.QWP_Click);
            // 
            // RWP
            // 
            this.RWP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RWP.Location = new System.Drawing.Point(99, 20);
            this.RWP.Name = "RWP";
            this.RWP.Size = new System.Drawing.Size(50, 44);
            this.RWP.TabIndex = 10;
            this.RWP.TabStop = false;
            this.RWP.Click += new System.EventHandler(this.RWP_Click);
            // 
            // NWP
            // 
            this.NWP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NWP.Location = new System.Drawing.Point(182, 20);
            this.NWP.Name = "NWP";
            this.NWP.Size = new System.Drawing.Size(52, 44);
            this.NWP.TabIndex = 11;
            this.NWP.TabStop = false;
            this.NWP.Click += new System.EventHandler(this.NWP_Click);
            // 
            // BWP
            // 
            this.BWP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BWP.Location = new System.Drawing.Point(266, 20);
            this.BWP.Name = "BWP";
            this.BWP.Size = new System.Drawing.Size(54, 44);
            this.BWP.TabIndex = 12;
            this.BWP.TabStop = false;
            this.BWP.Click += new System.EventHandler(this.BWP_Click);
            // 
            // BBP
            // 
            this.BBP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BBP.Location = new System.Drawing.Point(268, 82);
            this.BBP.Name = "BBP";
            this.BBP.Size = new System.Drawing.Size(52, 44);
            this.BBP.TabIndex = 16;
            this.BBP.TabStop = false;
            this.BBP.Click += new System.EventHandler(this.BBP_Click);
            // 
            // NBP
            // 
            this.NBP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NBP.Location = new System.Drawing.Point(184, 82);
            this.NBP.Name = "NBP";
            this.NBP.Size = new System.Drawing.Size(50, 44);
            this.NBP.TabIndex = 15;
            this.NBP.TabStop = false;
            this.NBP.Click += new System.EventHandler(this.NBP_Click);
            // 
            // RBP
            // 
            this.RBP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RBP.Location = new System.Drawing.Point(99, 82);
            this.RBP.Name = "RBP";
            this.RBP.Size = new System.Drawing.Size(50, 44);
            this.RBP.TabIndex = 14;
            this.RBP.TabStop = false;
            this.RBP.Click += new System.EventHandler(this.RBP_Click);
            // 
            // QBP
            // 
            this.QBP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.QBP.Location = new System.Drawing.Point(19, 82);
            this.QBP.Name = "QBP";
            this.QBP.Size = new System.Drawing.Size(52, 44);
            this.QBP.TabIndex = 13;
            this.QBP.TabStop = false;
            this.QBP.Click += new System.EventHandler(this.QBP_Click);
            // 
            // PromotionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(341, 145);
            this.Controls.Add(this.BBP);
            this.Controls.Add(this.NBP);
            this.Controls.Add(this.RBP);
            this.Controls.Add(this.QBP);
            this.Controls.Add(this.BWP);
            this.Controls.Add(this.NWP);
            this.Controls.Add(this.RWP);
            this.Controls.Add(this.QWP);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.Name = "PromotionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Promotion";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PromotionForm_FormClosing);
            this.Load += new System.EventHandler(this.PromotionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QWP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RWP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NWP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BWP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BBP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NBP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RBP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QBP)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox QWP;
        private System.Windows.Forms.PictureBox RWP;
        private System.Windows.Forms.PictureBox NWP;
        private System.Windows.Forms.PictureBox BWP;
        private System.Windows.Forms.PictureBox BBP;
        private System.Windows.Forms.PictureBox NBP;
        private System.Windows.Forms.PictureBox RBP;
        private System.Windows.Forms.PictureBox QBP;
    }
}