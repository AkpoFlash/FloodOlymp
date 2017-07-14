using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodOlymp
{
    class Validate
    {

        static public void checkBorder(int number, int min, int max, string message)
        {
            if (number < min || number > max)
                throw new Exception(message);
        }


    }
}
