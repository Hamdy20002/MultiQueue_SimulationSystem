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
        public List<SimulationCase> ServerRange = new List<SimulationCase>();
        public Form2(SimulationSystem SimSys)
        {
            this.SimSys = SimSys;
            InitializeComponent();
        }

        public List<SimulationCase> ShowGraph(int server)
        {
            List<SimulationCase> Customer = new List<SimulationCase>();

            foreach (SimulationCase j in SimSys.SimulationTable)
            {
                if (j.AssignedServer.ID == server)
                {
                    Customer.Add(j);
                }
            }

            return Customer;

        }


        private void Form2_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= SimSys.NumberOfServers; i++)
                comboBox1.Items.Add(i);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServerRange = ShowGraph(Convert.ToInt32(this.comboBox1.SelectedItem));

            string name = (this.comboBox1.SelectedItem).ToString();
            name = "Server " + name + " Busy Time";
            chart1.Series.Clear();
            Series S = chart1.Series.Add(name);
            S.ChartType = SeriesChartType.Area;
            chart1.ChartAreas[0].AxisX.Minimum = 0;

            int index = (SimSys.SimulationTable.Count - 1);
            int SysEndtime = SimSys.SimulationTable[index].EndTime;
            int i = 0;
            try
            {
                 i = ServerRange[0].StartTime;
            }
            catch
            {
                i = 0;
            }
            for (int j = 0; j < ServerRange.Count; j++)
            {
                while (i <= SysEndtime)
                {
                    i++;
                    if (i >= ServerRange[j].StartTime && i <= ServerRange[j].EndTime)
                    {
                        S.Points.AddXY(ServerRange[j].StartTime, 0);
                        S.Points.AddXY(i, 1);
                        S.Points.AddXY(ServerRange[j].EndTime, 0);
                        if (j != ServerRange.Count - 1)
                        {
                            if (ServerRange[j].EndTime == ServerRange[j + 1].StartTime)
                            {
                                for (int K = ServerRange[j].StartTime; K <= ServerRange[j + 1].EndTime; K++)
                                {
                                    S.Points.AddXY(K, 1);
                                }
                            }
                            else
                            {
                                for (int z = ServerRange[j].EndTime; z <= ServerRange[j + 1].StartTime; z++)
                                {
                                    S.Points.AddXY(z, 0);
                                }
                            }

                        }
                    }
                    else
                    {
                        S.Points.AddXY(i, 0);
                        break;

                    }
                }

            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
