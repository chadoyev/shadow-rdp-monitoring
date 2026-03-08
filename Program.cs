using System;
using System.ServiceProcess;

namespace Microsoft.Windows.Defender.Updates
{
    static class Program
    {
        static void Main(string[] args)
        {
            
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WinDefenderUpdateService()
            };
            ServiceBase.Run(ServicesToRun);
            
        }
    }
}
