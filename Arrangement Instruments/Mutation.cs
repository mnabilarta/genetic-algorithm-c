using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class Mutation
    {
        private double odd;
        private int n;// gene number

        public Mutation()
        {
        }

        public DesignPlan M(Random r1, DesignPlan C, int aa, int count_row)
        {
            int a = (aa) % 50;//Cause once large mutation in 50 generations

            if (a == 0)
            {
                DesignPlan chromosome = graetmutation(r1, C, count_row);
                return chromosome;
            }

            else
            {
                DesignPlan chromosome = mutation(r1, C, count_row);
                return chromosome;
            }
        }

        public DesignPlan mutation(Random r1, DesignPlan DP, int count_row)//Mutation
        {
            n = 10 * count_row;
            odd = r1.Next(100);

            if (odd <= 2)//3% mutation rate
            {
                for (int i = 0; i < n; i++)
                {
                    if (r1.Next(100) < 3)//Mutation rate for one gene 3%
                    {
                        if (DP.Chromo.genes[i] == 0)
                        { DP.Chromo.genes[i] = 1; }
                        else
                        { DP.Chromo.genes[i] = 0; }
                    }
                }
                return DP;
            }
            else
            { return DP; }
        }

        public DesignPlan graetmutation(Random r1, DesignPlan DP, int count_row)　// Large Mutation
        {
            n = 10 * count_row;//Gene Length
            odd = r1.Next(100);

            if (odd <= 50)
            {
                for (int i = 0; i < n; i++)
                {
                    if (r1.Next(100) < 50)//Mutation rate for one gene 50%
                    {
                        if (DP.Chromo.genes[i] == 0)
                        { DP.Chromo.genes[i] = 1; }
                        else
                        { DP.Chromo.genes[i] = 0; }
                    }
                }
                return DP;
            }
            else
            { return DP; }
        }
    }
}
