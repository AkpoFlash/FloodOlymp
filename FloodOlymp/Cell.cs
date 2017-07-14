using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodOlymp
{
    class Cell
    {

        private bool flooded = false;

        public bool Flooded
        {
            get { return flooded; }
            set { flooded = value; }
        }


        private bool horizontalWall = false;

        public bool HorizontalWall
        {
            get { return horizontalWall; }
            set { horizontalWall = value; }
        }


        private bool verticalWall = false;

        public bool VerticalWall
        {
            get { return verticalWall; }
            set { verticalWall = value; }
        }

    }
}
