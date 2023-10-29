using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class TimeDistribution
    {
        public int Time { get; set; }
        public decimal Probability { get; set; }
        public decimal CummProbability { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }

        public static List<TimeDistribution> Run(List<TimeDistribution> distributions)
        {
            decimal total_Prob = 0;
            int MinRange = 1;

            foreach (TimeDistribution i in distributions)
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
