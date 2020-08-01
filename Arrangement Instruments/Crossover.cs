using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class Crossover
    {
        private int s; //Repeat s time

        public Crossover()
        {
        }

        public Population evenCross(Random r1, Population P2, int size, int count_row,int n)　//Number of individuals is an even number
        {
            s = size / 2;
            Crossover CO2 = new Crossover();
            DesignPlan[] designplans = new DesignPlan[size];

            designplans[0] = P2.DP[0];
            designplans[1] = P2.DP[1];

            for (int i = 1; i < s; i++)//Crossover by performing, generating odd-numbered and even-numbered two designplans for loop once
            {
                int p = r1.Next(10 * count_row);//To gene length
                int a = i * 2,//Odd-numbered
                    b = a + 1;//Even-numbered

                DesignPlan[] designplan = CO2.Cross1(P2.DP[a], P2.DP[b], p, count_row);

                designplans[a] = designplan[0];
                designplans[b] = designplan[1];
            }

            Population population = new Population(designplans, size,n);
            return population;
        }

        public Population oddCross(Random r1, Population P2, int size, int count_row, int n)　//Number of individuals is an odd number
        {
            Crossover CO2 = new Crossover();
            DesignPlan[] designplans = new DesignPlan[size];

            designplans[0] = P2.DP[0];
            designplans[1] = P2.DP[1];

            s = (size - 1) / 2;
            for (int i = 1; i < s; i++)
            {
                int p = r1.Next(10 * count_row);//To gene length
                int a = i * 2,//Odd-numbered
                    b = a + 1;//Even-numbered

                DesignPlan[] designplan = CO2.Cross1(P2.DP[a], P2.DP[b], p, count_row);

                designplans[a] = designplan[0];
                designplans[b] = designplan[1];

            }

            int p2 = r1.Next(10 * count_row);//To gene length
            DesignPlan[] designplan2 = CO2.Cross1(P2.DP[size - 1], P2.DP[0], p2, count_row);
            designplans[size - 1] = designplan2[0];

            Population population = new Population(designplans, size,n);
            return population;
        }

        public DesignPlan[] Cross1(DesignPlan DP1, DesignPlan DP2, int p, int count_row)//1-point crossover
        {
            DesignPlan[] designplan = new DesignPlan[2];
            DesignPlan designplan1 = new DesignPlan(count_row);
            DesignPlan designplan2 = new DesignPlan(count_row);

            for (int i = 0; i < p; i++)
            {
                designplan1.Chromo.genes[i] = DP1.Chromo.genes[i];
                designplan2.Chromo.genes[i] = DP2.Chromo.genes[i];
            }

            for (int i = p; i < 10 * count_row; i++)//i is range for gene length
            {
                designplan1.Chromo.genes[i] = DP2.Chromo.genes[i];
                designplan2.Chromo.genes[i] = DP1.Chromo.genes[i];
            }

            designplan[0] = designplan1;
            designplan[1] = designplan2;

            return designplan;
        }
    }
}
