using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flood
{
    class Wall
    {
        public static int count;
        public Point start;
        public Point end;
        public bool horizontal;
        public int index;



        public Wall(Point StartPoint, Point EndPoint)
        {
            if (StartPoint.X == EndPoint.X)
            {
                this.horizontal = true;
                if (StartPoint.Y < EndPoint.Y)
                {
                    this.start = StartPoint;
                    this.end = EndPoint;
                }
                else
                {
                    this.start = EndPoint;
                    this.end = StartPoint;
                }
            }
            else
            {
                this.horizontal = false;
                if (StartPoint.X < EndPoint.X)
                {
                    this.start = StartPoint;
                    this.end = EndPoint;
                }
                else
                {
                    this.start = EndPoint;
                    this.end = StartPoint;
                };
            }

            this.index = ++count;
        }


    }
}
