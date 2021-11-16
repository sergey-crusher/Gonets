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

namespace plan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            string[] fileIn = File.ReadAllLines("plan.csv", Encoding.GetEncoding(1251));
            string fileOut = "";
            string[] line;
            string[] subLine;

            int n = int.Parse(number.Text);

            int start;
            int end;

            for (int i = 0; i < fileIn.Length-1; i++)
            {
                line = fileIn[i].Split(';');
                subLine = line[2].Split('-');

                start = int.Parse(subLine[0]);
                end = int.Parse(subLine[1]);

                if (line[1].IndexOf("И Е.К.") < 0 && start == 0)
                {
                    start = 1;
                }

                for (int j = start; j <= end; j += 4)
                {
                    richTextBox1.Text += $"{n};" +
                        $"{(int.Parse(line[0])).ToString("00")}.{month.Text}.{year.Text};" +
                        $"{j.ToString("00")}:00;" +
                        $"{line[1]}\r\n";
                    fileOut += $"{n};" +
                        $"{(int.Parse(line[0])).ToString("00")}.{month.Text}.{year.Text};" +
                        $"{j.ToString("00")}:00;" +
                        $"{line[1]};0;0\r\n";
                    n++;
                }
            }
            File.WriteAllText("out.csv", fileOut, Encoding.GetEncoding(1251));
            MessageBox.Show("Готово! Откройте файл out.csv");
        }
    }
}
