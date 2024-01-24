using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class responseExcept : Exception
    {
        private string message = "No response from the database!";

        public string Message
        {
            get { return message; }
        }

    }
}
