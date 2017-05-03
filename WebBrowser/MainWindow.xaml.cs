using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebBrowser
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoNavigateButton_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(this.AddressTextBox.Text, UriKind.RelativeOrAbsolute);

            // Only absolute URIs can be navigated to
            if (!uri.IsAbsoluteUri)
            {
                MessageBox.Show("The Address URI must be absolute eg 'http://www.microsoft.com'");
                return;
            }

            // Navigate to the desired URL by calling the .Navigate method
            this.MyWebBrowser.Navigate(uri);
        }
    }
}
