
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
 

namespace RelaxingMusic_Admin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
 

        public MainWindow()
        {
            InitializeComponent();
          
            Uri uri = new Uri("Pages/categories.xaml", UriKind.Relative);
            mainWinFrame.Navigate(uri);
           
        }

        private void btn_categories_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Pages/categories.xaml", UriKind.Relative);
            mainWinFrame.Navigate(uri);
        }

        private void btn_OSE_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Pages/addvideos.xaml", UriKind.Relative);
            mainWinFrame.Navigate(uri);     
        }
        

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Pages/delete.xaml", UriKind.Relative);
            mainWinFrame.Navigate(uri);
        }

        private void btn_remove_video_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Pages/Remove_Video.xaml", UriKind.Relative);
            mainWinFrame.Navigate(uri);
        }
    }
}
