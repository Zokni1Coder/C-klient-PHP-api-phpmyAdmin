using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace test
{
    class User
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                validator(value);
                password = value;
            }
        }
        private void validator(string password)
        {
            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).{8,}$"))
            {
                throw new passwordException(password);
            }
        }

        private bool admin;

        public bool Admin
        {
            get { return admin; }
            set { admin = value; }
        }
    }
}
