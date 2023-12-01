using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            DemandDistribution = new List<Distribution>();
            LeadDaysDistribution = new List<Distribution>();
            SimulationTable = new List<SimulationCase>();
            PerformanceMeasures = new PerformanceMeasures();
        }

        ///////////// INPUTS /////////////

        public int OrderUpTo { get; set; }
        public int ReviewPeriod { get; set; }
        public int NumberOfDays { get; set; }
        public int StartInventoryQuantity { get; set; }
        public int StartLeadDays { get; set; }
        public int StartOrderQuantity { get; set; }
        public List<Distribution> DemandDistribution { get; set; }
        public List<Distribution> LeadDaysDistribution { get; set; }

        ///////////// OUTPUTS /////////////

        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }

        public int DetermineDemand(int randomDigitForDemand, List<Distribution> demandDistribution)
        {
            foreach (var distribution in demandDistribution)
            {
                if (randomDigitForDemand >= distribution.MinRange && randomDigitForDemand <= distribution.MaxRange)
                {
                    return distribution.Value;
                }
            }

            return 0; // Default to 0 if no suitable demand found
        }

        public int DetermineLeadDays(int randomLeadDays, List<Distribution> leadDaysDistribution)
        {
            foreach (var distribution in leadDaysDistribution)
            {
                if (randomLeadDays >= distribution.MinRange && randomLeadDays <= distribution.MaxRange)
                {
                    return distribution.Value;
                }
            }

            return 0; // Default to 0 if no suitable lead days found
        }
        public void CalculateSimulationOutput(string path)
        {
            List<List<List<float>>> inputdis = Program.Run(path);

            #region forint_input
            OrderUpTo = (int)inputdis[0][0][0];
            ReviewPeriod = (int)inputdis[1][0][0];
            StartInventoryQuantity = (int)inputdis[2][0][0];
            StartLeadDays = (int)inputdis[3][0][0];
            StartOrderQuantity = (int)inputdis[4][0][0];
            NumberOfDays = (int)inputdis[5][0][0];
            #endregion

            #region readDemandDistribution
            var distribution1 = new Distribution();
            distribution1.Value = (int)inputdis[6][0][0];
            distribution1.Probability = (decimal)inputdis[6][0][1];

            var distribution2 = new Distribution();
            distribution2.Value = (int)inputdis[6][0][2];
            distribution2.Probability = (decimal)inputdis[6][0][3];

            var distribution3 = new Distribution();
            distribution3.Value = (int)inputdis[6][0][4];
            distribution3.Probability = (decimal)inputdis[6][0][5];

            var distribution4 = new Distribution();
            distribution4.Value = (int)inputdis[6][0][6];
            distribution4.Probability = (decimal)inputdis[6][0][7];

            var distribution5 = new Distribution();
            distribution5.Value = (int)inputdis[6][0][8];
            distribution5.Probability = (decimal)inputdis[6][0][9];


            DemandDistribution.Add(distribution1);
            DemandDistribution.Add(distribution2);
            DemandDistribution.Add(distribution3);
            DemandDistribution.Add(distribution4);
            DemandDistribution.Add(distribution5);


            //DemandDistribution = Distribution.Run(DemandDistribution);
            #endregion

            #region LeadDaysDistribution
            var distribution21 = new Distribution();
            distribution21.Value = (int)inputdis[7][0][0];
            distribution21.Probability = (decimal)inputdis[7][0][1];

            var distribution22 = new Distribution();
            distribution22.Value = (int)inputdis[7][0][2];
            distribution22.Probability = (decimal)inputdis[7][0][3];

            var distribution23 = new Distribution();
            distribution23.Value = (int)inputdis[7][0][4];
            distribution23.Probability = (decimal)inputdis[7][0][5];

            LeadDaysDistribution.Add(distribution21);
            LeadDaysDistribution.Add(distribution22);
            LeadDaysDistribution.Add(distribution23);

            //LeadDaysDistribution = Distribution.Run(LeadDaysDistribution);
            #endregion 

            #region fill

            Random rand = new Random();

            int dayy = 0;
            int order = 0;
            int time = 0;
            int endingprev = 0;
            for (int i = 1; i <= NumberOfDays; i++)
            {

                SimulationCase dayRecord = new SimulationCase();

                #region day&cycle&DayWithinCycle
                dayRecord.Day = i;
                dayRecord.Cycle = ((i - 1) / ReviewPeriod) + 1;
                dayRecord.DayWithinCycle = ((i - 1) % ReviewPeriod) + 1;


                #endregion

                #region RandomDemand&Demand
                int randomdigitfordemand = rand.Next(1, 101);
                DemandDistribution = Distribution.Run(DemandDistribution);
                dayRecord.RandomDemand = randomdigitfordemand;
                dayRecord.Demand = DetermineDemand(dayRecord.RandomDemand, DemandDistribution);
                #endregion

                #region LeadDays
                dayRecord.RandomLeadDays = rand.Next(1, 101);
                LeadDaysDistribution = Distribution.Run(LeadDaysDistribution);

                if (dayRecord.DayWithinCycle == ReviewPeriod)
                {
                    dayRecord.LeadDays = DetermineLeadDays(dayRecord.RandomLeadDays, LeadDaysDistribution);

                }
                #endregion



                #region ending&shortage

                if (i == 1)
                {
                    dayRecord.ShortageQuantity = 0;
                    dayRecord.BeginningInventory = StartInventoryQuantity;
                    dayRecord.EndingInventory = dayRecord.BeginningInventory - dayRecord.Demand;
                    if (dayRecord.BeginningInventory < dayRecord.Demand)
                    {
                        if (i == 1)
                        {
                            dayRecord.EndingInventory = 0;
                            dayRecord.ShortageQuantity = dayRecord.Demand - dayRecord.BeginningInventory;
                        }

                    }
                }

                if (i == StartLeadDays + 1)
                {

                    dayRecord.BeginningInventory = StartOrderQuantity + SimulationTable[i - 2].EndingInventory;
                    dayRecord.EndingInventory = dayRecord.BeginningInventory - (dayRecord.Demand + SimulationTable[i - 2].ShortageQuantity);
                    if (dayRecord.BeginningInventory - dayRecord.Demand < 0)
                    {
                        dayRecord.ShortageQuantity = dayRecord.Demand - dayRecord.BeginningInventory + SimulationTable[i - 2].ShortageQuantity;
                    }
                }
                else if (i != 1)
                {

                    dayRecord.BeginningInventory = SimulationTable[i - 2].EndingInventory;
                    dayRecord.EndingInventory = dayRecord.BeginningInventory - (dayRecord.Demand + SimulationTable[i - 2].ShortageQuantity);

                    if (dayRecord.EndingInventory < 0)
                    {
                        dayRecord.EndingInventory = 0;
                        dayRecord.ShortageQuantity = SimulationTable[i - 2].ShortageQuantity;
                    }
                    if (dayRecord.BeginningInventory < dayRecord.Demand)
                    {

                        dayRecord.EndingInventory = 0;
                        dayRecord.ShortageQuantity = dayRecord.Demand - dayRecord.BeginningInventory + SimulationTable[i - 2].ShortageQuantity;

                    }
                    if (dayRecord.EndingInventory >= SimulationTable[i - 2].ShortageQuantity && SimulationTable[i - 2].ShortageQuantity != 0)
                    {
                        dayRecord.ShortageQuantity = 0;
                    }
                    if (dayRecord.BeginningInventory - dayRecord.Demand < 0)
                    {
                        dayRecord.ShortageQuantity = dayRecord.Demand - dayRecord.BeginningInventory + SimulationTable[i - 2].ShortageQuantity;
                    }

                }





                #region OrderQuantity

                if (i % ReviewPeriod == 0)
                {
                    dayRecord.OrderQuantity = OrderUpTo - dayRecord.EndingInventory + dayRecord.ShortageQuantity;
                }

                #endregion

                if (i % ReviewPeriod == 0)
                {

                    order = dayRecord.OrderQuantity;
                    time = dayRecord.LeadDays;
                    dayy = dayRecord.Day;


                }

                if (i == dayy + time + 1 && i != 1)
                {
                    endingprev = SimulationTable[i - 2].EndingInventory;
                    dayRecord.BeginningInventory = order + endingprev;
                    dayRecord.EndingInventory = dayRecord.BeginningInventory - (dayRecord.Demand + SimulationTable[i - 2].ShortageQuantity);
                    if (dayRecord.EndingInventory >= SimulationTable[i - 2].ShortageQuantity)
                    {
                        dayRecord.ShortageQuantity = 0;
                    }
                    else if (dayRecord.EndingInventory <= SimulationTable[i - 2].ShortageQuantity)
                    {
                        dayRecord.ShortageQuantity = SimulationTable[i - 2].ShortageQuantity -dayRecord.EndingInventory ;
                    }
                    if (dayRecord.BeginningInventory - dayRecord.Demand >= SimulationTable[i - 2].ShortageQuantity)
                    {
                        dayRecord.ShortageQuantity = 0;
                    }
                    if (dayRecord.BeginningInventory- dayRecord.Demand < 0)
                    {
                        dayRecord.ShortageQuantity = dayRecord.Demand - dayRecord.BeginningInventory + SimulationTable[i - 2].ShortageQuantity;
                    }
                    if(dayRecord.BeginningInventory- dayRecord.Demand < SimulationTable[i - 2].ShortageQuantity)
                    {
                        dayRecord.ShortageQuantity = SimulationTable[i - 2].ShortageQuantity - (dayRecord.BeginningInventory - dayRecord.Demand);
                    }
                    
                }
                
                if(dayRecord.ShortageQuantity < 0)
                {
                    dayRecord.ShortageQuantity = 0;
                }
                if(dayRecord.EndingInventory < 0)
                {
                    dayRecord.EndingInventory = 0;
                }

                #endregion

                SimulationTable.Add(dayRecord);
                #endregion
            }
            #region performacne
            int totalEndingInventory = 0;
            int totalShortageQuantity = 0;

            foreach (var record in SimulationTable)
            {
                totalEndingInventory += record.EndingInventory;
                totalShortageQuantity += record.ShortageQuantity;
            }

            PerformanceMeasures.EndingInventoryAverage = (decimal)totalEndingInventory / NumberOfDays;
            PerformanceMeasures.ShortageQuantityAverage = (decimal)totalShortageQuantity / NumberOfDays;

            #endregion
        }
    }
}