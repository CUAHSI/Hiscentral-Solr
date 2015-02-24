using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class admin_deletenetwork : System.Web.UI.Page {
   public string NetworkName = "";
   public string id = "";
    protected void Page_Load(object sender, EventArgs e) {
        if (Session["NetworkID"] != null)
        {
            id = Session["NetworkID"].ToString();
            string connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connect);            
            con.Open();
            string sqlQuery = "Select NetworkName From HisNetworks Where NetworkId = " + id ;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlQuery;
            NetworkName = cmd.ExecuteScalar().ToString();
            con.Close();
        }
    }
    protected void btnNo_Click(object sender, EventArgs e) {
        Response.Redirect("admin.aspx");
    }

    //Delete network + harvested data
    protected void btnYes_Click(object sender, EventArgs e) {
        string networkid = "";
        if (Session["NetworkID"] != null) {
            networkid = Session["NetworkID"].ToString();
        } else {
            Response.Redirect("admin.aspx");
        }
        selectNetworkDetails(networkid);
        deleteHarvestedData(networkid);
        string connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connect);
        con.Open();
        var deleteSql = new StringBuilder();
        deleteSql.Append(String.Format("DELETE FROM seriesCatalog WHERE NetworkId = '{0}'; ", networkid));
        deleteSql.Append(String.Format("DELETE FROM sites WHERE NetworkID = '{0}'; ", networkid));
        deleteSql.Append(String.Format("DELETE FROM Variables WHERE NetworkID = '{0}'; ", networkid));
        deleteSql.Append(String.Format("DELETE from sourceFunding where SourceId in (select SourceId from Sources where NetworkId = '{0}');", networkid));
        deleteSql.Append(String.Format("DELETE FROM Variables WHERE NetworkID = '{0}'; ", networkid));
        deleteSql.Append(String.Format("DELETE FROM Sources WHERE NetworkID = '{0}'; ", networkid));
        deleteSql.Append(String.Format("DELETE FROM HISNetworks WHERE NetworkID = '{0}'; ", networkid));
        SqlCommand command = new SqlCommand(deleteSql.ToString(), con);
        command.CommandTimeout = 0;
        command.ExecuteNonQuery();
        command.Dispose();
        con.Close();
        Response.Write("<script>alert('Succesfully deleted Network = " + NetworkName + ",& Networkid= " + id + "'); window.location.href = 'admin.aspx'</script>"); 
        
    }

    //Delete harvested data only
    protected void Button1_Click(object sender, EventArgs e) {
        string networkid = "";
        if (Session["NetworkID"] != null) {
            networkid = Session["NetworkID"].ToString();
        } else {
            Response.Redirect("admin.aspx");
        }
        deleteHarvestedData(networkid);

        Response.Write("<script>alert('Succesfully deleted Network = " + NetworkName + ",& Networkid= " + id + "'); window.location.href = 'admin.aspx'</script>");      
    }
    private void deleteHarvestedData(string networkid) {
        string connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connect);
        con.Open();
        var deleteSql = new StringBuilder();
        deleteSql.Append(String.Format("DELETE FROM seriesCatalog_Stage WHERE NetworkID = '{0}'; ", networkid));
        deleteSql.Append(String.Format("DELETE FROM sites_Stage WHERE NetworkID = '{0}'; ", networkid));
        deleteSql.Append(String.Format("DELETE FROM Variables_Stage WHERE NetworkID = '{0}'; ", networkid));
        deleteSql.Append(String.Format("delete mappingsapproved from mappingsapproved  join variables on variables.variableid=mappingsapproved.variableID WHERE variables.networkid = '{0}';", networkid));
        deleteSql.Append(String.Format("DELETE FROM Sources_Stage WHERE NetworkID = '{0}'; ", networkid)); 
        SqlCommand command = new SqlCommand(deleteSql.ToString(), con);
        command.CommandTimeout = 0;
        command.ExecuteNonQuery();
        command.Dispose();
        con.Close();
    }

    private void selectNetworkDetails(string networkid)
    {
        string connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connect);
        con.Open();
        String insertNetwork = "INSERT INTO DeletedNetworks SELECT * FROM HISNETWORKS WHERE NetworkID = " + networkid;
        SqlCommand command = new SqlCommand(insertNetwork.ToString(), con);
        command.CommandTimeout = 0;
        command.ExecuteNonQuery();
        command.Dispose();
        con.Close();

    }
}
