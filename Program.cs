using System;
using System.ServiceProcess;

namespace Microsoft.Windows.Defender.Updates
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive || (args.Length > 0 && args[0].ToLower() == "/console"))
            {
                Console.WriteLine("Windows Defender Update Service - Debug Mode");
                Console.WriteLine("Press Enter to stop\n");
                
                var service = new WinDefenderUpdateService();
                service.StartConsoleMode();
                
                Console.ReadLine();
                
                service.StopConsoleMode();
            }
            else
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
}
