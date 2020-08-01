using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class Chromosome
    {
        private int size;//Full-length gene (placement plan of all of the cluster)
        private int[] gene_all;


        public int[] genes
        {
            get { return gene_all; }
            set { gene_all = value; }
        }

        public Chromosome(int count_row)
        {
            size = 13 * count_row;//Gene length of one cluster is 13
            genes = new int[size];
        }

        public int[] makeCluster_Gene(Random r1)
        {

            for (int i = 0; i < size; i++)
            {
                genes[i] = r1.Next(2);
            }

            return genes;
        }
    }
}
