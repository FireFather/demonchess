using System;
using System.Drawing;
using System.Windows.Forms;

namespace DemonChess
{
    public partial class PromotionForm : Form
    {
        public delegate void PassPromotionValue(int num);

        private static bool _clicked;
        public PassPromotionValue PassPro;

        public PromotionForm()
        {
            InitializeComponent();
        }

        private void PromotionForm_Load(object sender, EventArgs e)
        {
            label1.Text = @"Select piece for promotion...";
            QWP.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteQueen.png");
            QBP.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackQueen.png");

            RWP.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteRook.png");
            RBP.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackRook.png");

            NWP.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteKnight.png");
            NBP.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackKnight.png");

            BWP.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteBishop.png");
            BBP.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackBishop.png");
        }

        private void Label1_Click(object sender, EventArgs e)
        {
        }

        private void PromotionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_clicked) PassPro(5);
        }

        private void QWP_Click(object sender, EventArgs e)
        {
            PassPro(5);
            _clicked = true;
            Close();
        }

        private void RWP_Click(object sender, EventArgs e)
        {
            PassPro(4);
            _clicked = true;
            Close();
        }

        private void NWP_Click(object sender, EventArgs e)
        {
            PassPro(3);
            _clicked = true;
            Close();
        }

        private void BWP_Click(object sender, EventArgs e)
        {
            PassPro(2);
            _clicked = true;
            Close();
        }

        private void QBP_Click(object sender, EventArgs e)
        {
            PassPro(5);
            _clicked = true;
            Close();
        }

        private void RBP_Click(object sender, EventArgs e)
        {
            PassPro(4);
            _clicked = true;
            Close();
        }

        private void NBP_Click(object sender, EventArgs e)
        {
            PassPro(3);
            _clicked = true;
            Close();
        }

        private void BBP_Click(object sender, EventArgs e)
        {
            PassPro(2);
            _clicked = true;
            Close();
        }
    }
}