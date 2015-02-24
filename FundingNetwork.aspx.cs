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

public partial class FundingNetwork : System.Web.UI.Page
{
    public DataTable objDataTable;    
    public DataSet ds;
    public int count = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        string constring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constring);
        try
        {
            if (!IsPostBack)
            {
                if (Session["NetworkID"] == null) Response.Redirect("default.aspx");

                String sql = "select NetworkTitle from HISNetworks where networkid = " + Convert.ToInt32(Session["NetworkID"]); // 52;

                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                string NetworkName = cmd.ExecuteScalar().ToString();                
                lblNetworkName.Text =NetworkName ;
                con.Close();
                LoadGridData();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
    private void LoadGridData()
    {
        try
        {            
            objDataTable = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
            SqlConnection objConnection = new SqlConnection(constring);
            objConnection.Open();
            SqlDataAdapter objAdapter = new SqlDataAdapter("Retrive_Grants", objConnection);
            objAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            objAdapter.SelectCommand.Parameters.Add("@ID", SqlDbType.Int).Value = + Convert.ToInt32(Session["NetworkID"]) ; //Convert.ToInt32(Session["NetworkID"]) //52; 
            objAdapter.Fill(objDataTable);
            objConnection.Close();
            count = objDataTable.Rows.Count;

            ViewState["ds"] = ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [WebMethod]
    public static void SubmitSource(int source, string Agency, string AgencyID, string Complete, string startdate, string enddate, string url)  //, string startdate, string enddate, string url
    {
        string constring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constring);
        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("Insert_source_funding_record_new_dummy", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Source", source);
            cmd.Parameters.AddWithValue("@Agency", Agency);
            cmd.Parameters.AddWithValue("@AgencyID", AgencyID);
            cmd.Parameters.AddWithValue("@Complete", Complete);
            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);
            cmd.Parameters.AddWithValue("@url", url);
            SqlParameter pa = new SqlParameter();
            cmd.ExecuteNonQuery();

            con.Close();
        }
        catch (Exception e)
        {
            throw (e);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }

    }


    [WebMethod]
    public static void Deleted(string GrantID)
    {
        string constring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constring);
        try
        {
            con.Open();
            SqlCommand objCommand = new SqlCommand("Delete_Grants", con);
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Parameters.AddWithValue("@GrantID", GrantID);
            SqlParameter objParameter = new SqlParameter();
            objCommand.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception e)
        {
            throw (e);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }
    }


    [WebMethod]
    public static void UpdateSource(string GID, string Agency, string AgencyID, string complete, string start, string end, string url)
    {
        string constring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constring);
        try
        {
            con.Open();
            
            SqlCommand cmd = new SqlCommand("update_source_funding_record", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Source", GID);
            cmd.Parameters.AddWithValue("@Agency", Agency);
            cmd.Parameters.AddWithValue("@AgencyID", AgencyID);
            cmd.Parameters.AddWithValue("@complete", complete);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);
            cmd.Parameters.AddWithValue("@url", url);
            SqlParameter pa = new SqlParameter();
            cmd.ExecuteNonQuery();

            con.Close();
        }
        catch (Exception e)
        {
            throw (e);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }
    }
    
    protected void b1_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }
}