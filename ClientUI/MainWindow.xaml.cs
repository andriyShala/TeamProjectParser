using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ClientUI.ServiceReference1;
using MahApps;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;


namespace ClientUI
{
    /// <summary>
    /// MahApps.Metro.Controls.MetroWindow
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        private ServiceReference1.ParseServiceClient client;
        ServiceReference1.Vacancy[] vacancies;

        public MainWindow()
        {
            InitializeComponent();
            client = new ParseServiceClient();
            //var sites = client.GetSites();
            //foreach (var item in sites)
            //{
            //    siteChooseCB.Items.Add(item);
            //}
          
            //siteChooseCB.SelectedIndex = 0;
            //var category = client.GetCategory();
            //categoryChooseCB.Items.Add("All");
            //foreach (var item in category)
            //{
            //    categoryChooseCB.Items.Add(item);
            //}
           
            //categoryChooseCB.SelectedIndex = 0;
            //var cities = client.GetCity();
            //regionChooseCB.Items.Add("All");
            //foreach (var item in cities)
            //{
            //    regionChooseCB.Items.Add(item);
            //}
            //regionChooseCB.SelectedIndex = 0;
            //dateBox.SelectedIndex = 0;
            {
                ServiceReference1.Vacancy vac = new ServiceReference1.Vacancy();
                List<ServiceReference1.Vacancy> list = new List<ServiceReference1.Vacancy>();
                vac.Title = "Тренер по продажам / Гуру телефонных продаж";
                vac.Company = "Смарт Системс Девелопмент";
                vac.Сategory = "HR";
                vac.Location = "Львов";
                vac.ContactPerson = "Александр Матвийчук";
                vac.PhoneNumber = "380931371777";
                vac.Description =
                    "Компания Smart System Development ищет БОГА продаж, который продаст даже скрипт нашим менеджерам!";
                vac.Salary = "20 000 грн";
                vac.Experience = "Наявність досвіду роботи із документами та конфіденційною інформацією буде перевагою";
              
                vac.VacancyHref = "https://rabota.ua/company3221854/vacancy6708659";
                vac.CompanyWebSite = "https://rabota.ua/company3221854#3221854";
                vac.PublicationDate = DateTime.Now;

                list.Add(vac);
                vacListView.ItemsSource = list;

            }

        }



        private void SearchBut_OnClick(object sender, RoutedEventArgs e)
        {
            //vacanciesListBox.Items.Clear();
            //string cat = null;
            //if (categoryChooseCB.Text != "All")
            //    cat = categoryChooseCB.Text;

            //string city = null;
            //if (regionChooseCB.SelectedItem.ToString() != "All")
            //    city = regionChooseCB.Text;


            //if (String.IsNullOrEmpty(searchBox.Text))
            //{

            //    vacancies = client.GetVacancies(cat, city, siteChooseCB.Text,
            //        Convert.ToInt32(dateBox.Text));
                
            //    foreach (var item in vacancies)
            //    {
            //        ListBoxItem lb = new ListBoxItem();
            //        lb.Content = item.Title;
            //        lb.Tag = item;
            //        vacanciesListBox.Items.Add(lb);
            //    }
            //}
            //else
            //{
            //    vacancies = client.GetVacanciesBySearch(searchBox.Text, cat, city,
            //        siteChooseCB.Text, Convert.ToInt32(dateBox.Text));
            //    foreach (var item in vacancies)
            //    {
            //        vacanciesListBox.Items.Add(item);
            //    }
            //}
        }

     

        private void VacanciesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
         
        }

      

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
           InfoWindow info = new InfoWindow(vacListView.SelectedItem as ServiceReference1.Vacancy);
           info.Show();
        }
    }
}
