using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class SimulationSystem
    {
        #region Constructor
        public SimulationSystem()
        {
            this.Servers = new List<Server>();
            this.InterarrivalDistribution = new List<TimeDistribution>();
            this.PerformanceMeasures = new PerformanceMeasures();
            this.SimulationTable = new List<SimulationCase>();
        }

        #endregion

        #region bridge of TestCase Read

        List<List<List<float>>> inputdis = new List<List<List<float>>>();

        #endregion

        # region Inputs

        public int NumberOfServers { get; set; }
        public int StoppingNumber { get; set; }
        public List<Server> Servers { get; set; }
        public List<TimeDistribution> InterarrivalDistribution { get; set; }
        public Enums.StoppingCriteria StoppingCriteria { get; set; }
        public Enums.SelectionMethod SelectionMethod { get; set; }

        #endregion

        # region Outputs

        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }

        # endregion

        #region Needed Functions

        public int assignnum(int RandomInterArrival, List<TimeDistribution> table)
        {
            int x = 0;
            foreach (TimeDistribution j in table)
            {
                if (RandomInterArrival >= j.MinRange && RandomInterArrival <= j.MaxRange)
                {
                    x = j.Time;
                    break;
                }
            }
            return x;
        }



        public int[] CalculateMaxWaitingCustomers(List<SimulationCase> SimulationTable)
        {
            int arrival = 0;
            int start = 1;
            int localmax = 0;
            int[] Arr = { 0, 0 };
            foreach (SimulationCase customer in SimulationTable)
            {
                if (customer.TimeInQueue > 0)
                {
                    Arr[1]++; //total customers in queue
                    if (customer.ArrivalTime >= arrival & customer.ArrivalTime < start)
                    {
                        localmax++;
                    }
                    else
                    {
                        Arr[0] = Math.Max(Arr[0], localmax); // MaxQueueLength
                        localmax = 0;
                        arrival = customer.ArrivalTime;
                        start = customer.StartTime;
                    }
                }
                else
                {
                    continue;
                }


            }
            return Arr;
        }


        #endregion

        public void Run(string path)
        {
            #region Fill inputs

            inputdis = Program.Run(path);

            NumberOfServers = Convert.ToInt32(inputdis[0][0][0]);
            StoppingNumber = Convert.ToInt32(inputdis[0][0][1]);
            StoppingCriteria = (Enums.StoppingCriteria)inputdis[0][0][2];
            SelectionMethod = (Enums.SelectionMethod)inputdis[0][0][3];

            InterarrivalDistribution = new List<TimeDistribution>(inputdis[1].Count);

            for (int i = 0; i < inputdis[1].Count; i++)
            {
                var distribution = new TimeDistribution
                {
                    Time = Convert.ToInt32(inputdis[1][i][0]),
                    Probability = (decimal)inputdis[1][i][1]
                };
                InterarrivalDistribution.Add(distribution);
            }

            for (int i = 2; i < inputdis.Count; i++) // Servers
            {
                var server = new Server
                {
                    ID = i - 1,
                    IdleProbability = 0m,
                    AverageServiceTime = 0.0m,
                    Utilization = 0.0m,
                    TimeDistribution = new List<TimeDistribution>()
                };

                Servers.Add(server);

                for (int j = 0; j < inputdis[i].Count; j++) // Rows of Time Distribution
                {
                    var distribution = new TimeDistribution
                    {
                        Time = Convert.ToInt32(inputdis[i][j][0]),
                        Probability = (decimal)(inputdis[i][j][1])
                    };

                    Servers[i - 2].TimeDistribution.Add(distribution);
                }
            }

#endregion

            #region get started

            var random = new Random();
            int clock_time_of_arrival = 0;

            List<int> endtimeofserve = new List<int>();
            for (int i = 0; i < NumberOfServers; i++)
            {
                endtimeofserve.Add(0);
            }

            InterarrivalDistribution = TimeDistribution.Run(InterarrivalDistribution);
            foreach (Server x in Servers)
            {
                x.TimeDistribution = (TimeDistribution.Run(x.TimeDistribution));
            }

            SimulationCase customer = new SimulationCase();

            List<int> totalwait = new List<int>();

            int StopingFlag = 0;
            if (StoppingCriteria == Enums.StoppingCriteria.SimulationEndTime)
            {
                StopingFlag = 1;
            }

            #endregion

            # region SimulationTable raws

            int cus_num = 0;

            for (cus_num = 1; cus_num <= StoppingNumber; cus_num++)
            {
                customer = new SimulationCase();
                bool flag = false;

                //first column
                customer.CustomerNumber = cus_num;

                //second column
                customer.RandomInterArrival = random.Next(1, 101);

                customer.InterArrival = 0;

                if (cus_num > 1)
                {
                    //third column
                    customer.InterArrival = assignnum(customer.RandomInterArrival, InterarrivalDistribution);
                    clock_time_of_arrival += customer.InterArrival;
                }


                //fourth column
                customer.ArrivalTime = clock_time_of_arrival;

                //fifth column
                customer.RandomService = random.Next(1, 101);

                //customer.StartTime = customer.ArrivalTime;

                switch (SelectionMethod)
                {
                    #region HighestPriority

                    case Enums.SelectionMethod.HighestPriority:
                        {
                            int server = 0;
                            int nxt_choosenserver = 0;

                            while (server < NumberOfServers)
                            {
                                if (customer.ArrivalTime >= endtimeofserve[server])
                                {
                                    //Eighth column
                                    customer.StartTime = customer.ArrivalTime;
                                    //sixth column
                                    customer.ServiceTime = assignnum(customer.RandomService, Servers[server].TimeDistribution);
                                    endtimeofserve[server] = customer.StartTime + customer.ServiceTime;
                                    //ninth column
                                    customer.EndTime = endtimeofserve[server];
                                    //Tenth column
                                    customer.TimeInQueue = 0;
                                    Servers[server].TotalWorkingTime += customer.ServiceTime;
                                    //seventh column
                                    customer.AssignedServer = Servers[server];

                                    flag = true;
                                    break;

                                }
                                server++;
                            }

                            if (flag == false && server == 2)
                            {
                                //simulation queue

                                //Eighth column
                                int start = endtimeofserve.Min();
                                customer.StartTime = start;
                                nxt_choosenserver = endtimeofserve.IndexOf(customer.StartTime);
                                //sixth column
                                customer.ServiceTime = assignnum(customer.RandomService, Servers[nxt_choosenserver].TimeDistribution);
                                endtimeofserve[nxt_choosenserver] = customer.StartTime + customer.ServiceTime;
                                //ninth column
                                customer.EndTime = endtimeofserve[nxt_choosenserver];
                                //Tenth column
                                customer.TimeInQueue = (customer.StartTime - customer.ArrivalTime);
                                totalwait.Add(customer.TimeInQueue);
                                Servers[nxt_choosenserver].TotalWorkingTime += customer.ServiceTime;
                                //seventh column
                                customer.AssignedServer = Servers[nxt_choosenserver];
                            }

                            if (customer.EndTime > StoppingNumber && StopingFlag == 1)
                            {
                                goto passsystemtime;
                            }

                            break;
                        }

                    #endregion

                    # region Random

                    case Enums.SelectionMethod.Random:
                        {
                            List<int> availableServers = new List<int>();
                            int server = 0;
                            while (server < NumberOfServers)
                            {
                                if (customer.ArrivalTime >= endtimeofserve[server])
                                {
                                    availableServers.Add(server);
                                }
                                server++;
                            }

                            if (availableServers.Count > 0)
                            {
                                //random number of avalible servers
                                while (true)
                                {
                                    try
                                    {
                                        server = availableServers[random.Next(0, availableServers.Count)];
                                    }

                                    catch
                                    {
                                        continue;
                                    }

                                    break;
                                }

                                //Eighth column
                                customer.StartTime = customer.ArrivalTime;
                                //sixth column
                                customer.ServiceTime = assignnum(customer.RandomService, Servers[server].TimeDistribution);
                                endtimeofserve[server] = customer.StartTime + customer.ServiceTime;
                                //ninth column
                                customer.EndTime = endtimeofserve[server];
                                //Tenth column
                                customer.TimeInQueue = (customer.StartTime - customer.ArrivalTime);
                                Servers[server].TotalWorkingTime += customer.ServiceTime;
                                //seventh column
                                customer.AssignedServer = Servers[server];
                            }
                            else
                            {
                                //simulation queue

                                //Eighth column
                                int start = endtimeofserve.Min();
                                customer.StartTime = start;
                                int nxt_choosenserver = endtimeofserve.IndexOf(customer.StartTime);
                                //sixth column
                                customer.ServiceTime = assignnum(customer.RandomService, Servers[nxt_choosenserver].TimeDistribution);
                                endtimeofserve[nxt_choosenserver] = customer.StartTime + customer.ServiceTime;
                                //ninth column
                                customer.EndTime = endtimeofserve[nxt_choosenserver];
                                //Tenth column
                                customer.TimeInQueue = (customer.StartTime - customer.ArrivalTime);

                                Servers[nxt_choosenserver].TotalWorkingTime += customer.ServiceTime;
                                //seventh column
                                customer.AssignedServer = Servers[nxt_choosenserver];
                            }

                            if (StopingFlag == 1)
                            {
                                if (customer.EndTime > StoppingNumber)
                                {
                                    goto passsystemtime;
                                }
                            }

                            break;
                        }

                    #endregion

                    # region LeastUtilization

                    case Enums.SelectionMethod.LeastUtilization:
                    {
                        List<Server> availableServers = new List<Server>();
                        int server = 0;
                        while (server < NumberOfServers)
                        {
                            if (customer.ArrivalTime >= endtimeofserve[server])
                            {
                                availableServers.Add(Servers[server]);
                            }
                            server++;
                        }

                        if (availableServers.Count > 0)
                        {
                                //least utilization of avalible servers
                                availableServers.Sort((x,y) => x.TotalWorkingTime.CompareTo(y.TotalWorkingTime));
                                server = (availableServers[0].ID)-1;
                                //Eighth column
                                customer.StartTime = customer.ArrivalTime;
                                //sixth column
                                customer.ServiceTime = assignnum(customer.RandomService, Servers[server].TimeDistribution);
                                endtimeofserve[server] = customer.StartTime + customer.ServiceTime;
                                //ninth column
                                customer.EndTime = endtimeofserve[server];
                                //Tenth column
                                customer.TimeInQueue = (customer.StartTime - customer.ArrivalTime);
                                Servers[server].TotalWorkingTime += customer.ServiceTime;
                                //seventh column
                                customer.AssignedServer = Servers[server];
                                
                        }
                        else
                        {
                                //simulation queue

                                //Eighth column
                                int start = endtimeofserve.Min();
                                customer.StartTime = start;
                                int nxt_choosenserver = endtimeofserve.IndexOf(customer.StartTime);
                                //sixth column
                                customer.ServiceTime = assignnum(customer.RandomService, Servers[nxt_choosenserver].TimeDistribution);
                                endtimeofserve[nxt_choosenserver] = customer.StartTime + customer.ServiceTime;
                                //ninth column
                                customer.EndTime = endtimeofserve[nxt_choosenserver];
                                //Tenth column
                                customer.TimeInQueue = (customer.StartTime - customer.ArrivalTime);

                                Servers[nxt_choosenserver].TotalWorkingTime += customer.ServiceTime;
                                //seventh column
                                customer.AssignedServer = Servers[nxt_choosenserver];
                        }

                        if (StopingFlag == 1)
                        {
                            if (customer.EndTime > StoppingNumber)
                            {
                                goto passsystemtime;
                            }
                        }

                        break;
                    }

                    #endregion
                }

                SimulationTable.Add(customer);

            passsystemtime:
                if (StopingFlag == 1)
                {
                    break;
                }
            }

            #endregion

            # region Server Calculations

            foreach (Server serv in Servers)
            {
                int idle = (customer.EndTime - serv.TotalWorkingTime);
                serv.IdleProbability = serv.IdleProb(idle, customer.EndTime);
                serv.AverageServiceTime = serv.AvServiceTime(serv.TotalWorkingTime, SimulationTable.Count);
                serv.Utilization = serv.Utili(serv.TotalWorkingTime, customer.EndTime);

            }

            #endregion

            # region PerformanceMeasures Calculations

            int[] ret = CalculateMaxWaitingCustomers(SimulationTable);
            PerformanceMeasures.MaxQueueLength = ret[0];
            int sum = totalwait.Sum();
            PerformanceMeasures.AverageWaitingTime = PerformanceMeasures.AvWaitingTime(sum, cus_num);
            int count = totalwait.Count();
            PerformanceMeasures.WaitingProbability = PerformanceMeasures.WaitProbability(count, cus_num);

            # endregion

        }

    }
}
