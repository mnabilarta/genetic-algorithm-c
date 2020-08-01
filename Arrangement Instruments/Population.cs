using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class Population
    {
        private int size;　// Number of Individuals
        private int n;  // n Generation
        private DesignPlan[] designplans;
        private DesignPlan best;
        private DesignPlan second;

        public Population(int popu_size, int nn)
        {
            size = popu_size;
            n = nn;
        }

        public Population(DesignPlan[] aa, int s)
        {
            designplans = aa;
            size = s;
        }

        public Population(DesignPlan[] aa, int s, int nn)
        {
            designplans = aa;
            size = s;
            n = nn;
        }

        public void makePopulaiton1(Deck[] deck_GA, int count_row, Cluster cluster_GA, Random r1, Piping_System piping_GA) //First Generation
        {
            designplans = new DesignPlan[size];

            for (int i = 0; i < size; i++) //size: generation size
            {
                DesignPlan C1 = new DesignPlan(count_row);

                C1.makeDesignplan1(count_row, r1, cluster_GA, deck_GA);
                designplans[i] = C1;

                //-----------designplans[i] Initialize after Generation, The deck Information---------------

                Deck deck = new Deck();
                deck.Initialize(deck_GA);

                //------------------------------------------------------------------------

            }
        }

        public Population Tournament(Random r1, Population original, int count_row, Deck[] deck_GA, Cluster cluster_GA) //Tournament Selection
        {
            Tournament T = new Tournament(size);
            DesignPlan[] members = T.makeMember(r1, original, count_row, deck_GA, cluster_GA);
            Population P = new Population(members, size, n);
            return P;
        }

        public Population Crossover(Random r1, Population P, int count_row)// 1-point cross over
        {
            Crossover CO = new Crossover();
            int s = size % 2;
            if (s == 0)//Genap
            {
                Population P2 = CO.evenCross(r1, P, size, count_row,n);
                return P2;
            }
            else//Ganjil
            {
                Population P2 = CO.oddCross(r1, P, size, count_row,n);
                return P2;
            }
        }

        public Population Mutation(Random r1, Population P, int count_row)// Mutation
        {
            Mutation DM = new Mutation();
            DesignPlan[] DP1 = new DesignPlan[size];
            DP1[0] = P.DP[0];
            DP1[1] = P.DP[1];

            for (int i = 2; i < size; i++)
            {
                DesignPlan DP2 = DM.M(r1, P.DP[i], n, count_row);
                DP1[i] = DP2;
            }
            Population P3 = new Population(DP1, size, n);
            return P3;
        }

        public Population DesignPlan(Population P, Deck[] deck_GA, int count_row, Cluster cluster_GA, int g_size, Piping_System piping, Random r1)
        {
            for (int i = 0; i < size; i++)
            {
                designplans[i].makeDesignplan2(count_row, deck_GA, cluster_GA);

                //-----------designplans[i]Initialize after Generation, The deck Information---------------

                Deck deck = new Deck();
                deck.Initialize(deck_GA);

                //------------------------------------------------------------------------

            }

            P.Best(count_row, g_size, piping);
            return P;
        }

        public void Best(int count_row, int g_size, Piping_System piping) //Selection of the best individuals from the population
        {
            double F = 100000;　//Evaluation Value
            double F2 = 100000;
            int A = 0;  
            int B = 0;  

            for (int i = 0; i < size; i++)
            {
                double fit = designplans[i].Fittness(count_row, g_size, piping);

                if (fit < F)
                {
                    F = fit;
                    B = A;
                    A = i;
                }
                else if (fit < F2)
                {
                    F2 = fit;
                    B = i;
                }
            }
            best = designplans[A];
            second = designplans[B];
        }

        //chromosomesのgetset
        public DesignPlan[] DP
        {
            get { return designplans; }
            set { designplans = value; }
        }


        //bestのgetset
        public DesignPlan B
        {
            get { return best; }
            set { best = value; }
        }


        //secondのgetset
        public DesignPlan S
        {
            get { return second; }
            set { second = value; }
        }
    }
}
