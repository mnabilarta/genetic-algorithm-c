using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;//Excel用

namespace Arrangement_Instruments
{
    class Constraint
    {
        private int[,] constraint_all;

        public int[,] constraints
        {
            get { return constraint_all; }
            set { constraint_all = value; }
        }


        public Constraint(Excel.Worksheet ws, int d_height, int d_width, Excel.Range range)
        {            
            constraints = new int[d_height, d_width];

            if (range != null)
            {
                for (int i = 1; i <= d_height; i++)
                {
                    for (int j = 1; j <= d_width; j++)
                    {                        
                        constraints[i - 1, j - 1] = Convert.ToInt32(range.Value2[i,j]);
                    }
                }
            }
        }

    }
}

