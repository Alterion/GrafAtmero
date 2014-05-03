using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafAtmero
{
    class Pont
    {
        public int x {get; set;}
        public int y { get; set; }
        public int[] szomszedok;

        public Pont(int _x, int _y, int[] t)
        {
            x = _x;
            y = _y;
            szomszedok = t;
        }
        
    }
}
