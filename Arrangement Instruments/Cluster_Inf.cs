using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Arrangement_Instruments
{
    class Cluster_Inf
    {
        private string name;
        private int width;
        private int height;
        private int const_F;
        private int const_P;
        private int const_2ND;
        private int const_3RD;
        private string Side;

        private string pos_DECK;
        private int pos_ROW;
        private int pos_COL;

        private string cluster_data;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public string c_data
        {
            get { return cluster_data; }
            set { cluster_data = value; }
        }

        public int Const_F
        {
            get { return const_F; }
            set { const_F = value; }
        }

        public int Const_P
        {
            get { return const_P; }
            set { const_P = value; }
        }

        public int Const_2ND
        {
            get { return const_2ND; }
            set { const_2ND = value; }
        }

        public int Const_3RD
        {
            get { return const_3RD; }
            set { const_3RD = value; }
        }

        public string SIDE
        {
            get { return Side; }
            set { Side = value; }
        }

        public string Pos_DECK
        {
            get { return pos_DECK; }
            set { pos_DECK = value; }
        }

        public int Pos_ROW
        {
            get { return pos_ROW; }
            set { pos_ROW = value; }
        }

        public int Pos_COL
        {
            get { return pos_COL; }
            set { pos_COL = value; }
        }


        public Cluster_Inf(string cluster_name, int size_x, int size_y, int const_f, int const_p, int const_2nd, int const_3rd, string pos_deck, int pos_row, int pos_col, string side)
        {
            name = cluster_name;
            width = size_x;
            height = size_y;
            cluster_data = "ID:" + name + "   width:" + width + "   height:" + height;
            const_F = const_f;
            const_P = const_p;
            const_2ND = const_2nd;
            const_3RD = const_3rd;
            pos_DECK = pos_deck;
            pos_ROW = pos_row;
            pos_COL = pos_col;
            Side = side;

        }
    }
}
