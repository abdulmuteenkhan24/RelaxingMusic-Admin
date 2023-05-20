
using Blissful_Music_Admin.ViewModels;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

namespace Blissful_Music.Pages
{
    /// <summary>
    /// Interaction logic for Remove_Video.xaml
    /// </summary>
    public partial class Remove_Video : Page
    {
        private static FirebaseClient fbase;
        ArrayList Videos_Name_list;
        ArrayList Videos_Link_list;
        ArrayList list;
        string cat_url = "";
        public Remove_Video()
        {
            InitializeComponent();
            fbase = new FirebaseClient(constants.FirebaseDBUrl);
            list = new ArrayList();
            Info();
            Console.WriteLine(list.Count.ToString());




        }
        public void Info()
        {
            list.Clear();
             this.Dispatcher.BeginInvoke(new Action(async () =>
            {
                await GetInfo();
            }));
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
                    Key = item.Key,
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
            GetList();
        }


        private async Task GetList()
        {
            try
            {
                Videos_list.Items.Clear();
                if (Videos_Link_list != null)
                {
                    Videos_Name_list.Clear();
                    Videos_Link_list.Clear();
                    cat_url = null;
                }
                var dinos = await fbase
                .Child("Tables")
                .OrderByKey()
                .StartAt(dropdown.Text)
                .LimitToFirst(1)
                .OnceAsync<Data_ViewModel>();


                foreach (var dino in dinos)
                {
                    Videos_Name_list = new ArrayList(dino.Object.VIDEOS_NAME.Split(','));
                    Videos_Link_list = new ArrayList(dino.Object.VIDEOS_LINK.Split(','));
                    cat_url = dino.Object.IMAGE;

                }
                foreach (string s in Videos_Name_list)
                {
                    Videos_list.Items.Add(s);
                }
            }catch(Exception ex)
            {

            }
        }
        private void OnItemClicked(object sender, RoutedEventArgs e)

        {
            try
            {
                MenuItem menuItem = (MenuItem)e.Source;

                var myItem = menuItem.CommandParameter.ToString();
                dynamic index = Videos_Name_list.IndexOf(myItem);
                Videos_Name_list.RemoveAt(index);
                Videos_Link_list.RemoveAt(index);

                var Name_strings = Videos_Name_list.Cast<string>().ToArray();
                var Name_theString = string.Join(",", Name_strings);

                var url_strings = Videos_Link_list.Cast<string>().ToArray();
                var theurlString = string.Join(",", url_strings);
                if (!string.IsNullOrEmpty(dropdown.Text) && !string.IsNullOrEmpty(cat_url) && !string.IsNullOrEmpty(Name_theString))
                {

                    var putdata = new addcategoriesModel
                    {
                        NAME = dropdown.Text,
                        IMAGE = cat_url,
                        VIDEOS_LINK = theurlString,
                        VIDEOS_NAME = Name_theString,
                    };
                    fbase.Child("Tables/" + dropdown.Text).PutAsync(putdata);
                    index = null;
                    Name_strings = null;
                    Name_theString = null;
                    url_strings = null;
                    theurlString = null;
                    myItem = null;
                     GetList();
                   
                }
                else
                {
                    header.Content = "Someting Went Wrong!!";
                }
            }catch(Exception ex)
            {


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

 
    }

}


