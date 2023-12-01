using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewspaperSellerModels
{
    public class DayTypeDistribution
    {
        public Enums.DayType DayType { get; set; }
        public decimal Probability { get; set; }
        public decimal CummProbability { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }

        public static List<DayTypeDistribution> Run(List<DayTypeDistribution> distributions)
        {
            decimal total_Prob = 0;
            int MinRange = 1;

            foreach (DayTypeDistribution i in distributions)
            {
                i.CummProbability = total_Prob + i.Probability;
                total_Prob = i.CummProbability;

                i.MaxRange = Convert.ToInt32((i.CummProbability * 100));
                i.MinRange = MinRange;

                MinRange = i.MaxRange + 1;
            }

            return distributions;
        }
    }
    


}
