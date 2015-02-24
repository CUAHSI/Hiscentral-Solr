using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Series : System.Web.UI.Page
{
    public int NetworkId ;
    public DataTable objDataTableSeries;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.Count == 0) Response.Redirect("default.aspx");
            //if (Session["NetworkID"] == null) Response.Redirect("default.aspx");
            NetworkId = Convert.ToInt32(Request.QueryString["n"]);
            string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
            SqlConnection objconnection = new SqlConnection(connectionstring);
            String sql = "select NetworkTitle from HISNetworks where networkid = " + NetworkId; //52;

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

            objDataTableSeries = new DataTable();

            SqlCommand objsqlcommand = new SqlCommand("Retrive_Series", objconnection);
            objsqlcommand.CommandType = CommandType.StoredProcedure;
            //int NetworkID = Convert.ToInt32(Session["NetworkID"]);
            objsqlcommand.Parameters.AddWithValue("@Network", NetworkId);

            SqlDataAdapter objAdapter = new SqlDataAdapter(objsqlcommand);            

            objconnection.Open();
            objAdapter.Fill(objDataTableSeries);
            objconnection.Close();

            
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (objconnection.State == ConnectionState.Open)
                objconnection.Close();
            objDataTableSeries.Dispose();
        }
    }
}