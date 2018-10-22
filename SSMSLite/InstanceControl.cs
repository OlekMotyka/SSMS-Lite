using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Models;

namespace SSMSLiteWinForms
{
    public partial class InstanceControl : UserControl
    {
        public Instance Instance { get; set; }

        public InstanceControl(Instance instance)
        {
            InitializeComponent();
            Instance = instance;
            Instance.QueryAdded += Instance_QueryAdded;
            tree.Nodes.AddRange(getTreeList(Instance));
        }

        private void Instance_QueryAdded(object sender, QueryEventArgs e)
        {
            TabPage tab = new TabPage();
            tab.Text = e.Query.Name;
            QueryControl qc = new QueryControl(e.Query);
            tab.Controls.Add(qc);
            qc.Dock = DockStyle.Fill;
            tabControl.TabPages.Add(tab);
            tabControl.SelectedTab = tab;
        }

        private TreeNode[] getTreeList(Folder folder)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (Folder f in folder.Objects)
            {
                TreeNode n = new TreeNode(f.ToString());
                n.Nodes.AddRange(getTreeList(f));
                n.Tag = f;
                if (f is Database)
                    n.ImageKey = n.SelectedImageKey = "database.ico";
                else if (f is Table)
                    n.ImageKey = n.SelectedImageKey = "table.ico";
                else if (f is Models.View)
                    n.ImageKey = n.SelectedImageKey = "view.ico";
                else if (f is Column)
                    n.ImageKey = n.SelectedImageKey = (f as Column).PrimaryKey ? "key.ico" : "column.ico";
                if (!(f is Column))
                    n.Nodes.Add("dummy", "");
                nodes.Add(n);
            }
            return nodes.ToArray();
        }

        private void RefreshNode(TreeNode node = null)
        {
            if (node == null)
                node = tree.SelectedNode;
            node.Nodes.Clear();
            (node.Tag as Folder).LoadData(node.Tag as Folder);
            node.Nodes.AddRange(getTreeList(node.Tag as Folder));
        }

        private void tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.ContainsKey("dummy"))
            {
                e.Node.Nodes.RemoveByKey("dummy");
                RefreshNode(e.Node);
            }
            if (!(e.Node.Tag is Database) && !(e.Node.Tag is Table) && !(e.Node.Tag is Column) && !(e.Node.Tag is Models.View))
                e.Node.ImageKey = e.Node.SelectedImageKey = "folder-open.ico";
        }

        private void tree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (!(e.Node.Tag is Database) && !(e.Node.Tag is Table) && !(e.Node.Tag is Column) && !(e.Node.Tag is Models.View))
                e.Node.ImageKey = e.Node.SelectedImageKey = "folder.ico";

        }

        private void contextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            contextMenu.Items.Clear();

            if (tree.SelectedNode.Tag is Database) DatabaseMenu();
            else if (tree.SelectedNode.Tag is Table) TableMenu();
            else if (tree.SelectedNode.Tag is Models.View) ViewMenu();
            else if (tree.SelectedNode.Tag is Column) ColumnMenu();
            else if (tree.SelectedNode.Tag is Procedure) ProcedureMenu();
            else if (tree.SelectedNode.Tag is Folder) FolderMenu();

            contextMenu.Height = contextMenu.Items.Count * 22 + 3;
            e.Cancel = contextMenu.Items.Count == 0;
        }

        private void tree_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tree.SelectedNode = tree.GetNodeAt(e.X, e.Y);
            }
        }

        private void tabControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                var tab = tabControl.TabPages.Cast<TabPage>().Where((t, i) => tabControl.GetTabRect(i).Contains(e.Location)).First();
                Instance.RemoveQuery((tab.Controls[0] as QueryControl).Query);
                tabControl.TabPages.Remove(tab);
            }
        }

        private ToolStripItem treeMenuItem(string name, EventHandler onClick)
        {
            var res = new ToolStripButton(name);
            res.AutoSize = false;
            res.Height = 22 - res.Margin.Vertical;
            res.Width = contextMenu.Width - contextMenu.Padding.Horizontal - contextMenu.Margin.Horizontal - res.Margin.Horizontal;
            res.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            res.Click += onClick;
            res.Tag = tree.SelectedNode.Tag;
            return res;
        }

        #region Folder
        private void FolderMenu()
        {
            Folder folder = tree.SelectedNode.Tag as Folder;
            if (folder.Name == "Databases")
            {
                contextMenu.Items.Add(treeMenuItem("Create database", CreateDatabase_Click));
            }
            if (folder.Name == "Tables")
            {
                contextMenu.Items.Add(treeMenuItem("New table", CreateTable_Click));
            }
            if (folder.Name == "Views")
            {
                contextMenu.Items.Add(treeMenuItem("New View", FolderNewView_Click));
            }
            if (folder.Name == "Columns")
            {
                if (folder.Parent is Table)
                {
                    contextMenu.Items.Add(treeMenuItem("Add column", AddColumnsFolder_Click));
                }
            }
            if (folder.Name == "Procedures")
            {
                contextMenu.Items.Add(treeMenuItem("New Procedure", FolderNewProcedure_Click));
            }
            contextMenu.Items.Add(treeMenuItem("Refresh", Refresh_Click));
        }

        private void CreateDatabase_Click(object sender, EventArgs e)
        {
            ChangeName cn = new ChangeName();
            if (cn.ShowDialog() == DialogResult.OK)
            {
                Instance.CreateDatabase(cn.ChangedName);
                RefreshNode();
            }
        }

        private void CreateTable_Click(object sender, EventArgs e)
        {
            Folder folder = (sender as ToolStripItem).Tag as Folder;
            var cn = new ChangeName("", "Create table");
            if (cn.ShowDialog() == DialogResult.OK)
                (folder.Parent as Database).NewTable(cn.ChangedName);
        }

        private void FolderNewView_Click(object sender, EventArgs e)
        {
            Folder folder = (sender as ToolStripItem).Tag as Folder;
            var cn = new ChangeName("", "New View");
            if (cn.ShowDialog() == DialogResult.OK)
                (folder.Parent as Models.View).Create(cn.ChangedName);
        }

        private void AddColumnsFolder_Click(object sender, EventArgs e)
        {
            Folder folder = (sender as ToolStripItem).Tag as Folder;
            (folder.Parent as Table).AddColumn();
        }

        private void FolderNewProcedure_Click(object sender, EventArgs e)
        {
            Folder folder = (sender as ToolStripItem).Tag as Folder;
            (folder.Parent as Database).NewProcedure();

        }
        #endregion

        #region Database menu
        private void DatabaseMenu()
        {
            contextMenu.Items.Add(treeMenuItem("New query", NewDbQuery_Click));
            contextMenu.Items.Add(treeMenuItem("New database", CreateDatabase_Click));
            contextMenu.Items.Add(treeMenuItem("Backup to file", Backup_Click));
            contextMenu.Items.Add(treeMenuItem("Restore from file", Restore_Click));
            contextMenu.Items.Add(treeMenuItem("Rename", RenameDb_Click));
            contextMenu.Items.Add(treeMenuItem("Refresh", Refresh_Click));
            contextMenu.Items.Add(treeMenuItem("Delete database", DeleteDb_Click));
        }

        private void NewDbQuery_Click(object sender, EventArgs e)
        {
            Database db = (sender as ToolStripItem).Tag as Database;
            Instance.AddQuery(new Query(db));
        }

        private void Backup_Click(object sender, System.EventArgs e)
        {
            Database db = (sender as ToolStripItem).Tag as Database;
            backupFileDialog.InitialDirectory = Path.Combine(Instance.GetPropertyValue("InstanceDefaultDataPath"), "..\\Backup\\");
            if (backupFileDialog.ShowDialog() == DialogResult.OK)
            {
                db.Backup(backupFileDialog.FileName);
            }
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            Database db = (sender as ToolStripItem).Tag as Database;
            restoreFileDialog.InitialDirectory = Path.Combine(Instance.GetPropertyValue("InstanceDefaultDataPath"), "..\\Backup\\");
            if (restoreFileDialog.ShowDialog() == DialogResult.OK)
            {
                db.Restore(restoreFileDialog.FileName);
            }
        }

        private void RenameDb_Click(object sender, EventArgs e)
        {
            Database db = (sender as ToolStripItem).Tag as Database;
            var cn = new ChangeName(db.Name, "Rename database");
            if (cn.ShowDialog() == DialogResult.OK)
            {
                db.Rename(cn.ChangedName);
                RefreshNode();
            }
        }

        private void DeleteDb_Click(object sender, EventArgs e)
        {
            Database db = (sender as ToolStripItem).Tag as Database;
            if (MessageBox.Show("Are you sure you want remove this database?", "Drop database", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.Drop();
                db.LoadData(db);
                RefreshNode();
            }
        }
        #endregion

        #region View
        private void ViewMenu()
        {
            contextMenu.Items.Add(treeMenuItem("New View", NewView_Click));
            contextMenu.Items.Add(treeMenuItem("Select Top 1000 Rows", SelectAllView_Click));
            contextMenu.Items.Add(treeMenuItem("Design", EditView_Click));
            contextMenu.Items.Add(treeMenuItem("Rename", RenameView_Click));
            contextMenu.Items.Add(treeMenuItem("Delete", DeleteView_Click));
            contextMenu.Items.Add(treeMenuItem("Refresh", Refresh_Click));
        }

        private void SelectAllView_Click(object sender, System.EventArgs e)
        {
            Models.View view = (sender as ToolStripItem).Tag as Models.View;
            view.Select1000();
        }

        private void NewView_Click(object sender, EventArgs e)
        {
            Models.View view = (sender as ToolStripItem).Tag as Models.View;
            var cn = new ChangeName("", "New View");
            if (cn.ShowDialog() == DialogResult.OK)
                view.Create(cn.ChangedName);
        }

        private void EditView_Click(object sender, EventArgs e)
        {
            Models.View view = (sender as ToolStripItem).Tag as Models.View;
            view.Design();
        }

        private void RenameView_Click(object sender, EventArgs e)
        {
            Models.View view = (sender as ToolStripItem).Tag as Models.View;
            var cn = new ChangeName("", "New View");
            if (cn.ShowDialog() == DialogResult.OK)
                view.Rename(cn.ChangedName);
        }

        private void DeleteView_Click(object sender, EventArgs e)
        {
            Models.View view = (sender as ToolStripItem).Tag as Models.View;
            view.DeleteQuery();
        }
        #endregion

        #region Table Menu
        private void TableMenu()
        {
            contextMenu.Items.Add(treeMenuItem("New Table", NewTable_Click));
            contextMenu.Items.Add(treeMenuItem("Design", EditTable_Click));
            contextMenu.Items.Add(treeMenuItem("Select Top 1000 Rows", SelectAll_Click));
            contextMenu.Items.Add(treeMenuItem("Insert Script", InsertTable_Click));
            contextMenu.Items.Add(treeMenuItem("Update Script", Update_Click));
            contextMenu.Items.Add(treeMenuItem("Delete Script", DeleteTable_Click));
            contextMenu.Items.Add(treeMenuItem("Rename", RenameTable_Click));
            contextMenu.Items.Add(treeMenuItem("Delete", DropTable_Click));
            contextMenu.Items.Add(treeMenuItem("Refresh", Refresh_Click));
        }

        private void SelectAll_Click(object sender, System.EventArgs e)
        {
            Table table = (sender as ToolStripItem).Tag as Table;
            table.Select1000();
        }

        private void NewTable_Click(object sender, EventArgs e)
        {
            Table table = (sender as ToolStripItem).Tag as Table;
            var cn = new ChangeName("", "Create Table");
            if (cn.ShowDialog() == DialogResult.OK)
                table.Create(cn.ChangedName);

        }

        private void EditTable_Click(object sender, EventArgs e)
        {
            Table table = (sender as ToolStripItem).Tag as Table;
            table.Design();
        }

        private void InsertTable_Click(object sender, EventArgs e)
        {
            Table table = (sender as ToolStripItem).Tag as Table;
            table.Insert();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            Table table = (sender as ToolStripItem).Tag as Table;
            table.Update();
        }

        private void DeleteTable_Click(object sender, EventArgs e)
        {
            Table table = (sender as ToolStripItem).Tag as Table;
            table.DeleteQuery();
        }

        private void RenameTable_Click(object sender, EventArgs e)
        {
            Table table = (sender as ToolStripItem).Tag as Table;
            var cn = new ChangeName(table.Name, "Change table name");
            if (cn.ShowDialog() == DialogResult.OK)
                table.Rename(cn.ChangedName);
        }

        private void DropTable_Click(object sender, EventArgs e)
        {
            Table table = (sender as ToolStripItem).Tag as Table;
            table.Delete();
        }
        #endregion

        #region Column Menu
        private void ColumnMenu()
        {
            if (tree.SelectedNode.Parent.Parent.Tag is Table)
            {
                contextMenu.Items.Add(treeMenuItem("Add column", AddColumn_Click));
                contextMenu.Items.Add(treeMenuItem("Edit", EditColumn_Click));
                contextMenu.Items.Add(treeMenuItem("Rename", RenameColumn_Click));
                contextMenu.Items.Add(treeMenuItem("Delete", DeleteColumn_Click));
            }
            contextMenu.Items.Add(treeMenuItem("Refresh", Refresh_Click));
        }

        private void AddColumn_Click(object sender, EventArgs e)
        {
            Column col = (sender as ToolStripItem).Tag as Column;
            col.Add();
            RefreshNode();
        }

        private void EditColumn_Click(object sender, EventArgs e)
        {
            Column col = (sender as ToolStripItem).Tag as Column;
            col.Edit();
        }

        private void RenameColumn_Click(object sender, EventArgs e)
        {
            Column col = (sender as ToolStripItem).Tag as Column;
            var cn = new ChangeName(col.Name, "Rename column");
            if (cn.ShowDialog() == DialogResult.OK)
            {
                col.Rename(cn.ChangedName);
                RefreshNode();
            }
        }

        private void DeleteColumn_Click(object sender, EventArgs e)
        {
            Column col = (sender as ToolStripItem).Tag as Column;
            col.Delete();
            RefreshNode();
        }
        #endregion

        #region Procedure Menu
        private void ProcedureMenu()
        {
            contextMenu.Items.Add(treeMenuItem("New Procedure", NewProcedure_Click));
            contextMenu.Items.Add(treeMenuItem("Design", EditProcedure_Click));
            contextMenu.Items.Add(treeMenuItem("Execute", ExecuteProcedure_Click));
            contextMenu.Items.Add(treeMenuItem("Rename", RenameProcedure_Click));
            contextMenu.Items.Add(treeMenuItem("Delete", DropProcedure_Click));
            contextMenu.Items.Add(treeMenuItem("Refresh", Refresh_Click));
        }

        private void NewProcedure_Click(object sender, EventArgs e)
        {
            Procedure proc = (sender as ToolStripItem).Tag as Procedure;
            proc.Database.NewProcedure();
        }

        private void EditProcedure_Click(object sender, EventArgs e)
        {
            Procedure proc = (sender as ToolStripItem).Tag as Procedure;
            proc.Design();
        }

        private void ExecuteProcedure_Click(object sender, EventArgs e)
        {
            Procedure proc = (sender as ToolStripItem).Tag as Procedure;
            proc.Execute();
        }

        private void RenameProcedure_Click(object sender, EventArgs e)
        {
            Procedure proc = (sender as ToolStripItem).Tag as Procedure;
            var cn = new ChangeName(proc.Name, $"Rename {proc}");
            if (cn.ShowDialog() == DialogResult.OK)
                proc.Rename(cn.ChangedName);
        }

        private void DropProcedure_Click(object sender, EventArgs e)
        {
            Procedure proc = (sender as ToolStripItem).Tag as Procedure;
            proc.Delete();
        }
        #endregion

        private void Refresh_Click(object sender, EventArgs e)
        {
            RefreshNode();
        }
    }
}
