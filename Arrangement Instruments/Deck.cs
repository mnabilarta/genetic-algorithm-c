using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

//Class to represent the element, such as a design object area and the equipment
namespace Arrangement_Instruments
{
    class Deck
    {
        private Grid[,] grid_all;//Two-dimensional array representing the deck
        private int grid_size;
        private int deck_width;
        private int deck_height;
        private Constraint constraint;
        private int floor;

        private List<Grid> available_grid = new List<Grid>();//A one-dimensional array of deployable grid
        private List<Grid> penalty_grid = new List<Grid>();//A one-dimensional array of possible placement grid at the time of the penalty

        public Grid[,] grids
        {
            get { return grid_all; }
            set { grid_all = value; }
        }

        public Constraint Constraint
        {
            get { return constraint; }
            set { constraint = value; }
        }

        public int g_size
        {
            get { return grid_size; }
            set { grid_size = value; }
        }

        public int d_width
        {
            get { return deck_width; }
            set { deck_width = value; }
        }

        public int d_height
        {
            get { return deck_height; }
            set { deck_height = value; }
        }

        public int floor_No
        {
            get { return floor; }
            set { floor = value; }
        }

        public List<Grid> available_Grid
        {
            get { return available_grid; }
            set { available_grid = value; }
        }

        public List<Grid> penalty_Grid
        {
            get { return penalty_grid; }
            set { penalty_grid = value; }
        }

        public Deck()
        {
        }

        public Deck(int g_size, int width, int height, PictureBox pictureBox, int No)
        {
            grid_size = g_size;
            deck_width = width;
            deck_height = height;
            floor = No;

            //----------------Generation of two-dimensional deck---------------------------------------------------------------------------------------            

            grid_all = new Grid[height + 1, width + 1];//grid_all [1,1] indicates the top left, shows the lower right grid_all [width + 1, height + 1] is the most           

            for (int i = 0; i <= height; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    int x1 = g_size * j;
                    int y1 = g_size * i;
                    int x2 = g_size + g_size * j;
                    int y2 = y1;
                    int x3 = x2;
                    int y3 = g_size + g_size * i;
                    int x4 = x1;
                    int y4 = y3;

                    Point point1 = new Point(x1, y1);
                    Point point2 = new Point(x2, y2);
                    Point point3 = new Point(x3, y3);
                    Point point4 = new Point(x4, y4);

                    Vertex vertex1 = new Vertex(point1);
                    Vertex vertex2 = new Vertex(point2);
                    Vertex vertex3 = new Vertex(point3);
                    Vertex vertex4 = new Vertex(point4);

                    grid_all[i, j] = new Grid(vertex1, vertex2, vertex3, vertex4, i, j, floor);

                    if (i == 0 || j == 0)
                    {
                        grid_all[i, j].Outside = true;
                    }
                }
            }

            //-------------------------------------------------------------------------------------------------------------------------


            Pen drawingPen = new Pen(Color.DodgerBlue, 1);
            System.Drawing.Point[] points = new System.Drawing.Point[4];
            Graphics g1 = pictureBox.CreateGraphics();

            for (int i = 1; i <= width; i++)
            {
                for (int roop_d = 1; roop_d <= height; roop_d++)
                {
                    Grid design = (Grid)grids[roop_d, i];

                    for (int j = 0; j < 4; j++)
                    {
                        points[j] = new System.Drawing.Point((int)design.Vertexes[j].P.X, (int)design.Vertexes[j].P.Y);
                    }

                    g1.DrawPolygon(drawingPen, points);
                }
            }
        }

        public void makeConst(Excel.Worksheet ws, Excel.Range range)
        {
            constraint = new Constraint(ws, d_height, d_width, range);
        }

        public void Constration_Condition()
        {
            for (int i = 0; i < d_height; i++)
            {
                for (int j = 0; j < d_width; j++)
                {
                    if (constraint.constraints[i, j] == 2)
                        grids[i + 1, j + 1].Hull = true;

                    if (constraint.constraints[i, j] == 1)
                        grids[i + 1, j + 1].Constraint = true;
                }
            }
        }
        
        //Drawing of the deck information
        public void Arrangement(PictureBox pictureBox)
        {
            for (int i = 1; i <= d_height; i++)
            {
                for (int j = 1; j <= d_width; j++)
                {
                    Brush drawingBrush = new SolidBrush(Color.White);

                    if (grid_all[i, j].Hull == true)
                    {
                        drawingBrush = new SolidBrush(Color.Gray);
                    }

                    if (grid_all[i, j].Constraint == true)
                    {
                        drawingBrush = new SolidBrush(Color.Aqua);
                    }

                    if (grid_all[i, j].State == 0)//Here only it is easier to State
                    {
                        drawingBrush = new SolidBrush(Color.White);
                    }

                    if (grid_all[i, j].Module == true)
                    {
                        drawingBrush = new SolidBrush(Color.Pink);
                    }

                    if (grid_all[i, j].Valve == true)//By definition of the order, the valve is overcoated on the equipment
                    {
                        drawingBrush = new SolidBrush(Color.Green);
                    }

                    //Fill the grid
                    Graphics g1 = pictureBox.CreateGraphics();
                    System.Drawing.Point[] points = new System.Drawing.Point[4];
                    Grid design = (Grid)grids[i, j];
                    for (int k = 0; k < 4; k++)
                    {
                        points[k] = new System.Drawing.Point((int)design.Vertexes[k].P.X, (int)design.Vertexes[k].P.Y);
                    }
                    g1.FillPolygon(drawingBrush, points, FillMode.Winding);

                    //Display grid (for lines are overcoated)
                    Pen drawingPen = new Pen(Color.DodgerBlue, 1);
                    g1.DrawPolygon(drawingPen, points);
                }

            }
        }

        public void Initialize(Deck[] form_deck)
        {
            for (int deck_No = 0; deck_No < 4; deck_No++)
            {
                for (int i = 0; i <= form_deck[deck_No].d_height; i++)
                {
                    for (int j = 0; j <= form_deck[deck_No].deck_width; j++)
                    {
                        form_deck[deck_No].grid_all[i, j].Module = false;
                        form_deck[deck_No].grid_all[i, j].Valve = false;
                        form_deck[deck_No].grid_all[i, j].State = 0;
                    }
                }

                for (int i = 0; i <= form_deck[deck_No].d_height; i++)
                {
                    form_deck[deck_No].grid_all[i, 0].Outside = true;
                }

                for (int j = 0; j <= form_deck[deck_No].deck_width; j++)
                {
                    form_deck[deck_No].grid_all[0, j].Outside = true;
                }

                Constration_Condition();
            }
        }

        public void Check_availableGrid(Deck form_deck, Cluster cluster_orig, int cluster_No)//If you know the size of the cluster, it returns a set of possible placement grid
        {
            if (cluster_orig.Inf[cluster_No].SIDE == "P")
            {
                for (int i = 1; i <= (form_deck.d_height / 2) - (cluster_orig.Inf[cluster_No].Height - 1); i++)//Search that takes into account the size of the cluster
                {
                    for (int j = 1; j <= form_deck.d_width - (cluster_orig.Inf[cluster_No].Width - 1); j++)
                    {
                        if (form_deck.grid_all[i, j].State == 0)
                        {
                            string judge = "OK";

                            for (int k = 0; k < cluster_orig.Inf[cluster_No].Height; k++)//Make a determination of the lattice one line at a time (the same as the drawing of the grid
                            {
                                for (int l = 0; l < cluster_orig.Inf[cluster_No].Width; l++)
                                {
                                    if (form_deck.grids[i + k, j + l].State != 0)
                                    {
                                        judge = "NG";
                                    }
                                }
                            }

                            if (judge == "OK")
                            {
                                available_Grid.Add(form_deck.grids[i, j]);
                            }
                        }
                    }
                }
            }


            else if (cluster_orig.Inf[cluster_No].SIDE == "S")
            {
                for (int i = (form_deck.d_height / 2) + 1; i <= form_deck.d_height - (cluster_orig.Inf[cluster_No].Height - 1); i++)//Search that takes into account the size of the cluster
                {
                    for (int j = 1; j <= form_deck.d_width - (cluster_orig.Inf[cluster_No].Width - 1); j++)
                    {
                        if (form_deck.grid_all[i, j].State == 0)
                        {
                            string judge = "OK";

                            for (int k = 0; k < cluster_orig.Inf[cluster_No].Height; k++)//Make a determination of the lattice one line at a time (the same as the drawing of the grid
                            {
                                for (int l = 0; l < cluster_orig.Inf[cluster_No].Width; l++)
                                {
                                    if (form_deck.grids[i + k, j + l].State != 0)
                                    {
                                        judge = "NG";
                                    }
                                }
                            }

                            if (judge == "OK")
                            {
                                available_Grid.Add(form_deck.grids[i, j]);
                            }
                        }
                    }
                }
            }



            else
            {
                for (int i = 1; i <= form_deck.d_height - (cluster_orig.Inf[cluster_No].Height - 1); i++)//Search that takes into account the size of the cluster
                {
                    for (int j = 1; j <= form_deck.d_width - (cluster_orig.Inf[cluster_No].Width - 1); j++)
                    {
                        if (form_deck.grid_all[i, j].State == 0)
                        {
                            string judge = "OK";

                            for (int k = 0; k < cluster_orig.Inf[cluster_No].Height; k++)//Make a determination of the lattice one line at a time (the same as the drawing of the grid
                            {
                                for (int l = 0; l < cluster_orig.Inf[cluster_No].Width; l++)
                                {
                                    if (form_deck.grids[i + k, j + l].State != 0)
                                    {
                                        judge = "NG";
                                    }
                                }
                            }

                            if (judge == "OK")
                            {
                                available_Grid.Add(form_deck.grids[i, j]);
                            }
                        }
                    }
                }
            }
        }

        public void Check_penaltyGrid(Deck form_deck, Cluster cluster_orig, int cluster_No)//If you know the size of the cluster, it returns a set of possible placement grid
        {
            if (cluster_orig.Inf[cluster_No].SIDE == "P")
            {
                for (int i = 1; i <= (form_deck.d_height / 2) - (cluster_orig.Inf[cluster_No].Height - 1); i++)//Search that takes into account the size of the cluster
                {
                    for (int j = 1; j <= form_deck.d_width - (cluster_orig.Inf[cluster_No].Width - 1); j++)
                    {

                        if (form_deck.grids[i, j].Valve != true)//It is set so that the valve with each other do not overlap?
                        {

                            if (form_deck.grid_all[i, j].State == 0 || form_deck.grids[i, j].State == 1)
                            {
                                string judge = "OK";

                                for (int k = 0; k < cluster_orig.Inf[cluster_No].Height; k++)//One line at a time makes a determination of the lattice (the same as the drawing of the grid)
                                {
                                    for (int l = 0; l < cluster_orig.Inf[cluster_No].Width; l++)
                                    {
                                        if (form_deck.grids[i + k, j + l].State != 0 & form_deck.grids[i + k, j + l].State != 1 & form_deck.grids[i + k, j + l].State != 2)
                                        {
                                            judge = "NG";
                                        }
                                    }
                                }

                                if (judge == "OK")
                                {
                                    penalty_Grid.Add(form_deck.grids[i, j]);
                                }
                            }
                        }
                    }
                }
            }

            else if (cluster_orig.Inf[cluster_No].SIDE == "S")
            {
                for (int i = (form_deck.d_height / 2) + 1; i <= form_deck.d_height - (cluster_orig.Inf[cluster_No].Height - 1); i++)//Search that takes into account the size of the cluster

                {
                    for (int j = 1; j <= form_deck.d_width - (cluster_orig.Inf[cluster_No].Width - 1); j++)
                    {
                        if (form_deck.grids[i, j].Valve != true)//It is set so that the valve with each other do not overlap?
                        {
                            if (form_deck.grid_all[i, j].State == 0 || form_deck.grids[i, j].State == 1)
                            {
                                string judge = "OK";

                                for (int k = 0; k < cluster_orig.Inf[cluster_No].Height; k++)//One line at a time makes a determination of the lattice (the same as the drawing of the grid)
                                {
                                    for (int l = 0; l < cluster_orig.Inf[cluster_No].Width; l++)
                                    {
                                        if (form_deck.grids[i + k, j + l].State != 0 & form_deck.grids[i + k, j + l].State != 1 & form_deck.grids[i + k, j + l].State != 2)
                                        {
                                            judge = "NG";
                                        }
                                    }
                                }

                                if (judge == "OK")
                                {
                                    penalty_Grid.Add(form_deck.grids[i, j]);
                                }
                            }
                        }
                    }
                }
            }


            else
            {

                for (int i = 1; i <= form_deck.d_height - (cluster_orig.Inf[cluster_No].Height - 1); i++)//Search that takes into account the size of the cluster
                {
                    for (int j = 1; j <= form_deck.d_width - (cluster_orig.Inf[cluster_No].Width - 1); j++)
                    {
                        if (form_deck.grids[i, j].Valve != true)//It is set so that the valve with each other do not overlap?
                        {
                            if (form_deck.grid_all[i, j].State == 0 || form_deck.grids[i, j].State == 1 )
                            {
                                string judge = "OK";

                                for (int k = 0; k < cluster_orig.Inf[cluster_No].Height; k++)//One line at a time makes a determination of the lattice (the same as the drawing of the grid)
                                {
                                    for (int l = 0; l < cluster_orig.Inf[cluster_No].Width; l++)
                                    {
                                        if (form_deck.grids[i + k, j + l].State != 0 & form_deck.grids[i + k, j + l].State != 1 & form_deck.grids[i + k, j + l].State != 2)
                                        {
                                            judge = "NG";
                                        }
                                    }
                                }

                                if (judge == "OK")
                                {
                                    penalty_Grid.Add(form_deck.grids[i, j]);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}