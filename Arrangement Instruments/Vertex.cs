using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class Vertex
    {
        private Point point;

        public Vertex()
        {
        }

        public Vertex(Point aa)
        {
            point = aa;
        }

        public Point P
        {
            get { return point; }
            set { point = value; }
        }
    }
}
