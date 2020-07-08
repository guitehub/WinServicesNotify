using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceProcess;
using System.Windows.Forms;

namespace WinServicesNotify
{
    //public partial class MyServiceCtrlItem : Component
    public partial class MyServiceCtrlItem : ToolStripMenuItem
    {
        //this class allows a ToolStripMenuItem to store a ServiceController and access its methodes via the Form
        private ServiceController _sc;

        public ServiceController Sc
        {
            get { return _sc; }
            set { _sc = value; }
        }

        public MyServiceCtrlItem()
        {
            InitializeComponent();
        }

        public MyServiceCtrlItem(ServiceController sc)
        {
            InitializeComponent();

            this.Sc = sc;
        }

        //public MyServiceCtrlItem(IContainer container)
        //{
        //    container.Add(this);

        //    InitializeComponent();
        //}
    }
}
