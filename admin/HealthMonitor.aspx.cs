using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_HealthMonitor : System.Web.UI.Page
{
    public DataTable objDataTable;
    protected void Page_Load(object sender, EventArgs e)
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new SqlConnection(connectionstring);
        try {

            objDataTable = new DataTable();
            string sql = "Select * from HealthMonitor";
            SqlCommand objsqlcommand = new SqlCommand(sql, objconnection);
            objsqlcommand.CommandType = CommandType.Text;
            SqlDataAdapter objAdapter = new SqlDataAdapter(objsqlcommand);
            objconnection.Open();
            objAdapter.Fill(objDataTable);
            objconnection.Close();

        } catch (Exception ex) {
            throw (ex);
        } finally {
            if (objconnection.State == ConnectionState.Open)
                objconnection.Close();
            objDataTable.Dispose();
        }
    }
}