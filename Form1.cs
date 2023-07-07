using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace 桌面管理应用
{
    public partial class Form1 : Form
    {
        ArrayList allstr = new ArrayList();
        List<KeyValuePair<string[], int>> pp = new List<KeyValuePair<string[], int>>();
        DateTime date = DateTime.Now;
        public Form1()
        {
            try
            {
                string[] strs1 = File.ReadAllLines(@"file.tmp");
                foreach (string str in strs1)
                {
                    string[] res = str.Split('|');
                    if (res.Length == 3)
                    {
                        allstr.Add(res);
                    }
                    
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            InitializeComponent();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // 输入文字
            listBox1.Items.Clear();
            string input = textBox1.Text.ToLower();

            if (input.Length == 0)
            {
                return;
            }

            pp.Clear();
            foreach (string[] str in allstr)
            {
                string name = str[0];
                string path = str[1];
                string[] des = str[2].Split(',');
                int score = getScore(input, des);
                if (score > 0)
                {
                    KeyValuePair<string[], int> p = new KeyValuePair<string[], int>(str,score);
                    pp.Add(p);
                }
            }

            pp.Sort((p1, p2) => { 
                return p2.Value.CompareTo(p1.Value);
            });

            foreach (KeyValuePair<string[], int> p in pp)
            {
                listBox1.Items.Add(p.Key[0]);
            }
        }
        /**
         * 获取当前输入的和某组描述的得分情况，根据得分排序
         * */
        private int getScore(string input, string[] des)
        {
            foreach (string str in des)
            {
                if (str.Equals(input))
                {
                    return 100;
                }
            }
            foreach (string str in des)
            {
                if (str.Contains(input))
                {
                    return 10;
                }
            }
            int con = 0;
            foreach (string str in des)
            {
                int c = 0;
                foreach(char ch in input)
                {
                    if(str.Contains(ch))
                    {
                        c++;
                    }
                }
                if (c == input.Length && c == str.Length)
                {
                    return 80;
                }
                if (con == 0)
                {
                    con = c;
                }
                else
                {
                    con = c>con ? c :con;
                }
            }
            return con;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                // 双击回车键打开第一个
            {
                if ((DateTime.Now - date).TotalSeconds < 1)
                {
                    if (pp.Count > 0)
                    {
                        string path = pp[0].Key[1];
                        start(path);
                    }
                    
                }
                else
                {
                    date = DateTime.Now;
                }
                
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            if (index == System.Windows.Forms.ListBox.NoMatches)
            {
                return;
            }

            string path = pp[index].Key[1];
            start(path);
        }
        /*
         * 运行某程序 
         */
        private void start(string path)
        {
            string[] p = path.Split(new String[] { ".exe" }, StringSplitOptions.RemoveEmptyEntries );

            System.Diagnostics.Process.Start(p[0]+".exe", string.Join(" ", p.Skip(1)));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("this is about.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            start("Notepad.exe file.tmp");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("F:\\快捷方式\\没放入程序中的");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Left = 1000;
            this.Top = 100;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.ImeMode = ImeMode.Disable;
            // 关闭输入法
            SwitchToLanguageMode("en-US");
        }

        private void SwitchToLanguageMode(string cultureType)
        {
            var installedInputLanguages = InputLanguage.InstalledInputLanguages;

            if (installedInputLanguages.Cast<InputLanguage>().Any(i => i.Culture.Name == cultureType))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(System.Globalization.CultureInfo.GetCultureInfo(cultureType));
                // CurrentLanguage = cultureType;
            }
        }
    }
}
