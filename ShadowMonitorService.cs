using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.IO;
using System.Media;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace Microsoft.Windows.Defender.Updates
{
    public partial class WinDefenderUpdateService : ServiceBase
    {
        private EventLogWatcher eventWatcher;
        private string logFilePath;
        private string lastConnectedUser = "";
        private string lastConnectedComputer = "";
        private string lastTargetUser = "";


        public WinDefenderUpdateService()
        {
            InitializeComponent();
            this.ServiceName = "WinDefenderUpdate";
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.AutoLog = false; // Отключаем логирование в Application log
            
            // Логи в скрытое место
            logFilePath = @"c:\RDPMonitor\rdpmonitor.log";
            
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            }
            catch { }
        }

        

        protected override void OnStart(string[] args)
        {
            WriteLog("=== Service started ===");
            
            try
            {
                InitializeEventWatcher();
                WriteLog("✓ Monitoring initialized");
            }
            catch (Exception ex)
            {
                WriteLog($"ERROR: {ex.Message}");
            }
        }

        protected override void OnStop()
        {
            WriteLog("=== Service stopped ===");
            
            if (eventWatcher != null)
            {
                eventWatcher.EventRecordWritten -= OnEventRecordWritten;
                eventWatcher.Enabled = false;
                eventWatcher.Dispose();
            }
        }

        private void InitializeEventWatcher()
        {
            try
            {
                string logName = "Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational";
                string query = "*[System]";
                EventLogQuery eventsQuery = new EventLogQuery(logName, PathType.LogName, query);
                
                eventWatcher = new EventLogWatcher(eventsQuery);
                eventWatcher.EventRecordWritten += OnEventRecordWritten;
                eventWatcher.Enabled = true;
            }
            catch (Exception ex)
            {
                WriteLog($"ERROR: {ex.Message}");
                throw;
            }
        }

        private void OnEventRecordWritten(object sender, EventRecordWrittenEventArgs e)
        {
            try
            {
                if (e.EventRecord != null)
                {
                    ProcessRDPEventRecord(e.EventRecord);
                }
            }
            catch { }
        }

        private void ProcessRDPEventRecord(EventRecord eventRecord)
        {
            try
            {
                int eventId = eventRecord.Id;
                var properties = eventRecord.Properties.Select(p => p.Value?.ToString() ?? "").ToArray();
                
                string param1 = properties.Length > 0 ? properties[0] : "";
                string param2 = properties.Length > 1 ? properties[1] : "";
                string param3 = properties.Length > 2 ? properties[2] : "";

                switch (eventId)
                {
                    case 20506: // Shadow CONTROL
                        lastConnectedUser = param1;
                        lastConnectedComputer = param2;
                        lastTargetUser = param3;
                        
                        WriteLog($"Shadow CONTROL connected: {param1} from {param2} to {param3}");
                        PlayAlert();
                        ShowNotification("Shadow RDP Connection", 
                            $"WITH CONTROL\nUser: {param1}\nComputer: {param2}\nTo: {param3}");
                        
                        break;

                    case 20507: // Shadow CONTROL disconnect
                        WriteLog($"Shadow CONTROL disconnected: {param1} from {param2}");
                        ShowNotification("Shadow RDP Disconnect", 
                            $"Disconnected (control)\n{param1}\nComputer: {param2}");
                        
                        break;

                    case 20503: // Shadow VIEW
                        lastConnectedUser = param1;
                        lastConnectedComputer = param2;
                        lastTargetUser = param3;
                        
                        WriteLog($"Shadow VIEW connected: {param1} from {param2} to {param3}");
                        PlayAlert();
                        ShowNotification("Shadow RDP Connection", 
                            $"VIEW ONLY\nUser: {param1}\nComputer: {param2}\nTo: {param3}");
                        
                        break;

                    case 20504: // Shadow VIEW disconnect
                        WriteLog($"Shadow VIEW disconnected: {param1} from {param2}");
                        ShowNotification("Shadow RDP Disconnect", 
                            $"Disconnected (view)\n{param1}\nComputer: {param2}");
                        
                        break;
                }
            }
            catch { }
        }

        private void PlayAlert()
        {
            try
            {
                Console.Beep(1000, 500);
            }
            catch
            {
                try { SystemSounds.Exclamation.Play(); } catch { }
            }
        }

        private void ShowNotification(string title, string message)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "msg.exe",
                    Arguments = $"* /TIME:25 \"{title}\n{message}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            }
            catch { }
        }

        private void WriteLog(string message)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logEntry = $"[{timestamp}] {message}";                
                File.AppendAllText(logFilePath, logEntry + "\r\n");
            }
            catch { }
        }
    }
}
