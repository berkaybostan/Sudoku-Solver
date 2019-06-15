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
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SudokuSolver
{
    public partial class Form3 : Form
    {
        Dictionary<string, string[]> results = new Dictionary<string, string[]>();
        string winner;
        int tickCounter = 0;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string project_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

            string[] thrNames = { "t1", "t2", "t3" };

            foreach (var item in thrNames)
            {
                string read_path = project_path + "\\" + item + ".txt";
                results.Add(item, File.ReadAllLines(read_path));
            }

            winner = checkWin(results);
            thrNames = thrNames.Where(w => w != winner).ToArray();
            winner = thrNames[1];
            timer1.Enabled = true;
            Console.SetOut(new TextBoxWriter(textBox1));
        }

        public string checkWin(Dictionary<string, string[]> checkDict)
        {

            foreach (var item in checkDict)
            {
                foreach (var qwe in item.Value)
                {
                    if (qwe.Contains("RunTime"))
                    {
                        return item.Key;
                    }
                }
            }
            return "noWin";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (results[winner].Length > tickCounter & ((tickCounter % 1 == 0) | results[winner].Length - 2 == tickCounter))
            {

                if (results[winner][tickCounter][0] == '[')
                {
                    var me = SplitToDictionary(results[winner][tickCounter]);


                    string rows = "ABCDEFGHI";
                    string cols = "123456789";

                    string[] squares = (from a in rows from b in cols select "" + a + b).ToArray();

                    var width = 1 + (from s in squares select me[s].Length).Max();
                    var line = "\n" + String.Join("+", Enumerable.Repeat(new String('-', width * 3), 3).ToArray());
                    textBox1.Text = "";
                    foreach (var r in rows)
                    {
                        var stringinim = String.Join("",
                            (from c in cols
                             select myCenter(me["" + r + c], width) + ("36".Contains(c) ? "|" : "")).ToArray())
                                + ("CF".Contains(r) ? line : "");

                        Console.WriteLine(stringinim);

                    }

                }

            }

            tickCounter++;

            if (tickCounter > results[winner].Length)
            {
                timer1.Stop();
            }
        }

        public string myCenter(string s, int width)
        {
            var n = width - s.Length;
            if (n <= 0) return s;
            var half = n / 2;

            if (n % 2 > 0 && width % 2 > 0) half++;

            return new string(' ', half) + s + new String(' ', n - half);
        }

        public static Dictionary<string, string> SplitToDictionary(string input)
        {
            Regex regex = new Regex(@"\[\w{2}\s{1}\w+\]");

            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (Match match in regex.Matches(input))
            {
                var strarr = Regex.Replace(match.Value, @"[\[\]]", "").Split(' ');

                result.Add(strarr[0], strarr[1]);
            }

            return result;
        }
    }
}
