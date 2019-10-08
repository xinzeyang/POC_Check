using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace POC集成验证
{
    public partial class AddIns : Form
    {
        public AddIns()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void AddIns_Load(object sender, EventArgs e)
        {
            if (!File.Exists(@"./Model"))
            {
                File.WriteAllBytes(@"./Model", POC集成验证.Properties.Resources.Model);
            }
            
            string[] contents = File.ReadAllLines(@"./Model");
            
            //foreach(string key in contents)
            //{
            //    key.Split(':')[0]
            //}
            for (int i = 0; i < contents.Length; i++)
            {
                dataGridView1[i, 0].Value = contents[i].Split(':')[0];
                dataGridView1[i+1, 0].Value = contents[i].Split(':')[1];
                //MessageBox.Show(contents[i].Split(':')[1]);


            }
            //dataGridView1[1, 0].Value = "dataGridView";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
