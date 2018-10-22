using Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SSMSLiteWinForms
{
    public partial class ConnectionControl : UserControl
    {
        Connection connection;
        
        public event EventHandler<ConnectionEventArgs> Delete;

        public ConnectionControl(Connection conn)
        {
            InitializeComponent();
            this.connection = conn;

            name.Text = conn.Name;
            server.Text = conn.Server;
            user.Text = conn.SqlAuth ? conn.Username : "Windows Authentication";
        }

        public Connection Connection {get => connection;}

        private void Connection_MouseClick(object sender, MouseEventArgs e)
        {
            Instances.Add(connection.getInstance());
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Instances.Add(connection.getInstance());
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete?.Invoke(this, new ConnectionEventArgs(connection));
        }


        private void Connection_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.LightGray;
        }

        private void Connection_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }
    }

    public class ConnectionEventArgs : EventArgs
    {
        public Connection Connection { get; set; }

        public ConnectionEventArgs(Connection conn)
        {
            Connection = conn;
        }
    }
}
