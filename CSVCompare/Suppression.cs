using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSVCompare
{
    public partial class Suppression : Form
    {
        public Suppression()
        {
            InitializeComponent();
            dgv.AutoGenerateColumns = true;
            ds.Tables.Add();
            ds.Tables[0].Columns.Add("Matched Entries");
            bs.DataSource = ds.Tables[0];
            dgv.DataSource = bs;
        }

        public void NewListEntry(string matched)
        {
            ds.Tables[0].Rows.Add(matched);
            dgv.Refresh();
        }

        public void changelabelval(string newlabel)
        {
            label1.Text = newlabel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/iwyatt/csvcompare");
            }
            catch
            {
                MessageBox.Show("Written by Isaac Wyatt \r\n https://www.isaacwyatt.com");
            }
        }
    }
}
