using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo01
{
    public partial class AddGroup : Form
    {
        public bool bOK = false;
        public string strGroupName = "";
        public AddGroup()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(this.textBox1.Text))
            {
                MessageBox.Show("请输入组名");
            }else
            {
                strGroupName = this.textBox1.Text;
                bOK = true;
                this.Close();
            }
        }
    }
}
