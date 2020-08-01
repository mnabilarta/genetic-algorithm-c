using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;//Excel用

namespace Arrangement_Instruments
{
    class Piping_System
    {
        private int[,] interaction_all;
        private double[,] p_length;

        double z_length;//Perpendicular direction of the pipe length to the plane grating
        double a_length;//Pipe length to avoid an engine



        public double[,] P_length
        {
            get { return p_length; }
            set { p_length = value; }
        }

        public Piping_System()
        {
        }

        public void Interaction(int count_row,Excel.Worksheet ws2)
        {
            interaction_all = new int[count_row, count_row];
            p_length = new double[count_row, count_row];

            for (int i = 1; i <= count_row; i++)
            {
                for (int j = 1; j <= count_row; j++)
                {
                    Excel.Range rng = ws2.Cells[i + 1, j + 1];//Because only in the class Range not read the value of the cell, which in this way                        
                    interaction_all[i - 1, j - 1] = Convert.ToInt32(rng.Value2);
                }
            }
        }

        public double Calc_Length(int count_row, List<ClusterList> clusters, double g_size)
        {
            double total_length = 0;

            for (int i = 0; i < count_row; i++)
            {
                for (int j = 0; j < count_row; j++)
                {
                    if (i < j)
                    {
                        if (interaction_all[i, j] == 1)
                        {
                            if (clusters[i].p_deck_No != clusters[j].p_deck_No)//Calculation of the length of the different deck
                            {

                                //--------------------------The length of the calculation 6 pattern between the different deck--------------------------------------------------------------                                

                                if (clusters[i].p_deck_No == 0 & clusters[j].p_deck_No == 1 || clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 0)
                                {
                                    z_length = 22;
                                }

                                else if (clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 2 || clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 1)
                                {
                                    z_length = 29;
                                }

                                else if (clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 3 || clusters[i].p_deck_No == 3 & clusters[j].p_deck_No == 2)
                                {
                                    z_length = 24;
                                }

                                else if (clusters[i].p_deck_No == 0 & clusters[j].p_deck_No == 2 || clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 0)
                                {
                                    z_length = 51;
                                }

                                else if (clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 3 || clusters[i].p_deck_No == 3 & clusters[j].p_deck_No == 1)
                                {
                                    z_length = 83;
                                }

                                else
                                {
                                    z_length = 105;
                                }

                                //-----------------------------------------------------------------------------------------------------------------------------


                                //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A")
                                {
                                    double[] a_lengths = new double[4];
                                    a_lengths[0] = clusters[i].center_X - 36;
                                    a_lengths[1] = clusters[j].center_X - 36;
                                    a_lengths[2] = 164 - clusters[i].center_X;
                                    a_lengths[3] = 164 - clusters[j].center_X;

                                    double min = a_lengths.Min();
                                    a_length = min * 2;                                    
                                }

                                else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B")
                                {
                                    double[] a_lengths = new double[4];
                                    a_lengths[0] = clusters[i].center_Y - 124;
                                    a_lengths[1] = clusters[j].center_Y - 124;
                                    a_lengths[2] = 220 - clusters[i].center_Y;
                                    a_lengths[3] = 220 - clusters[j].center_Y;

                                    double min = a_lengths.Min();
                                    a_length = min * 2;
                                }

                                else
                                {
                                    a_length = 0;
                                }

                                //-----------------------------------------------------------------------------------------------------------------------------

                                p_length[i, j] = Math.Abs(clusters[i].center_X - clusters[j].center_X) + Math.Abs(clusters[i].center_Y - clusters[j].center_Y) + z_length; //+ a_length;
                                
                            }

                            else//Calculation of the length between the same deck
                            {
                                if (clusters[i].p_deck_No == 0)//FLOOR case
                                {
                                    //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                    if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_X - 60;
                                        a_lengths[1] = clusters[j].center_X - 60;
                                        a_lengths[2] = 156 - clusters[i].center_X;
                                        a_lengths[3] = 156 - clusters[j].center_X;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_Y - 132;
                                        a_lengths[1] = clusters[j].center_Y - 132;
                                        a_lengths[2] = 196 - clusters[i].center_Y;
                                        a_lengths[3] = 196 - clusters[j].center_Y;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else
                                    {
                                        a_length = 0;
                                    }

                                    //---------------------------------------------------------------------------------------------------------------------------
                                }

                                else if (clusters[i].p_deck_No != 0)
                                {

                                    //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                    if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_X - 36;
                                        a_lengths[1] = clusters[j].center_X - 36;
                                        a_lengths[2] = 164 - clusters[i].center_X;
                                        a_lengths[3] = 164 - clusters[j].center_X;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_Y - 124;
                                        a_lengths[1] = clusters[j].center_Y - 124;
                                        a_lengths[2] = 220 - clusters[i].center_Y;
                                        a_lengths[3] = 220 - clusters[j].center_Y;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else
                                    {
                                        a_length = 0;
                                    }

                                    //-----------------------------------------------------------------------------------------------------------------------------
                                }

                                p_length[i, j] = Math.Abs(clusters[i].center_X - clusters[j].center_X) + Math.Abs(clusters[i].center_Y - clusters[j].center_Y); //+ a_length;
                                
                            }
                        }
                        
                        
                        else if (interaction_all[i, j] == 5)
                        {
                            if (clusters[i].p_deck_No != clusters[j].p_deck_No)//Calculation of the length of the different deck
                            {

                                //--------------------------The length of the calculation 6 pattern between the different deck--------------------------------------------------------------                                

                                if (clusters[i].p_deck_No == 0 & clusters[j].p_deck_No == 1 || clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 0)
                                {
                                    z_length = 22;
                                }

                                else if (clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 2 || clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 1)
                                {
                                    z_length = 29;
                                }

                                else if (clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 3 || clusters[i].p_deck_No == 3 & clusters[j].p_deck_No == 2)
                                {
                                    z_length = 24;
                                }

                                else if (clusters[i].p_deck_No == 0 & clusters[j].p_deck_No == 2 || clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 0)
                                {
                                    z_length = 51;
                                }

                                else if (clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 3 || clusters[i].p_deck_No == 3 & clusters[j].p_deck_No == 1)
                                {
                                    z_length = 83;
                                }

                                else
                                {
                                    z_length = 105;
                                }

                                //-----------------------------------------------------------------------------------------------------------------------------


                                //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A")
                                {
                                    double[] a_lengths = new double[4];
                                    a_lengths[0] = clusters[i].center_X - 36;
                                    a_lengths[1] = clusters[j].center_X - 36;
                                    a_lengths[2] = 164 - clusters[i].center_X;
                                    a_lengths[3] = 164 - clusters[j].center_X;

                                    double min = a_lengths.Min();
                                    a_length = min * 2;                                    
                                }

                                else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B")
                                {
                                    double[] a_lengths = new double[4];
                                    a_lengths[0] = clusters[i].center_Y - 124;
                                    a_lengths[1] = clusters[j].center_Y - 124;
                                    a_lengths[2] = 220 - clusters[i].center_Y;
                                    a_lengths[3] = 220 - clusters[j].center_Y;

                                    double min = a_lengths.Min();
                                    a_length = min * 2;
                                }

                                else
                                {
                                    a_length = 0;
                                }

                                //-----------------------------------------------------------------------------------------------------------------------------

                                p_length[i, j] = (Math.Abs(clusters[i].center_X - clusters[j].center_X) + Math.Abs(clusters[i].center_Y - clusters[j].center_Y) + z_length) * 5; //+ a_length) * 5;
                                
                            }

                            else//Calculation of the length between the same deck
                            {
                                if (clusters[i].p_deck_No == 0)//FLOOR case
                                {
                                    //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                    if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_X - 60;
                                        a_lengths[1] = clusters[j].center_X - 60;
                                        a_lengths[2] = 156 - clusters[i].center_X;
                                        a_lengths[3] = 156 - clusters[j].center_X;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_Y - 132;
                                        a_lengths[1] = clusters[j].center_Y - 132;
                                        a_lengths[2] = 196 - clusters[i].center_Y;
                                        a_lengths[3] = 196 - clusters[j].center_Y;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else
                                    {
                                        a_length = 0;
                                    }

                                    //---------------------------------------------------------------------------------------------------------------------------
                                }

                                else if (clusters[i].p_deck_No != 0)
                                {

                                    //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                    if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_X - 36;
                                        a_lengths[1] = clusters[j].center_X - 36;
                                        a_lengths[2] = 164 - clusters[i].center_X;
                                        a_lengths[3] = 164 - clusters[j].center_X;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_Y - 124;
                                        a_lengths[1] = clusters[j].center_Y - 124;
                                        a_lengths[2] = 220 - clusters[i].center_Y;
                                        a_lengths[3] = 220 - clusters[j].center_Y;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else
                                    {
                                        a_length = 0;
                                    }

                                    //-----------------------------------------------------------------------------------------------------------------------------
                                }

                                p_length[i, j] = (Math.Abs(clusters[i].center_X - clusters[j].center_X) + Math.Abs(clusters[i].center_Y - clusters[j].center_Y)) * 5;  //+ a_length) * 5;
                                
                            }
                        } 
                        
                        else if (interaction_all[i, j] == 4) 
                        {
                            if (clusters[i].p_deck_No != clusters[j].p_deck_No)//Calculation of the length of the different deck
                            {

                                //--------------------------The length of the calculation 6 pattern between the different deck--------------------------------------------------------------                                

                                if (clusters[i].p_deck_No == 0 & clusters[j].p_deck_No == 1 || clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 0) {
                                    z_length = 22;
                                } else if (clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 2 || clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 1) {
                                    z_length = 29;
                                } else if (clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 3 || clusters[i].p_deck_No == 3 & clusters[j].p_deck_No == 2) {
                                    z_length = 24;
                                } else if (clusters[i].p_deck_No == 0 & clusters[j].p_deck_No == 2 || clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 0) {
                                    z_length = 51;
                                } else if (clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 3 || clusters[i].p_deck_No == 3 & clusters[j].p_deck_No == 1) {
                                    z_length = 83;
                                } else {
                                    z_length = 105;
                                }

                                //-----------------------------------------------------------------------------------------------------------------------------


                                //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A") {
                                    double[] a_lengths = new double[4];
                                    a_lengths[0] = clusters[i].center_X - 36;
                                    a_lengths[1] = clusters[j].center_X - 36;
                                    a_lengths[2] = 164 - clusters[i].center_X;
                                    a_lengths[3] = 164 - clusters[j].center_X;

                                    double min = a_lengths.Min();
                                    a_length = min * 2;
                                } else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B") {
                                    double[] a_lengths = new double[4];
                                    a_lengths[0] = clusters[i].center_Y - 124;
                                    a_lengths[1] = clusters[j].center_Y - 124;
                                    a_lengths[2] = 220 - clusters[i].center_Y;
                                    a_lengths[3] = 220 - clusters[j].center_Y;

                                    double min = a_lengths.Min();
                                    a_length = min * 2;
                                } else {
                                    a_length = 0;
                                }

                                //-----------------------------------------------------------------------------------------------------------------------------

                                p_length[i, j] = (Math.Abs(clusters[i].center_X - clusters[j].center_X) + Math.Abs(clusters[i].center_Y - clusters[j].center_Y) + z_length) * 4; // + a_length) * 4;
                                


                            }
                            else//Calculation of the length between the same deck
                            {
                                if (clusters[i].p_deck_No == 0)//FLOOR case
                                {
                                    //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                    if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A") {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_X - 60;
                                        a_lengths[1] = clusters[j].center_X - 60;
                                        a_lengths[2] = 156 - clusters[i].center_X;
                                        a_lengths[3] = 156 - clusters[j].center_X;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    } else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B") {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_Y - 132;
                                        a_lengths[1] = clusters[j].center_Y - 132;
                                        a_lengths[2] = 196 - clusters[i].center_Y;
                                        a_lengths[3] = 196 - clusters[j].center_Y;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    } else {
                                        a_length = 0;
                                    }

                                    //---------------------------------------------------------------------------------------------------------------------------
                                } else if (clusters[i].p_deck_No != 0) {

                                    //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                    if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A") {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_X - 36;
                                        a_lengths[1] = clusters[j].center_X - 36;
                                        a_lengths[2] = 164 - clusters[i].center_X;
                                        a_lengths[3] = 164 - clusters[j].center_X;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    } else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B") {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_Y - 124;
                                        a_lengths[1] = clusters[j].center_Y - 124;
                                        a_lengths[2] = 220 - clusters[i].center_Y;
                                        a_lengths[3] = 220 - clusters[j].center_Y;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    } else {
                                        a_length = 0;
                                    }

                                    //-----------------------------------------------------------------------------------------------------------------------------
                                }

                                p_length[i, j] = (Math.Abs(clusters[i].center_X - clusters[j].center_X) + Math.Abs(clusters[i].center_Y - clusters[j].center_Y)) * 4;  //+ a_length) * 4;

                            }
                        }
                        else if (interaction_all[i, j] == 2)
                        {
                            if (clusters[i].p_deck_No != clusters[j].p_deck_No)//Calculation of the length of the different deck
                            {

                                //--------------------------The length of the calculation 6 pattern between the different deck--------------------------------------------------------------                                

                                if (clusters[i].p_deck_No == 0 & clusters[j].p_deck_No == 1 || clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 0)
                                {
                                    z_length = 22;
                                }

                                else if (clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 2 || clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 1)
                                {
                                    z_length = 29;
                                }

                                else if (clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 3 || clusters[i].p_deck_No == 3 & clusters[j].p_deck_No == 2)
                                {
                                    z_length = 24;
                                }

                                else if (clusters[i].p_deck_No == 0 & clusters[j].p_deck_No == 2 || clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 0)
                                {
                                    z_length = 51;
                                }

                                else if (clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 3 || clusters[i].p_deck_No == 3 & clusters[j].p_deck_No == 1)
                                {
                                    z_length = 83;
                                }

                                else
                                {
                                    z_length = 105;
                                }

                                //-----------------------------------------------------------------------------------------------------------------------------


                                //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A")
                                {
                                    double[] a_lengths = new double[4];
                                    a_lengths[0] = clusters[i].center_X - 36;
                                    a_lengths[1] = clusters[j].center_X - 36;
                                    a_lengths[2] = 164 - clusters[i].center_X;
                                    a_lengths[3] = 164 - clusters[j].center_X;

                                    double min = a_lengths.Min();
                                    a_length = min * 2;
                                }

                                else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B")
                                {
                                    double[] a_lengths = new double[4];
                                    a_lengths[0] = clusters[i].center_Y - 124;
                                    a_lengths[1] = clusters[j].center_Y - 124;
                                    a_lengths[2] = 220 - clusters[i].center_Y;
                                    a_lengths[3] = 220 - clusters[j].center_Y;

                                    double min = a_lengths.Min();
                                    a_length = min * 2;
                                }

                                else
                                {
                                    a_length = 0;
                                }

                                //-----------------------------------------------------------------------------------------------------------------------------

                                p_length[i, j] = (Math.Abs(clusters[i].center_X - clusters[j].center_X) + Math.Abs(clusters[i].center_Y - clusters[j].center_Y) + z_length) * 2; // + a_length) * 2;

                            }

                            else//Calculation of the length between the same deck
                            {
                                if (clusters[i].p_deck_No == 0)//FLOOR case
                                {
                                    //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                    if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_X - 60;
                                        a_lengths[1] = clusters[j].center_X - 60;
                                        a_lengths[2] = 156 - clusters[i].center_X;
                                        a_lengths[3] = 156 - clusters[j].center_X;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_Y - 132;
                                        a_lengths[1] = clusters[j].center_Y - 132;
                                        a_lengths[2] = 196 - clusters[i].center_Y;
                                        a_lengths[3] = 196 - clusters[j].center_Y;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else
                                    {
                                        a_length = 0;
                                    }

                                    //---------------------------------------------------------------------------------------------------------------------------
                                }

                                else if (clusters[i].p_deck_No != 0)
                                {

                                    //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                    if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_X - 36;
                                        a_lengths[1] = clusters[j].center_X - 36;
                                        a_lengths[2] = 164 - clusters[i].center_X;
                                        a_lengths[3] = 164 - clusters[j].center_X;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B")
                                    {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_Y - 124;
                                        a_lengths[1] = clusters[j].center_Y - 124;
                                        a_lengths[2] = 220 - clusters[i].center_Y;
                                        a_lengths[3] = 220 - clusters[j].center_Y;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    }

                                    else
                                    {
                                        a_length = 0;
                                    }

                                    //-----------------------------------------------------------------------------------------------------------------------------
                                }

                                p_length[i, j] = (Math.Abs(clusters[i].center_X - clusters[j].center_X) + Math.Abs(clusters[i].center_Y - clusters[j].center_Y)) * 2; //  + a_length) * 2;

                            }
                        } 



                        
                        else if (interaction_all[i, j] == 3) 
                        {
                            if (clusters[i].p_deck_No != clusters[j].p_deck_No)//Calculation of the length of the different deck
                            {

                                //--------------------------The length of the calculation 6 pattern between the different deck--------------------------------------------------------------                                

                                if (clusters[i].p_deck_No == 0 & clusters[j].p_deck_No == 1 || clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 0) {
                                    z_length = 22;
                                } else if (clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 2 || clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 1) {
                                    z_length = 29;
                                } else if (clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 3 || clusters[i].p_deck_No == 3 & clusters[j].p_deck_No == 2) {
                                    z_length = 24;
                                } else if (clusters[i].p_deck_No == 0 & clusters[j].p_deck_No == 2 || clusters[i].p_deck_No == 2 & clusters[j].p_deck_No == 0) {
                                    z_length = 51;
                                } else if (clusters[i].p_deck_No == 1 & clusters[j].p_deck_No == 3 || clusters[i].p_deck_No == 3 & clusters[j].p_deck_No == 1) {
                                    z_length = 83;
                                } else {
                                    z_length = 105;
                                }

                                //-----------------------------------------------------------------------------------------------------------------------------


                                //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A") {
                                    double[] a_lengths = new double[4];
                                    a_lengths[0] = clusters[i].center_X - 36;
                                    a_lengths[1] = clusters[j].center_X - 36;
                                    a_lengths[2] = 164 - clusters[i].center_X;
                                    a_lengths[3] = 164 - clusters[j].center_X;

                                    double min = a_lengths.Min();
                                    a_length = min * 2;
                                } else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B") {
                                    double[] a_lengths = new double[4];
                                    a_lengths[0] = clusters[i].center_Y - 124;
                                    a_lengths[1] = clusters[j].center_Y - 124;
                                    a_lengths[2] = 220 - clusters[i].center_Y;
                                    a_lengths[3] = 220 - clusters[j].center_Y;

                                    double min = a_lengths.Min();
                                    a_length = min * 2;
                                } else {
                                    a_length = 0;
                                }

                                //-----------------------------------------------------------------------------------------------------------------------------

                                p_length[i, j] = (Math.Abs(clusters[i].center_X - clusters[j].center_X) + Math.Abs(clusters[i].center_Y - clusters[j].center_Y) + z_length) * 3; // + a_length) * 3;
                                


                            }
                            else//Calculation of the length between the same deck
                            {
                                if (clusters[i].p_deck_No == 0)//FLOOR case
                                {
                                    //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                    if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A") {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_X - 60;
                                        a_lengths[1] = clusters[j].center_X - 60;
                                        a_lengths[2] = 156 - clusters[i].center_X;
                                        a_lengths[3] = 156 - clusters[j].center_X;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    } else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B") {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_Y - 132;
                                        a_lengths[1] = clusters[j].center_Y - 132;
                                        a_lengths[2] = 196 - clusters[i].center_Y;
                                        a_lengths[3] = 196 - clusters[j].center_Y;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    } else {
                                        a_length = 0;
                                    }

                                    //---------------------------------------------------------------------------------------------------------------------------
                                } else if (clusters[i].p_deck_No != 0) {

                                    //--------------------------Pipe length calculations to avoid an engine---------------------------------------------------------------------

                                    if (clusters[i].Zone == "A" & clusters[j].Zone == "C" || clusters[i].Zone == "C" & clusters[j].Zone == "A") {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_X - 36;
                                        a_lengths[1] = clusters[j].center_X - 36;
                                        a_lengths[2] = 164 - clusters[i].center_X;
                                        a_lengths[3] = 164 - clusters[j].center_X;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    } else if (clusters[i].Zone == "B" & clusters[j].Zone == "D" || clusters[i].Zone == "D" & clusters[j].Zone == "B") {
                                        double[] a_lengths = new double[4];
                                        a_lengths[0] = clusters[i].center_Y - 124;
                                        a_lengths[1] = clusters[j].center_Y - 124;
                                        a_lengths[2] = 220 - clusters[i].center_Y;
                                        a_lengths[3] = 220 - clusters[j].center_Y;

                                        double min = a_lengths.Min();
                                        a_length = min * 2;
                                    } else {
                                        a_length = 0;
                                    }

                                    //-----------------------------------------------------------------------------------------------------------------------------
                                }

                                p_length[i, j] = (Math.Abs(clusters[i].center_X - clusters[j].center_X) + Math.Abs(clusters[i].center_Y - clusters[j].center_Y)) * 3; //  + a_length) * 3;

                            }
                        }                                              
                        
                        else
                        {
                            p_length[i, j] = 0;
                        }
                        
                        total_length = total_length + p_length[i, j];

                    }


                    else
                    {
                        p_length[i, j] = 0;
                    }
                }
            }
            total_length = total_length / 10;// display
            //total_length = total_length / g_size;//g_sizeで割り，格子数で計算
            return total_length;
        }
    }
}
