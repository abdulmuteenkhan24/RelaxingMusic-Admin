using Blissful_Music;
using Blissful_Music_Admin.ViewModels;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Expense_MS.Pages
{
    /// <summary>
    /// Interaction logic for categories.xaml
    /// </summary>
    public partial class categories : Page
    {
        string url;
        public static FirebaseClient fbase;
      
        public categories()
        {
            InitializeComponent();
            image.Source = new BitmapImage(new Uri("/images/logo.jpeg", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                messages.Content = "";
                System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
                // image filters  
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine(open.FileName);
                    uploadAsync(open.FileName);
                    image_path.Content = "Wait.....";

                }
            }catch(Exception ex)
            {

            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                fbase = new FirebaseClient(constants.FirebaseDBUrl);

                if (!string.IsNullOrEmpty(categ_name.Text) && !string.IsNullOrEmpty(url))
                {

                    if (!categ_name.Text.Contains(',') && !categ_name.Text.Contains('/'))
                    {

                        var putdata = new addcategoriesModel
                    {
                        NAME = categ_name.Text,
                        IMAGE = url,
                        VIDEOS_LINK = "",
                        VIDEOS_NAME = "",
                    };
                    fbase.Child("Tables/" + categ_name.Text).PutAsync(putdata);
                    messages.Content = "Categories Added successful";

                }
                else
                {
                    messages.Content = "YOU Can’t use (, {} '; // \\ [] ) in Name";
                }
            }
                else
                    messages.Content = "Please Enter Categories Name";
            }
            catch(Exception ex)
            {
                
            }

        }

        private async Task uploadAsync(string name )
        {
            try
            {
                var stream = new FileStream(name, FileMode.Open);
                var lastPart = name.Split('\\').Last();
                var storage = new FirebaseStorage(constants.FirebaseSTGUrl).Child("Images").Child(lastPart).PutAsync(stream);
                storage.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");
                url = await storage;
                image_path.Content = lastPart;

                image.Source = new BitmapImage(new Uri(name, UriKind.RelativeOrAbsolute));
               
            }
            catch(Exception ex)
            {
                messages.Content = ex.Message;
             }
        }

    }
}
