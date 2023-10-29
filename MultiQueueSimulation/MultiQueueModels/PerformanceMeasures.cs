using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class PerformanceMeasures
    {
        public decimal AverageWaitingTime { get; set; }
        public int MaxQueueLength { get; set; }
        public decimal WaitingProbability { get; set; }

        # region Caculation Functions

        public decimal AvWaitingTime(int X, int Y)
        {
            decimal res = ((decimal)X / (decimal)Y);
            res = ( Math.Round(res, 2)+0.01M );
            return res;
        }

        public decimal WaitProbability(int X, int Y)
        {
            decimal res = ((decimal)X / (decimal)Y);
            res = (Math.Round(res, 2) + 0.01M);
            return res;
        }

        #endregion
    }
}
