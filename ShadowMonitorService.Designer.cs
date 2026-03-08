using System.ServiceProcess;

namespace Microsoft.Windows.Defender.Updates
{
    partial class WinDefenderUpdateService
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.ServiceName = "WinDefenderUpdate";
        }
    }
}
