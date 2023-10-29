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
using System.IO;
namespace MultiQueueSimulation
{
    public partial class Form1 : Form
    {
        public SimulationSystem SimSys;
        
        public Form1(SimulationSystem SimSys)
        {
            this.SimSys = SimSys;
            InitializeComponent();
        }
         public string res;
         

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            //res = Convert.ToString(result);
            if (result == DialogResult.OK) // Test result.
            {
                res = openFileDialog1.FileName;            
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            dataGridView1.ColumnCount = 10;
            dataGridView1.Columns[0].Name = "CustomerNumber";
            dataGridView1.Columns[1].Name = "RandomInterArrival";
            dataGridView1.Columns[2].Name = "InterArrival";
            dataGridView1.Columns[3].Name = "ArrivalTime";
            dataGridView1.Columns[4].Name = "RandomService";
            dataGridView1.Columns[5].Name = "ServiceTime";
            dataGridView1.Columns[6].Name = "AssignedServer";
            dataGridView1.Columns[7].Name = "StartTime";
            dataGridView1.Columns[8].Name = "EndTime";
            dataGridView1.Columns[9].Name = "TimeInQueue";

            // Populating DataGridView with simulation data
            foreach (SimulationCase simulationCase in SimSys.SimulationTable)
            {
                dataGridView1.Rows.Add(
                    simulationCase.CustomerNumber,
                    simulationCase.RandomInterArrival,
                    simulationCase.InterArrival,
                    simulationCase.ArrivalTime,
                    simulationCase.RandomService,
                    simulationCase.ServiceTime,
                    simulationCase.AssignedServer.ID, // Assuming 'ID' is the server's identifier
                    simulationCase.StartTime,
                    simulationCase.EndTime,
                    simulationCase.TimeInQueue
                );
            }

            AvgWaitTime.Text = (SimSys.PerformanceMeasures.AverageWaitingTime).ToString();
            MaxQueueLength.Text = (SimSys.PerformanceMeasures.MaxQueueLength).ToString();
            ProbCustWait.Text = (SimSys.PerformanceMeasures.WaitingProbability).ToString();
            for (int i = 1; i <= SimSys.NumberOfServers; i++)
                comboBox1.Items.Add(i);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
      
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int server = Convert.ToInt32(this.comboBox1.SelectedIndex);
            AvgServiceTime.Text = Convert.ToString(SimSys.Servers[server].AverageServiceTime);
            IdleProb.Text = Convert.ToString(SimSys.Servers[server].IdleProbability);
            Utilization.Text = Convert.ToString(SimSys.Servers[server].Utilization);
        }

        private void AvgWaitTime_TextChanged(object sender, EventArgs e)
        {
 
        }

        private void MaxQueueLength_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
