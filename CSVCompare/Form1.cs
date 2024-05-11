using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System.Threading;

namespace CSVCompare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string listfilepath;
        static string suppressionfilepath;
        static Char c = "\r\n".ToCharArray()[0];

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            listfilepath = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            suppressionfilepath = openFileDialog1.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Suppression spview = new Suppression();
            spview.Show();

            List<string> listTosuppress = new List<string>();

            listTosuppress = CompareLists(spview);
            MessageBox.Show("All Done!");
        }

        private List<string> CompareLists(Suppression spview)
        {
            CsvReader supressioncsv = new CsvReader(new StreamReader(suppressionfilepath), false, c);
            Dictionary<string, string> listdict = Csv2MD5Dictionary(listfilepath,boolMD5.Checked);
            List<string> records2suppress = new List<string>();

            int suppcount = 0;
            int i = 0;

            while (supressioncsv.ReadNextRecord())
            {
                foreach (KeyValuePair<string, string> kvp in listdict)
                {
                    if (kvp.Value.Trim() == supressioncsv[i, 0].Trim())
                    {
                        records2suppress.Add(kvp.Value);
                        spview.NewListEntry(kvp.Key);
                        suppcount += 1;
                    }
                }
                i++;
                
                spview.changelabelval(i.ToString());
                spview.Refresh();
            }
            return records2suppress;
        }

        private Dictionary<string,string> Csv2MD5Dictionary(string csvfilepath, bool encode)
        {
            Dictionary<string, string> csvdict = new Dictionary<string, string>();
            CsvReader csv = new CsvReader(new StreamReader(csvfilepath), false,c);
            int i = 0;
            while (csv.ReadNextRecord())
            {
                if (!csvdict.ContainsKey(csv[i, 0]))
                {
                    if (encode)
                    {
                        csvdict.Add(csv[i, 0].Trim(), HashString(csv[i, 0]).Trim());
                    }
                    else
                    {
                        csvdict.Add(csv[i, 0].Trim(), csv[i, 0].Trim());
                    }
                }
                i++;
            }
            return csvdict;
        }

        private string HashString(string Value) //this source retrieved from: http://www.ibt4im.com/?guid=20040923142406
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }

    }
}
