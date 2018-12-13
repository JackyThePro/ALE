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
            Reader r = new Reader();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();
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
            listBox1.Items.Add("------------------------");

            listBox1.Items.AddRange(r.CheckFinite().Split('\n'));

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

            //testing the output of NDFA tree
            //foreach (State s in States)
            //{
            //    foreach (Transmission t in s.outgoing)
            //    {
            //        Console.WriteLine(t.In.Stat + "-->" + t.Value+ ", " + t.Out.Stat);
            //    }
            //}

            //Create string for new file with NDFA
            List<char> alphabet = new List<char>();
            List<string> states = new List<string>();
            List<string> final = new List<string>();

            foreach (char c in r.Alpha)
            {
                alphabet.Add(c);
            }

            foreach (State s in States)
            {
                states.Add(s.Stat);
                if (s.Final)
                {
                    final.Add(s.Stat);
                }
                
            }
            string alphabets = String.Join("", alphabet.ToArray());
            string statess = String.Join(",", states.ToArray());
            string finals = String.Join(",", final.ToArray());

            string fileContent;
            fileContent = "alphabet: " + alphabets + Environment.NewLine;
            fileContent += "states: " + statess + Environment.NewLine;
            fileContent += "states: " + finals + Environment.NewLine;
            fileContent += "transitions:" + Environment.NewLine;
            foreach (State s in States)
            {
                foreach (Transmission t in s.outgoing)
                {
                    fileContent += t.In.Stat + "-->" + t.Value + ", " + t.Out.Stat + Environment.NewLine;
                }
            }
            fileContent += Environment.NewLine + "end.";

            Console.WriteLine(fileContent);

            // Choosing path
            SaveFileDialog choofdlog = new SaveFileDialog();
            choofdlog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            choofdlog.FilterIndex = 1;

            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                string sFileName = choofdlog.FileName;
                File.WriteAllText(sFileName, fileContent);
            }

        }
    }
}
