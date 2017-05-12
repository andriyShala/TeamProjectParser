using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using System.Threading;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        IEnumerable<Vacancy> hh()
        {
            int i = 0;
            while (true)
            {
                if(i==10)
                {
                    yield break;
                }
                yield return new Vacancy() { Title=(i++).ToString()};
                Thread.Sleep(2000);
             }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServiceReference1.ParseServiceClient client = new ServiceReference1.ParseServiceClient();
           

            client.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Run(() => sdas());   
        }
        void sdas()
        {
            foreach (var item in hh())
            {
                listBox1.Invoke(new Action(() => listBox1.Items.Add(item.Title)));
            }
        }
    }
}
