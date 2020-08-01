using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrangement_Instruments
{
    class Cluster
    {
        private List<Cluster_Inf> informations= new List<Cluster_Inf>();
        private List<Cluster_Pos> positions= new List<Cluster_Pos>();

        public List<Cluster_Inf> Inf
        {
            get { return informations; }
            set { informations = value; }
        }

        public List<Cluster_Pos> Pos
        {
            get { return positions; }
            set { positions = value; }
        }

        public Cluster(List<Cluster_Inf> form_cluster_Inf)
        {
            informations = form_cluster_Inf;
        }

        public void GetPosition(List<Cluster_Pos> c_positions)
        {
            positions = c_positions;
        }

    }
}
