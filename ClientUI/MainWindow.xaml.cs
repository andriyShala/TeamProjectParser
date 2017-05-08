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
            var sites = client.GetSites();
            foreach (var item in sites)
            {
                siteChooseCB.Items.Add(item);
            }
          
            siteChooseCB.SelectedIndex = 0;
            var category = client.GetCategory();
            categoryChooseCB.Items.Add("All");
            foreach (var item in category)
            {
                categoryChooseCB.Items.Add(item);
            }
           
            categoryChooseCB.SelectedIndex = 0;
            var cities = client.GetCity();
            regionChooseCB.Items.Add("All");
            foreach (var item in cities)
            {
                regionChooseCB.Items.Add(item);
            }
            regionChooseCB.SelectedIndex = 0;
            dateBox.SelectedIndex = 0;
            Vacancy vac = new Vacancy();



        }



        private void SearchBut_OnClick(object sender, RoutedEventArgs e)
        {
            vacanciesListBox.Items.Clear();
            string cat = null;
            if (categoryChooseCB.Text != "All")
                cat = categoryChooseCB.Text;

            string city = null;
            if (regionChooseCB.SelectedItem.ToString() != "All")
                city = regionChooseCB.Text;


            if (String.IsNullOrEmpty(searchBox.Text))
            {

                vacancies = client.GetVacancies(cat, city, siteChooseCB.Text,
                    Convert.ToInt32(dateBox.Text));
                
                foreach (var item in vacancies)
                {
                    ListBoxItem lb = new ListBoxItem();
                    lb.Content = item.Title;
                    lb.Tag = item;
                    vacanciesListBox.Items.Add(lb);
                }
            }
            else
            {
                vacancies = client.GetVacanciesBySearch(searchBox.Text, cat, city,
                    siteChooseCB.Text, Convert.ToInt32(dateBox.Text));
                foreach (var item in vacancies)
                {
                    vacanciesListBox.Items.Add(item);
                }
            }
        }

        private void VacancyHrefB_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(vacancyHrefB.NavigateUri.ToString());
        }

        private void VacanciesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                ServiceReference1.Vacancy vac =
                    (vacanciesListBox.SelectedItems[0] as ListBoxItem).Tag as ServiceReference1.Vacancy;
                titleB.Text = vac.Title;
                categoryB.Text = vac.Сategory;
                locationB.Text = vac.Location;
                companyB.Text = vac.Company;
                phoneNumberB.Text = vac.PhoneNumber;
                contactPersonB.Text = vac.ContactPerson;
                if (vac.CompanyWebSite != null)
                    companyWebSiteB.NavigateUri = new Uri(vac.CompanyWebSite);
                publicationDateB.Text = vac.PublicationDate.ToShortDateString();
                experienceB.Text = vac.Experience;
                educationB.Text = vac.Education;
                salaryB.Text = vac.Salary;
                descriptionB.Text = vac.Description;
                if (vac.VacancyHref != null)
                    vacancyHrefB.NavigateUri = new Uri(vac.VacancyHref);
            }
            catch (Exception)
            {
                
                
            }
         
        }

        private void CompanyWebSiteB_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(companyWebSiteB.NavigateUri.ToString());
        }
    }
}
