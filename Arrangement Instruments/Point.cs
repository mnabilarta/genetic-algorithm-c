using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class Point
    {
        private double x;
        private double y;

        public Point()
        {
        }

        public Point(double aa, double bb)
        {
            x = aa;
            y = bb;
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}
