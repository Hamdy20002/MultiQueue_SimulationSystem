using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerModels;
using NewspaperSellerTesting;

namespace NewspaperSellerSimulation
{
    public partial class Form1 : Form
    {
        public SimulationSystem SimSys;
        public Form1(SimulationSystem SimSys)
        {
            this.SimSys = SimSys;
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();

            // Define the column headers
            dataGridView1.Columns.Add("DayNo", "Day No");
            dataGridView1.Columns.Add("RandomNewsDayType", "Random News Day Type");
            dataGridView1.Columns.Add("NewsDayType", "News Day Type");
            dataGridView1.Columns.Add("RandomDemand", "Random Demand");
            dataGridView1.Columns.Add("Demand", "Demand");
            dataGridView1.Columns.Add("DailyCost", "Daily Cost");
            dataGridView1.Columns.Add("SalesProfit", "Sales Profit");
            dataGridView1.Columns.Add("LostProfit", "Lost Profit");
            dataGridView1.Columns.Add("ScrapProfit", "Scrap Profit");
            dataGridView1.Columns.Add("DailyNetProfit", "Daily Net Profit");

            // Populating DataGridView with simulation data
            foreach (SimulationCase simulationCase in SimSys.SimulationTable) // Replace 'simulationCases' with your list of SimulationCase
            {
                dataGridView1.Rows.Add(
                    simulationCase.DayNo,
                    simulationCase.RandomNewsDayType,
                    simulationCase.NewsDayType,
                    simulationCase.RandomDemand,
                    simulationCase.Demand,
                    simulationCase.DailyCost,
                    simulationCase.SalesProfit,
                    simulationCase.LostProfit,
                    simulationCase.ScrapProfit,
                    simulationCase.DailyNetProfit
                );
            }
        }
    }
}
