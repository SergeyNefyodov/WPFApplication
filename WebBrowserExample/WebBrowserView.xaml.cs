using Microsoft.Win32;
using System.Windows;

namespace WPFApplication.WebBrowserExample
{
    /// <summary>
    /// Логика взаимодействия для WebBrowserView.xaml
    /// </summary>
    public partial class WebBrowserView : Window
    {
        public WebBrowserView()
        {
            InitializeComponent();
            Browser.Navigate(@"https://dzen.ru/a/ZkhXjePFq3NCNlGR");
        }

        private void SelectLocalPage(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Веб страницы (*.html)|*.html"
            };
            if (dialog.ShowDialog() == false) return;

            Browser.Navigate(new Uri(dialog.FileName));
        }
    }
}
