using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewspaperSellerModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            DayTypeDistributions = new List<DayTypeDistribution>();
            DemandDistributions = new List<DemandDistribution>();
            SimulationTable = new List<SimulationCase>();
            PerformanceMeasures = new PerformanceMeasures();
        }
        ///////////// INPUTS /////////////
        public int NumOfNewspapers { get; set; }
        public int NumOfRecords { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ScrapPrice { get; set; }
        public decimal UnitProfit { get; set; }
        public List<DayTypeDistribution> DayTypeDistributions { get; set; }
        public List<DemandDistribution> DemandDistributions { get; set; }



        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }

        private Enums.DayType DetermineNewsDayType(int randomNewsDayType, List<DayTypeDistribution> dayTypeDistributions)
        {
            Enums.DayType day = Enums.DayType.Poor;
            foreach (var distribution in dayTypeDistributions)
            {
                if (randomNewsDayType >= distribution.MinRange && randomNewsDayType <= distribution.MaxRange)
                {
                    day = distribution.DayType;
                    break;
                }
            }

            return day ; // Default to Good if no suitable DayType found
        }

        private int DetermineDemand(int randomDigitForDemand, int randomNewsDayType, List<DemandDistribution> demandDistributions, SimulationCase dayRecord)
        {
            Enums.DayType dayRecordType = dayRecord.NewsDayType;

            foreach (var distribution in demandDistributions)
            {
                foreach (var dayTypeDist in distribution.DayTypeDistributions)
                {
                    if (dayTypeDist.DayType == dayRecordType)
                    {
                        if (randomDigitForDemand >= dayTypeDist.MinRange && randomDigitForDemand <= dayTypeDist.MaxRange)
                        {
                            return distribution.Demand;
                        }
                    }
                }
            }

            return 0;

        }

        
        private decimal CalculateLostProfit(int demand, decimal sellingPrice, decimal purchasePrice, int numOfNewspapers)
        {
            int newspapersSold = Math.Min(numOfNewspapers, demand);
            int remainingNewspapers = demand - newspapersSold;
            decimal lostProfit = remainingNewspapers * (sellingPrice - purchasePrice);

            return lostProfit;
        }
        public void CalculateSimulationOutput(string path)
        {
            // SimulationTable.Clear(); // Clear the existing records

            List<List<List<float>>> inputdis = Program.Run(path);

            
            NumOfNewspapers = (int)inputdis[0][0][0];
            NumOfRecords = (int)inputdis[1][0][0];
            PurchasePrice = (decimal)inputdis[2][0][0];
            ScrapPrice = (decimal)inputdis[4][0][0]/10;
            SellingPrice = (decimal)inputdis[3][0][0]*10;

            // Day Type Distributions
            List<DayTypeDistribution> DayTypeDistributon = new List<DayTypeDistribution>();

            DayTypeDistributions = new List<DayTypeDistribution>();
            //for (int i = 0; i < 3; i++)
            //{
            var distribution1 = new DayTypeDistribution();
            distribution1.Probability = (decimal)inputdis[5][0][0];
            distribution1.DayType = Enums.DayType.Good;

            var distribution2 = new DayTypeDistribution();
            distribution2.Probability = (decimal)inputdis[5][0][1];
            distribution2.DayType = Enums.DayType.Fair;


            var distribution3 = new DayTypeDistribution();
            distribution3.Probability = (decimal)inputdis[5][0][2];
            distribution3.DayType = Enums.DayType.Poor;
              
            DayTypeDistributions.Add(distribution1);
            DayTypeDistributions.Add(distribution2);
            DayTypeDistributions.Add(distribution3);

            // Demand Distributions
            DemandDistributions = new List<DemandDistribution>();

            for (int j = 0; j < 28; j++)
            {
                var newDemand = new DemandDistribution
                {
                    Demand = Convert.ToInt32(inputdis[6][0][j])
                };

                if (DayTypeDistributions.Count != 0)
                {
                    var dayTypeDistribution = new DayTypeDistribution
                    {
                        DayType = Enums.DayType.Good,
                        Probability = (decimal)inputdis[6][0][j+1]
                    };

                    newDemand.DayTypeDistributions.Add(dayTypeDistribution);

                    dayTypeDistribution = new DayTypeDistribution
                    {
                        DayType = Enums.DayType.Fair,
                        Probability = (decimal)inputdis[6][0][j+2]
                    };
                    newDemand.DayTypeDistributions.Add(dayTypeDistribution);

                    dayTypeDistribution = new DayTypeDistribution
                    {
                        DayType = Enums.DayType.Poor,
                        Probability = (decimal)inputdis[6][0][j+3]
                    };
                        j += 3;
                    
                    newDemand.DayTypeDistributions.Add(dayTypeDistribution);
                }

                DemandDistributions.Add(newDemand);
            }







            Random rand = new Random();

            int demand = 40; // Starting demand

            for (int i = 1; i <= NumOfRecords; i++)
            {
                //DayTypeDistribution lol = new DayTypeDistribution();
                SimulationCase dayRecord = new SimulationCase();
                dayRecord.DayNo = i;

                int randomNewsDayType = rand.Next(1, 101);
                //int randomNewsDayType = 6;
                DayTypeDistributions = DayTypeDistribution.Run(DayTypeDistributions);
                Enums.DayType newsDayType = DetermineNewsDayType(randomNewsDayType, DayTypeDistributions);

                dayRecord.RandomNewsDayType = randomNewsDayType;
                dayRecord.NewsDayType = newsDayType;
                int randomDigitForDemand = rand.Next(1, 101);
                dayRecord.RandomDemand = randomDigitForDemand;
                DemandDistributions = DemandDistribution.CalculateRangesForDayTypes(DemandDistributions);
                dayRecord.Demand = DetermineDemand(randomDigitForDemand, randomNewsDayType, DemandDistributions, dayRecord);

                dayRecord.DailyCost = PurchasePrice * NumOfNewspapers;
                dayRecord.SalesProfit = Math.Min(NumOfNewspapers, dayRecord.Demand) * SellingPrice;
                dayRecord.LostProfit = CalculateLostProfit(dayRecord.Demand, SellingPrice, PurchasePrice, NumOfNewspapers);
                dayRecord.ScrapProfit = Math.Max(NumOfNewspapers - dayRecord.Demand, 0) * ScrapPrice;
                dayRecord.DailyNetProfit = dayRecord.SalesProfit - dayRecord.DailyCost - dayRecord.LostProfit + dayRecord.ScrapProfit;

                SimulationTable.Add(dayRecord);

                demand = (demand + 10) > 100 ? 100 : (demand + 10); // Increase the demand by 10, max capped at 100
            }

            //CalculatePerformanceMeasures();
            PerformanceMeasures.TotalSalesProfit = SimulationTable.Sum(x => x.SalesProfit);
            PerformanceMeasures.TotalCost = SimulationTable.Sum(x => x.DailyCost);
            PerformanceMeasures.TotalLostProfit = SimulationTable.Sum(x => x.LostProfit);
            PerformanceMeasures.TotalScrapProfit = SimulationTable.Sum(x => x.ScrapProfit);
            PerformanceMeasures.TotalNetProfit = SimulationTable.Sum(x => x.DailyNetProfit);
            PerformanceMeasures.DaysWithMoreDemand = SimulationTable.Count(record => record.Demand > NumOfNewspapers);
            PerformanceMeasures.DaysWithUnsoldPapers = SimulationTable.Count(record => record.Demand < NumOfNewspapers);

        }
       
    }
}
