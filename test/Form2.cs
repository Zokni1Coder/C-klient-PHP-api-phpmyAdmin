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
using System.Text.RegularExpressions;

namespace test
{
    public partial class Form2 : Form
    {
        String URL = "http://localhost/SOP%20Beadando/restapiLog.php";
        String ROUTE = "restapiLog.php";
        public Form2()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form3 form = new Form3();
            this.Hide();
            form.Show();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var temp = await Search(textBox1.Text);
                if (temp == null || temp.Count == 0)
                {
                    MessageBox.Show("Invalid username or password!");
                }
                foreach (var user in temp)
                {
                    if (user.UserName == textBox1.Text && user.Password == textBox2.Text)
                    {
                        if (user.Admin != true)
                        {
                            this.Hide();
                            Form1 form = new Form1();
                            form.HozzaAd.Enabled = false;
                            form.Torles.Enabled = false;
                            form.Modosit.Enabled = false;
                            form.Show();

                        }
                        else
                        {
                            this.Hide();
                            Form1 form = new Form1();
                            form.Show();
                        }
                    }
                }
            }
            catch (passwordException)
            {
                MessageBox.Show("The password does not meet the criteria!\n\n\n -It must be at least 8 characters long\n -It must contain both upper and lowercase letters!");
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!");
            }
        }

        private async Task<List<User>> Search(string userName)
        {
            var client = new RestClient(URL);
            string ROUTE = "restapiLog.php" + "?userName=" + userName;
            var request = new RestRequest(ROUTE, Method.Get);
            var response = await client.ExecuteAsync<List<User>>(request);

            if (response.Data == null || response.Data.Count == 0)
            {
                return new List<User>();
            }

            return response.Data;
        }

        private void button1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
