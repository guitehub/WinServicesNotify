using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.ServiceProcess;

namespace WinServicesNotify
{
    public partial class ServicesNotify : Form
    {
        public ServicesNotify()
        {
            InitializeComponent();

            this.listServices();
        }

        private void formShow(object sender, EventArgs e)
        {
            this.Show();
        }

        private void formHide(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Tricks to ensure to "minimized" the window in the icon tray not the taskbar
        /// </summary>
        private void ServicesNotify_Move(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Append the list of services in the ToolStripMenuItem 
        /// with the right control (start/stop/restart) for each one and an icon indicator of current status
        /// </summary>
        private void listServices()
        {
            string pcName = Environment.MachineName.ToString();

            //public static System.ServiceProcess.ServiceController[] GetServices(string machineName);
            ServiceController[] scServices = ServiceController.GetServices(pcName);

            foreach (ServiceController scTemp in scServices)
            {
                ToolStripMenuItem tmpTSMI = new MyServiceMenuItem(scTemp);

                this.servicesMenuStrip.Items.Add(tmpTSMI);
            }
        }

        private void clearMenu()
        {
            // TODO
        }
    }
}
