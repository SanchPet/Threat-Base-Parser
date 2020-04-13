
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Homework_2_Csharp_Courses
{   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool programTriggered = false;

        public MainWindow()
        {
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            if (!programTriggered)
            {
                programTriggered = true;
                if (DataBase.IsDataBaseInstalled())
                {
                    AfterBaseController afterBaseController = new AfterBaseController();
                    afterBaseController.Show();
                    Close();
                }
            }
            
        }

        public MainWindow(bool started)
        {
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            if (!started)
            {
                programTriggered = true;
                if (DataBase.IsDataBaseInstalled())
                {
                    AfterBaseController afterBaseController = new AfterBaseController();
                    afterBaseController.Show();
                    Close();
                }
            }
        }

        private void CreateBaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataBase.MainDirectory == null)
            {
                MessageBox.Show("Вы не выбрали путь установки!", "Ошибка...", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            
                PleaseWait wait = new PleaseWait();
                wait.Show();
                DataBase.DownloadBase();
                wait.Close();
                       
            AfterBaseController afterBaseController = new AfterBaseController();
            afterBaseController.Show();
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetPathButton_Click(object sender, RoutedEventArgs e)
        {
            DataBase.SetPath();
            installPathLabel.Text = DataBase.MainDirectory;
        }
    }

}