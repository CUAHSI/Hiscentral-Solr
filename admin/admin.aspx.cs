using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class admin_harvesting : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        if (!User.IsInRole("Administrator")) Response.Redirect("../Default.aspx");
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e) {

        if (e.CommandName == "Harvest") {
            int index = int.Parse(e.CommandArgument.ToString());
            GridView1.SelectedIndex = index;
            Session["NetworkID"] = this.GridView1.SelectedValue;
            Response.Redirect("~/admin/harvest.aspx");
        } else if (e.CommandName == "ChangeOwner") {
            int index = int.Parse(e.CommandArgument.ToString());
            GridView1.SelectedIndex = index;
            Session["NetworkID"] = this.GridView1.SelectedValue;
            Response.Redirect("~/admin/netowner.aspx");
        } else if (e.CommandName == "Details") {
            int index = int.Parse(e.CommandArgument.ToString());
            GridView1.SelectedIndex = index;
            Session["NetworkID"] = this.GridView1.SelectedValue;
            Response.Redirect("network.aspx");
        } else if (e.CommandName == "Delete") {
            int index = int.Parse(e.CommandArgument.ToString());
            GridView1.SelectedIndex = index;
            Session["NetworkID"] = this.GridView1.SelectedValue;
            Response.Redirect("deletenetwork.aspx");
        }
        else if (e.CommandName == "Suspend") {
            string[] arg = new string[3];
            arg = e.CommandArgument.ToString().Split(';');
            int NetworkId = Convert.ToInt32(arg[0]);
            bool  Ispublic = Convert.ToBoolean(arg[1]);
            string NetworkName = Convert.ToString(arg[2]);
            SuspendNetworks(NetworkId,Ispublic);
            if (Ispublic == true) {
                Response.Write("<script>alert('Network " + NetworkName + " Suspended Successfully !'); window.location.href = 'admin.aspx'</script>");
            } else {
                Response.Write("<script>alert('Network " + NetworkName + " Restored Successfully !'); window.location.href = 'admin.aspx'</script>");

            }

        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e) {

    }
    protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e) {
        if (e.Row.RowType == DataControlRowType.DataRow) {
            
            if (e.Row.Cells[8].Text == "Not Harvested") e.Row.Cells[8].CssClass = "hint";
           
            //if (((CheckBox)e.Row.Cells[6].Controls[0]).Checked == false) e.Row.CssClass = "ChangeColor";

        }
    }
    protected void SuspendNetworks(int NetworkId,bool IsPublic) {
        string conn = "";
        conn = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ToString();
        SqlConnection objsqlconn = new SqlConnection(conn);
        objsqlconn.Open();
        SqlCommand objcmd = new SqlCommand();
        if (IsPublic == true) {
             objcmd = new SqlCommand("Update HisNetworks Set Ispublic = 0 Where NetworkId = " + NetworkId + "", objsqlconn);
        } else {
             objcmd = new SqlCommand("Update HisNetworks Set Ispublic = 1 Where NetworkId = " + NetworkId + "", objsqlconn);
        }
        objcmd.ExecuteNonQuery();
        objsqlconn.Close();

    }
}
