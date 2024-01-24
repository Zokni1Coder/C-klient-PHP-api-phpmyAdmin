using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class userNameException : Exception
    {
        private string option1 = "taken";
        private string option2 = "is incorrect";
        public userNameException(string userName, int which)
        {
            this.UserName = userName;
            switch (which)
            {
                case 1:
                    this.text = option1;
                    break;
                case 2:
                    this.text = option2;
                    break;
                default:
                    break;
            }
        }

        public readonly string UserName;
        public readonly string text;
    }
}
