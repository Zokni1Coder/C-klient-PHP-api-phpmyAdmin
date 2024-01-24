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
    public partial class Form3 : Form
    {
        String URL = "http://localhost/SOP%20Beadando/restapiLog.php";
        String ROUTE = "restapiLog.php";

        //ADMIN VERIFICATION CODE!
        private string adminCode = "LH44";
        public Form3()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form = new Form2();
            this.Hide();
            form.Show();
        }

        private async Task<List<User>> Search(string userName)
        {
            var client = new RestClient(URL);
            string ROUTE = "restapiLog.php" + "?userName=" + userName;
            var request = new RestRequest(ROUTE, Method.Get);
            var response = await client.ExecuteAsync<List<User>>(request);
            return response.Data;
        }

        private void validator(string password, string confirm, string userName)
        {
            if (password != confirm)
            {
                throw new nemEgyezikAKodException();
            }
            if (userName.Length < 4)
            {
                throw new userNameException(userName,2);
            }
        }

        private bool adminconfirmation(string password)
        {
            if (password != "" && password != adminCode)
            {
                throw new adminException();
            }
            else if (password == adminCode)
            {
                return true;
            }
            else
                return false;
        }
        private string empty = "";
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient(URL);
                var request = new RestRequest(ROUTE, Method.Post);
                request.RequestFormat = DataFormat.Json;
                if (textBox2.Text != textBox3.Text)
	            {
                    throw new nemEgyezikAKodException();
	            }
                request.AddBody(new User
                {
                    UserName = textBox1.Text,
                    Password = textBox2.Text,
                    Admin = adminconfirmation(textBox4.Text)
                });
                var result = await Search(textBox1.Text);
                if (result != null && result.Any(user => user.UserName == textBox1.Text))
                {
                    throw new userNameException(textBox1.Text,1);
                }
                validator(textBox2.Text, textBox3.Text,textBox1.Text);
                RestResponse response = client.Execute(request);
                textBox1.Text = empty;
                textBox2.Text = empty;
                textBox3.Text = empty;
                textBox4.Text = empty;
            }
            catch (userNameException ex)
            {
                MessageBox.Show($"The (\" {ex.UserName} \") username {ex.text}!");
                textBox1.Text = empty;
            }
            catch (nemEgyezikAKodException)
            {
                MessageBox.Show("The two passwords do not match!");
            }
            catch (passwordException)
            {
                MessageBox.Show("The password does not meet the criteria!\n\n\n -It must be at least 8 characters long\n -It must contain both upper and lowercase letters!");
            }            
            catch (adminException)
            {
                MessageBox.Show("The password for admin rights is incorrect!");
                textBox4.Text = empty;
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!");
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
