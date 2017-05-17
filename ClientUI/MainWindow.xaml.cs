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
using System.Threading;


namespace ClientUI
{
    /// <summary>
    /// MahApps.Metro.Controls.MetroWindow
    /// </summary>
    /// 

    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        private static object locks = new object();
       /// <summary>
       /// поиск вакансий по фильтрам
       /// </summary>
        private void SerchVacancy()
        {
            Search = true;
            ThreadPool.QueueUserWorkItem(Timer, this);

            this.Invoke(new Action(() => vacListView.Items.Clear()));
            string cat = null;
            string city = null;
            string site = null;
            string stringSearch = null;
            int date = 1;
            this.Invoke((delegate
            {
                if (categoryChooseCB.Text != "All")
                    cat = categoryChooseCB.Text;
                if (regionChooseCB.SelectedItem.ToString() != "All")
                    city = regionChooseCB.Text;
                site = siteChooseCB.Text;
                if (searchBox.Text != string.Empty)
                {
                    stringSearch = searchBox.Text;
                }
                date = Convert.ToInt32(dateBox.Text);
            }));

           
                if (stringSearch == null)
                {

                    vacancies = client.GetVacancies(cat, city, site,
                        date);
                }
                else
                {
                    vacancies = client.GetVacanciesBySearch(stringSearch, cat, city,
                       site, date);

                }
                this.Invoke((delegate
                {
                    foreach (var item in vacancies)
                    {

                        vacListView.Items.Add(item);
                    }
                }));

            Search = false;

        }
        /// <summary>
        /// Инициализация окна
        /// (загрузка єлементов с сервиса)
        /// </summary>
        private void LoadWindow()
        {
            foreach (var item in client.GetSites())
            {
                this.Invoke(new Action(() => siteChooseCB.Items.Add(item)));
            }
            this.Invoke(new Action(() => siteChooseCB.SelectedIndex = 0));
            this.Invoke(new Action(() => categoryChooseCB.Items.Add("All")));
            foreach (var item in client.GetCategory())
            {
                this.Invoke(new Action(() => categoryChooseCB.Items.Add(item)));
            }

            this.Invoke(new Action(() => categoryChooseCB.SelectedIndex = 0));
            this.Invoke(new Action(() => regionChooseCB.Items.Add("All")));
            foreach (var item in client.GetCity())
            {
                this.Invoke(new Action(() => regionChooseCB.Items.Add(item)));
            }
            this.Invoke(new Action(() => regionChooseCB.SelectedIndex = 0));
            this.Invoke(new Action(() => dateBox.SelectedIndex = 0));
        }
        private ServiceReference1.ParseServiceClient client;
        ServiceReference1.Vacancy[] vacancies;
        private static bool Search = false;
        public MainWindow()
        {
            InitializeComponent();
            client = new ParseServiceClient();
            Task.Run(() => LoadWindow());
            ThreadPool.SetMaxThreads(5, 5);
        }
        /// <summary>
        /// Таймер поиска
        /// </summary>
        /// <param name="obj"></param>
        private static void Timer(object obj)
        {
            MainWindow main = obj as MainWindow;
            int sec = 0;
            while (Search == true)
            {
                Thread.Sleep(1000);
                main.Invoke(new Action(() => main.Title = "Search - " + sec++));
            }
            main.Invoke(new Action(() => main.Title = "Vacantion Parser"));
        }

        private void SearchBut_OnClick(object sender, RoutedEventArgs e)
        {
            lock (locks)
            {
                Task.Run(() => SerchVacancy());
            }
        }

        private void VacanciesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }


        /// <summary>
        /// Обработка двойного клика в listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            InfoWindow info = new InfoWindow(vacListView.SelectedItem as ServiceReference1.Vacancy);

            info.Show();
            info.Activate();
            info.Focus();
        }
    }
}
