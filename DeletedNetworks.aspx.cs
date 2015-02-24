using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DeletedNetworks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.IsInRole("Administrator")) Response.Redirect("../Default.aspx");
    }
    //protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    //{

    //    if (e.CommandName == "Harvest")
    //    {
    //        int index = int.Parse(e.CommandArgument.ToString());
    //        GridView1.SelectedIndex = index;
    //        Session["NetworkID"] = this.GridView1.SelectedValue;
    //        Response.Redirect("~/admin/harvest.aspx");
    //    }
    //    else if (e.CommandName == "ChangeOwner")
    //    {
    //        int index = int.Parse(e.CommandArgument.ToString());
    //        GridView1.SelectedIndex = index;
    //        Session["NetworkID"] = this.GridView1.SelectedValue;
    //        Response.Redirect("~/admin/netowner.aspx");
    //    }
    //    else if (e.CommandName == "Details")
    //    {
    //        int index = int.Parse(e.CommandArgument.ToString());
    //        GridView1.SelectedIndex = index;
    //        Session["NetworkID"] = this.GridView1.SelectedValue;
    //        Response.Redirect("network.aspx");
    //    }
    //    else if (e.CommandName == "Delete")
    //    {
    //        int index = int.Parse(e.CommandArgument.ToString());
    //        GridView1.SelectedIndex = index;
    //        Session["NetworkID"] = this.GridView1.SelectedValue;
    //        Response.Redirect("deletenetwork.aspx");
    //    }
    //}
    //protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}
    //protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {


    //        if (e.Row.Cells[8].Text == "Not Harvested") e.Row.Cells[8].CssClass = "hint";

    //    }
    //}
}