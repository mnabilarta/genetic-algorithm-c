using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class Cluster_Pos
    {
        private int p_row;
        private int p_column;
        private int p_deck_No;
        private double center_x;
        private double center_y;

        private string zone;//At Piping_System, used when calculating the pipe length to shade the engine


        //private List<Grid> grids;//クラスターである格子の集合

        public int P_row
        {
            get { return p_row; }
            set { p_row = value; }
        }

        public int P_column
        {
            get { return p_column; }
            set { p_column = value; }
        }

        public int P_deck_No
        {
            get { return p_deck_No; }
            set { p_deck_No = value; }
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

        public string Zone
        {
            get { return zone; }
            set { zone = value; }
        }

        /*
        public List<Grid> Grids
        {
            get { return grids; }
            set { grids = value; }
        }*/

        //-----------String search------------------------------------------
        static bool HasString(string target, string word)
        {
            if (word == "")
                return false;
            if (target.IndexOf(word) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //---------------------------------------------------------------


        public Cluster_Pos(Deck[] deck_GA, int r_deck_No, int r_row, int r_column, Cluster_Inf c_inf)
        {
            p_row = r_row;
            p_column = r_column;
            p_deck_No = r_deck_No;
            Calc_Center(deck_GA, c_inf);
        }

        public void Calc_Center(Deck[] deck_GA, Cluster_Inf c_inf)
        {

            if (p_deck_No == 0)
            {
                for (int i = 0; i < c_inf.Height; i++)//We put the cluster information of the grid line-by-line
                {
                    for (int j = 0; j < c_inf.Width; j++)
                    {
                        if (HasString(c_inf.Name, "VALVE") == true)
                        {
                            deck_GA[0].grids[p_row + i, p_column + j].Valve = true;
                        }

                        else
                        {
                            deck_GA[0].grids[p_row + i, p_column + j].Module = true;
                        }
                    }
                }

                //To calculate the center coordinate
                double cluster_x = c_inf.Width;
                double cluster_y = c_inf.Height;
                center_x = deck_GA[0].grids[p_row, p_column].Vertexes[0].P.X + cluster_x / 2 * deck_GA[0].g_size - deck_GA[0].g_size;
                center_y = deck_GA[0].grids[p_row, p_column].Vertexes[0].P.Y + cluster_y / 2 * deck_GA[0].g_size - deck_GA[0].g_size;

                //-----------Zone of judgment ----------------------------------------

                if (center_x >= 64 & center_x <= 152 & center_y <= 136)
                {
                    zone = "A";
                }

                else if (center_x >= 152 & center_y >= 136 & center_y <= 192)
                {
                    zone = "B";
                }

                else if (center_x >= 64 & center_x <= 152 & center_y >= 192)
                {
                    zone = "C";
                }

                else if (center_x <= 64 & center_y >= 136 & center_y <= 192)
                {
                    zone = "D";
                }

                //---------------------------------------------------------------
            }

            if (p_deck_No == 1)
            {
                for (int i = 0; i < c_inf.Height; i++)//Go put one to the grid line-by-line
                {
                    for (int j = 0; j < c_inf.Width; j++)
                    {
                        if (HasString(c_inf.Name, "VALVE") == true)
                        {
                            deck_GA[1].grids[p_row + i, p_column + j].Valve = true;
                        }

                        else
                        {
                            deck_GA[1].grids[p_row + i, p_column + j].Module = true;
                        }
                    }
                }

                //To calculate the center coordinate
                double cluster_x = c_inf.Width;
                double cluster_y = c_inf.Height;
                center_x = deck_GA[1].grids[p_row, p_column].Vertexes[0].P.X + cluster_x / 2 * deck_GA[1].g_size - deck_GA[1].g_size;
                center_y = deck_GA[1].grids[p_row, p_column].Vertexes[0].P.Y + cluster_y / 2 * deck_GA[1].g_size - deck_GA[1].g_size;

                //-----------Zone of judgment----------------------------------------

                if (center_x >= 64 & center_x <= 152 & center_y <= 136)
                {
                    zone = "A";
                }

                else if (center_x >= 152 & center_y >= 136 & center_y <= 192)
                {
                    zone = "B";
                }

                else if (center_x >= 64 & center_x <= 152 & center_y >= 192)
                {
                    zone = "C";
                }

                else if (center_x <= 64 & center_y >= 136 & center_y <= 192)
                {
                    zone = "D";
                }

                //---------------------------------------------------------------

            }

            if (p_deck_No == 2)
            {
                for (int i = 0; i < c_inf.Height; i++)//Go put one to the grid line-by-line
                {
                    for (int j = 0; j < c_inf.Width; j++)
                    {
                        if (HasString(c_inf.Name, "VALVE") == true)
                        {
                            deck_GA[2].grids[p_row + i, p_column + j].Valve = true;
                        }

                        else
                        {
                            deck_GA[2].grids[p_row + i, p_column + j].Module = true;
                        }
                    }
                }

                //To calculate the center coordinate
                double cluster_x = c_inf.Width;
                double cluster_y = c_inf.Height;
                center_x = deck_GA[2].grids[p_row, p_column].Vertexes[0].P.X + cluster_x / 2 * deck_GA[2].g_size - deck_GA[2].g_size;
                center_y = deck_GA[2].grids[p_row, p_column].Vertexes[0].P.Y + cluster_y / 2 * deck_GA[2].g_size - deck_GA[2].g_size;

                //-----------Zone of judgment----------------------------------------

                if (center_x >= 40 & center_x <= 160 & center_y <= 128)
                {
                    zone = "A";
                }

                else if (center_x >= 160 & center_y >= 128 & center_y <= 216)
                {
                    zone = "B";
                }

                else if (center_x >= 40 & center_x <= 160 & center_y >= 216)
                {
                    zone = "C";
                }

                else if (center_x <= 40 & center_y >= 128 & center_y <= 216)
                {
                    zone = "D";
                }
                                
                //---------------------------------------------------------------

            }

            if (p_deck_No == 3)
            {
                for (int i = 0; i < c_inf.Height; i++)//Go put one to the grid line-by-line
                {
                    for (int j = 0; j < c_inf.Width; j++)
                    {
                        if (HasString(c_inf.Name, "VALVE") == true)
                        {
                            deck_GA[3].grids[p_row + i, p_column + j].Valve = true;
                        }

                        else
                        {
                            deck_GA[3].grids[p_row + i, p_column + j].Module = true;
                        }
                    }
                }

                //To calculate the center coordinate
                double cluster_x = c_inf.Width;
                double cluster_y = c_inf.Height;
                center_x = deck_GA[3].grids[p_row, p_column].Vertexes[0].P.X + cluster_x / 2 * deck_GA[3].g_size - deck_GA[3].g_size;
                center_y = deck_GA[3].grids[p_row, p_column].Vertexes[0].P.Y + cluster_y / 2 * deck_GA[3].g_size - deck_GA[3].g_size;

                //-----------Zone of judgment----------------------------------------

                if (center_x >= 40 & center_x <= 160 & center_y <= 128)
                {
                    zone = "A";
                }

                else if (center_x >= 160 & center_y >= 128 & center_y <= 216)
                {
                    zone = "B";
                }

                else if (center_x >= 40 & center_x <= 160 & center_y >= 216)
                {
                    zone = "C";
                }

                else if (center_x <= 40 & center_y >= 128 & center_y <= 216)
                {
                    zone = "D";
                }

                //---------------------------------------------------------------

            }
        }

    }
}
