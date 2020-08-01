using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class Grid
    {
        private Vertex[] vertexes = new Vertex[4];
        private bool outside;
        private bool hull;
        private bool constraint;
        private bool module;
        private bool valve;
        private int state;
        private int row;
        private int column;
        private int deck_No;

        public Vertex[] Vertexes
        {
            get { return vertexes; }
            set { vertexes = value; }
        }

        public bool Outside
        {
            get { return outside; }
            set { outside = value; }
        }

        public bool Hull
        {
            get { return hull; }
            set { hull = value; }
        }

        public bool Constraint
        {
            get { return constraint; }
            set { constraint = value; }
        }

        public bool Module
        {
            get { return module; }
            set { module = value; }
        }

        public bool Valve
        {
            get { return valve; }
            set { valve = value; }
        }

        public int State
        {
            get
            {
                if (outside == true)
                {
                    state = -3;
                    return state;
                }

                else if (hull == true)
                {
                    state = -2;
                    return state;
                }

                else if (constraint == true)
                {
                    state = -1;
                    return state;
                }

                else if (module == true)
                {
                    state = 1;
                    return state;
                }

                else if (valve == true)
                {
                    state = 2;
                    return state;
                }

                else
                {
                    return state;
                }
            }
            set { state = value; }
        }

        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        public int Col
        {
            get { return column; }
            set { column = value; }
        }

        public int Deck_No
        {
            get { return deck_No; }
            set { deck_No = value; }
        }

        public Grid()
        {
        }

        public Grid(Vertex ver1, Vertex ver2, Vertex ver3, Vertex ver4, int i, int j, int floor)
        {
            vertexes[0] = ver1; 
            vertexes[1] = ver2; 
            vertexes[2] = ver3; 
            vertexes[3] = ver4; 
            row = i;
            column = j;
            deck_No = floor;
        }

    }
}