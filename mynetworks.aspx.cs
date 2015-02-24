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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using WebSite1;

public partial class mynetworks : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        Label1.Visible = false;
        if (Context.GetOwinContext().Authentication.User.Identity == null) {
            Response.Redirect("~/Login.aspx");
        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, GridViewCommandEventArgs e) {

        int index = int.Parse(e.CommandArgument.ToString());
        GridView1.SelectedIndex = index;
        if (e.CommandName == "Select") {
            Session["NetworkID"] = this.GridView1.SelectedValue;
            Session["NetworkName"] = this.GridView1.Rows[index].Cells[3].Text;
            Response.Redirect("network.aspx");
        } else {
            Session["NetworkName"] = this.GridView1.Rows[index].Cells[3].Text;
            Session["NetworkWSDL"] = this.GridView1.Rows[index].Cells[4].Text;
            Response.Redirect("testpage.aspx");
        }



    }
    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e) {

    }
    protected void SqlDataSource1_Init(object sender, EventArgs e) {
        //if (Membership.GetUser() == null) Response.Redirect("Login.aspx");
        if (Context.GetOwinContext().Authentication.User.Identity == null) Response.Redirect("Login.aspx");
        var loggedInUser = Context.GetOwinContext().Authentication.User.Identity.Name;
            //Membership.GetUser().UserName;

        if (Request.QueryString["sa"] == "sa") {
            if (!DataAccess.SuperAdmins.Contains(loggedInUser)) {
                SqlDataSource1.SelectCommand += "Where networkID > 0" ;//AND username = '" + loggedInUser + "'";
                GridView1.Columns[3].Visible = false;
            } else {
                SqlDataSource1.SelectCommand += " WHERE networkID > 0";
            }

        } else {
            SqlDataSource1.SelectCommand += "Where networkID > 0 AND username = '" + loggedInUser + "'";
            GridView1.Columns[3].Visible = false;
           // if(SqlDataSource1.)
        }




        var ProviderName = IdentityHelper.GetProviderNameFromRequest(Request);
        var gmail = Session["gmail"] != null ? Session["gmail"].ToString() : String.Empty;


    }


    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        var arg = e.CommandArgument.ToString();
        int cleanIndex = -1;
        Int32.TryParse(arg, out cleanIndex);
        if (cleanIndex > -1) {
            GridView1.SelectedIndex = cleanIndex;
            if (e.CommandName == "Select") {
                Session["NetworkID"] = this.GridView1.SelectedValue;
                Session["NetworkName"] = this.GridView1.Rows[cleanIndex].Cells[3].Text;
                Response.Redirect("network.aspx");
            } 
        }

    }


    protected void GridView1_SelectedIndexChanged1(object sender, EventArgs e) {
        var test123 = 3232;
    }
    //protected void GridView1_DataBound(object sender, GridViewCommandEventArgs e) {
    //    if (e.Row.RowType == DataControlRowType.DataRow) {
    //        e.Row.Attributes["onClick"] = "location.href='view.aspx?id=" + DataBinder.Eval(e.Row.DataItem, "id") + "'";
    //    }
    //}
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e) {
        if (e.Row.RowType == DataControlRowType.DataRow) {
            var thirdCell = e.Row.Cells[2];
            var networkName = e.Row.Cells[4].Text;
            thirdCell.Controls.Clear();
            thirdCell.Controls.Add(new HyperLink { NavigateUrl = "~/report.aspx?n=" + Server.UrlEncode(networkName), Text = "Usage Report" });

            var secondCell = e.Row.Cells[1];
            string val = GridView1.DataKeys[e.Row.RowIndex]["NetworkID"].ToString();
            var networkId = val;
            secondCell.Controls.Clear();
            secondCell.Controls.Add(new HyperLink { NavigateUrl = "~/testpage.aspx?n=" + Server.UrlEncode(networkId), Text = "Test" });
        }
    }
    protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e) {

        if (e.AffectedRows < 1) //Here AffectedRows gives you the count of returned rows.
   {
       Label2.Visible = false;
       Label1.Visible = true;
        }

    }
}
