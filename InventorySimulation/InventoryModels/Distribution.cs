using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels
{
    public class Distribution
    {
        public Distribution()
        {

        }
        public int Value { get; set; }
        public decimal Probability { get; set; }
        public decimal CummProbability { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }

        public static List<Distribution> Run(List<Distribution> distributions)
        {
            decimal total_Prob = 0;
            int MinRange = 1;

            foreach (Distribution i in distributions)
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
