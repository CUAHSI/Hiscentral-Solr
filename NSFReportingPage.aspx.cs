using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NSF_Reporting_Page : System.Web.UI.Page
{
    public DataTable objDataTableGrant;
    public DataTable objDataTableSource;
    public int sites;
    public int variables;
    public int seriesCatalog;
    public int values;
    
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["NetworkID"] == null) Response.Redirect("default.aspx");
            string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
            SqlConnection objconnection = new SqlConnection(connectionstring);

            String sql = "select networkname from HISNetworks where networkid = " + Convert.ToInt32(Session["NetworkID"]); //52;

            objconnection.Open();
            SqlCommand cmd = new SqlCommand(sql, objconnection);
            string NetworkName = cmd.ExecuteScalar().ToString();
            string sourceid = Session["NetworkID"].ToString();
            lblNetworkName.Text = "Funding For [ " + NetworkName + " ]";
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
            //objconnection.Open();
            //SqlCommand objsqlcommand = new SqlCommand("count_from_source", objconnection);
            //objsqlcommand.CommandType = CommandType.StoredProcedure;               
                       

            //objsqlcommand.Parameters.Add("@source", SqlDbType.Int);
            //objsqlcommand.Parameters["@source"].Direction = ParameterDirection.Output;
            
            //objsqlcommand.Parameters.Add("@sourcefunding", SqlDbType.Int);
            //objsqlcommand.Parameters["@sourcefunding"].Direction = ParameterDirection.Output;
            //objsqlcommand.ExecuteNonQuery();
            //sourceCount = (int)objsqlcommand.Parameters["@source"].Value;
            //sourceFundingCount = (int)objsqlcommand.Parameters["@sourcefunding"].Value;                     

            //objdatatable = new DataTable();
            //SqlCommand objsqlcommandd = new SqlCommand("select Sources.sourceid, Sources.organization, sourcefunding.grantagency, sourcefunding.grantnumber FROM Sources JOIN sourcefunding ON Sources.sourceid = sourcefunding.sourceid", objconnection);
            //objsqlcommandd.ExecuteNonQuery();
            int NetworkID = Convert.ToInt32(Session["NetworkID"]); //52;

            objDataTableGrant = new DataTable();
            objDataTableSource = new DataTable();
            SqlCommand objsqlcommand1 = new SqlCommand("rows_source_and_funding", objconnection);
            objsqlcommand1.CommandType = CommandType.StoredProcedure;
            objsqlcommand1.Parameters.AddWithValue("@Network",NetworkID);


            SqlDataAdapter objAdapter = new SqlDataAdapter(objsqlcommand1);
            DataSet objDataSet = new DataSet();

            objconnection.Open();
            objAdapter.Fill(objDataSet);
            objconnection.Close();

            objDataTableGrant = objDataSet.Tables[2];
            sites = Convert.ToInt32(objDataTableGrant.Rows[0][0].ToString());

            objDataTableGrant = objDataSet.Tables[3];
            variables = Convert.ToInt32(objDataTableGrant.Rows[0][0].ToString());

            objDataTableGrant = objDataSet.Tables[4];
            seriesCatalog = Convert.ToInt32(objDataTableGrant.Rows[0][0].ToString());

            objDataTableGrant = objDataSet.Tables[5];
            values = Convert.ToInt32(objDataTableGrant.Rows[0][0].ToString());

            objDataTableGrant = objDataSet.Tables[0];
            objDataTableSource = objDataSet.Tables[1];
            
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
            objDataTableSource.Dispose();
        }
    }
}