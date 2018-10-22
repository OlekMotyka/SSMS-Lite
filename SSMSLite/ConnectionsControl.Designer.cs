namespace SSMSLiteWinForms
{
    partial class ConnectionsControl
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
            this.layout = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.saveInstance = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.conServer = new System.Windows.Forms.TextBox();
            this.conPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.conUser = new System.Windows.Forms.TextBox();
            this.conSubmit = new System.Windows.Forms.Button();
            this.conAuth = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.conEncrypted = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // layout
            // 
            this.layout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layout.Location = new System.Drawing.Point(3, 221);
            this.layout.Name = "layout";
            this.layout.Size = new System.Drawing.Size(865, 406);
            this.layout.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.saveInstance);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.conServer);
            this.panel1.Controls.Add(this.conPass);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.conUser);
            this.panel1.Controls.Add(this.conSubmit);
            this.panel1.Controls.Add(this.conAuth);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.conEncrypted);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(286, 188);
            this.panel1.TabIndex = 14;
            // 
            // saveInstance
            // 
            this.saveInstance.Image = global::SSMSLiteWinForms.Properties.Resources.save;
            this.saveInstance.Location = new System.Drawing.Point(7, 143);
            this.saveInstance.Name = "saveInstance";
            this.saveInstance.Size = new System.Drawing.Size(38, 35);
            this.saveInstance.TabIndex = 14;
            this.saveInstance.UseVisualStyleBackColor = true;
            this.saveInstance.Click += new System.EventHandler(this.saveInstance_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 24);
            this.label2.TabIndex = 15;
            this.label2.Text = "New Connection";
            // 
            // conServer
            // 
            this.conServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.conServer.Location = new System.Drawing.Point(70, 27);
            this.conServer.Name = "conServer";
            this.conServer.Size = new System.Drawing.Size(211, 20);
            this.conServer.TabIndex = 0;
            this.conServer.Text = ".\\SQLEXPRESS";
            // 
            // conPass
            // 
            this.conPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.conPass.Location = new System.Drawing.Point(70, 106);
            this.conPass.Name = "conPass";
            this.conPass.Size = new System.Drawing.Size(211, 20);
            this.conPass.TabIndex = 6;
            this.conPass.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "User Name";
            // 
            // conUser
            // 
            this.conUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.conUser.Location = new System.Drawing.Point(70, 80);
            this.conUser.Name = "conUser";
            this.conUser.Size = new System.Drawing.Size(211, 20);
            this.conUser.TabIndex = 4;
            // 
            // conSubmit
            // 
            this.conSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.conSubmit.Location = new System.Drawing.Point(70, 155);
            this.conSubmit.Name = "conSubmit";
            this.conSubmit.Size = new System.Drawing.Size(211, 23);
            this.conSubmit.TabIndex = 12;
            this.conSubmit.Text = "Connect";
            this.conSubmit.UseVisualStyleBackColor = true;
            this.conSubmit.Click += new System.EventHandler(this.conSubmit_Click);
            // 
            // conAuth
            // 
            this.conAuth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.conAuth.FormattingEnabled = true;
            this.conAuth.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this.conAuth.Location = new System.Drawing.Point(70, 53);
            this.conAuth.Name = "conAuth";
            this.conAuth.Size = new System.Drawing.Size(211, 21);
            this.conAuth.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Auth";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server";
            // 
            // conEncrypted
            // 
            this.conEncrypted.AutoSize = true;
            this.conEncrypted.Location = new System.Drawing.Point(102, 132);
            this.conEncrypted.Name = "conEncrypted";
            this.conEncrypted.Size = new System.Drawing.Size(74, 17);
            this.conEncrypted.TabIndex = 11;
            this.conEncrypted.Text = "Encrypted";
            this.conEncrypted.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(3, 194);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(174, 24);
            this.label6.TabIndex = 17;
            this.label6.Text = "Saved Connections";
            // 
            // ConnectionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.layout);
            this.Name = "ConnectionsControl";
            this.Size = new System.Drawing.Size(871, 630);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel layout;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button saveInstance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox conServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button conSubmit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox conAuth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox conPass;
        private System.Windows.Forms.CheckBox conEncrypted;
        private System.Windows.Forms.TextBox conUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
    }
}
