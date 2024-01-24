using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;

namespace test
{
    public partial class Form1 : Form
    {
        String URL = "http://localhost/SOP%20Beadando/restapi.php";
        String ROUTE = "restapi.php";
        string[] f1Teams = {"Red Bull Racing", "Mercedes-AMG PETRONAS F1 Team", "Scuderia Ferrari" ,
        "Alpine F1 Team", "McLaren F1 Team", "Aston Martin Cognizant F1 Team", "Scuderia AlphaTauri",
         "Haas F1 Team", "Alfa Romeo Racing F1 Team", "Williams Racing"};
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            listing();
        }

        private async void listing()
        {
            try
            {
                var client = new RestClient(URL);
                var request = new RestRequest(ROUTE, Method.Get);

                var response = await client.ExecuteAsync<List<Driver>>(request);

                if (response.IsSuccessful)
                {
                    listView1.Clear();
                    ListCols();

                    foreach (var driver in response.Data)
                    {
                        ListViewItem view = new ListViewItem();
                        view.SubItems[0].Text = driver.Rajtszam.ToString();
                        view.SubItems.Add(driver.Nev.ToString());
                        listView1.Items.Add(view);
                    }
                }
                else
                {
                    throw new responseExcept();
                }
            }
            catch (responseExcept ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!");
            }
        }


        public class Driver
        {
            private string rajtszam;

            public string Rajtszam
            {
                get { return rajtszam; }
                set
                {
                    int a = int.Parse(value);
                    if (!(a < 100 && a > 0))
                    {
                        throw new rajtszamException(a, false);
                    }
                    rajtszam = value;
                }
            }

            private string nev;

            public string Nev
            {
                get { return nev; }
                set { nev = value; }
            }

            public string csapat { get; set; }

            private int szuletesiev;

            public int Szuletesiev
            {
                get { return szuletesiev; }
                set
                {
                    if (!(value > 1923 && value < (DateTime.Now.Year - 18)))
                    {
                        throw new evszamException(value);
                    }
                    szuletesiev = value;
                }
            }

        }

        public Button HozzaAd {
            get
            {
                return button3;
            } 
        }

        public Button Torles
        {
            get
            {
                return button4;
            }
        }
        public Button Modosit
        {
            get
            {
                return button5;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            ListCols();
            try
            {
                var temp = await Search(textBox1.Text);

                if (temp.Count == 0)
                {
                    MessageBox.Show("No results!");
                }

                foreach (var driver in temp)
                {
                    ListViewItem view = new ListViewItem();
                    view.SubItems[0].Text = driver.Rajtszam.ToString();
                    view.SubItems.Add(driver.Nev.ToString());
                    listView1.Items.Add(view);
                }
                textBox1.Text = "";
            }
            catch (responseExcept ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!");
            }
        }

        private async Task<List<Driver>> Search(string rajtszam)
        {
            var client = new RestClient(URL);
            string ROUTE = "restapi.php" + "?rajtszam=" + rajtszam;
            var request = new RestRequest(ROUTE, Method.Get);
            var response = await client.ExecuteAsync<List<Driver>>(request);
            return response.Data;
        }

        public async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient(URL);
                var request = new RestRequest(ROUTE, Method.Post);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(new Driver
                {
                    Rajtszam = textBox2.Text,
                    Nev = textBox3.Text,
                    csapat = comboBox1.Text,
                    Szuletesiev = int.Parse(textBox4.Text)
                });
                var result = await Search(textBox2.Text);
                if (result.Count != 0)
                {
                    throw new rajtszamException(int.Parse(textBox2.Text), true);
                }
                RestResponse response = client.Execute(request);
                string empty = "";
                textBox2.Text = empty;
                textBox3.Text = empty;
                comboBox1.Text = empty;
                textBox4.Text = empty;
                listing();
                fillCombo();
            }
            catch (evszamException ex)
            {
                MessageBox.Show($"Age can range from 18 to 100. The age you entered ({ex.Ev}) is incorrect!");
            }
            catch (rajtszamException ex)
            {
                if (ex.Foglalt == true)
                {
                    MessageBox.Show($"The racing number you entered ({ex.Szam}) is already taken!");
                }
                else
                {
                    MessageBox.Show($"Racing numbers range from 1 to 99. The number you entered ({ex.Szam}) is incorrect!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private async void fillCombo()
        {
            try
            {
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                comboBox3.Items.Clear();
                var client = new RestClient(URL);
                var request = new RestRequest(ROUTE, Method.Get);

                var response = await client.ExecuteAsync<List<Driver>>(request);

                if (response.IsSuccessful)
                {
                    foreach (var driver in response.Data)
                    {
                        comboBox2.Items.Add(driver.Rajtszam.ToString());
                    }
                    foreach (string team in f1Teams)
                    {
                        comboBox1.Items.Add(team);
                        comboBox3.Items.Add(team);
                    }
                }
                else
                {
                    throw new responseExcept();
                }
            }
            catch (responseExcept ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient(URL);
                string ROUTE = "restapi.php/{rajtszam}";
                var request = new RestRequest(ROUTE, Method.Delete);
                request.AddParameter("rajtszam", comboBox2.Text);
                RestResponse response = client.Execute(request);
                listing();
                fillCombo();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fillCombo();
        }

        private void ListCols()
        {
            listView1.Columns.Add("Racing number", 100);
            listView1.Columns.Add("Name", 200);
            listView1.View = View.Details;
        }

        private async void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                var rajtszam = item.SubItems[0].Text;
                var temp = await Search(rajtszam);
                DataTable dt = new DataTable();
                dt.Columns.Add("Racing number", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Team", typeof(string));
                dt.Columns.Add("Birth date", typeof(string));
                foreach (var data in temp)
                {
                    dt.Rows.Add(data.Rajtszam, data.Nev, data.csapat, data.Szuletesiev);

                    label6.Text = rajtszam;
                    textBox5.Text = data.Nev;
                    textBox6.Text = data.Szuletesiev.ToString();
                    comboBox3.SelectedItem = data.csapat;
                }
                dataGridView1.DataSource = dt;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient(URL);
                var request = new RestRequest(ROUTE, Method.Put);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(new Driver
                {
                    Rajtszam = label6.Text,
                    Nev = textBox5.Text,
                    csapat = comboBox3.Text,
                    Szuletesiev = int.Parse(textBox6.Text)
                });
                RestResponse response = client.Execute(request);
                string empty = "";
                textBox2.Text = empty;
                textBox3.Text = empty;
                comboBox1.Text = empty;
                textBox4.Text = empty;
                listing();
                fillCombo();
            }
            catch (rajtszamException ex)
            {
                if (ex.Foglalt == true)
                {
                    MessageBox.Show($"The racing number you entered ({ex.Szam}) is already taken!");
                }
                else
                {
                    MessageBox.Show($"Racing numbers range from 1 to 99. The number you entered ({ex.Szam}) is incorrect!");
                }
            }
            catch (evszamException ex)
            {
                MessageBox.Show($"Age can range from 18 to 100. The age you entered ( {DateTime.Now.Year - ex.Ev} ) is incorrect!");
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!");
            }
        }
    }


}
