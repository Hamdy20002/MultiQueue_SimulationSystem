using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewspaperSellerModels
{
    public class DemandDistribution
    {
        public DemandDistribution()
        {
            DayTypeDistributions = new List<DayTypeDistribution>();
        }
        public int Demand { get; set; }
        public List<DayTypeDistribution> DayTypeDistributions { get; set; }

        public static List<DemandDistribution> CalculateRangesForDayTypes(List<DemandDistribution> demandDistributions)
        {

            int MinRangeG = 1;
            int MinRangeP = 1;
            int MinRangeF = 1;
            
            decimal total_ProbG = 0;
            decimal total_ProbP = 0;
            decimal total_ProbF = 0;
            foreach (var demandDistribution in demandDistributions)
            {
                
                foreach (var dayTypeDist in demandDistribution.DayTypeDistributions)
                {

                   
                    //decimal currentRange = dayTypeDist.CummProbability * 100;

                    if (dayTypeDist.DayType == Enums.DayType.Good)
                    {

                        dayTypeDist.CummProbability = total_ProbG + dayTypeDist.Probability;
                        total_ProbG = dayTypeDist.CummProbability;
                        dayTypeDist.MaxRange = Convert.ToInt32((dayTypeDist.CummProbability * 100));
                        dayTypeDist.MinRange = MinRangeG;

                        MinRangeG = dayTypeDist.MaxRange + 1;
                    }
                    else if (dayTypeDist.DayType == Enums.DayType.Fair)
                    {
                        dayTypeDist.CummProbability = total_ProbF + dayTypeDist.Probability;
                        total_ProbF = dayTypeDist.CummProbability;
                        if (dayTypeDist.CummProbability == 0)
                        {
                            dayTypeDist.MaxRange = 0;
                            dayTypeDist.MinRange = 0;
                        }
                        else
                        {
                            dayTypeDist.MaxRange = Convert.ToInt32((dayTypeDist.CummProbability * 100));
                            dayTypeDist.MinRange = MinRangeF;

                            MinRangeF = dayTypeDist.MaxRange + 1;
                        }

                    }
                    else if (dayTypeDist.DayType == Enums.DayType.Poor)
                    {
                        dayTypeDist.CummProbability = total_ProbP + dayTypeDist.Probability;
                        total_ProbP = dayTypeDist.CummProbability;
                        if (dayTypeDist.CummProbability == 0)
                        {
                            dayTypeDist.MaxRange = 0;
                            dayTypeDist.MinRange = 0;
                        }
                        else
                        {
                            dayTypeDist.MaxRange = Convert.ToInt32((dayTypeDist.CummProbability * 100));
                            dayTypeDist.MinRange = MinRangeP;

                            MinRangeP = dayTypeDist.MaxRange + 1;
                        }
                    }
                }
            }
            return demandDistributions;
        }


    }
}

