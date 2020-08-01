using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;//Excel用

namespace Arrangement_Instruments
{
    public partial class Form1 : Form
    {
        int g_size;//異なるメソッドで使用
        int count_row;//Excelで入力したクラスタの数
        //int count_column;//正方行列なので使わない
        

        Cluster form_cluster;
        Piping_System piping;
        private List<Cluster_Inf> form_cluster_Inf = new List<Cluster_Inf>();
        private Deck[] form_decks = new Deck[4];
        public static ProgressBar progress1;//プログレスバー


        public Form1()
        {
            InitializeComponent();
            Text = "Arrangement Instruments";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            g_size = Convert.ToInt32(textBox1.Text);
            int width = Convert.ToInt32(textBox2.Text);
            int height = Convert.ToInt32(textBox3.Text);

            form_decks[0] = new Deck(g_size, width, height, pictureBox1, 0);
            form_decks[1] = new Deck(g_size, width, height, pictureBox2, 1);
            form_decks[2] = new Deck(g_size, width, height, pictureBox3, 2);
            form_decks[3] = new Deck(g_size, width, height, pictureBox4, 3);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox7.Text = openFileDialog1.FileName;
                string ExcelBookFileName = textBox7.Text;
                Excel.Application ExcelApp = new Excel.Application();
                ExcelApp.Visible = false;
                Excel.Workbook wb = ExcelApp.Workbooks.Open(ExcelBookFileName);

                //----------制約条件の読み取り----------------------------
                Excel.Worksheet ws1 = wb.Sheets[1];
                ws1.Select(Type.Missing);
                Excel.Range range1 = ExcelApp.get_Range("A1", "CV100");//100×100までの情報は読み取り可能
                form_decks[0].makeConst(ws1, range1);

                Excel.Worksheet ws2 = wb.Sheets[2];
                ws2.Select(Type.Missing);
                Excel.Range range2 = ExcelApp.get_Range("A1", "CV100");//100×100までの情報は読み取り可能
                form_decks[1].makeConst(ws2, range2);

                Excel.Worksheet ws3 = wb.Sheets[3];
                ws3.Select(Type.Missing);
                Excel.Range range3 = ExcelApp.get_Range("A1", "CV100");//100×100までの情報は読み取り可能
                form_decks[2].makeConst(ws3, range3);

                Excel.Worksheet ws4 = wb.Sheets[4];
                ws4.Select(Type.Missing);
                Excel.Range range4 = ExcelApp.get_Range("A1", "CV100");//100×100までの情報は読み取り可能
                form_decks[3].makeConst(ws4, range4);
                //--------------------------------------------------------

                wb.Close(false, Type.Missing, Type.Missing);
                ExcelApp.Quit();
            }

            form_decks[0].Constration_Condition();
            form_decks[0].Arrangement(pictureBox1);
            form_decks[1].Constration_Condition();
            form_decks[1].Arrangement(pictureBox2);
            form_decks[2].Constration_Condition();
            form_decks[2].Arrangement(pictureBox3);
            form_decks[3].Constration_Condition();
            form_decks[3].Arrangement(pictureBox4);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog1.FileName;
                string ExcelBookFileName = textBox4.Text;
                Excel.Application ExcelApp = new Excel.Application();
                ExcelApp.Visible = false;
                Excel.Workbook wb = ExcelApp.Workbooks.Open(ExcelBookFileName);
                Excel.Worksheet ws1 = wb.Sheets[1];
                ws1.Select(Type.Missing);
                count_row = ws1.get_Range("A2").End[Excel.XlDirection.xlDown].Row - 1;//A1から数えているので"-1"している count_row = cluster数
                //count_column = ws1.get_Range("A2").End[Excel.XlDirection.xlToRight].Column;//A1から数えている

                Excel.Range range = ExcelApp.get_Range("A1", "K150");//K150までの情報は読み取り可能→一応クラスタは150個まで
                if (range != null)
                {
                    for (int i = 0; i < count_row; i++)
                    {
                        string cluster_name = Convert.ToString(range.Value2[i + 2, 1]);//ID
                        int size_x = Convert.ToInt32(range.Value2[i + 2, 2]);//width
                        int size_y = Convert.ToInt32(range.Value2[i + 2, 3]);//height
                        int const_f = Convert.ToInt32(range.Value2[i + 2, 4]);//floor
                        int const_p = Convert.ToInt32(range.Value2[i + 2, 5]);//partial
                        int const_3rd = Convert.ToInt32(range.Value2[i + 2, 6]);//3rd dk
                        int const_2nd = Convert.ToInt32(range.Value2[i + 2, 7]);//2nd dk
                        string side = Convert.ToString(range.Value2[i + 2, 8]);//(S)or(P)
                        string pos_deck = Convert.ToString(range.Value2[i + 2, 9]);//deck
                        int pos_row = Convert.ToInt32(range.Value2[i + 2, 10]);//row
                        int pos_col = Convert.ToInt32(range.Value2[i + 2, 11]);//column

                        form_cluster_Inf.Add(new Cluster_Inf(cluster_name, size_x, size_y, const_f, const_p, const_2nd, const_3rd, pos_deck, pos_row, pos_col, side));
                        listBox1.Items.Add(form_cluster_Inf[i].c_data);
                    }
                }
                form_cluster = new Cluster(form_cluster_Inf);

                //----------接続関係の読み取り----------------------------
                Excel.Worksheet ws2 = wb.Sheets[2];
                ws2.Select(Type.Missing);
                piping = new Piping_System();
                piping.Interaction(count_row, ws2);
                //--------------------------------------------------------

                wb.Close(false, Type.Missing, Type.Missing);
                ExcelApp.Quit();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //数値の読み込み
            int generation_size = Convert.ToInt32(textBox5.Text);//世代数
            int popu_size = Convert.ToInt32(textBox6.Text);//個体数
            //変数
            Random r1 = new Random();

            //プログレスバーの設定
            progress1 = new ProgressBar();
            progress1 = progressBar1;
            progress1.Minimum = 0;
            progress1.Maximum = generation_size;
            progress1.Value = 0;


            //世代の生成
            GA ga = new GA(form_cluster, form_decks, piping);
            ga.makeGenerations(r1, generation_size, popu_size, count_row, g_size, progress1);

            for (int i = 0; i < count_row; i++)
            {
                ga.Generations[generation_size - 1].popu.B.C_List[i].Calc_Center(form_decks);
            }

            form_decks[0].Arrangement(pictureBox1);
            form_decks[1].Arrangement(pictureBox2);
            form_decks[2].Arrangement(pictureBox3);
            form_decks[3].Arrangement(pictureBox4);

            ga.Generations[generation_size - 1].popu.B.Fittness(count_row, g_size, piping);//最適解の配管長を再計算する（出力用）


            //------------各クラスタ配置X,Yの表示------------------------------------------------------------------
            for (int i = 0; i < count_row; i++)
            {
                string DECK = Convert.ToString(ga.Generations[generation_size - 1].popu.B.C_List[i].p_deck_No);
                string ROW = Convert.ToString(ga.Generations[generation_size - 1].popu.B.C_List[i].c_p_row);
                string COL = Convert.ToString(ga.Generations[generation_size - 1].popu.B.C_List[i].c_p_column);
                string pos = form_cluster_Inf[i].Name + " DECK:" + DECK + " ROW:" + ROW + " COL:" + COL;
                listBox1.Items.Add(pos);
            }
            //-----------------------------------------------------------------------------------------------------

            string pena = Convert.ToString(ga.Generations[generation_size - 1].popu.B.Penalty.Count);
            string fit = Convert.ToString(ga.Generations[generation_size - 1].popu.B.Fit);
            string results = "Penalty" + pena + "   " + "Fit" + fit;
            listBox1.Items.Add(results);


            //--------和田さんのを参考に--------------------------------------------------------------------------------------------------------
            //-------グラフの出力-----------------------------
            //自動グラフ作成-------------------------------------------------------------------------------------------------------------
            button1.Enabled = false;//謎
            Excel.Application oXL = new Excel.Application();//WindowsのスタートメニューからExcelを起動するようなもの
            Excel._Workbook oWB;//Workbookオブジェクトを生成
            Excel._Worksheet oSheet;//Workbookオブジェクトに含まれるWorksheetオブジェクトを生成
            Excel._Chart oChart;//おそらくグラフを表すオブジェクト
            Excel.Range oRng;//ワークシート上の指定された範囲のセルを管理

            //よくわからない部分
            oXL.Visible = true;
            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Type.Missing));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;

            //シートの1行目
            oSheet.Cells[1, 1] = "sedai";
            oSheet.Cells[1, 2] = "fitness";

            for (int i = 0; i < generation_size; i++)
            {
                oSheet.Cells[i + 2, 1] = i + 1;
                oSheet.Cells[i + 2, 2] = ga.Generations[i].popu.B.Fit;
            }


            //--------配管長の出力----------------------------------------------------------

            for (int i = 0; i < count_row; i++)
            {
                for (int j = 0; j < count_row; j++)
                {
                    if (i == 0)
                    {
                        oSheet.Cells[i + 1, j + 5] = j + 1;
                    }

                    if (j == 0)
                    {
                        oSheet.Cells[i + 2, j + 4] = i + 1;
                    }

                    oSheet.Cells[i + 2, j + 5] = (piping.P_length[i, j])/10;
                }
            }

            //------------------------------------------------------------------------------

            //-------------------------------------------------------------------
            oChart = (Excel._Chart)oWB.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            //読み取る部分をExcelに指定
            oRng = oSheet.get_Range("A1:B2001", Type.Missing);//とりあえず2000世代までは書き込み可能
            oChart.ChartType = Excel.XlChartType.xlXYScatterLinesNoMarkers;
            oChart.SetSourceData(oRng, Type.Missing);

            //グラフ1作成
            Excel.Axis xAxis = (Excel.Axis)oChart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            xAxis.MajorUnit = 50;//主な目盛
            xAxis.HasTitle = true;
            xAxis.AxisTitle.Text = "generation";
            Excel.Axis yAxis = (Excel.Axis)oChart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            yAxis.HasTitle = true;
            yAxis.AxisTitle.Text = "fitness";
            
            MessageBox.Show("終了");
            //------------------------------------------------------------------------------------------------------------------------------------

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
               
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;*/
            ExportToExcel();
        }
        private void ExportToExcel()
        {
            // Creating a Excel object.
            int width = Convert.ToInt32(textBox2.Text);
            int height = Convert.ToInt32(textBox3.Text);
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "DatGrid4";
                
                int cellRowIndex = 1;
                int cellColumnIndex = 1;
                //Loop through each row and read value from each column.
                for (int i = 1; i <= height ; i++)
                {
                    for (int j = 1; j <= width; j++)
                    {
                        // Excel index starts from 1,1. As first Row would have the Column headers, adding a condition check.                        
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = form_decks[3].grids[i,j].State;                                                
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                Microsoft.Office.Interop.Excel._Worksheet worksheet3 =excel.Worksheets.Add(Type.Missing,Type.Missing, Type.Missing, Type.Missing);                
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "DatGrid3";
                cellRowIndex = 1;
                cellColumnIndex = 1;
                //Loop through each row and read value from each column.
                for (int i = 1; i <= height; i++)
                {
                    for (int j = 1; j <= width; j++)
                    {
                        // Excel index starts from 1,1. As first Row would have the Column headers, adding a condition check.                        
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = form_decks[2].grids[i, j].State;
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }
                Microsoft.Office.Interop.Excel._Worksheet worksheet2 = excel.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "DatGrid2";
                cellRowIndex = 1;
                cellColumnIndex = 1;
                //Loop through each row and read value from each column.
                for (int i = 1; i <= height; i++)
                {
                    for (int j = 1; j <= width; j++)
                    {
                        // Excel index starts from 1,1. As first Row would have the Column headers, adding a condition check.                        
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = form_decks[1].grids[i, j].State;
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }
                Microsoft.Office.Interop.Excel._Worksheet worksheet1 = excel.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "DatGrid1";
                cellRowIndex = 1;
                cellColumnIndex = 1;
                //Loop through each row and read value from each column.
                for (int i = 1; i <= height; i++)
                {
                    for (int j = 1; j <= width; j++)
                    {
                        // Excel index starts from 1,1. As first Row would have the Column headers, adding a condition check.                        
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = form_decks[0].grids[i, j].State;
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }



                //Getting the location and file name of the excel to save from user.
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.FilterIndex = 2;

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Export Successful");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }

        }


    }
}
