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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServicesNotify_Move(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        //private void menuShow(object sender, EventArgs e)
        //{
        //    servicesMenuStrip.Show();
        //}

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //private void BalloonTest(object sender, EventArgs e)
        //{
        //    notifyIcon1.ShowBalloonTip(42000, "Test", "ouais", ToolTipIcon.Info);
        //}

        /// <summary>
        /// Append the list of services in the ToolStripMenuItem 
        /// with the right control (start/stop/restart) for each one ans a icon indicator of current status
        /// </summary>
        private void listServices()
        {
            string pcName = Environment.MachineName.ToString();

            //public static System.ServiceProcess.ServiceController[] GetServices(string machineName);
            ServiceController[] scServices = ServiceController.GetServices(pcName);

            foreach (ServiceController scTemp in scServices)
            {
                ToolStripMenuItem tmpTSMI = new System.Windows.Forms.ToolStripMenuItem();

                tmpTSMI.Text = scTemp.ServiceName.ToString();
                tmpTSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { });

                if (scTemp.Status.Equals(ServiceControllerStatus.Stopped))
                {
                    ToolStripItem startItem = new MyServiceCtrlItem(scTemp);

                    startItem.Text = "Start";
                    startItem.Click += new System.EventHandler(this.scStarter);

                    tmpTSMI.Image = global::WinServicesNotify.Properties.Resources.rdRed;
                    tmpTSMI.DropDownItems.Add(startItem);
                }
                if (scTemp.Status.Equals(ServiceControllerStatus.Running))
                {
                    ToolStripItem stopItem = new MyServiceCtrlItem(scTemp);
                    ToolStripItem restartItem = new MyServiceCtrlItem(scTemp);

                    tmpTSMI.Image = global::WinServicesNotify.Properties.Resources.rdGreen;

                    if (scTemp.CanStop)
                    {
                        stopItem.Text = "Stop";
                        stopItem.Click += new System.EventHandler(this.scStoper);

                        restartItem.Text = "Restart";
                        restartItem.Click += new System.EventHandler(this.scRestarter);

                    }
                    else
                    {
                        tmpTSMI.ForeColor = System.Drawing.SystemColors.GrayText;

                        stopItem.Text = "Can't Stop";
                        stopItem.Image = global::WinServicesNotify.Properties.Resources._lock;
                        stopItem.ForeColor = System.Drawing.SystemColors.GrayText;

                        restartItem.Text = "Can't restart";
                        restartItem.Image = global::WinServicesNotify.Properties.Resources._lock;
                        restartItem.ForeColor = System.Drawing.SystemColors.GrayText;
                    }

                    tmpTSMI.DropDownItems.Add(stopItem);
                    tmpTSMI.DropDownItems.Add(restartItem);
                }

                this.servicesMenuStrip.Items.Add(tmpTSMI);
            }
        }

        /// <summary>
        /// Handle the starting of the passed service and notify it via Balloon Tip
        /// </summary>
        /// <param name="sender">ServiceController</param>
        /// <param name="e"></param>
        private void scStarter(object sender, EventArgs e)
        {
            MyServiceCtrlItem msci = sender as MyServiceCtrlItem;

            try
            {
                msci.Sc.Start();
                msci.Sc.WaitForStatus(ServiceControllerStatus.Running);
                notifyIcon1.ShowBalloonTip(4200, "Service started", msci.Sc.DisplayName.ToString() + " started", ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(4200, "Service error", msci.Sc.DisplayName.ToString() + " can't start", ToolTipIcon.Error);

                throw ex;
            }
        }

        /// <summary>
        /// Handle the stopping of the passed service and notify it via Balloon Tip
        /// </summary>
        /// <param name="sender">ServiceController</param>
        /// <param name="e"></param>
        private void scStoper(object sender, EventArgs e)
        {
            MyServiceCtrlItem msci = sender as MyServiceCtrlItem;

            try
            {
                msci.Sc.Stop();
                msci.Sc.WaitForStatus(ServiceControllerStatus.Stopped);
                notifyIcon1.ShowBalloonTip(4200, "Service stopped", msci.Sc.DisplayName.ToString() + " stopped", ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(4200, "Service error", msci.Sc.DisplayName.ToString() + " can't stop", ToolTipIcon.Error);

                throw ex;
            }
        }

        /// <summary>
        /// Handle the restarting of the passed service and notify it via Balloon Tip
        /// </summary>
        /// <param name="sender">ServiceController</param>
        /// <param name="e"></param>
        private void scRestarter(object sender, EventArgs e)
        {

            MyServiceCtrlItem msci = sender as MyServiceCtrlItem;

            try
            {
                msci.Sc.Stop();
                notifyIcon1.ShowBalloonTip(4200, "Service stopped", msci.Sc.ServiceName.ToString() + " stopped", ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(4200, "Service error", msci.Sc.DisplayName.ToString() + " can't stop", ToolTipIcon.Error);

                throw ex;
            }

            msci.Sc.WaitForStatus(ServiceControllerStatus.Stopped);

            try
            {
                msci.Sc.Start();
                notifyIcon1.ShowBalloonTip(4200, "Service started", msci.Sc.DisplayName.ToString() + " restarted", ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(4200, "Service error", msci.Sc.DisplayName.ToString() + " can't start", ToolTipIcon.Error);

                throw ex;
            }
        }

        private void clearMenu()
        {
            // TODO
        }
    }
}
