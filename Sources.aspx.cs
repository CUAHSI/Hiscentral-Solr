using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sources : System.Web.UI.Page
{
    public int NetworkId;
    public DataTable objDataTableSource;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Session["NetworkID"] == null) Response.Redirect("default.aspx");
            if (Request.QueryString.Count == 0) Response.Redirect("default.aspx");
            NetworkId = Convert.ToInt32(Request.QueryString["n"]);
            string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
            SqlConnection objconnection = new SqlConnection(connectionstring);

            String sql = "select NetworkTitle from HISNetworks where networkid = " + NetworkId; //52;

            objconnection.Open();
            SqlCommand cmd = new SqlCommand(sql, objconnection);
            string NetworkName = cmd.ExecuteScalar().ToString();
            //string sourceid = Session["NetworkID"].ToString();
            lblNetworkName.Text = NetworkName;
            objconnection.Close();
            FillDataTable();
        }
    }
    private void FillDataTable()
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new System.Data.SqlClient.SqlConnection(connectionstring);
        try
        {
            //int NetworkID = Convert.ToInt32(Session["NetworkID"]); //52;

            objDataTableSource = new DataTable();
            SqlCommand objsqlcommand1 = new SqlCommand("rows_source_and_funding", objconnection);
            objsqlcommand1.CommandType = CommandType.StoredProcedure;
            objsqlcommand1.Parameters.AddWithValue("@Network",  NetworkId);

            SqlDataAdapter objAdapter = new SqlDataAdapter(objsqlcommand1);
            DataSet objDataSet = new DataSet();

            objconnection.Open();
            objAdapter.Fill(objDataSet);
            objconnection.Close();

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
            objDataTableSource.Dispose();
        }
    }
}