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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientUI.ServiceReference1;
using MahApps;
using MahApps.Metro;
using MahApps.Metro.Controls;


namespace ClientUI
{
    /// <summary>
    /// MahApps.Metro.Controls.MetroWindow
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        private ServiceReference1.ParseServiceClient client;
        ServiceReference1.Vacancy []vacancies;
        public MainWindow()
        {
            InitializeComponent();
            client = new ParseServiceClient();
            var sites = client.GetSites();
            foreach (var item in sites)
            {
                siteChooseCB.Items.Add(item);
            }
            siteChooseCB.SelectedIndex = 0;
            var category = client.GetCategory();
            foreach (var item in category)
            {
                categoryChooseCB.Items.Add(item);
            }
            categoryChooseCB.SelectedIndex = 0;
            var cities = client.GetCity();
            foreach (var item in cities)
            {
                regionChooseCB.Items.Add(item);
            }
            regionChooseCB.SelectedIndex = 0;
            dateBox.SelectedIndex = 0;
        }

      

        private void SearchBut_OnClick(object sender, RoutedEventArgs e)
        {
            vacanciesListBox.Items.Clear();
          if(String.IsNullOrEmpty(searchBox.Text))
            {
                vacancies = client.GetVacancies(categoryChooseCB.Text, regionChooseCB.Text, siteChooseCB.Text, Convert.ToInt32(dateBox.Text));
                foreach (var item in vacancies)
                {
                    vacanciesListBox.Items.Add(item.VacancyId + ": " + item.Title);
                }
            }
            else
            {
                vacancies = client.GetVacanciesBySearch(searchBox.Text,categoryChooseCB.Text, regionChooseCB.Text, siteChooseCB.Text, Convert.ToInt32(dateBox.Text));
                foreach (var item in vacancies)
                {
                    vacanciesListBox.Items.Add(item.VacancyId + ": " + item.Title);
                }
            }
        }
    }
}
