using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodOlymp
{
    class Wall
    {
        private static int Count { get; set; }
        public Point Start { get; set; }
        public Point End { get; set; }
        public bool Horizontal { get; set; }
        public int Index { get; set; }


        public Wall(Point startPoint, Point endPoint)
        {
            this.Horizontal = (startPoint.X == endPoint.X) ? true : false;

            bool validSequence = (this.Horizontal) ? startPoint.Y < endPoint.Y : startPoint.X < endPoint.X;

            if (validSequence)
            {
                this.Start = startPoint;
                this.End = endPoint;
            }
            else
            {
                this.Start = endPoint;
                this.End = startPoint;
            }

            this.Index = ++Count;
        }



    }
}
