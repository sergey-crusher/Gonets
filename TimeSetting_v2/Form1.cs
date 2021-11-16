using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeSetting_v2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Основной таймер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainTimer_Tick(object sender, EventArgs e)
        {
            SynchronizationTime();
        }

        /// <summary>
        /// Отображает текущее время
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeNow_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToString();
        }

        private void SynchronizationTime()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            DateTime PCTime = DateTime.Now;
            try
            {
                string sURL = "http://worldtimeapi.org/api/timezone/Europe/Moscow";
                WebRequest wrGETURL = WebRequest.Create(sURL);
                Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();

                StreamReader objReader = new StreamReader(objStream);

                string sLine = "";
                sLine = objReader.ReadToEnd();

                string dt = sLine;
                dt = dt.Substring(dt.IndexOf("\"datetime\":\""));

                string time = dt.Substring(dt.IndexOf("T") + 1);
                time = time.Substring(0, time.IndexOf(".") + 3);
                time = time.Replace(".", ",");

                string command = "time " + time + " && exit";//команда, которую вы хотите выполнить

                stopWatch.Stop();

                PCTime = PCTime.AddSeconds(stopWatch.Elapsed.TotalSeconds);

                Process.Start("C:\\WINDOWS\\system32\\cmd.exe", "/k "+command.Replace(".", ","));

                //Заносим данные в лог-файл
                File.AppendAllText("log.log", $"Время в системе {PCTime}," +
                    $"{PCTime.Millisecond.ToString("00").Substring(0, 2)} " +
                    $"время сервера {time}\r\n");

                //Выводим время последней синхронизации в интерфейс
                synchronization.Text = DateTime.Now.ToString();
            }
            catch (Exception e)
            {
                File.AppendAllText("log.log", $"Произошла ОШИБКА - Время в системе {PCTime} \r\n");
                File.AppendAllText("log.log", e.Message+"\r\n");
            }
        }

        /// <summary>
        /// Принудительная синхронизация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSynchronization_Click(object sender, EventArgs e)
        {
            SynchronizationTime();
        }
    }
}
