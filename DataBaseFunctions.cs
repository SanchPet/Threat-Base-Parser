using System;
using System.Collections.Generic;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Forms;
using System.Net;

namespace Homework_2_Csharp_Courses
{
    class DataBase
    {
        public static string MainDirectory;
        public static List<Threat> dataBase;
        public static string pathFileFolder = "D:\\Self development as a programmer\\C#\\C# Courses\\Labs\\Homework_2_Csharp_Courses\\Base\\InstallPath\\";

        public static int Count { get=>dataBase.Count; }

        public Threat this[int index]
        {
            get
            {
                return dataBase[index];
            }
        }


        public static List<Threat> ExcelToThreadList(string path, string filename)
        {
            Excel.Application excelApp = new Excel.Application();
            List<List<string>> maping = new List<List<string>>();
            try
            {               
                excelApp.Workbooks.Open(path + filename);
                int row = 3;
                Excel.Worksheet currentSheet = (Excel.Worksheet)excelApp.Workbooks[1].Worksheets[1];
                while (currentSheet.get_Range("A" + row).Value2 != null)
                {
                    List<string> tempList = new List<string>();
                    for (char column = 'A'; column < 'K'; column++)
                    {
                        Excel.Range cell = currentSheet.get_Range(column.ToString() + row.ToString());
                        tempList.Add(cell != null ? cell.Value2.ToString() : "");
                    }
                    maping.Add(tempList);
                    row++;
                }
                excelApp.Workbooks.Close();
                excelApp.Quit();
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show("Ошибка парсера, " + e.Message);
                excelApp.Quit();
            }
            List<Threat> resultList = new List<Threat>();
            foreach (List<string> record in maping)
            {
                resultList.Add(new Threat(Int32.Parse(record[0]), record[1], record[2], record[3], record[4], record[5] == "1" ? true : false, record[6] == "1" ? true : false, record[7] == "1" ? true : false));
            }
            return resultList;
        }


        public static string DifferencesInThreatLists(List<Threat> original, List<Threat> downloaded)
        {
            int refreshCount = 0;
            int addCount = 0;
            string changeLog = "";
            for (int i = 0; i < original.Count; i++)
            {
                bool recordIsRefreshed = false;
                if (original[i].Name != downloaded[i].Name)
                {
                    changeLog += $"Номер: {original[i].Identificator}, раздел: Наименование. Изменение: {original[i].Name} -> {downloaded[i].Name}.\n";
                    recordIsRefreshed = true;
                }
                if (original[i].Description != downloaded[i].Description)
                {
                    changeLog += $"Номер: {original[i].Identificator}, раздел: Описание. Изменение: {original[i].Description} -> {downloaded[i].Description}.\n";
                    recordIsRefreshed = true;
                }
                if (original[i].Source != downloaded[i].Source)
                {
                    changeLog += $"Номер: {original[i].Identificator}, раздел: Источник. Изменение: {original[i].Source} -> {downloaded[i].Source}.\n";
                    recordIsRefreshed = true;
                }
                if (original[i].Objective != downloaded[i].Objective)
                {
                    changeLog += $"Номер: {original[i].Identificator}, раздел: Объект. Изменение: {original[i].Objective} -> {downloaded[i].Objective}.\n";
                    recordIsRefreshed = true;
                }
                if (original[i].IsAccessibilityBroken != downloaded[i].IsAccessibilityBroken)
                {
                    changeLog += $"Номер: {original[i].Identificator}, раздел: Доступность. Изменение: {(original[i].IsAccessibilityBroken ? "Нарушена" : "Не нарушена")}-> {(downloaded[i].IsAccessibilityBroken ? "Нарушена" : "Не нарушена")}.\n";
                    recordIsRefreshed = true;
                }
                if (original[i].IsConfodentialityBroken != downloaded[i].IsConfodentialityBroken)
                {
                    changeLog += $"Номер: {original[i].Identificator}, раздел: Конфиденциальность. Изменение: {(original[i].IsConfodentialityBroken ? "Нарушена" : "Не нарушена")}-> {(downloaded[i].IsConfodentialityBroken ? "Нарушена" : "Не нарушена")}.\n";
                    recordIsRefreshed = true;
                }
                if (original[i].IsIntegrityBroken != downloaded[i].IsIntegrityBroken)
                {
                    changeLog += $"Номер: {original[i].Identificator}, раздел: Целостность. Изменение: {(original[i].IsIntegrityBroken ? "Нарушена" : "Не нарушена")}-> {(downloaded[i].IsIntegrityBroken ? "Нарушена" : "Не нарушена")}.\n";
                    recordIsRefreshed = true;
                }
                if (recordIsRefreshed) refreshCount++;
            }

            if (downloaded.Count != original.Count)
            {
                changeLog += "\n\nДобавлены следующие угрозы:\n\n";
                for (int i = original.Count; i < downloaded.Count; i++)
                {
                    changeLog += $"Номер: {downloaded[i].Identificator}, Наименование: {downloaded[i].Name}.\n";
                    addCount++;
                }
            }
            string changeLogHeader = $"База данных обновлена! Обновлено {refreshCount} секций, добавлено {addCount}.\n\nОбновлены следующие секции:\n\n";
            if (changeLog.Length == 0) return null;
            else return changeLogHeader + changeLog;
        }

        

        public static bool IsDataBaseInstalled()
        {
            if (Directory.GetFiles(pathFileFolder, "installPath.txt").Length == 0)
            {
                System.Windows.MessageBox.Show("Похоже, что вы здесь в первый раз!\n\nСейчас вы будете переправлены на экран загрузки базы данных.", "Добро пожаловать!", MessageBoxButton.OK, MessageBoxImage.Information);               
                return false;
            }
            else
            {
                StreamReader sr = new StreamReader(pathFileFolder + "installPath.txt");
                MainDirectory = sr.ReadToEnd();
                System.Windows.MessageBox.Show("Мы заметили, что вы уже скачивали базу данных!\n\nВ таком случае, вы можете сразу приступить к работе с базой.", "Добро пожаловать вновь!", MessageBoxButton.OK, MessageBoxImage.Information);
                dataBase = LoadDataBase(MainDirectory + "database.s");
                return true;
            }
        }

        public static void SetPath()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    MainDirectory = fbd.SelectedPath + "\\";
                }
            }
        }

        public static void DownloadBase()
        {
            string link = @"https://bdu.fstec.ru/files/documents/thrlist.xlsx";
            WebClient webClient = new WebClient();
            if (Directory.GetFiles(MainDirectory, "database.s").Length != 0)
            {
                System.Windows.MessageBox.Show("Вы и так уже скачали эту базу.", "Ошибка...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                webClient.DownloadFile(new Uri(link), MainDirectory + "base.xls");
                dataBase = ExcelToThreadList(MainDirectory, "base.xls");
                File.Delete(MainDirectory + "base.xls");
                try
                {                              
                    SaveDataBase(dataBase);
                    File.WriteAllText(pathFileFolder + "installPath.txt", MainDirectory, Encoding.Default);
                    System.Windows.MessageBox.Show("База данных создана!", "Вы совершили действие...", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            }   
        }

        public static void RefreshTheBase()
        {
            string link = @"https://bdu.fstec.ru/files/documents/thrlist.xlsx";
            WebClient webClient = new WebClient();
            webClient.DownloadFile(new Uri(link), MainDirectory + "base.xls");
            List<Threat> newDataBase = ExcelToThreadList(MainDirectory, "base.xlsx");
            string result = DifferencesInThreatLists(dataBase, newDataBase);
            if (result == null)
            {
                System.Windows.MessageBox.Show("Обновлять нечего! На вашем компьютере скачана актуальная база.", "Вы совершили действие...", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show(result, "Вы совершили действие...", MessageBoxButton.OK, MessageBoxImage.Information);
                SaveDataBase(newDataBase);
                dataBase = newDataBase;
            }
            File.Delete(MainDirectory + "base.xls");
        }

        public static void SaveDataBase(List<Threat> source)
        {
            FileStream fs = new FileStream(MainDirectory + "database.s", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, source);
            fs.Close();
        }

        public static List<Threat> LoadDataBase(string path)
        {
            List<Threat> result;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter bf = new BinaryFormatter();
            result = (List<Threat>)bf.Deserialize(fs);
            fs.Close();
            return result;
        }
    }
}
