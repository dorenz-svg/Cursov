using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Service;
namespace Cursov
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            string[] arrstring = textBox1.Text.Replace("\r", "").Split('\n');
            formater(arrstring);
            List<Rule> list = Infrastructure.FillList(arrstring);
            if (!Infrastructure.SearchCycles(list))
                textBox2.Text = Infrastructure.FindMaxWord(list);
            else
                textBox2.Text = "Язык бесконечен";


        }
        private void formater(string[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = str[i].Replace(" ", "");
                str[i] = str[i].Replace("-", " ");
                str[i] = str[i].Replace("|", " ");
            }
        }
    }
}
