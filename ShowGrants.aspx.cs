using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShowGrants : System.Web.UI.Page {
    public string urlVal = HttpContext.Current.Request.Url.Authority;
    public DataTable objDataTableSearchGrants;
    public DataTable objDataTableVariables;
    public DataTable objDataTableSites;
    public string grantAgency;
    public string grantNumber;
    public string networkName;
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            if (Request.QueryString.Count > 0) {
                FillDataTable();
                FillDataTablesites();
            } else {
                Response.Redirect("default.aspx");
            }
        }
         grantAgency = Convert.ToString(Request.QueryString["agency"]);
         grantNumber = Convert.ToString(Request.QueryString["number"]);
        GetData(grantAgency, grantNumber);
        GetNetworkId(grantAgency,grantNumber);

    }

    private void GetNetworkId(string grantAgency,string grantNumber) {

        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new SqlConnection(connectionstring);

        String sql = "select networkid from sources where sourceId = (select sourceId from SourceFunding where GrantAgency = '" + grantAgency + "' and grantNumber = " + grantNumber + ")";
        objconnection.Open();
        SqlCommand cmd = new SqlCommand(sql, objconnection);
        Session["NetworkID"] = cmd.ExecuteScalar();
        GetNetworkName();
    }


    private void GetNetworkName() {
        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new SqlConnection(connectionstring);

        String sql = "select NetworkTitle from HISNetworks where networkid = " + Convert.ToInt32(Session["NetworkID"]);

        objconnection.Open();
        SqlCommand cmd = new SqlCommand(sql, objconnection);
        networkName = cmd.ExecuteScalar().ToString();
        //string sourceid = NetworkId.ToString();
        lnkNetworkName.Text= networkName;
        lnkNetworkName.NavigateUrl = "~/pub_network.aspx?n=" + Convert.ToInt32(Session["NetworkID"]);
        objconnection.Close();
    
    }

    private void GetData(string agency, string number) {
        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new SqlConnection(connectionstring);
        try {
            objDataTableSearchGrants = new DataTable();

            SqlCommand objsqlcommand = new SqlCommand("Search_Grants", objconnection);
            objsqlcommand.CommandType = CommandType.StoredProcedure;
            objsqlcommand.CommandTimeout = 0;
            objsqlcommand.Parameters.AddWithValue("@agency", agency);
            objsqlcommand.Parameters.AddWithValue("@number", number);

            SqlDataAdapter objAdapter = new SqlDataAdapter(objsqlcommand);

            objconnection.Open();
            objAdapter.Fill(objDataTableSearchGrants);
            objconnection.Close();

        } catch (Exception ex) {
            throw (ex);
        } finally {
            if (objconnection.State == ConnectionState.Open)
                objconnection.Close();
            objDataTableSearchGrants.Dispose();
        }
    }


    private void FillDataTable() {
        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new SqlConnection(connectionstring);
        try {

            objDataTableVariables = new DataTable();

            SqlCommand objsqlcommand = new SqlCommand("Retrive_Variables", objconnection);
            objsqlcommand.CommandType = CommandType.StoredProcedure;
            int NetworkID = Convert.ToInt32(Session["NetworkID"]);// 52;
            objsqlcommand.Parameters.AddWithValue("@Network", NetworkID);
            SqlDataAdapter objAdapter = new SqlDataAdapter(objsqlcommand);
            objconnection.Open();
            objAdapter.Fill(objDataTableVariables);
            objconnection.Close();

        } catch (Exception ex) {
            throw (ex);
        } finally {
            if (objconnection.State == ConnectionState.Open)
                objconnection.Close();
            objDataTableVariables.Dispose();
        }
    }
    private void FillDataTablesites() {
        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new SqlConnection(connectionstring);
        try {

            objDataTableSites = new DataTable();

            SqlCommand objsqlcommand = new SqlCommand("Retrive_Sites", objconnection);
            objsqlcommand.CommandType = CommandType.StoredProcedure;
            int NetworkID = Convert.ToInt32(Session["NetworkID"]); //52;
            objsqlcommand.Parameters.AddWithValue("@Network", NetworkID);

            SqlDataAdapter objAdapter = new SqlDataAdapter(objsqlcommand);

            objconnection.Open();
            objAdapter.Fill(objDataTableSites);
            objconnection.Close();

        } catch (Exception ex) {
            throw (ex);
        } finally {
            if (objconnection.State == ConnectionState.Open)
                objconnection.Close();
            objDataTableSites.Dispose();
        }
    }
}