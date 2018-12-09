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

namespace ALE2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Reader r = new Reader();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;
                string text = "";
                using (var streamReader = new StreamReader(FileName, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        text = line.Replace(" ", "");
                        listBox1.Items.Add(line + r.ReadFile(text));
                    }
                }
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string input = textBox1.Text;
            input = input.Replace(" ", "");
            input = input.Replace("(", "");
            input = input.Replace(")", "");

            Reader r = new Reader();
            List<Node> tree = r.ReadFormula(input);
            Node topNode = tree[0];

            State firstSate = new State("1", false, true);

            var States = new List<State>();
            States.Add(firstSate);
            var ndfa = new NDFA(tree, 1, States);

            ndfa.Selectf(topNode);
            States.Last().Final = true;
            foreach (State s in States)
            {
                foreach (Transmission t in s.outgoing)
                {
                    Console.WriteLine(t.In.Stat + "-->" + t.Value+ ", " + t.Out.Stat);
                }
            }
        }
    }
}
