using Blissful_Music;
using Blissful_Music_Admin.ViewModels;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
 
namespace Expense_MS.Pages
{
    /// <summary>
    /// Interaction logic for addvideos.xaml
    /// </summary>
    public partial class addvideos : Page
    {

        public static FirebaseClient fbase;
        ArrayList list;

        string Video_Links;
        string FVideo_Links;
        string Video_Names;
        string FVideo_Names;
        string url;
        string Cat_name;
        string Cats_imgurl;


        public addvideos()
        {
            try
            {
                fbase = new FirebaseClient(constants.FirebaseDBUrl);
                list = new ArrayList();
                Info();
                Console.WriteLine(list.Count.ToString());
            }
            catch(Exception ex) { }
        }


        public void Info()
        {
            try
            {
               
                list.Clear();
                
                this.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    await GetInfo();
                }));
            }
            catch(Exception ex) { }
        }
   

        public async Task<ArrayList> GetInfo()
        {
            try
            {


                var aaa = await fbase
                 .Child("Tables")
                 .OnceAsync<Data_ViewModelKey>();
                aaa.Select(item => new Data_ViewModelKey
                {
                    Key = item.Key,    //the node name is here
                }).ToList();
                if (this.dropdown.Items.Count > 0)
                {
                    dropdown.Items.Clear();
                }
                foreach (var bb in aaa)
                {
                    dropdown.Items.Add(bb.Key.ToString());
                }

            }
            catch (Exception ex)
            {

            }

            return list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                next_btn.Visibility = Visibility.Hidden;
                selet_label.Visibility = Visibility.Hidden;
                refresh_btn.Visibility = Visibility.Hidden;
                dropdown.Visibility = Visibility.Hidden;

                up_video.Visibility = Visibility.Visible;
                Label_.Visibility = Visibility.Visible;
                done_btn.Visibility = Visibility.Hidden;
                video_name.Visibility = Visibility.Visible;

                if (!string.IsNullOrEmpty(dropdown.Text))
                {
                    var postData = fbase.Child("Tables").OrderByKey().EqualTo(dropdown.Text).AsObservable<Data_ViewModel>().Subscribe(async shot =>
                    {
                        Video_Names = shot.Object.VIDEOS_NAME;
                        Video_Links = shot.Object.VIDEOS_LINK;
                        Cat_name = shot.Object.NAME;
                        Cats_imgurl = shot.Object.IMAGE;
                    });
                    header.Content = dropdown.Text;
                }else
                    header.Content = "Please select Categories!";
            }
            catch (Exception ex)
            {
                header.Content = ex.Message;
            }
        }


            private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Info();
            }
            catch (Exception ex)
            {

            }
        }

        private void done_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(video_name.Text))
            {
                if (!string.IsNullOrEmpty(url) )
                {
                    if (string.IsNullOrEmpty(Video_Names))
                    {
                        FVideo_Names = video_name.Text;
                    }
                    else { FVideo_Names = $"{Video_Names},{video_name.Text}"; }

                    if (string.IsNullOrEmpty(Video_Links))
                    {
                        FVideo_Links = url;
                    }
                    else { FVideo_Links = $"{Video_Links},{url}"; }

                    if (!video_name.Text.Contains(',') && !FVideo_Names.Contains('/'))
                    {

                        var putdata = new addcategoriesModel
                        {
                            NAME = Cat_name,
                            IMAGE = Cats_imgurl,
                            VIDEOS_LINK = FVideo_Links,
                            VIDEOS_NAME = FVideo_Names,


                        };
                        fbase.Child("Tables/" + Cat_name).PutAsync(putdata);
                        header.Content = "Upload successful";

                        if (header.Content == "Upload successful") {
                            Video_Links = null;
                            FVideo_Links = null;
                            Video_Names = null;
                            FVideo_Names = null;
                            url = null;
                            Cat_name = null;
                            Cats_imgurl = null;
                            video_name.Text = null;

                            MessageBox.Show("Upload successful");

                        }
                    }
                    else
                    {
                        Label_.Content = "YOU Can’t use (, {} '; // \\ [] ) in Video Name";
                    }
                
                }
                else
                {
                    header.Content = "Some Thing went Worng!";
                }
            }
            else
            {
                header.Content = "Please Enter Video Name!";
            }
        }

        private void up_video_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();

                dlg.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";

                dlg.ShowDialog();

                uploadAsync(dlg.FileName);

                header.Content = "Wait";
            }
            catch(Exception ex)
            {
                header.Content = ex.Message;
            }
        }


        private async Task uploadAsync(string name)
        {
            try
            {
                var stream = new FileStream(name, FileMode.Open);
                var lastPart = name.Split('\\').Last();
                var storage = new FirebaseStorage("").Child("Videos").Child(Cat_name).Child(lastPart).PutAsync(stream);
                storage.Progress.ProgressChanged += (s, e) => header.Content = $"Progress:{e.Percentage} %";
                url = await storage;
                header.Content = "Click on Done Button";
                done_btn.Visibility= Visibility.Visible;

            }
            catch (Exception ex)
            {
                header.Content = ex.Message;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/categories.xaml", UriKind.Relative));
        }
    }
}
