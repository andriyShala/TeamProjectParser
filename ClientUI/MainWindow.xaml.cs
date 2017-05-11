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
            {
                Vacancy vac = new Vacancy();
                Button b = new Button();
                Grid g = new Grid();

                ColumnDefinition c1 = new ColumnDefinition();
                c1.Width = GridLength.Auto;
               
                ColumnDefinition c2 = new ColumnDefinition();
                c2.Width = GridLength.Auto;

                RowDefinition r1 = new RowDefinition();
                r1.Height = GridLength.Auto;
                RowDefinition r2 = new RowDefinition();
                r2.Height = GridLength.Auto;

                g.ColumnDefinitions.Add(c1);
                g.ColumnDefinitions.Add(c2);

                g.RowDefinitions.Add(r1);
                g.RowDefinitions.Add(r2);

                Label t = new Label();
                t.Content = "Test";
                Label t1 = new Label();
                t.Content = "Test1";

                g.Children.Add(t);
                g.Children.Add(t1);

                Grid.SetColumn(t, 0);
                Grid.SetRow(t, 0);

                Grid.SetColumn(t1, 1);
                Grid.SetRow(t1, 0);

                b.Margin = new Thickness(10);
                b.Height = 50;
                b.Content = g;
                
            }

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
