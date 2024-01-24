using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class evszamException : Exception
    {
        public readonly int Ev;

        public evszamException(int ev)
        {
            this.Ev = ev;
        }
    }
}
