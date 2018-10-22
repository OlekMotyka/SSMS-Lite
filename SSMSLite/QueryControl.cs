using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Models;

namespace SSMSLiteWinForms
{
    public partial class QueryControl : UserControl
    {
        public Query Query { get; set; }

        private Stopwatch stopwatch = new Stopwatch();
        private System.Timers.Timer timer = new System.Timers.Timer();

        public QueryControl(Query query)
        {
            InitializeComponent();
            Query = query;
            query.AfterExecute += Query_AfterExecute;
            query.BeforeExecute += Query_BeforeExecute;
            Editor.Text = Query.QueryText;
            HighlihghtSyntax();
            timer.Elapsed += Timer_Tick;

            databasesComboBox.Items.AddRange(Query.Database.Instance.Databases.ToArray());
            databasesComboBox.SelectedItem = Query.Database;
        }

        private void Query_BeforeExecute(object sender, QueryEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                Cursor = Cursors.WaitCursor;
                stopwatch.Restart();
                timer.Interval = 10;
                timer.Start();
            });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeaLabel.Text = stopwatch.Elapsed.ToString("mm':'ss'.'fff");
        }

        private void Query_AfterExecute(object sender, EventArgs e)
        {
            Console.Text = Query.ConsoleResult;
            stopwatch.Stop();
            timer.Stop();
            timeaLabel.Text = stopwatch.Elapsed.ToString("mm':'ss'.'fff");
            LoadGrid();
            tabControl.SelectedTab = Query.Result == null ? tabConsole : tabGrid;
            Cursor = Cursors.Default;
        }

        private void LoadGrid()
        {
            grid.AutoGenerateColumns = true;
            grid.Columns.Clear();
            grid.DataSource = Query.Result;
            grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
            rowCountLabel.Text = grid.RowCount + " rows";
        }

        public void Execute()
        {
            Query.QueryText = Editor.Text;
            Query.Execute();
        }

        private void Editor_TextChanged(object sender, EventArgs e)
        {
            Query.QueryText = Editor.Text;
            HighlihghtSyntax();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private void databasesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Query.Database = databasesComboBox.SelectedItem as Database;
        }

        private void grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                grid.ClearSelection();
                grid.Columns[e.ColumnIndex].Selected = true;
                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    grid.Rows[i].Cells[e.ColumnIndex].Selected = true;
                }
            }
        }

        private void grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private bool CommandMode = false;
        private void Editor_KeyUp(object sender, KeyEventArgs e)
        {
            int CaretPosition = Editor.SelectionStart;
            if (e.KeyCode == Keys.F5)
            {
                Execute();
            }
            else if (e.KeyData == (Keys.Control | Keys.K))
                CommandMode = true;
            else if (e.KeyData == (Keys.Control | Keys.C) && CommandMode)
            {
                CommandMode = false;
                Editor.Text = Editor.Text.Insert(Editor.GetFirstCharIndexOfCurrentLine(), "--");
                CaretPosition += 2;
            }
            else if (e.KeyData == (Keys.Control | Keys.U) && CommandMode)
            {
                CommandMode = false;
                if (Editor.Lines[Editor.GetLineFromCharIndex(Editor.GetFirstCharIndexOfCurrentLine())].StartsWith("--"))
                {
                    Editor.Text = Editor.Text.Remove(Editor.GetFirstCharIndexOfCurrentLine(), 2);
                    CaretPosition -= 2;
                }
            }
            else CommandMode = false;
            Editor.SelectionStart = CaretPosition;
        }


        private void HighlihghtSyntax()
        {
            int CaretPosition = Editor.SelectionStart;
            Editor.SelectAll();
            Editor.SelectionColor = Color.Black;
            string[] keywords = new string[] { "ADD", "ALL", "ALTER", "AND", "ANY", "AS", "ASC", "AUTHORIZATION", "BACKUP", "BEGIN",
                "BETWEEN", "BREAK", "BROWSE", "BULK", "BY", "CASCADE", "CASE", "CHECK", "CHECKPOINT", "CLOSE", "CLUSTERED", "COALESCE",
                "COLLATE", "COLUMN", "COMMIT", "COMPUTE", "CONSTRAINT", "CONTAINS", "CONTAINSTABLE", "CONTINUE", "CONVERT", "CREATE",
                "CROSS", "CURRENT", "CURRENT_DATE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "DATABASE", "DBCC",
                "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DENY", "DESC", "DISK", "DISTINCT", "DISTRIBUTED", "DOUBLE", "DROP",
                "DUMMY", "DUMP", "ELSE", "END", "ERRLVL", "ESCAPE", "EXCEPT", "EXEC", "EXECUTE", "EXISTS", "EXIT", "FETCH", "FILE",
                "FILLFACTOR", "FOR", "FOREIGN", "FREETEXT", "FREETEXTTABLE", "FROM", "FULL", "FUNCTION", "GOTO", "GRANT", "GROUP",
                "HAVING", "HOLDLOCK", "IDENTITY", "IDENTITY_INSERT", "IDENTITYCOL", "IF", "IN", "INDEX", "INNER", "INSERT",
                "INTERSECT", "INTO", "IS", "JOIN", "KEY", "KILL", "LEFT", "LIKE", "LINENO", "LOAD", "NATIONAL", "NOCHECK",
                "NONCLUSTERED", "NOT", "NULL", "NULLIF", "OF", "OFF", "OFFSETS", "ON", "OPEN", "OPENDATASOURCE", "OPENQUERY",
                "OPENROWSET", "OPENXML", "OPTION", "OR", "ORDER", "OUTER", "OVER", "PERCENT", "PLAN", "PRECISION", "PRIMARY", "PRINT",
                "PROC", "PROCEDURE", "PUBLIC", "RAISERROR", "READ", "READTEXT", "RECONFIGURE", "REFERENCES", "REPLICATION", "RESTORE",
                "RESTRICT", "RETURN", "REVOKE", "RIGHT", "ROLLBACK", "ROWCOUNT", "ROWGUIDCOL", "RULE", "SAVE", "SCHEMA", "SELECT",
                "SESSION_USER", "SET", "SETUSER", "SHUTDOWN", "SOME", "STATISTICS", "SYSTEM_USER", "TABLE", "TEXTSIZE", "THEN", "TO",
                "TOP", "TRAN", "TRANSACTION", "TRIGGER", "TRUNCATE", "TSEQUAL", "UNION", "UNIQUE", "UPDATE", "UPDATETEXT", "USE",
                "USER", "VALUES", "VARYING", "VIEW", "WAITFOR", "WHEN", "WHERE", "WHILE", "WITH", "WRITETEXT" };

            foreach (Match m in Regex.Matches(Editor.Text, "\\w+", RegexOptions.IgnoreCase))
            {
                if (keywords.Contains(m.Value.ToUpper()))
                {
                    Editor.Select(m.Index, m.Length);
                    Editor.SelectionColor = Color.Blue;
                }
            }
            foreach (Match m in Regex.Matches(Editor.Text, "\\w+\\(.*\\)"))
            {
                string f = m.Value.Substring(0, m.Value.IndexOf('('));
                Editor.Select(m.Index, f.Length);
                Editor.SelectionColor = Color.DeepPink;
            }
            foreach (Match m in Regex.Matches(Editor.Text, "--.*"))
            {
                Editor.Select(m.Index, m.Length);
                Editor.SelectionColor = Color.Green;
            }
            foreach (Match m in Regex.Matches(Editor.Text, "'(.*?)'"))
            {
                Editor.Select(m.Index, m.Length);
                if (Editor.SelectionColor != Color.Green)
                    Editor.SelectionColor = Color.Red;
            }
            Editor.Select(CaretPosition, 0);
        }

        private void saveResultAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveResult.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (saveResult.ShowDialog() == DialogResult.OK)
            {
                ExportToFile(saveResult.FileName);
            }
        }

        private void ExportToFile(string path)
        {
            StreamWriter writer = new StreamWriter(path);
            if (path.EndsWith(".csv"))
                writer.Write(getResultString(";", true));
            else if (path.EndsWith(".txt"))
                writer.Write(getResultString("\t", true));
            writer.Close();
        }

        private string getResultString(string separator = ";", bool withHeaders = false)
        {
            string result = "";

            if (withHeaders)
            {
                foreach (int i in grid.SelectedCells.Cast<DataGridViewCell>().Select(x => x.ColumnIndex).Distinct().OrderBy(x => x))
                {
                    result += grid.Columns[i].HeaderText + separator;
                }
                result = result.Remove(result.Length - 1) + "\n";
            }
            foreach (int i in grid.SelectedCells.Cast<DataGridViewCell>().Select(x => x.RowIndex).Distinct().OrderBy(x => x))
            {
                result += string.Join(separator, grid.SelectedCells.Cast<DataGridViewCell>().Where(z => z.RowIndex == i).OrderBy(x => x.ColumnIndex).Select(x => x.Value.ToString())) + "\n";
            }
            return result;
        }

        private void grid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                gridMenu.Show(grid, e.Location);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(getResultString("\t"));
        }

        private void copyWithHeadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(getResultString("\t", true));
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid.SelectAll();
        }
    }
}
