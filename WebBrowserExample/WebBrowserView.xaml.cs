using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

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
