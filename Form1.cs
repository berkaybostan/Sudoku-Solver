using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        string inputString;
        string winner;
        int winnerTime;
        Dictionary<string, string[]> results = new Dictionary<string, string[]>();
        Dictionary<string, string> valueToShow = new Dictionary<string, string>();
        int tickCounter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Text Documents |*.txt", Multiselect = false, ValidateNames = true })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamReader sr = new StreamReader(ofd.FileName))
                        {
                            inputString = await sr.ReadToEndAsync();
                            txtValue.Text = inputString;
                            string replacement = Regex.Replace(inputString, @"\t|\n|\r", "");
                            
                            var tokenSource = new CancellationTokenSource();
                            var token = tokenSource.Token;

                            Thread t1 = new Thread(() => ThreadJob(15, "t1", token, tokenSource, replacement));
                            Thread t2 = new Thread(() => ThreadJob(100, "t2", token, tokenSource, replacement));
                            Thread t3 = new Thread(() => ThreadJob(80, "t3", token, tokenSource, replacement));
                            t1.Start();
                            t2.Start();
                            t3.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void showStatus(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(showStatus), new object[] { value });
                return;
            }
            statusBox.Text = value;
        }

        public void ThreadJob(int inputVal, string thrName, CancellationToken token, CancellationTokenSource tokenSource, string inputGrid)
        {

            string project_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string threadOut_path = project_path + "\\" + thrName + ".txt";
            showStatus("Kisitlari kullanarak cozuyor...");
            var myGrid = SudokuSolver.parse_grid(inputGrid, thrName, threadOut_path, token, tokenSource);
            showStatus("Arama algoritmasi calisiyor...");
            Stopwatch stopWatch2 = new Stopwatch();
            stopWatch2.Start();
            SudokuSolver.search(myGrid, thrName, threadOut_path, token, tokenSource, stopWatch2);
            showStatus("Cozuldu!!!");
        }

        private void showSlnSteps_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();

            Form3 f3 = new Form3();
            f3.Show();
            string project_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

            string[] thrNames = { "t1", "t2", "t3" };

            foreach (var item in thrNames)
            {
                string read_path = project_path + "\\" + item + ".txt";
                results.Add(item, File.ReadAllLines(read_path));
            }

            winner = checkWin(results);
            winnerTime = calctime(results, winner);
            txtIslemsuresi.Text = winnerTime.ToString() + "ms";

            timer1.Enabled = true;
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

        public int calctime(Dictionary<string, string[]> checkDict, string winner)
        {
            int holder = 0;
            Regex rgxtime = new Regex(@"\d+ms");
            foreach (var item in checkDict[winner])
            {
                if (item.Contains("ms"))
                {
                    foreach (Match match in rgxtime.Matches(item))
                    {
                        var deletems = Regex.Replace(match.Value, @"ms", "");
                        holder += Convert.ToInt32(deletems);
                    }
                }
            }
            return holder;
        }


        public void print_board_toForm(Dictionary<string, string> values)
        {
            string rows = "ABCDEFGHI";
            string cols = "123456789";

            string[] squares = (from a in rows from b in cols select "" + a + b).ToArray();

            var width = 1 + (from s in squares select values[s].Length).Max();
            var line = "\n" + String.Join("+", Enumerable.Repeat(new String('-', width * 3), 3).ToArray());

            foreach (var r in rows)
            {
                var stringinim = String.Join("",
                    (from c in cols
                     select myCenter(values["" + r + c], width) + ("36".Contains(c) ? "|" : "")).ToArray())
                        + ("CF".Contains(r) ? line : "");
                
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
                    showSln3.Text = "";
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

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.SetOut(new TextBoxWriter(showSln3));
        }
    }

    public class TextBoxWriter : TextWriter
    {
        TextBox _output = null;

        public TextBoxWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _output.AppendText(value.ToString());
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

    public static class StaticRandom
    {
        static int seed = Environment.TickCount;

        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public static int Rand(int rng)
        {
            return random.Value.Next(rng);
        }
    }

    static class SudokuSolver
    {
        static string rows = "ABCDEFGHI";
        static string cols = "123456789";
        static string digits = "123456789";
        static string[] squares = cross(rows, cols);
        static List<string[]> unitlist;
        static Dictionary<string, IEnumerable<string>> peers;
        static Dictionary<string, IGrouping<string, string[]>> units;

        public static IEnumerable<T> OrderRandomly<T>(this IEnumerable<T> sequence)
        {

            List<T> copy = sequence.ToList();

            while (copy.Count > 0)
            {
                int index = StaticRandom.Rand(copy.Count);
                yield return copy[index];
                copy.RemoveAt(index);
            }
        }


        static string[] cross(string A, string B)
        {
            return (from a in A from b in B select "" + a + b).ToArray();
        }

        static SudokuSolver()
        {

            unitlist = ((from c in cols select cross(rows, c.ToString()))
                .Concat(from r in rows select cross(r.ToString(), cols))
                .Concat(from rs in (new[] { "ABC", "DEF", "GHI" }) from cs in (new[] { "123", "456", "789" }) select cross(rs, cs))).ToList();


            units = (from s in squares from u in unitlist where u.Contains(s) group u by s into g select g).ToDictionary(g => g.Key);
            peers = (from s in squares from u in units[s] from s2 in u where s2 != s group s2 by s into g select g).ToDictionary(g => g.Key, g => g.Distinct());

        }

        static string[][] zip(string[] A, string[] B)
        {
            var n = Math.Min(A.Length, B.Length);
            string[][] sd = new string[n][];
            for (var i = 0; i < n; i++)
            {
                sd[i] = new string[] { A[i].ToString(), B[i].ToString() };
            }
            return sd;
        }

        public static Dictionary<string, string> parse_grid(string grid, string threadName, string threadOutPath, CancellationToken token, CancellationTokenSource tokenSource)
        {
            Stopwatch stopWatch = new Stopwatch();


            if (!File.Exists(threadOutPath))
            {
                using (StreamWriter sw = File.CreateText(threadOutPath))
                {
                }
            }
            else
            {
                File.WriteAllText(threadOutPath, String.Empty);
            }

            var values = squares.ToDictionary(s => s, s => digits);
            var ctr = 0;
            stopWatch.Start();
            foreach (var sd in zip(squares, (from s in grid select s.ToString()).ToArray()).OrderRandomly())
            {
                if (token.IsCancellationRequested)
                {
                    stopWatch.Stop();
                    var elapsedMsCancel = stopWatch.ElapsedMilliseconds;
                    return values;
                }

                ctr++;
                var s = sd[0];
                var d = sd[1];

                if (digits.Contains(d) && assign(values, s, d) == null)
                {
                    return null;
                }

                if (all(from ss in squares select values[ss].Length == 1 ? "" : null))
                {
                    stopWatch.Stop();
                    var elapsedMs = stopWatch.ElapsedMilliseconds;
                    tokenSource.Cancel();
                    using (StreamWriter sw = File.AppendText(threadOutPath))
                    {
                        txtWriter(values, sw);
                        sw.WriteLine("RunTime " + elapsedMs + "ms using thread " + threadName);
                    }
                    return values;
                }
                using (StreamWriter sw = File.AppendText(threadOutPath))
                {
                    txtWriter(values, sw);
                }
            }
            stopWatch.Stop();
            var elapsedMsEndpropNOsln = stopWatch.ElapsedMilliseconds;
            using (StreamWriter sw = File.AppendText(threadOutPath))
            {
                sw.WriteLine("Propagation finished w/o sln at " + elapsedMsEndpropNOsln.ToString() + "ms");
            }
            return values;
        }


        public static Dictionary<string, string> search(Dictionary<string, string> values, string threadName, string threadOutPath, CancellationToken token, CancellationTokenSource tokenSource, Stopwatch stopWatchSearch)
        {
            if (token.IsCancellationRequested)
            {
                stopWatchSearch.Stop();
                var elapsedMsCancel = stopWatchSearch.ElapsedMilliseconds;
                return values;
            }

            if (values == null)
            {
                return null;
            }

            if (all(from s in squares select values[s].Length == 1 ? "" : null))
            {
                stopWatchSearch.Stop();
                var elapsedMs = stopWatchSearch.ElapsedMilliseconds;
                tokenSource.Cancel();
                using (StreamWriter sw = File.AppendText(threadOutPath))
                {
                    txtWriter(values, sw);
                    sw.WriteLine("RunTime " + elapsedMs + "ms. In search, using thread " + threadName);

                }
                print_board(values);
                return values;
            }

            using (StreamWriter sw = File.AppendText(threadOutPath))
            {
                txtWriter(values, sw);
            }

            var s2 = (from s in squares where values[s].Length > 1 orderby values[s].Length ascending select s).OrderRandomly().First();

            return some(from d in values[s2]
                        select search(assign(new Dictionary<string, string>(values), s2, d.ToString()), threadName, threadOutPath, token, tokenSource, stopWatchSearch));
        }


        static Dictionary<string, string> assign(Dictionary<string, string> values, string s, string d)
        {
            if (all(
                    from d2 in values[s]
                    where d2.ToString() != d
                    select eliminate(values, s, d2.ToString())))
            {
                return values;
            }
            return null;
        }


        static Dictionary<string, string> eliminate(Dictionary<string, string> values, string s, string d)
        {
            if (!values[s].Contains(d))
            {
                return values;
            }
            values[s] = values[s].Replace(d, "");
            if (values[s].Length == 0)
            {
                return null;
            }
            else if (values[s].Length == 1)
            {

                var d2 = values[s];
                if (!all(from s2 in peers[s] select eliminate(values, s2, d2)))
                {
                    return null;
                }
            }


            foreach (var u in units[s])
            {

                var dplaces = from s2 in u where values[s2].Contains(d) select s2;
                if (dplaces.Count() == 0)
                {
                    return null;
                }
                else if (dplaces.Count() == 1)
                {

                    if (assign(values, dplaces.First(), d) == null)
                    {
                        return null;
                    }
                }
            }
            return values;
        }


        static bool all<T>(IEnumerable<T> seq)
        {
            foreach (var e in seq)
            {
                if (e == null) return false;
            }
            return true;
        }


        static T some<T>(IEnumerable<T> seq)
        {
            foreach (var e in seq)
            {
                if (e != null) return e;
            }
            return default(T);
        }

        static string Center(this string s, int width)
        {
            var n = width - s.Length;
            if (n <= 0) return s;
            var half = n / 2;

            if (n % 2 > 0 && width % 2 > 0) half++;

            return new string(' ', half) + s + new String(' ', n - half);
        }

        public static Dictionary<string, string> print_board(Dictionary<string, string> values)
        {
            if (values == null) return null;

            var width = 1 + (from s in squares select values[s].Length).Max();
            var line = "\n" + String.Join("+", Enumerable.Repeat(new String('-', width * 3), 3).ToArray());

            foreach (var r in rows)
            {

            }

            return values;
        }

        public static void txtWriter(Dictionary<string, string> values, StreamWriter sw)
        {
            foreach (var item in values)
            {
                sw.Write("[{0} {1}]", item.Key, item.Value);
            }
            sw.WriteLine();
        }



    }
}
