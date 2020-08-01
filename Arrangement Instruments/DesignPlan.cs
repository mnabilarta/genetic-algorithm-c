using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class DesignPlan
    {
        private Chromosome chromosome;
        private double fittness;
        private double PipeLength;
        private List<ClusterList> clusterList = new List<ClusterList>();
        private List<string> penalty = new List<string>();


        int r_deck_No;
        int r_row;
        int r_column;
        int cluster_No;


        public Chromosome Chromo
        {
            get { return chromosome; }
            set { chromosome = value; }
        }

        public List<ClusterList> C_List
        {
            get { return clusterList; }
            set { clusterList = value; }
        }

        public List<string> Penalty
        {
            get { return penalty; }
            set { penalty = value; }
        }


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




        public DesignPlan(int count_row)
        {
            chromosome = new Chromosome(count_row);
        }

        public void makeDesignplan1(int count_row, Random r1, Cluster cluster_GA, Deck[] deck_GA)
        {
            List<Cluster_Pos> c_positions = new List<Cluster_Pos>();

            //----------------Production of the gene sequence---------------------------------------------------------------
            chromosome.makeCluster_Gene(r1);//Note that the GA of the past seniors is the order of the different array !!
            //---------------------------------------------------------------------------------------------

            for (cluster_No = 0; cluster_No < count_row; cluster_No++)
            {
                if (cluster_GA.Inf[cluster_No].Pos_DECK == null)//If you do not have a place specified cluster
                {
                    //------Generation of arrangement possible candidate---------------------------------------------------------------------

                    Deck deck = new Deck();//availableGrids用

                    if (cluster_GA.Inf[cluster_No].Const_F == 1)//Check the cluster of constraints, perform a search if possible placement
                    {
                        deck.Check_availableGrid(deck_GA[0], cluster_GA, cluster_No);
                    }
                    if (cluster_GA.Inf[cluster_No].Const_P == 1)
                    {
                        deck.Check_availableGrid(deck_GA[1], cluster_GA, cluster_No);
                    }
                    if (cluster_GA.Inf[cluster_No].Const_3RD == 1)
                    {
                        deck.Check_availableGrid(deck_GA[2], cluster_GA, cluster_No);
                    }
                    if (cluster_GA.Inf[cluster_No].Const_2ND == 1)
                    {
                        deck.Check_availableGrid(deck_GA[3], cluster_GA, cluster_No);
                    }

                    //---------------------------------------------------------------------------------------------

                    //----------------Gene sequence (binary) → location information (decimal)----------------------------------------

                    double rnd = 0;

                    for (int i = 0; i < 13; i++)
                    {
                        if (Chromo.genes[i + (13 * cluster_No)] == 1)
                            rnd = rnd + (double)Math.Pow(2, i);
                    }

                    //---------------------------------------------------------------------------------------------

                    //------If no allocable area, placed at overlapping, to have a penalty------------------------
                    //------Or if the "Valve", placed at overlapping, numeric penalty is not to have-----------------------

                    if (deck.available_Grid.Count == 0 || HasString(cluster_GA.Inf[cluster_No].Name, "VALVE") == true)
                    {
                        if (cluster_GA.Inf[cluster_No].Const_F == 1)
                        {
                            deck.Check_penaltyGrid(deck_GA[0], cluster_GA, cluster_No);
                        }
                        if (cluster_GA.Inf[cluster_No].Const_P == 1)
                        {
                            deck.Check_penaltyGrid(deck_GA[1], cluster_GA, cluster_No);
                        }
                        if (cluster_GA.Inf[cluster_No].Const_3RD == 1)
                        {
                            deck.Check_penaltyGrid(deck_GA[2], cluster_GA, cluster_No);
                        }
                        if (cluster_GA.Inf[cluster_No].Const_2ND == 1)
                        {
                            deck.Check_penaltyGrid(deck_GA[3], cluster_GA, cluster_No);
                        }

                        //----------------The determination of the provisional arrangement-----------------------------------------------------------------

                        double rnd2 = (rnd / (8191 + 1)) * (deck.penalty_Grid.Count);//"8191 + 1", in order to produce a random number from 0 to 0.999
                        int rnd3 = (int)Math.Floor(rnd2);//Truncation

                        r_deck_No = deck.penalty_Grid[rnd3].Deck_No;//Stop here if there is no place where
                        r_row = deck.penalty_Grid[rnd3].Row;
                        r_column = deck.penalty_Grid[rnd3].Col;

                        //---------------------------------------------------------------------------------------------

                        if (HasString(cluster_GA.Inf[cluster_No].Name, "VALVE") == false)//Penalty if not a valve
                        {
                            penalty.Add(cluster_GA.Inf[cluster_No].Name);
                        }
                    }

                    //---------------------------------------------------------------------------------------------

                    else
                    {
                        //----------------The determination of the provisional arrangement-----------------------------------------------------------------

                        double rnd2 = (rnd / (8191 + 1)) * (deck.available_Grid.Count);//"8191 + 1", in order to produce a random number from 0 to 0.999
                        int rnd3 = (int)Math.Floor(rnd2);//Truncation

                        r_deck_No = deck.available_Grid[rnd3].Deck_No;//Stop here if there is no place where
                        r_row = deck.available_Grid[rnd3].Row;
                        r_column = deck.available_Grid[rnd3].Col;

                        //---------------------------------------------------------------------------------------------
                    }
                }

                else//If you are the placement specified cluster
                {
                    int No = 10;//As you get an error when it is four to input those not applicable follows

                    if (cluster_GA.Inf[cluster_No].Pos_DECK == "F")
                    {
                        No = 0;
                    }
                    else if (cluster_GA.Inf[cluster_No].Pos_DECK == "P")
                    {
                        No = 1;
                    }
                    else if (cluster_GA.Inf[cluster_No].Pos_DECK == "3")
                    {
                        No = 2;
                    }
                    else if (cluster_GA.Inf[cluster_No].Pos_DECK == "2")
                    {
                        No = 3;
                    }

                    r_deck_No = No;
                    r_row = cluster_GA.Inf[cluster_No].Pos_ROW;
                    r_column = cluster_GA.Inf[cluster_No].Pos_COL;
                }

                //----------------Replication of the cluster configuration information of the gene sequence (save function)-----------------------------------------

                Cluster_Pos c_position = new Cluster_Pos(deck_GA, r_deck_No, r_row, r_column, cluster_GA.Inf[cluster_No]);//Determination of the position information of the cluster
                ClusterList copy_cluster = new ClusterList(cluster_GA.Inf[cluster_No], c_position);
                C_List.Add(copy_cluster);

                //-----------------------------------------------------------------------------------------------------

            }
        }

        public void makeDesignplan2(int count_row, Deck[] deck_GA, Cluster cluster_GA)
        {

            for (cluster_No = 0; cluster_No < count_row; cluster_No++)
            {
                if (cluster_GA.Inf[cluster_No].Pos_DECK == null)//If you do not have a place specified cluster
                {

                    //------Generation of arrangement possible candidate---------------------------------------------------------------------

                    Deck deck = new Deck();//availableGrids用

                    if (cluster_GA.Inf[cluster_No].Const_F == 1)//Check the cluster of constraints, perform a search if possible placement
                    {
                        deck.Check_availableGrid(deck_GA[0], cluster_GA, cluster_No);
                    }
                    if (cluster_GA.Inf[cluster_No].Const_P == 1)
                    {
                        deck.Check_availableGrid(deck_GA[1], cluster_GA, cluster_No);
                    }
                    if (cluster_GA.Inf[cluster_No].Const_3RD == 1)
                    {
                        deck.Check_availableGrid(deck_GA[2], cluster_GA, cluster_No);
                    }
                    if (cluster_GA.Inf[cluster_No].Const_2ND == 1)
                    {
                        deck.Check_availableGrid(deck_GA[3], cluster_GA, cluster_No);
                    }

                    //---------------------------------------------------------------------------------------------

                    //----------------Gene sequence (binary) → location information (decimal)----------------------------------------

                    double rnd = 0;

                    for (int i = 0; i < 13; i++)
                    {
                        if (Chromo.genes[i + (13 * cluster_No)] == 1)
                            rnd = rnd + (double)Math.Pow(2, i);
                    }

                    //---------------------------------------------------------------------------------------------

                    //------If no allocable area, placed at overlapping, to have a penalty------------------------
                    //------Or if the "Valve", placed at overlapping, numeric penalty is not to have-----------------------

                    if (deck.available_Grid.Count == 0 || HasString(cluster_GA.Inf[cluster_No].Name, "VALVE") == true)
                    {
                        if (cluster_GA.Inf[cluster_No].Const_F == 1)
                        {
                            deck.Check_penaltyGrid(deck_GA[0], cluster_GA, cluster_No);
                        }
                        if (cluster_GA.Inf[cluster_No].Const_P == 1)
                        {
                            deck.Check_penaltyGrid(deck_GA[1], cluster_GA, cluster_No);
                        }
                        if (cluster_GA.Inf[cluster_No].Const_3RD == 1)
                        {
                            deck.Check_penaltyGrid(deck_GA[2], cluster_GA, cluster_No);
                        }
                        if (cluster_GA.Inf[cluster_No].Const_2ND == 1)
                        {
                            deck.Check_penaltyGrid(deck_GA[3], cluster_GA, cluster_No);
                        }

                        double rnd2 = (rnd / (8191 + 1)) * (deck.penalty_Grid.Count);//"8191 + 1", in order to produce a random number from 0 to 0.999
                        int rnd3 = (int)Math.Floor(rnd2);//Truncation

                        r_deck_No = deck.penalty_Grid[rnd3].Deck_No;//Stop here if there is no place where
                        r_row = deck.penalty_Grid[rnd3].Row;
                        r_column = deck.penalty_Grid[rnd3].Col;

                        if (HasString(cluster_GA.Inf[cluster_No].Name, "VALVE") == false)//Penalty if not a valve
                        {
                            penalty.Add(cluster_GA.Inf[cluster_No].Name);
                        }
                    }
                    //---------------------------------------------------------------------------------------------

                    //----------------The determination of the provisional arrangement-----------------------------------------------------------------
                    else
                    {
                        double rnd2 = (rnd / (8191 + 1)) * (deck.available_Grid.Count);//"8191 + 1", in order to produce a random number from 0 to 0.999
                        int rnd3 = (int)Math.Floor(rnd2);//Truncation

                        r_deck_No = deck.available_Grid[rnd3].Deck_No;//Stop here if there is no place where
                        r_row = deck.available_Grid[rnd3].Row;
                        r_column = deck.available_Grid[rnd3].Col;
                    }
                    //---------------------------------------------------------------------------------------------
                }
                else//If you are the placement specified cluster
                {
                    int No = 10;//As you get an error when it is four to input those not applicable follows

                    if (cluster_GA.Inf[cluster_No].Pos_DECK == "F")
                    {
                        No = 0;
                    }
                    else if (cluster_GA.Inf[cluster_No].Pos_DECK == "P")
                    {
                        No = 1;
                    }
                    else if (cluster_GA.Inf[cluster_No].Pos_DECK == "3")
                    {
                        No = 2;
                    }
                    else if (cluster_GA.Inf[cluster_No].Pos_DECK == "2")
                    {
                        No = 3;
                    }

                    r_deck_No = No;
                    r_row = cluster_GA.Inf[cluster_No].Pos_ROW;
                    r_column = cluster_GA.Inf[cluster_No].Pos_COL;
                }

                //----------------Replication of the cluster configuration information of the gene sequence (save function）-----------------------------------------

                Cluster_Pos c_position = new Cluster_Pos(deck_GA, r_deck_No, r_row, r_column, cluster_GA.Inf[cluster_No]);//Determination of the position information of the cluster
                ClusterList copy_cluster = new ClusterList(cluster_GA.Inf[cluster_No], c_position);
                C_List.Add(copy_cluster);

                //-----------------------------------------------------------------------------------------------------

            }
        }

        public double Fittness(int count_row, int g_size, Piping_System piping) //Calculation of fitness
        {

            if (penalty.Count != 0)//Give the greater the number of overlapping penalty
            {
                PipeLength = piping.Calc_Length(count_row, C_List, g_size);
                fittness = PipeLength + (100 * penalty.Count);//Number only 100 lattice amount of penalty of overlap
                return fittness;
            }

            {
                PipeLength = piping.Calc_Length(count_row, C_List, g_size);
                fittness = PipeLength;
                return fittness;
            }
        }

        public double Fit
        {
            get { return fittness; }
            set { fittness = value; }
        }
    }
}
