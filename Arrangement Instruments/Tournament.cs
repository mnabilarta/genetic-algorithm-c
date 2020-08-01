using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class Tournament
    {
        private DesignPlan[] designplans;// Next Generation
        private int size;

        public Tournament(int s)
        {
            size = s;
            designplans = new DesignPlan[size];
        }


        public DesignPlan[] makeMember(Random r1, Population original, int count_row, Deck[] deck_GA, Cluster cluster_GA)
        {
            Elite(original, count_row, deck_GA, cluster_GA); //Elite Selection

            for (int i = 2; i < size; i++)
            {
                designplans[i] = selectMember(r1, original.DP, count_row);
            }
            return designplans;
        }



        public void Elite(Population original, int count_row, Deck[] deck_all, Cluster cluster_GA)  //Elite Selection
        {
            DesignPlan DP1 = new DesignPlan(count_row);
            designplans[0] = DP1;
            DesignPlan DP2 = new DesignPlan(count_row);
            designplans[1] = DP2;

            designplans[0].Chromo.genes = original.B.Chromo.genes;
            designplans[0].makeDesignplan2(count_row, deck_all, cluster_GA);

            designplans[1].Chromo.genes = original.S.Chromo.genes;
            designplans[1].makeDesignplan2(count_row, deck_all, cluster_GA);
        }


        public DesignPlan selectMember(Random r1, DesignPlan[] original, int count_row)  //Selection from 2 individuals
        {
            DesignPlan DP = new DesignPlan(count_row);
            int a = r1.Next(size);
            int b = r1.Next(size);

            if (original[a].Fit < original[b].Fit)
            {
                DP.Chromo.genes = original[a].Chromo.genes;
                return DP;
            }

            else
            {
                DP.Chromo.genes = original[b].Chromo.genes;
                return DP;
            }
        }
    }
}
