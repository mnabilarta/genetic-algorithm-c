using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class ClusterList
    {
        private string name;
        private int width;
        private int height;

        private int p_row;
        private int p_column;
        private int position_deck_No;
        private double center_x;
        private double center_y;
        private string zone;


        public string c_name
        {
            get { return name; }
            set { name = value; }
        }

        public int c_width
        {
            get { return width; }
            set { width = value; }
        }

        public int c_height
        {
            get { return height; }
            set { height = value; }
        }


        public double center_X
        {
            get { return center_x; }
            set { center_x = value; }
        }

        public double center_Y
        {
            get { return center_y; }
            set { center_y = value; }
        }

        public int p_deck_No
        {
            get { return position_deck_No; }
            set { position_deck_No = value; }
        }

        public int c_p_row
        {
            get { return p_row; }
            set { p_row = value; }
        }

        public int c_p_column
        {
            get { return p_column; }
            set { p_column = value; }
        }

        public string Zone
        {
            get { return zone; }
            set { zone = value; }
        }

        //-----------String search------------------------------------------
        static bool HasString(string target, string word)
        {
            if (word == "")
                return false;
            if (target.IndexOf(word) >= 0) {
                return true;
            } else {
                return false;
            }
        }
        //---------------------------------------------------------------


        public ClusterList()
        {
        }

        public ClusterList(Cluster_Inf form_cluster, Cluster_Pos c_position)
        {
            name = form_cluster.Name;
            width = form_cluster.Width;
            height = form_cluster.Height;

            position_deck_No = c_position.P_deck_No;
            p_row = c_position.P_row;
            p_column = c_position.P_column;
            center_x = c_position.center_X;
            center_y = c_position.center_Y;
            zone = c_position.Zone;
        }

        public void Calc_Center(Deck[] deck_GA)//Method for the last result output
        {

            if (p_deck_No == 0) {
                for (int i = 0; i < c_height; i++)//Go put one to the grid line-by-line
                {
                    for (int j = 0; j < c_width; j++) {
                        if (HasString(name, "VALVE") == true) {
                            deck_GA[0].grids[p_row + i, p_column + j].Valve = true;
                        } else {
                            deck_GA[0].grids[p_row + i, p_column + j].Module = true;
                        }
                    }
                }
            }

            if (p_deck_No == 1) {
                for (int i = 0; i < c_height; i++)//Go put one to the grid line-by-line
                {
                    for (int j = 0; j < c_width; j++) {
                        if (HasString(name, "VALVE") == true) {
                            deck_GA[1].grids[p_row + i, p_column + j].Valve = true;
                        } else {
                            deck_GA[1].grids[p_row + i, p_column + j].Module = true;
                        }
                    }
                }
            }

            if (p_deck_No == 2) {
                for (int i = 0; i < c_height; i++)//Go put one to the grid line-by-line
                {
                    for (int j = 0; j < c_width; j++) {
                        if (HasString(name, "VALVE") == true) {
                            deck_GA[2].grids[p_row + i, p_column + j].Valve = true;
                        } else {
                            deck_GA[2].grids[p_row + i, p_column + j].Module = true;
                        }
                    }
                }
            }

            if (p_deck_No == 3) {
                for (int i = 0; i < c_height; i++)//Go put one to the grid line-by-line
                {
                    for (int j = 0; j < c_width; j++) {
                        if (HasString(name, "VALVE") == true) {
                            deck_GA[3].grids[p_row + i, p_column + j].Valve = true;
                        } else {
                            deck_GA[3].grids[p_row + i, p_column + j].Module = true;
                        }
                    }
                }
            }
        }

    }
}
