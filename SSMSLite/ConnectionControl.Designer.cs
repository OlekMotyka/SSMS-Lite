namespace SSMSLiteWinForms
{
    partial class ConnectionControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.name = new System.Windows.Forms.Label();
            this.menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.server = new System.Windows.Forms.Label();
            this.user = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.ContextMenuStrip = this.menu;
            this.name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.name.Location = new System.Drawing.Point(3, 0);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(154, 24);
            this.name.TabIndex = 0;
            this.name.Text = "Nazwa Instancji";
            this.name.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Connection_MouseClick);
            this.name.MouseLeave += new System.EventHandler(this.Connection_MouseLeave);
            this.name.MouseHover += new System.EventHandler(this.Connection_MouseHover);
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(120, 48);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // server
            // 
            this.server.AutoSize = true;
            this.server.ContextMenuStrip = this.menu;
            this.server.Location = new System.Drawing.Point(48, 34);
            this.server.Name = "server";
            this.server.Size = new System.Drawing.Size(66, 13);
            this.server.TabIndex = 4;
            this.server.Text = "ServerName";
            this.server.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Connection_MouseClick);
            this.server.MouseLeave += new System.EventHandler(this.Connection_MouseLeave);
            this.server.MouseHover += new System.EventHandler(this.Connection_MouseHover);
            // 
            // user
            // 
            this.user.AutoSize = true;
            this.user.ContextMenuStrip = this.menu;
            this.user.Location = new System.Drawing.Point(48, 70);
            this.user.Name = "user";
            this.user.Size = new System.Drawing.Size(122, 13);
            this.user.TabIndex = 5;
            this.user.Text = "Windows Authentication";
            this.user.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Connection_MouseClick);
            this.user.MouseLeave += new System.EventHandler(this.Connection_MouseLeave);
            this.user.MouseHover += new System.EventHandler(this.Connection_MouseHover);
            // 
            // label2
            // 
            this.label2.Image = SSMSLiteWinForms.Properties.Resources.database;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(4, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 36);
            this.label2.TabIndex = 3;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Connection_MouseClick);
            // 
            // label3
            // 
            this.label3.Image = SSMSLiteWinForms.Properties.Resources.user;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(4, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 36);
            this.label3.TabIndex = 2;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Connection_MouseClick);
            // 
            // ConnectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContextMenuStrip = this.menu;
            this.Controls.Add(this.name);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.server);
            this.Controls.Add(this.user);
            this.Controls.Add(this.label2);
            this.Name = "ConnectionControl";
            this.Size = new System.Drawing.Size(208, 99);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Connection_MouseClick);
            this.MouseLeave += new System.EventHandler(this.Connection_MouseLeave);
            this.MouseHover += new System.EventHandler(this.Connection_MouseHover);
            this.menu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label server;
        private System.Windows.Forms.Label user;
        private System.Windows.Forms.ContextMenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}
