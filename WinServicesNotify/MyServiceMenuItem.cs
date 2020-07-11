using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.ServiceProcess;

namespace WinServicesNotify
{
    public partial class MyServiceMenuItem : ToolStripMenuItem
    {
        private ServiceController _sc;

        public ServiceController Sc
        {
            get { return _sc; }
            set { _sc = value; }
        }

        public MyServiceMenuItem()
        {
            InitializeComponent();
        }

        public MyServiceMenuItem(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public MyServiceMenuItem(ServiceController sc)
        {
            InitializeComponent();

            this.Sc = sc;
            this.Text = this.Sc.ServiceName.ToString();
            this.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { });
        }

        private void buildMenu()
        {
            if (this.Sc.Status.Equals(ServiceControllerStatus.Stopped))
            {
                ToolStripItem startItem = new MyServiceCtrlItem(this.Sc);

                startItem.Text = "Start";
                startItem.Click += new System.EventHandler(this.ServiceCtrlHandlerStarter);

                this.Image = global::WinServicesNotify.Properties.Resources.rdRed;
                this.DropDownItems.Add(startItem);
            }
            if (this.Sc.Status.Equals(ServiceControllerStatus.Running))
            {
                ToolStripItem stopItem = new MyServiceCtrlItem(this.Sc);
                ToolStripItem restartItem = new MyServiceCtrlItem(this.Sc);

                this.Image = global::WinServicesNotify.Properties.Resources.rdGreen;

                if (this.Sc.CanStop)
                {
                    stopItem.Text = "Stop";
                    stopItem.Click += new System.EventHandler(this.ServiceCtrlHandlerStopper);

                    restartItem.Text = "Restart";
                    restartItem.Click += new System.EventHandler(this.ServiceCtrlHandlerRestart);

                }
                else
                {
                    this.ForeColor = System.Drawing.SystemColors.GrayText;

                    stopItem.Text = "Can't Stop";
                    stopItem.Image = global::WinServicesNotify.Properties.Resources._lock;
                    stopItem.ForeColor = System.Drawing.SystemColors.GrayText;

                    restartItem.Text = "Can't restart";
                    restartItem.Image = global::WinServicesNotify.Properties.Resources._lock;
                    restartItem.ForeColor = System.Drawing.SystemColors.GrayText;
                }

                this.DropDownItems.Add(stopItem);
                this.DropDownItems.Add(restartItem);
            }
        }

        /// <summary>
        /// Handle the starting of the passed service and notify it via Balloon Tip
        /// </summary>
        /// <param name="sender">ServiceController</param>
        /// <param name="e"></param>
        protected void ServiceCtrlHandlerStarter(object sender, EventArgs e)
        {
            MyServiceCtrlItem msci = sender as MyServiceCtrlItem;
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;

            this.ServiceCtrlStarter(msci.Sc);
            tsmi.Image = global::WinServicesNotify.Properties.Resources.rdGreen;
        }

        /// <summary>
        /// Handle the stopping of the passed service and notify it via Balloon Tip
        /// </summary>
        /// <param name="sender">ServiceController</param>
        /// <param name="e"></param>
        protected void ServiceCtrlHandlerStopper(object sender, EventArgs e)
        {
            MyServiceCtrlItem msci = sender as MyServiceCtrlItem;

            this.ServiceCtrlStopper(msci.Sc);
        }

        /// <summary>
        /// Handle the restarting of the passed service and notify it via Balloon Tip
        /// </summary>
        /// <param name="sender">ServiceController</param>
        /// <param name="e"></param>
        protected void ServiceCtrlHandlerRestart(object sender, EventArgs e)
        {
            MyServiceCtrlItem msci = sender as MyServiceCtrlItem;

            this.ServiceCtrlStopper(msci.Sc);
            this.ServiceCtrlStarter(msci.Sc);
        }

        // Some defines for Balloons natifications
        public const int TOV = 42000;
        public const String ALRTUP = "Service started";
        public const String ALRTDW = "Service stopped";
        public const String ALRTERR = "Service error";

        //
        //
        // COMMENT REVENIR SUR LE PARENT ?
        //
        //

        private void ServiceCtrlStarter(ServiceController sc)
        {
            try
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
                //notifyIcon1.ShowBalloonTip(ServicesNotify.TOV,
                //    ServicesNotify.ALRTUP,
                //    sc.DisplayName.ToString() + " now " + sc.Status.ToString().ToLower(),
                //    ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                //notifyIcon1.ShowBalloonTip(ServicesNotify.TOV,
                //    ServicesNotify.ALRTERR,
                //    sc.DisplayName.ToString() + " can't start",
                //    ToolTipIcon.Error);

                throw ex;
            }
        }
        private void ServiceCtrlStopper(ServiceController sc)
        {
            try
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                notifyIcon1.ShowBalloonTip(ServicesNotify.TOV,
                    ServicesNotify.ALRTDW,
                    sc.ServiceName.ToString() + " now " + sc.Status.ToString().ToLower(),
                    ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(ServicesNotify.TOV,
                    ServicesNotify.ALRTERR,
                    sc.DisplayName.ToString() + " can't stop",
                    ToolTipIcon.Error);

                throw ex;
            }
        }
        private void ServiceCtrlInvoker(MyServiceCtrlItem msci, ServiceController sc)
        {
            // TODO
        }
    }
}
