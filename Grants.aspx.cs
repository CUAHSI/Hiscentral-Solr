using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Grants : System.Web.UI.Page
{
    public DataTable objCount;
    public DataTable objDataTableGrant;
    public DataTable objDataTableSource;
    public int sites;
    public int variables;
    public int seriesCatalog;
    public int values;
    public string NetworkId = "";
    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (!IsPostBack)
        {
            if (Request.QueryString.Count == 0) Response.Redirect("default.aspx");
            //if (Session["NetworkID"] == null) Response.Redirect("default.aspx");
            NetworkId = Convert.ToString(Request.QueryString["n"]);
           
            string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
            SqlConnection objconnection = new SqlConnection(connectionstring);

            String sql = "select NetworkTitle from HISNetworks where networkid = " + Convert.ToInt32(NetworkId);

            objconnection.Open();
            SqlCommand cmd = new SqlCommand(sql, objconnection);
            string NetworkName = cmd.ExecuteScalar().ToString();
            string sourceid = NetworkId.ToString();
            lblNetworkName.Text = NetworkName;
            objconnection.Close();
            FillDataTable();
        }
    }

    private void FillDataTable()
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new SqlConnection(connectionstring);
        try
        {
 
            objDataTableGrant = new DataTable();

            SqlCommand objsqlcommand1 = new SqlCommand("rows_source_and_funding", objconnection);
            objsqlcommand1.CommandType = CommandType.StoredProcedure;
            int NetworkID = Convert.ToInt32(NetworkId);
            objsqlcommand1.Parameters.AddWithValue("@Network", NetworkID);

            SqlDataAdapter objAdapter = new SqlDataAdapter(objsqlcommand1);
            DataSet objDataSet = new DataSet();

            objconnection.Open();
            objAdapter.Fill(objDataSet);
            objconnection.Close();

            //objDataTableGrant = objDataSet.Tables[1];

            objCount = objDataSet.Tables[2];
            sites = Convert.ToInt32(objCount.Rows[0][0].ToString());

            objCount = objDataSet.Tables[3];
            variables = Convert.ToInt32(objCount.Rows[0][0].ToString());

            objCount = objDataSet.Tables[4];
            seriesCatalog = Convert.ToInt32(objCount.Rows[0][0].ToString());

            objCount = objDataSet.Tables[5];
            values = Convert.ToInt32(objCount.Rows[0][0].ToString());

            objDataTableGrant = objDataSet.Tables[1];
            objDataTableSource = objDataSet.Tables[0];
            
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (objconnection.State == ConnectionState.Open)
                objconnection.Close();
            objDataTableGrant.Dispose();
        }
    }
}