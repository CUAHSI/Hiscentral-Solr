using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SourceReports : System.Web.UI.Page {
    public DataTable objCount;
    public DataTable objDataTableGrant;
    public DataTable objDataTableSource;
    public int sites;
    public int variables;
    public int seriesCatalog;
    public int values;
    public string NetworkId;
    public string grantAgency;
    public string grantNumber;
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            if (Request.QueryString.Count == 0) Response.Redirect("default.aspx");
            //if (Session["NetworkID"] == null) Response.Redirect("default.aspx");
            //NetworkId = Convert.ToString(Request.QueryString["n"]);
            int nid = Convert.ToInt32(Request.QueryString["n"]);
            if (nid > 0 && nid != null) {
                NetworkId = Convert.ToString(nid);
                string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
                SqlConnection objconnection = new SqlConnection(connectionstring);
                String sql = "select NetworkTitle from HISNetworks where networkid = " + Convert.ToInt32(NetworkId);
                objconnection.Open();
                SqlCommand cmd1 = new SqlCommand(sql, objconnection);
                string NetworkName = cmd1.ExecuteScalar().ToString();
                objconnection.Close();
                lblNetworkName.Text = NetworkName;
                fldset1.Visible = false;
                lblGrantAgency.Visible = false;
                lblGrantNumber.Visible = false;
                lblErrorMessage.Visible = false;
                objconnection.Close();
                FillDataTable();
                LoadGridData();
            } else {
                grantAgency = Convert.ToString(Request.QueryString["GrantAgency"]);
                grantNumber = Convert.ToString(Request.QueryString["GrantNumber"]);
                string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
                SqlConnection objconnection = new SqlConnection(connectionstring);
                string sql1 = "Select NetworkId from sources where SourceId in (select SourceId from sourcefunding where GrantAgency = '" + grantAgency + "' and grantNumber = " + grantNumber + ")";
                objconnection.Open();
                SqlCommand cmd = new SqlCommand(sql1, objconnection);
                NetworkId = cmd.ExecuteScalar().ToString();
                if (NetworkId != null) {
                    String sql = "select NetworkTitle from HISNetworks where networkid = " + Convert.ToInt32(NetworkId);
                    SqlCommand cmd1 = new SqlCommand(sql, objconnection);
                    string NetworkName = cmd1.ExecuteScalar().ToString();
                    objconnection.Close();
                    lblNetworkName.Text = NetworkName;
                    lblGrantAgency.Text = "Agency = " + grantAgency;
                    lblGrantNumber.Text = "Grant Number = " + grantNumber;
                    lblErrorMessage.Visible = false;
                    objconnection.Close();
                    FillDataTable();
                    LoadGridData();
                } else {
                    lblNetworkName.Visible = false;
                    lblGrantAgency.Visible = false;
                    lblGrantNumber.Visible = false;
                    lblErrorMessage.Text = "Sorry no data found !!";
                }
            }
        }

    }
    private void LoadGridData() {

        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new SqlConnection(connectionstring);
        try {

           
            string query = "select SourceId,dbo.fun_GetSourceName(SourceId)AS [Source],count(distinct sourceId)AS TotalSources," +
                            " count(distinct variableid)As TotalVariables,count(distinct siteid)As TotalSites,sum(valuecount)As TotalValues," +
                             " count(*)As TotalSeries from seriesCatalog where networkId = " + NetworkId + " group by sourceId Order By SourceId";
            SqlCommand objsqlcommand1 = new SqlCommand(query, objconnection);
            objsqlcommand1.CommandType = CommandType.Text;

            SqlDataAdapter objAdapter = new SqlDataAdapter(objsqlcommand1);
            DataSet objDataSet = new DataSet();
            objconnection.Open();
            objAdapter.Fill(objDataSet);
            objconnection.Close();
            DataTable dt = objDataSet.Tables[0];
            gvSourceDetails.DataSource = dt;

            gvSourceDetails.DataBind();
        } catch { }
    }
    private void FillDataTable() {
        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new SqlConnection(connectionstring);
        try {

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

        } catch (Exception ex) {
            throw (ex);
        } finally {
            if (objconnection.State == ConnectionState.Open)
                objconnection.Close();
            objDataTableGrant.Dispose();
        }
    }

}