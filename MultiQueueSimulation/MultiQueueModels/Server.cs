using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class Server
    {
        public Server()
        {
            this.TimeDistribution = new List<TimeDistribution>();
        }

        public int ID { get; set; }
        public decimal IdleProbability { get; set; }
        public decimal AverageServiceTime { get; set; }
        public decimal Utilization { get; set; }

        public List<TimeDistribution> TimeDistribution;

        public int NoCustomer { get; set; }

        //optional if needed use them
        public int FinishTime { get; set; }
        public int TotalWorkingTime { get; set; }

        # region Needed Functions

        public decimal IdleProb(decimal totalidletime, decimal totalrun)
        {
            return (decimal)(totalidletime / totalrun);
        }
        public decimal AvServiceTime(decimal totalservicetime, decimal cusnumber)
        {
            try
            {
                return (decimal)(totalservicetime / cusnumber);
            }
            catch
            {
                return 0;
            }
            
        }

        public decimal Utili(decimal totalservicetime, decimal totalruntime)
        {
            return (decimal)(totalservicetime / totalruntime);
        }

        # endregion
    }
}