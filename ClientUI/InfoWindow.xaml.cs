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
using System.Windows.Shapes;

namespace ClientUI
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : MahApps.Metro.Controls.MetroWindow
    {
        public InfoWindow()
        {
            InitializeComponent();
        }


        public InfoWindow(ServiceReference1.Vacancy vac)
        {
            InitializeComponent();
            try
            {
              

              

               
            

                titleB.Text = (String.IsNullOrEmpty(vac.Title)) ? "No data" : vac.Title;
                categoryB.Text = (String.IsNullOrEmpty(vac.Сategory)) ? "No data" : vac.Сategory;
                locationB.Text = (String.IsNullOrEmpty(vac.Location)) ? "No data" : vac.Location;
                companyB.Text = (String.IsNullOrEmpty(vac.Company)) ? "No data" : vac.Company;
                phoneNumberB.Text = (String.IsNullOrEmpty(vac.PhoneNumber)) ? "No data" : vac.PhoneNumber;
                contactPersonB.Text = (String.IsNullOrEmpty(vac.ContactPerson)) ? "No data" : vac.ContactPerson;
                companyWebSiteB.Text = (String.IsNullOrEmpty(vac.CompanyWebSite)) ? "No data" : vac.CompanyWebSite;
                publicationDateB.Text = (vac.PublicationDate == null) ? "No data" : vac.PublicationDate.ToShortDateString();
                educationB.Text = (String.IsNullOrEmpty(vac.Education)) ? "No data" : vac.Education;
                experienceB.Text = (String.IsNullOrEmpty(vac.Experience)) ? "No data" : vac.Experience;
                salaryB.Text = (String.IsNullOrEmpty(vac.Salary)) ? "No data" : vac.Salary;
                descriptionB.Text = (String.IsNullOrEmpty(vac.Description)) ? "No data" : vac.Description;
                vacancyHrefB.Text = (String.IsNullOrEmpty(vac.VacancyHref)) ? "No data" : vac.VacancyHref;

               
            }
            catch
            {


            }
        }

        private void VacancyHrefB_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            try
            {
                Process.Start(vacancyHrefB.Text);
            }
            catch
            {


            }
        }

        private void CompanyWebSiteB_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start(companyWebSiteB.Text);
            }
            catch
            {


            }
        }
    }
}
