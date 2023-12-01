using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventoryModels;
using InventoryTesting;


namespace InventorySimulation
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
            EndingAvg.Text = (SimSys.PerformanceMeasures.EndingInventoryAverage).ToString();
            ShortageAvg.Text = (SimSys.PerformanceMeasures.ShortageQuantityAverage).ToString();

            dataGridView1.Columns.Clear();

            // Define the column headers
            dataGridView1.Columns.Add("Day", "Day");
            dataGridView1.Columns.Add("Cycle", "Cycle");
            dataGridView1.Columns.Add("DayWithinCycle", "Day Within Cycle");
            dataGridView1.Columns.Add("BeginningInventory", "Beginning Inventory");
            dataGridView1.Columns.Add("RandomDemand", "Random Demand");
            dataGridView1.Columns.Add("Demand", "Demand");
            dataGridView1.Columns.Add("EndingInventory", "Ending Inventory");
            dataGridView1.Columns.Add("ShortageQuantity", "Shortage Quantity");
            dataGridView1.Columns.Add("OrderQuantity", "Order Quantity");
            dataGridView1.Columns.Add("RandomLeadDays", "Random Lead Days");
            dataGridView1.Columns.Add("LeadDays", "Lead Days");

            // Populating DataGridView with simulation data
            foreach (SimulationCase simulationCase in SimSys.SimulationTable) // Replace 'simulationCases' with your list of SimulationCase
            {
                dataGridView1.Rows.Add(
                    simulationCase.Day,
                    simulationCase.Cycle,
                    simulationCase.DayWithinCycle,
                    simulationCase.BeginningInventory,
                    simulationCase.RandomDemand,
                    simulationCase.Demand,
                    simulationCase.EndingInventory,
                    simulationCase.ShortageQuantity,
                    simulationCase.OrderQuantity,
                    simulationCase.RandomLeadDays,
                    simulationCase.LeadDays
                );
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

