using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueModels;
using MultiQueueTesting;
using System.Windows.Forms.DataVisualization.Charting;

namespace MultiQueueSimulation
{
    public partial class Form2 : Form
    {
        public SimulationSystem SimSys;
        public List<SimulationCase> ServerRange;
        public Form2(SimulationSystem SimSys)
        {
            this.SimSys = SimSys;
            InitializeComponent();
        }

        public List<SimulationCase> ShowGraph(int server)
        {
            List<SimulationCase> Customer = new List<SimulationCase>();

            for (int i = 0; i < SimSys.NumberOfServers; i++)
            {
                foreach (SimulationCase j in SimSys.SimulationTable)
                {
                    if(j.AssignedServer.ID == i+1)
                    {
                        Customer.Add(j);
                    }
                }
            }
                

            return Customer;
            //List<List<List<int>>> Serverrange = new List<List<List<int>>>();
            //List<int> Customer_count = new List<int>();
            //foreach (SimulationCase i in SimSys.SimulationTable)
            //{
            //    int ServerNum = (i.AssignedServer.ID - 1);
            //    Serverrange[ServerNum][Customer_count[ServerNum]][0] = i.StartTime;
            //    Serverrange[(i.AssignedServer.ID - 1)][Customer_count[ServerNum]][1] = i.EndTime;
            //    Customer_count[(i.AssignedServer.ID - 1)] += 1;
            //}
            //return Serverrange;
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= SimSys.NumberOfServers; i++)
                comboBox1.Items.Add(i);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServerRange = ShowGraph(Convert.ToInt32(this.comboBox1.SelectedIndex));

            //chart1.Series.Clear();
            //chart1.Series.Add("ServerBusyTime");
            //chart1.Series["ServerBusyTime"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            //chart1.Series["ServerBusyTime"].Color = Color.Red;

            chart1.Series.Clear();
            Series S = chart1.Series.Add("Server Busy Time");
            S.ChartType = SeriesChartType.Area;
            chart1.ChartAreas[0].AxisX.Minimum = 0;


            int index = (SimSys.SimulationTable.Count - 1);
            int SysEndtime = SimSys.SimulationTable[index].EndTime;
            int i = -1;
            for (int j = 0; j < ServerRange.Count; j++)
            {
                while (i <= SysEndtime)
                {
                    i++;
                    if (i >= ServerRange[j].StartTime && i <= ServerRange[j].EndTime)
                    {
                        S.Points.AddXY(i, 1);
                    }
                    else
                    {
                        S.Points.AddXY(i-1, 0);
                        S.Points.AddXY(i, 0);
                        S.Points.AddXY(i+1, 0);
                        break;
                    }
                }

            }


            











            //int server = 
            //chart1.Series.Add("Server_Busy_Time");
            //chart1.Series["Server_Busy_Time"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;

            //int index = (SimSys.SimulationTable.Count - 1);
            //int SysEndtime = SimSys.SimulationTable[index].EndTime;

            //int Currenttime = -1;
            //for (int j = 0; j < Serverranges[server].Count; j++)
            //{
            //    while(Currenttime <= SysEndtime)
            //    {
            //        Currenttime++;
            //        if (Currenttime >= Serverranges[server][j][0] && Currenttime <= Serverranges[server][j][1])
            //        {
            //            chart1.Series["Server_Busy_Time"].Points.AddXY(Currenttime, 1);
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }

            //}
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
