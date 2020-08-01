using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class Generation
    {
        private int size;　// number of generation
        private Population populations;

        public Generation(int generation_size)
        {
            size = generation_size;
        }
        
        public Population popu
        {
            get { return populations; }
            set { populations = value; }
        }

        public void makeGeneration1(int popu_size, int n, Deck[] deck_GA, int count_row, Cluster cluster_GA, int g_size, Piping_System piping_GA, Random r1)
        {
            Population P = new Population(popu_size, n);
            P.makePopulaiton1(deck_GA, count_row, cluster_GA, r1, piping_GA);
            P.Best(count_row, g_size, piping_GA);
            populations = P;
        }

        public void makeGeneration2(Random r1, Generation GB, int popu_size, int n, Deck[] form_decks, int count_row, Cluster cluster_orig, int g_size, Piping_System piping_orig)//1-point crossover
        {
            Population P1 = new Population(popu_size, n);
            Population P2 = P1.Tournament(r1, GB.populations, count_row, form_decks, cluster_orig);
            Population P3 = P2.Crossover(r1, P2, count_row);
            Population P4 = P3.Mutation(r1, P3, count_row);
            Population P5 = P4.DesignPlan(P4, form_decks, count_row, cluster_orig, g_size, piping_orig, r1);
            populations = P5;
        }
    }
}
