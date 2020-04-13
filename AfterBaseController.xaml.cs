using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualBasic;

namespace Homework_2_Csharp_Courses
{
    /// <summary>
    /// Interaction logic for AfterBaseController.xaml
    /// </summary>
    public partial class AfterBaseController : Window
    {
        public AfterBaseController()
        {
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void RefreshBase_Click(object sender, RoutedEventArgs e)
        {
            PleaseWait wait = new PleaseWait();
            wait.Show();
            if (!DataBase.RefreshTheBase())
            {
                wait.Close();
                MainWindow mainWindow = new MainWindow(true);
                mainWindow.Show();
                Close();
            }
            else
            {
                wait.Close();
            }
            
        }

        private void AllThreatsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoWindow showInfoWindow = new ShowInfoWindow();
            showInfoWindow.Owner = this;
            showInfoWindow.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InfoAboutThreatButton_Click(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                try
                {
                    int index = Int32.Parse(Interaction.InputBox($"Введите номер записи:\n\n\nВсего записей: {DataBase.Count}", "Окошечко ввода", "Введите номер"));
                    
                    InfoAboutOne.recievedThreat = new DataBase()[index - 1];
                    break;
                }
                catch
                {
                    MessageBox.Show("Вы что! Такой записи в базе нет...", "Ошибка...", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            InfoAboutOne infoAboutOne = new InfoAboutOne();          
            infoAboutOne.Owner = this;
            infoAboutOne.Show();   
        }
    }
}
