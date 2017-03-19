using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OpenTKExtension
{
    public class PerformanceCheck
    {
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;

        public PerformanceCheck()
        {
         

            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        }
        public double CpuUsage()
        {
            return cpuCounter.NextValue();
        }
    }
}
