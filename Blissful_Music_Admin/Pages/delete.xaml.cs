using Blissful_Music_Admin.ViewModels;
using Expense_MS;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Blissful_Music.Pages
{
    /// <summary>
    /// Interaction logic for delete.xaml
    /// </summary>
    public partial class delete : Page
    {
        private static FirebaseClient fbase;

        ArrayList list;
        public delete()
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
         //   dropdown.Items.Clear();
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
            catch(Exception ex)
            {

            }

            return list;
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(dropdown.Text))
            {
                fbase.Child("Tables/" + dropdown.Text).DeleteAsync();
                header.Content = "Deleted";
            }
            else
            {
                header.Content = "Please Select Categorie";
            }
            Info();
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
