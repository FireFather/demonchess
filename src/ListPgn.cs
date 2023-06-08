using System;
using System.Windows.Forms;

namespace DemonChess
{
    public partial class ListPgn : Form
    {
        public delegate void PassValueControl(int indexPgnFile);

        public PassValueControl PassValue;

        public ListPgn()
        {
            InitializeComponent();
        }

        public void LoadPgnFile(string[] listPgnFile)
        {
            for (var i = 0; i < listPgnFile.Length; i++)
            {
                var temp = listPgnFile[i];

                if (temp == null || temp.Equals("") || temp.IndexOf("White", StringComparison.Ordinal) < 0) continue;
                try
                {
                    _ = dtGrid.Rows.Add();
                    var startIndex = temp.IndexOf("White", StringComparison.Ordinal);
                    var endIndex = temp.IndexOf("]", startIndex, StringComparison.Ordinal);
                    var value = temp.Substring(startIndex + 5, endIndex - startIndex - 5);
                    dtGrid.Rows[i].Cells[0].Value = value;

                    startIndex = temp.IndexOf("Black", StringComparison.Ordinal);

                    if (startIndex >= 0)
                    {
                        endIndex = temp.IndexOf("]", startIndex, StringComparison.Ordinal);
                        value = temp.Substring(startIndex + 5, endIndex - startIndex - 5);
                        dtGrid.Rows[i].Cells[1].Value = value;
                    }

                    startIndex = temp.IndexOf("Event", StringComparison.Ordinal);

                    if (startIndex >= 0)
                    {
                        endIndex = temp.IndexOf("]", startIndex, StringComparison.Ordinal);
                        value = temp.Substring(startIndex + 6, endIndex - startIndex - 6);
                        dtGrid.Rows[i].Cells[2].Value = value;
                    }

                    startIndex = temp.IndexOf("Result", StringComparison.Ordinal);

                    if (startIndex >= 0)
                    {
                        endIndex = temp.IndexOf("]", startIndex, StringComparison.Ordinal);
                        value = temp.Substring(startIndex + 7, endIndex - startIndex - 7);
                        dtGrid.Rows[i].Cells[3].Value = value;
                    }

                    startIndex = temp.IndexOf("Round", StringComparison.Ordinal);

                    if (startIndex >= 0)
                    {
                        endIndex = temp.IndexOf("]", startIndex, StringComparison.Ordinal);
                        value = temp.Substring(startIndex + 6, endIndex - startIndex - 6);
                        dtGrid.Rows[i].Cells[4].Value = value;
                    }

                    startIndex = temp.IndexOf("Date", StringComparison.Ordinal);

                    if (startIndex >= 0)
                    {
                        endIndex = temp.IndexOf("]", startIndex, StringComparison.Ordinal);
                        value = temp.Substring(startIndex + 5, endIndex - startIndex - 5);
                        dtGrid.Rows[i].Cells[5].Value = value;
                    }

                    dtGrid.Rows[i].Cells[7].Value = i.ToString();
                }
                catch (Exception)
                {
                    _ = MessageBox.Show(@"");
                    return;
                }
            }
        }

        private void DtGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (PassValue == null) return;
            try
            {
                if (dtGrid.CurrentRow != null) PassValue(int.Parse(dtGrid.CurrentRow.Cells[7].Value.ToString()));
                Close();
            }
            catch (Exception)
            {
                _ = MessageBox.Show(@"");
            }
        }

        private void CmdExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ListPgn_Load(object sender, EventArgs e)
        {
            dtGrid.AllowUserToOrderColumns = false;
        }

        private void DtGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}