using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.IO;
using System.Media;

namespace PARSE_log_AT
{
    public partial class Form1 : Form
    {
        
        //Переменные для работы с запросами
        String strURL;
        HttpWebRequest objWebRequest;
        HttpWebResponse objWebResponse;
        StreamReader streamReader;

        /// <summary>
        /// Строковое значение АПО
        /// </summary>
        string APO = "";

        int START_parse;    // разрешение и запрещение парсинга

        string IP_adress_AT;    // утанавливаем IP-адрес для запроса

        string date_now; // текущая дата
        string date_now_new_f; // отформатированная дата
        string date_old;

        int count_find_error;   // счетчик парсинга

        int p_save; // позиция начала поиска событий

        string time_s; // время события

        public int num_str;    // текущий номер строки заполнения в таблице

        public Form1()
        {
            InitializeComponent();

            START_parse = 0;    // запрещаем парсинг
        }
        //////////////////////////////////////////////////////////////////////////////
        //                                                                          //
        //      СТАРТ парсинга                                                      //
        //                                                                          //
        //////////////////////////////////////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                IP_adress_AT = textBox1.Text;    // утанавливаем IP-адрес для запроса

                START_parse = 1;    // разрешаем парсинг

                timer1.Enabled = true;  //включить таймер

                count_find_error = 58;  // 

                p_save = 0; // ищем с первого символа

                // таблица с данными закладки
                dataGridView1.RowHeadersVisible = false;        // убрать фиксированную строку 
                dataGridView1.ColumnHeadersVisible = false;     // убрать фиксированную колонку
                dataGridView1.ReadOnly = true;                  // запрет редактирования таблицы
                dataGridView1.AllowUserToResizeColumns = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.ColumnCount = 2;
                dataGridView1.RowCount = 2;
                //dataGridView1.Columns[0].Width = 125;

                dataGridView1.Rows[0].Cells[0].Value = "Время";
                dataGridView1.Rows[0].Cells[1].Value = "Событие";

                // таблица с данными МС
                dataGridView2.RowHeadersVisible = false;        // убрать фиксированную строку 
                dataGridView2.ColumnHeadersVisible = false;     // убрать фиксированную колонку
                dataGridView2.ReadOnly = true;                  // запрет редактирования таблицы
                dataGridView2.AllowUserToResizeColumns = false;
                dataGridView2.AllowUserToResizeRows = false;
                dataGridView2.ColumnCount = 3;
                dataGridView2.RowCount = 2;
                //dataGridView2.Columns[0].Width = 125;

                dataGridView2.Rows[0].Cells[0].Value = "Время";
                dataGridView2.Rows[0].Cells[1].Value = "Расп. КА";
                dataGridView2.Rows[0].Cells[2].Value = "Борт и тип МС";

                button1.Enabled = false;
                button2.Enabled = true;
                textBox1.Enabled = false;
                textBox1.BackColor = Color.PaleGreen;

                richTextBox2.Clear();       // чистим поле, куда выводятся ошибки

            }
            catch (Exception ex)
            {
                richTextBox2.Text += "\n" + DateTime.Now.ToString() + "  " + ex.Message;
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        //////////////////////////////////////////////////////////////////////////////
        //                                                                          //
        //      СТОП парсинга                                                       //
        //                                                                          //
        //////////////////////////////////////////////////////////////////////////////
        private void button2_Click(object sender, EventArgs e)
        {
            START_parse = 0;    // запрет парсинга
            toolStripStatusLabel2.Text = "";

            button1.Enabled = true;
            button2.Enabled = false;
            textBox1.Enabled = true;
            textBox1.BackColor = Color.White;
        }

        //////////////////////////////////////////////////////////////////////////////
        //                                                                          //
        //      Поиск событий                                                       //
        //                                                                          //
        //////////////////////////////////////////////////////////////////////////////
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (START_parse == 1)
            {
                count_find_error = count_find_error + 1;
                toolStripStatusLabel2.Text = (60 - count_find_error).ToString();    // выводим обратный отсчет

                if (count_find_error == 60)
                {
                    try
                    {

                        date_now = DateTime.Now.ToShortDateString(); // узнаем текущую дату

                        date_now_new_f = date_now[0].ToString() + date_now[1].ToString() + "-" + date_now[3].ToString() + date_now[4].ToString() + "-" + date_now[8].ToString() + date_now[9].ToString();

                        strURL = "http://" + IP_adress_AT + "/log/" + date_now_new_f + ".log";  // формируем адрес для запроса

                        if (date_now != date_old)   // отлов перехода дат
                        {
                            p_save = 0; // ищем с первого символа

                            // таблица с данными закладки
                            dataGridView1.ColumnCount = 2;
                            dataGridView1.RowCount = 2;

                            dataGridView1.Rows[0].Cells[0].Value = "Время";
                            dataGridView1.Rows[0].Cells[1].Value = "Событие";

                            // таблица с данными МС
                            dataGridView2.ColumnCount = 3;
                            dataGridView2.RowCount = 2;

                            dataGridView2.Rows[0].Cells[0].Value = "Время";
                            dataGridView2.Rows[0].Cells[1].Value = "Расп. КА";
                            dataGridView2.Rows[0].Cells[2].Value = "Борт и тип МС";
                        }

                        //richTextBox2.Text += "old = " + date_old + "now = " + date_now + "\n";

                        objWebRequest = (HttpWebRequest)WebRequest.Create(strURL);  // 
                        objWebRequest.Method = "GET";

                        objWebRequest.Credentials = new NetworkCredential("admin", "nimda");    // делаем авторизированный вход для АТ

                        objWebResponse = (HttpWebResponse)objWebRequest.GetResponse();          // парсим страницу и запихиваем в поток
                        streamReader = new StreamReader(objWebResponse.GetResponseStream(), System.Text.Encoding.Default);

                        string body = streamReader.ReadToEnd(); //весь код страницы

                        richTextBox1.Text = date_now_new_f + "\n\n" + body;

                        richTextBox1.SelectionStart = richTextBox1.Text.Length; // прокрутка каретки в конец
                        richTextBox1.ScrollToCaret();

                        //создать (если нет) либо открыть если есть и записать текст (путем замены если что то      было   записано)
                        File.WriteAllText("log_AT.txt", body);

                        for (int i=0; i<=500; i++)
                            body += " ";    // наполняем данные пробелами 
                        
                        streamReader.Close();       // убираем за собой
                        objWebResponse.Close();
                        objWebRequest.Abort();


                        int Read_Data = body.Length;

                        for (int a = p_save; a < Read_Data; a++)
                        {
                            //////////////////////////////////////////////////////////////////////////////
                            //      Ищем АПО                                                            //
                            //////////////////////////////////////////////////////////////////////////////
                            
                            if ((body[a] == 'N') && (body[a+1] == 'o') && (body[a+2] == 'i') && (body[a+3] == 's') && (body[a+4] == 'e'))
                            {
                                APO = "";
                                int i = 0;
                                while(body[a + 7 + i] != 0x0D)
                                {
                                    APO += body[a + 7 + i];

                                    i++;
                                }
                                p_save = a + 6; // позиция для сохранения
                            }

                            /* Ищем  */

                            //////////////////////////////////////////////////////////////////////////////
                            //       Нет ответа на запрос                                               //
                            //////////////////////////////////////////////////////////////////////////////
                            if ((body[a] == 'Н') && (body[a + 1] == 'е') && (body[a + 2] == 'т') && (body[a + 3] == ' ') && 
                                (body[a + 4] == 'о') && (body[a + 5] == 'т') && (body[a + 6] == 'в') && (body[a + 7] == 'е'))
                            {
                                time_s = "";

                                for (int j = a-9; j <= a-1; j++)    // выделяем время
                                {
                                   time_s += body[j].ToString();
                                }

                                num_str = dataGridView1.Rows.Add(); // добавить новую строку

                                dataGridView1.Rows[num_str].Cells[0].Value = date_now + "  " + time_s;
                                dataGridView1.Rows[num_str].Cells[1].Value = "Нет ответа на запрос";

                                dataGridView1.FirstDisplayedCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];   // автоматическая прокрутка


                                p_save = a + 10; // позиция для сохранения
                            }

                            //////////////////////////////////////////////////////////////////////////////
                            //       Закладка выполнена успешно                                         //
                            //////////////////////////////////////////////////////////////////////////////
                            if ((body[a] == 'З') && (body[a + 1] == 'а') && (body[a + 2] == 'к') && (body[a + 3] == 'л') &&
                                (body[a + 4] == 'а') && (body[a + 5] == 'д') && (body[a + 6] == 'к') && (body[a + 7] == 'а') &&
                                (body[a + 8] == ' ') && (body[a + 9] == 'в') && (body[a + 10] == 'ы') && (body[a + 11] == 'п') &&
                                (body[a + 12] == 'о') && (body[a + 13] == 'л') && (body[a + 14] == 'н') && (body[a + 15] == 'е'))
                            {
                                time_s = "";

                                for (int j = a - 23; j <= a - 15; j++)    // выделяем время
                                {
                                    time_s += body[j].ToString();
                                }

                                num_str = dataGridView1.Rows.Add(); // добавить новую строку

                                dataGridView1.Rows[num_str].Cells[0].Value = date_now + "  " + time_s;
                                dataGridView1.Rows[num_str].Cells[1].Value = "Закладка выполнена успешно";

                                dataGridView1.FirstDisplayedCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];   // автоматическая прокрутка

                                p_save = a + 20; // позиция для сохранения
                            }

                            //////////////////////////////////////////////////////////////////////////////
                            //       Поик МС                                                            //
                            //////////////////////////////////////////////////////////////////////////////
                            if (((body[a] == 'M') && (body[a + 1] == 'С') && (body[a + 2] == '1')) ||
                                ((body[a] == 'M') && (body[a + 1] == 'С') && (body[a + 2] == '2')))
                            {
                                time_s = "";

                                for (int j = a - 9; j <= a - 1; j++)    // выделяем время
                                {
                                    time_s += body[j].ToString();
                                }

                                string MS_KA = "";

                                for (int j = a; j <= a + 12; j++)    // описание маркера
                                {
                                    MS_KA += body[j].ToString();
                                }

                                int MS_pos = 0;     // ищем нахождение спутника
                                int i = 0;
                                while (body[a + i] != 0x0A)
                                {
                                    MS_pos = a + i;

                                    i++;
                                }


                                num_str = dataGridView2.Rows.Add(); // добавить новую строку

                                dataGridView2.Rows[num_str].Cells[0].Value = date_now + "  " + time_s;

                                if ((body[MS_pos + 2] == 'В') && (body[MS_pos + 3] == 'н') && (body[MS_pos + 4] == 'е'))
                                {
                                    dataGridView2.Rows[num_str].Cells[1].Value = "Вне радиуса";
                                }
                                else
                                {
                                    dataGridView2.Rows[num_str].Cells[1].Value = "В зоне";
                                }
                                                               
                                dataGridView2.Rows[num_str].Cells[2].Value = MS_KA;

                                dataGridView2.FirstDisplayedCell = dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[0];   // автоматическая прокрутка


                                p_save = a + 10; // позиция для сохранения
                            }
                        }

                        date_old = date_now;
                        toolStripStatusLabel1.Text = "APO:   " + APO + $"{AverageAPO(APO)}                      ";

                        /*Проверка на отсутсвие закладок*/
                        var gv1 = dataGridView1;
                        if (gv1.Rows.Count > 2)
                        {
                            if (Convert.ToDateTime(gv1.Rows[gv1.Rows.Count - 2].Cells[0].Value.ToString()) < DateTime.Now.AddHours(-2))
                            {
                                SoundPlayer simpleSound = new SoundPlayer(@"1h.wav");
                                simpleSound.Play();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                        richTextBox2.Text += "\n" + DateTime.Now.ToString() + "  " + ex.Message;
                    }

                    count_find_error = 0;
                }
            }
        }

        #region Анализ АПО
        /// <summary>
        /// Сигнализация при привышение среднего значения АПО
        /// </summary>
        /// <param name="APO">строка со значениями АПО</param>
        /// <returns>Среднее арифметическое</returns>
        private string AverageAPO(string APO)
        {
            if (APO != string.Empty)
            {
                //Преобразование в значение
                string[] tempArr = APO.Split(',');

                //Парсинг
                for (int i = 0; i < tempArr.Length; i++)
                {
                    string x = tempArr[i];
                    tempArr[i] = x.Substring(x.IndexOf("(") + 1, x.IndexOf(")") - (x.IndexOf("(") + 1));
                }

                //Среднее арифметическое
                float avr = Average(tempArr);

                //Голосовое оповещение
                int levelAPO;
                int.TryParse(toolStripTextBoxAPO.Text, out levelAPO);
                if (avr > levelAPO)
                {
                    SoundPlayer simpleSound = new SoundPlayer(@"interference2.wav");
                    simpleSound.Play();
                }

                //Возвращение значения
                return $" ~ { avr }";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Среднее арифметическое
        /// </summary>
        /// <param name="list">Список чисел (строкой)</param>
        /// <returns>Среднее арифметическое</returns>
        private float Average(string[] list)
        {
            float res = 0;
            foreach (var item in list)
                res += float.Parse(item);
            return res / list.Length;
        }
        #endregion




        //////////////////////////////////////////////////////////////////////////////
        //                                                                          //
        //      Раскрашиваем таблицы                                                //
        //                                                                          //
        //////////////////////////////////////////////////////////////////////////////
        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)	// Раскрашиваем ячейки
        {
            if (e.RowIndex == 0 || e.ColumnIndex == 0)
            {
                e.CellStyle.BackColor = Color.Gainsboro;
            }
            if (Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == "Нет ответа на запрос")
            {
                e.CellStyle.BackColor = Color.Pink;
            }
            if (Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == "Закладка выполнена успешно" || Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == "НОРМА")
            {
                e.CellStyle.BackColor = Color.GreenYellow;
            }
        }

        void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)	// Раскрашиваем ячейки
        {
            if (e.RowIndex == 0 || e.ColumnIndex == 0)
            {
                e.CellStyle.BackColor = Color.Gainsboro;
            }
            if (Convert.ToString(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == "В зоне")
            {
                e.CellStyle.BackColor = Color.GreenYellow;
            }
        }
    }
}
