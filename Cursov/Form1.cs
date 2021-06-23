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
            list = Infrastructure.Normalize(list);
            if (list.Count == 0)
            {
                textBox2.Text = " язык не порождает слов";
                return;
            }
            if (!Infrastructure.SearchCycles(list))
            {
                string temp = Infrastructure.FindMaxWord(list);
                temp = IsNotEpsilon(temp) ? temp = "ε" : temp.Replace("ε", "");
                textBox2.Text = temp;
            }
            else
                textBox2.Text = "Язык бесконечен ";


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
        public static bool IsNotEpsilon(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != 'ε')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
