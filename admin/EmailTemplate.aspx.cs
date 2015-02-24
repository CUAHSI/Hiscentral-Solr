using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_EmailTemplate : System.Web.UI.Page
{
    string sortDirection, sortExpression;
    // sql connection
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString); 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            BindGridView();
        }
    }

    //method for binding GridView
    protected void BindGridView() {
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter("Select ID, Email from Emails", con);
        con.Open();
        da.Fill(ds);
        con.Close();

        if (ds.Tables.Count > 0) {
            GridView1.DataSource = ds;
            GridView1.DataBind();
            ViewState["ds"] = ds;
        }
    }

    // insert new record in database
    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        // find values for update
        TextBox txtEmail = (TextBox)GridView1.HeaderRow.FindControl("txt_Email");
 
        // insert values into databaset 
        SqlCommand cmd = new SqlCommand("insert into Emails (Email) values('" + txtEmail.Text + "')", con);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
 
        BindGridView();
    }
    //protected void btnSubmit_Click(object sender, EventArgs e) {
    //    var Emails = hdnEmails.Value;
    //}
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e) {
        Label lblId = (Label)GridView1.Rows[e.RowIndex].FindControl("lblId");
        SqlCommand cmd = new SqlCommand("delete from Emails where Id = "+Convert.ToInt32(lblId.Text.Trim())+"", con);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();

        BindGridView();

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        GridView1.PageIndex = e.NewPageIndex;
        BindGridView();
        GridView1.DataBind();
    }
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e) {
        if (ViewState["SortDirection"] == null || ViewState["SortExpression"].ToString() != e.SortExpression) {
            ViewState["SortDirection"] = "ASC";
            GridView1.PageIndex = 0;

        } else if (ViewState["SortDirection"].ToString() == "ASC") {
            ViewState["SortDirection"] = "DESC";
        } else if (ViewState["SortDirection"].ToString() == "DESC") {
            ViewState["SortDirection"] = "ASC";
        }
        ViewState["SortExpression"] = e.SortExpression;
        DataSet ds = ViewState["ds"] as DataSet;
        if (ds.Tables.Count > 0) {
            DataView dv = ds.Tables[0].DefaultView;
            if (ViewState["SortDirection"] != null) {
                sortDirection = ViewState["SortDirection"].ToString();
            }
            if (ViewState["SortExpression"] != null) {
                sortExpression = ViewState["SortExpression"].ToString();
                dv.Sort = string.Concat(sortExpression, " ", sortDirection);
            }
            GridView1.DataSource = dv;
            GridView1.DataBind();
            //     LoadGridData();
        }
    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e) {
        if (e.Row.RowType == DataControlRowType.Header) {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell = new TableCell();
            HeaderCell.Text = "Email";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Action";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);

            GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);

        } 
    }
}