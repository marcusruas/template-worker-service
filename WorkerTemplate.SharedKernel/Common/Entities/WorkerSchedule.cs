using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkerTemplate.SharedKernel.Common.Entities
{
    public class WorkerSchedule
    {
        public bool Enabled { get; set; }
        public int WorkerFrequencyInSeconds { get; set; }
        public int[]? Monday { get; set; }
        public int[]? Tuesday { get; set; }
        public int[]? Wednesday { get; set; }
        public int[]? Thursday { get; set; }
        public int[]? Friday { get; set; }
        public int[]? SaturDay { get; set; }
        public int[]? Sunday { get; set; }
    }
}