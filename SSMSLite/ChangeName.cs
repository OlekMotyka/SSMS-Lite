using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SSMSLiteWinForms
{
    public partial class ChangeName : Form
    {
        public string ChangedName { get; set; }

        public ChangeName(string text = "", string label = "Give a name")
        {
            InitializeComponent();
            ChangedName = text;
            textBox1.Text = text;
            Text = label;
            textBox1.Focus();
        }

        private void save_Click(object sender, EventArgs e)
        {
            ChangedName = textBox1.Text.Trim();
            DialogResult = DialogResult.OK;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                save.PerformClick();
            }
        }
    }
}
