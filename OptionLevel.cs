using System;
using System.Globalization;
using System.Windows.Forms;

namespace DemonChess
{
    public partial class OptionLevel : Form
    {
        public delegate void PassLevelControl(int num);

        public PassLevelControl PassLevel;

        public OptionLevel()
        {
            InitializeComponent();
        }

        private void CmdSetLevel_Click(object sender, EventArgs e)
        {
            try
            {
                var value = int.Parse(nLevel.Value.ToString(CultureInfo.InvariantCulture));
                PassLevel(value);
                Close();
            }
            catch (Exception)
            {
                PassLevel(2);
                _ = MessageBox.Show(@"Must be a integer number");
            }
        }

        public void SetLevelMaxDepth(int val)
        {
            nLevel.Value = val;
        }

        private void NLevel_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}