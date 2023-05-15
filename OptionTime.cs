using System;
using System.Globalization;
using System.Windows.Forms;

namespace DemonChess
{
    public partial class OptionTime : Form
    {
        public delegate void PassTimeControl(int num);

        public PassTimeControl PassTime;

        public OptionTime()
        {
            InitializeComponent();
        }

        private void SetTimeCmd_Click(object sender, EventArgs e)
        {
            try
            {
                PassTime?.Invoke(int.Parse(numericTimeUpDown.Value.ToString(CultureInfo.InvariantCulture)));
                Close();
            }
            catch (Exception)
            {
                _ = MessageBox.Show(@"");
            }
        }
    }
}