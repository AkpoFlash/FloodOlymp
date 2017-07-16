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


        public Wall(Point StartPoint, Point EndPoint)
        {
            this.Horizontal = (StartPoint.X == EndPoint.X) ? true : false;

            bool validSequence = (this.Horizontal) ? StartPoint.Y < EndPoint.Y : StartPoint.X < EndPoint.X;

            if (validSequence)
            {
                this.Start = StartPoint;
                this.End = EndPoint;
            }
            else
            {
                this.Start = EndPoint;
                this.End = StartPoint;
            }

            this.Index = ++Count;
        }



    }
}
