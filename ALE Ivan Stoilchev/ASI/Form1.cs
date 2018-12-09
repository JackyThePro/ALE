using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASI
{
    public partial class Form1 : Form
    {
        //=( >(A,B), |( ~(A) ,B) )
        //|(=(~(a),b),a)
        //&(|(a,~(b)),c)
        Tree tree = new Tree();
        Simplifier simple = new Simplifier();
        Normalizer norm = new Normalizer();
        Nandify nan = new Nandify();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            listView2.Clear();
            string input = textBox1.Text;
            input = input.Replace(" ", "");
            input = input.Replace("(", "");
            input = input.Replace(")", "");

            List<Node> listerino = new List<Node>();
            listerino = tree.ReadFormula(input);
            foreach (Node item in listerino)
            {
                Console.WriteLine("Id is: " + item.Id);
                Console.WriteLine("Sign is: " + item.Param);
                Console.WriteLine("Parent is: " + item.ParId);
                Console.WriteLine("First child is: " + item.ChildOneId);
                Console.WriteLine("Second child is: " + item.ChildTwoId);
                Console.WriteLine( item.Letter);
                Console.WriteLine("--------------------------------------------");
            }

            // Answer
            List<char> letters;
            letters = tree.GetLetters();
            letters.Sort();
            bool[,] table = tree.CreateTable(letters.Count);
            bool[] answer = new bool[table.GetLength(0)];

            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    foreach (Node a in listerino)
                    {
                        if (a.Param == letters[j])
                        {
                            a.Val = table[i, j];
                        }
                    }
                }
                answer[i] = tree.BM(1);
            }
            
            //Table creation
            foreach (char c in letters)
            {
                listView1.Columns.Add(c.ToString());
                listView2.Columns.Add(c.ToString());
            }
            listView1.Columns.Add("Answer");
            listView2.Columns.Add("Answer");


            for (int i = 0; i < table.GetLength(0); i++)
            {
                ListViewItem I = listView1.Items.Add(BoolToString(table[i, 0]));
                for (int j = 1; j < table.GetLength(1); j++)
                {
                    I.SubItems.Add(BoolToString(table[i, j]));
                }
                I.SubItems.Add(BoolToString(answer[i]));
            }

            // Nandify
            nan.GetList(listerino);
            textBox3.Text = nan.BM(1);
        }

        public string BoolToString(bool b)
        {
            return b ? "1" : "0";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> zeros = new List<string>();
            List<string> ones = new List<string>();

            
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string columns = String.Empty;
                columns += listView1.Items[i].Text;
                for (int j = 1; j < listView1.Items[i].SubItems.Count; j++)
                {
                    columns += listView1.Items[i].SubItems[j].Text;
                }
                if (columns[columns.Length - 1] == '0')
                {
                    columns = columns.Remove(columns.Length - 1);
                    zeros.Add(columns);
                }
                else if (columns[columns.Length - 1] == '1')
                {
                    columns = columns.Remove(columns.Length - 1);
                    ones.Add(columns);
                }
            }
            List<string> simlified = simple.Simlify(ones,zeros);

            listView2.Items.Clear();
            foreach (string s in zeros)
            {
                ListViewItem I = listView2.Items.Add(s[0].ToString());
                for (int i = 1; i < s.Length; i++)
                {
                    I.SubItems.Add(s[i].ToString());
                }
                I.SubItems.Add("0");
            }
            foreach (string s in simlified)
            {
                ListViewItem I = listView2.Items.Add(s[0].ToString());
                for (int i = 1; i < s.Length; i++)
                {
                    I.SubItems.Add(s[i].ToString());
                }
                I.SubItems.Add("1");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Normal(listView1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Normal(listView2);
        }

        public void Normal(ListView listview)
        {
            textBox2.Clear();
            List<string> lis = new List<string>();
            string columns = string.Empty;
            for (int i = 0; i < listview.Columns.Count - 1; i++)
            {
                columns += listview.Columns[i].Text;
            }
            lis.Add(columns);
            for (int l = 0; l < listview.Items.Count; l++)
            {
                columns = String.Empty;
                columns += listview.Items[l].Text;                
                for (int j = 1; j < listview.Items[l].SubItems.Count; j++)
                {
                    columns += listview.Items[l].SubItems[j].Text;
                }
                if (columns[columns.Length - 1] == '1')
                {
                    columns = columns.Remove(columns.Length - 1);
                    lis.Add(columns);
                }                                
            }
            textBox2.Text = norm.Normalize(lis);
        }
    }

    public class Calculate
    {
        public bool GetSign(char w, bool a, bool b)
        {
            switch (w)
            {
                case '|':
                    return AorB(a, b);
                    break;
                case '&':
                    return AandB(a, b);
                    break;
                case '~':
                    return NotA(a);
                    break;
                case '>':
                    return Implication(a, b);
                    break;
                case '=':
                    return OnlyIF(a, b);
                    break;
                case '%':
                    return Nand(a, b);
                    break;
                default:
                    return false;
            }
        }

        public bool AorB(bool a, bool b)
        {
            return (a || b);
        }

        public bool AandB(bool a, bool b)
        {
            return (a && b);
        }

        public bool NotA(bool a)
        {
            return a = !a;
        }

        public bool Implication(bool a, bool b)
        {
            return !a || b;
        }

        public bool OnlyIF(bool a, bool b)
        {
            return (a && b) || (!a && !b);
        }

        public bool Nand(bool a, bool b)
        {
            return !(a && b);
        }
    }
}
