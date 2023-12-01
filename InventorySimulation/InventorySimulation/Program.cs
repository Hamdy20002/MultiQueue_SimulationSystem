﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventoryModels;
using InventoryTesting;


namespace InventorySimulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SimulationSystem system = new SimulationSystem();
            system.CalculateSimulationOutput("C:/0 MY DATA/College/Modeling/Task 3/[Students]_Template/InventorySimulation/InventorySimulation/TestCases/TestCase2.txt");
            string result = TestingManager.Test(system, Constants.FileNames.TestCase2);
            MessageBox.Show(result);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(system));
        }   
    }
}
