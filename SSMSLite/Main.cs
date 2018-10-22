using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Models;

namespace SSMSLiteWinForms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            tabConnections.Controls.Add(new ConnectionsControl());

            Instances.InstanceAdded += InstanceAdded;
        }

        private void InstanceAdded(object sender, InstanceEventArgs e)
        {
            InstanceControl ic = new InstanceControl(e.Instance);
            var tp = TabPage(ic);
            tabControl.TabPages.Add(tp);
            tabControl.SelectedTab = tp;
        }

        private TabPage TabPage(InstanceControl ic)
        {
            var tp = new TabPage();
            tp.Text = ic.Instance.Name;
            tp.Controls.Add(ic);
            ic.Dock = DockStyle.Fill;
            return tp;
        }

        private void newConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl.SelectedIndex = 0;
        }

        private void tabControl_MouseClick(object sender, MouseEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabs = tabControl.TabPages;

            if (e.Button == MouseButtons.Middle)
            {
                var tab = tabs.Cast<TabPage>()
                        .Where((t, i) => tabControl.GetTabRect(i).Contains(e.Location))
                        .First();
                if (tab != tabConnections)
                {
                    var instance = (tab.Controls[0] as InstanceControl).Instance;
                    Instances.Remove(instance);
                    tabControl.TabPages.Remove(tab);
                }
            }
        }
    }
}
