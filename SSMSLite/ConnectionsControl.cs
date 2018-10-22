using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Models;

namespace SSMSLiteWinForms
{
    public partial class ConnectionsControl : UserControl
    {
        List<Connection> connections;

        public ConnectionsControl()
        {
            InitializeComponent();
            conAuth.SelectedIndex = 0;
            LoadData();
            Refresh();
        }

        private void LoadData()
        {
            connections = new List<Connection>();
            connections.AddRange(XmlSave.Read());
        }

        public override void Refresh()
        {
            base.Refresh();
            layout.Controls.Clear();

            foreach (Connection con in connections)
            {
                ConnectionControl cc = new ConnectionControl(con);
                cc.Tag = con;
                cc.Delete += Connection_Delete;
                layout.Controls.Add(cc);
            }

        }

        private void Connection_Delete(object sender, ConnectionEventArgs e)
        {
            connections.Remove(e.Connection);
            Save();
            Refresh();
        }

        public void Save()
        {
            XmlSave.Save(connections.ToArray());
        }

        private Connection GetConnection()
        {
            Connection result = new Connection();
            result.Name = conServer.Text.Trim();
            result.Server = conServer.Text.Trim();
            result.SqlAuth = conAuth.SelectedIndex == 1;
            result.Username = conUser.Text.Trim();
            result.Password = conPass.Text.Trim();
            result.Encrypted = conEncrypted.Checked;
            return result;

        }

        private void saveInstance_Click(object sender, EventArgs e)
        {
            var con = GetConnection();
            var cn = new ChangeName(con.Name);
            if (cn.ShowDialog() == DialogResult.OK)
            {
                con.Name = cn.ChangedName;
            }
            connections.Add(con);
            Save();
            Refresh();
        }

        private void conSubmit_Click(object sender, EventArgs e)
        {
            Instances.Add(GetConnection().getInstance());
        }
    }
}
