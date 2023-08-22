using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkerTemplate.SharedKernel.Common.Entities
{
    public class WorkerSchedule
    {
        public WorkerSchedule() { }
        public WorkerSchedule(bool enabled, int workerFrequencyInMinutes)
        {
            Enabled = enabled;
            WorkerFrequencyInMinutes = workerFrequencyInMinutes;
            Monday = new int[24];
            Tuesday = new int[24];
            Wednesday = new int[24];
            Thursday = new int[24];
            Friday = new int[24];
            SaturDay = new int[24];
            Sunday = new int[24];
        }

        public bool Enabled { get; set; }
        public int WorkerFrequencyInMinutes { get; set; }
        public int[]? Monday { get; set; }
        public int[]? Tuesday { get; set; }
        public int[]? Wednesday { get; set; }
        public int[]? Thursday { get; set; }
        public int[]? Friday { get; set; }
        public int[]? SaturDay { get; set; }
        public int[]? Sunday { get; set; }
    }
}