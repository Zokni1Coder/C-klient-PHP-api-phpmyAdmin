using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class passwordException : Exception
    {
        public passwordException(string password)
        {
            this.Password = password;
        }
        public readonly string Password;
    }
}
