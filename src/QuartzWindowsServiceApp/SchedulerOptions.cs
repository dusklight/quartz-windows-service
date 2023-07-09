using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzWindowsServiceApp;

public class SchedulerOptions
{
    public int MaxConcurrency { get; set; }

    public int HelloWorldJobIntervalInSeconds { get; set; }

}
