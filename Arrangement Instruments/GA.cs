using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Arrangement_Instruments
{
    class GA
    {
        private Generation[] generations;
        //---------Initial setting----------------
        private Cluster cluster_GA;
        private Deck[] deck_GA;
        private Piping_System piping_GA;
        //---------------------------------

        public Generation[] Generations
        {
            get { return generations; }
            set { generations = value; }
        }

        public GA(Cluster form_cluster, Deck[] form_decks, Piping_System piping)
        {
            cluster_GA = form_cluster;
            deck_GA = form_decks;
            piping_GA = piping;
        }

        public void makeGenerations(Random r1, int generation_size, int popu_size, int count_row, int g_size, ProgressBar progress1) //Generation of generation
        {
            generations = new Generation[generation_size];

            for (int i = 0; i < generation_size; i++)
            {
                Generation G = new Generation(generation_size);

                if (i == 0) //Generation of the first generation
                {
                    G.makeGeneration1(popu_size, 1, deck_GA, count_row, cluster_GA, g_size, piping_GA, r1);
                    generations[i] = G;
                }

                else //Second-generation and later, generated until the second generation g_size
                {
                    G.makeGeneration2(r1, generations[i - 1], popu_size, i+1, deck_GA, count_row, cluster_GA, g_size, piping_GA);
                    generations[i] = G;
                }

                progress1.Value = progress1.Value + 1;//Progress +1
            }
        }
    }
}
