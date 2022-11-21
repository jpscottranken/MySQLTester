using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MySQLTester
{
    public partial class frmTester : Form
    {
        public frmTester()
        {
            InitializeComponent();
        }


        //  Global database variables

        //  Taken from URL:
        //  https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection?view=dotnet-plat-ext-6.0
        //  A SqlConnection object represents a unique session
        //  to a SQL Server data source. With a client/server
        //  database system, it is equivalent to a network
        //  connection to the server. SqlConnection is used
        //  together with SqlDataAdapter and SqlCommand to
        //  increase performance when connecting to a Microsoft
        //  SQL Server database. For all third-party SQL
        //  Server products and other OLE DB-supported data
        //  sources, use OleDbConnection.
        SqlConnection conn;

        //  Taken from URL:
        //  https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand?view=dotnet-plat-ext-6.0
        //  When an instance of SqlCommand is created, the
        //  read/write properties are set to their initial values. 
        SqlCommand queryCommand;

        //  Taken from the following URL:
        //  https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldataadapter?view=dotnet-plat-ext-6.0
        //  The SqlDataAdapter, serves as a bridge between a
        //  DataSet and SQL Server for retrieving and saving data.
        //
        //  The SqlDataAdapter provides this bridge by mapping Fill,
        //  which changes the data in the DataSet to match the data
        //  in the data source, and Update, which changes the data
        //  in the data source to match the data in the DataSet,
        //  using the appropriate Transact-SQL statements against
        //  the data source.
        SqlDataAdapter queryAdapter;

        //  Taken from the following URL:
        //  https://www.c-sharpcorner.com/UploadFile/mahesh/datatable-in-C-Sharp/
        //
        //  The DataTable class in C# ADO.NET is a database table
        //  representation and provides a collection of columns and
        //  rows to store data in a grid form. 
        DataTable table;
        private void btnExecuteQuery_Click(object sender, EventArgs e)
        {
            attemptToExecuteQuery();
        }

        private void attemptToExecuteQuery()
        {
            //  "Blank out" database field
            queryCommand = null;

            //  Instantiate new queryAdapter
            queryAdapter = new SqlDataAdapter();

            //  Instantiate new DataTable
            table = new DataTable();

            //  Check for nothing in the query input textBox
            if (txtQuery.Text.Trim() == "")
            {
                MessageBox.Show("Nothing Entered Into Query TextBox!",
                                "QUERY TEXTBOX EMPTY",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                txtQuery.Focus();
                return;
            }
            //  Was content in textBox
            //  Validate that the query is valid
            try
            {
                //  Instantiate new SqlCommand object
                queryCommand = new SqlCommand(txtQuery.Text, conn);

                //  Use queryAdapter as a "bridge" between
                //  the program and the database itself.
                queryAdapter.SelectCommand = queryCommand;

                //  Fill the datatable with the output of
                //  queryAdapter.
                queryAdapter.Fill(table);

                //  Set the data source for the datagriview
                dgvResults.DataSource = table;

                //  Set the # records returned in the label
                lblNumberOfRecords.Text = table.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Query In TextBox!"
                                + "\n\n" + ex.Message,
                                "QUERY TEXTBOX INVALID" ,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                txtQuery.Text = "";
                txtQuery.Focus();
                return;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearTheGUI();
        }

        private void clearTheGUI()
        {
            //  https://www.codeproject.com/Questions/332902/how-to-clear-datagridview-in-csharp
            if (this.dgvResults.DataSource != null)
            {
                this.dgvResults.DataSource = null;
            }
            else
            {
                this.dgvResults.Rows.Clear();
            }

            lblNumberOfRecords.Text = "0";

            txtQuery.Text = "";
            txtQuery.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            exitProgramOrNot();
        }

        private void exitProgramOrNot()
        {
            DialogResult dialog = MessageBox.Show(
                "Do You Really Want To Exit?",
                "EXIT NOW?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                //  Close connection
                conn.Close();
                Application.Exit();
            }
        }

        private void frmTester_Load(object sender, EventArgs e)
        {
            var connString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=MMABooksDB;Integrated Security=SSPI";

            //  Create SqlConnection based on the connString above
            conn = new SqlConnection(connString);

            //  Open connection
            conn.Open();
        }
    }
}
