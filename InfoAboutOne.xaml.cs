using System;
using System.Windows;
using Microsoft.VisualBasic;

namespace Homework_2_Csharp_Courses
{
    /// <summary>
    /// Interaction logic for InfoAboutOne.xaml
    /// </summary>
    public partial class InfoAboutOne : Window
    {
        public static Threat recievedThreat;

        public InfoAboutOne()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.ResizeMode = ResizeMode.NoResize;

            InitializeComponent();
            SetLabels();
        }

        private void ChangeThreatButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                recievedThreat = new DataBase()[Int32.Parse(Interaction.InputBox($"Введите номер записи:\n\n\nВсего записей: {DataBase.Count}", "Окошечко ввода", "Введите номер")) - 1];
            }
            catch
            {
                MessageBox.Show("Вы что! Такой записи в базе нет...", "Ошибка...", MessageBoxButton.OK, MessageBoxImage.Error);
                ChangeThreatButton_Click(sender, e);
            }
            SetLabels();
        }

        private void SetLabels()
        {
            identLabel.Content = recievedThreat.Identificator;
            nameLabel.Content = recievedThreat.Name;
            descriptionLabel.Text = recievedThreat.Description;
            descriptionLabel.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            descriptionLabel.IsReadOnly = true;
            objectiveLabel.Text = recievedThreat.Objective;
            objectiveLabel.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            descriptionLabel.IsReadOnly = true;
            sourceLabel.Content = recievedThreat.Source;
            confidencialityLabel.Content = recievedThreat.IsConfodentialityBroken ? "Нарушена" : "Не нарушена";
            accessibilityLabel.Content = recievedThreat.IsAccessibilityBroken ? "Нарушена" : "Не нарушена";
            integrityLabel.Content = recievedThreat.IsIntegrityBroken ? "Нарушена" : "Не нарушена";

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
