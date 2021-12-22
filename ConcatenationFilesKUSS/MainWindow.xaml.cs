using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConcatenationFilesKUSS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 21,22
        /// </summary>
        string PPK_SU;
        /// <summary>
        /// 23,24
        /// </summary>
        string PPK_SU_01;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Кнопка на форме "Сформировать файл"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = Convert.ToDateTime(DateStart.SelectedDate);
            DateTime end = Convert.ToDateTime(DateEnd.SelectedDate.Value);

            string[] files = GetData(PPK_SU_01, "23", start, end);
            Result(files, start, end, "23");
            files = GetData(PPK_SU_01, "24", start, end);
            Result(files, start, end, "24");
            files = GetData(PPK_SU, "21", start, end);
            Result(files, start, end, "21");
            files = GetData(PPK_SU, "22", start, end);
            Result(files, start, end, "22");

            MessageBox.Show("Операция завершена");
        }

        /// <summary>
        /// Склеянный файл по каждому КУСС
        /// </summary>
        /// <param name="files">Массив используемых файлов</param>
        /// <param name="start">Дата начала</param>
        /// <param name="end">Дата окончания</param>
        private void Result(string[] files, DateTime start, DateTime end, string KUSS)
        {
            StringBuilder stringBuilder = new StringBuilder();
            #region вывод имён файлов
            //stringBuilder.Append("Список используемых файлов:");
            //foreach (var item in files)
            //{
            //    stringBuilder.Append(item+"\r\n");
            //}
            //MessageBox.Show(stringBuilder.ToString());
            //stringBuilder.Clear();
            #endregion

            using (var destStream = File.Create($"./result/КУСС_{KUSS}.txt"))
            {
                foreach (var file in files)
                {
                    using (var srcStream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) srcStream.CopyTo(destStream);
                }
            }

            //File.Copy(files[0], $"./result/КУСС_{KUSS}.txt", true);


            //foreach (var file in files)
            //{
            //    foreach (var line in File.ReadAllLines(file, Encoding.Default))
            //    {
            //        var date_line = Convert.ToDateTime(line.Substring(0, 10));
            //        if (date_line >= start && date_line <= end)
            //        {
            //            stringBuilder.Append(line + "\r\n");
            //        }
            //    }
            //}
            //File.WriteAllText($"./result/КУСС_{KUSS}.txt", stringBuilder.ToString(), Encoding.Default);
        }

        /// <summary>
        /// Получение данных с одного КУСС
        /// </summary>
        /// <returns>Массив записей одного КУСС</returns>
        private string[] GetData(string path, string KUSS, DateTime start, DateTime end)
        {
            // Получаем список файлов необходимого КУСС
            IEnumerable<FileInfo> files = new DirectoryInfo(path).GetFiles().Where(x => x.Name.Count() > 7 && x.Name.Substring(5,2) == KUSS);

            // Сортируем файлы по дате изменения
            files = files.OrderBy(x => x.LastWriteTime);

            // Определяем начальную дату
            start = start.AddDays(-1);
            string start_str = start.ToString("yyyy_MM_dd");
            int start_shift = 0;
            FileInfo start_file = null;
            for (int i=0; i<10; i++)
            {
                if (files.Where(x => x.Name.Substring(8, 10) == start_str).Count() > 0)
                {
                    start_file = files.First(x => x.Name.Substring(8, 10) == start_str);
                    break;
                }
                else
                {
                    start_shift--;
                    start_str = start.AddDays(start_shift).ToString("yyyy_MM_dd");
                }
            }

            if (start_shift <= -9)
                MessageBox.Show("Ошибка с определением начальной даты");

            // Определяем конечную дату
            //end = end.AddDays(1);
            string end_str = end.ToString("yyyy_MM_dd");
            int end_shift = 0;
            FileInfo end_file = null;
            for (int i = 0; i < 10; i++)
            {
                if (files.Where(x => x.Name.Substring(8, 10) == end_str).Count() > 0)
                {
                    end_file = files.First(x => x.Name.Substring(8, 10) == end_str);
                    break;
                }
                else
                {
                    end_shift++;
                    end_str = end.AddDays(end_shift).ToString("yyyy_MM_dd");
                }
            }
            if (end_file == null)
            {
                end_file = files.Last();
            }

            if (start_shift >= 9)
                MessageBox.Show("Ошибка с определением начальной даты");
            StringBuilder result = new StringBuilder();

            return files
                .Where(x => x.LastWriteTime >= start_file.LastWriteTime && x.LastWriteTime <= end_file.LastWriteTime)
                .Select(x => x.FullName)
                .ToArray<string>();
        }

        /// <summary>
        /// Метод инициализации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Initialized(object sender, EventArgs e)
        {
            string[] path = File.ReadAllLines("./config.cf", Encoding.Default);
            PPK_SU = path[1];
            PPK_SU_01 = path[2];
        }
    }
}
