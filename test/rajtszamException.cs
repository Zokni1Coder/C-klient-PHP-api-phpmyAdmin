using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class rajtszamException : Exception
    {
        public rajtszamException(int szam, bool foglalt)
        {
            this.Szam = szam;
            this.Foglalt = foglalt;
        }

        public readonly int Szam;
        public readonly bool Foglalt;
    }
}
