using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sites : System.Web.UI.Page
{
    public int NetworkId;
    public DataTable objDataTableSites;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // if (Session["NetworkID"] == null) Response.Redirect("default.aspx");
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
        SqlConnection objconnection = new SqlConnection(connectionstring);
        try
        {

            objDataTableSites = new DataTable();

            SqlCommand objsqlcommand = new SqlCommand("Retrive_Sites", objconnection);
            objsqlcommand.CommandType = CommandType.StoredProcedure;
            //int NetworkID = Convert.ToInt32(Session["NetworkID"]); //52;
            objsqlcommand.Parameters.AddWithValue("@Network", NetworkId);

            SqlDataAdapter objAdapter = new SqlDataAdapter(objsqlcommand);            

            objconnection.Open();
            objAdapter.Fill(objDataTableSites);
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
            objDataTableSites.Dispose();
        }
    }
}