using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SourceFunding : System.Web.UI.Page
{
    System.Data.DataTable dt = null;
    string sortDirection, sortExpression;
    protected void Page_Load(object sender, EventArgs e) 
    {
        
        if (!IsPostBack)
        {
            if (Session["NetworkID"] == null) Response.Redirect("default.aspx");

            String sql = "select networkname from HISNetworks where networkid = " + Convert.ToInt32(Session["NetworkID"]);

            string constring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            string NetworkName = cmd.ExecuteScalar().ToString();
            string sourceid = Session["NetworkID"].ToString();
            lblNetworkName.Text  ="Funding For [ " + NetworkName + " ]";
            con.Close();
            LoadGridData();
        }
    }

    private void LoadGridData()
    {
        //String sql = "Select * from  sourcefunding  sf  right  join sources s  on s.SourceID =sf.SourceID   where  networkid = " + Convert.ToInt32(Session["NetworkID"]);
        String sql = "Select s.SourceID AS SourceId ,sf.*  from  sourcefunding  sf  right  join sources s  on s.SourceID =sf.SourceID   where  networkid = " + 286;
        DataSet ds = new DataSet();
        string constring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constring);

        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(ds);
        }
        con.Close();
        this.GridView1.DataSource = ds;
        ViewState["ds"] = ds;
        this.GridView1.DataBind();
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
       //LoadGridData();
    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        LoadGridData();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label lbllblGrantId = (Label)GridView1.Rows[e.RowIndex].FindControl("lblGrantId");

        Label lblSourceId = (Label)GridView1.Rows[e.RowIndex].FindControl("lblSourceId");
        TextBox txtAgency = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtAgency");
        TextBox txtAward = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtAward");
        string constring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constring);
        con.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = con;
        cmd.CommandText = "sp_InsertUpdateSourceFunding";
        cmd.Parameters.AddWithValue("@SourceID", Convert.ToInt32(lblSourceId.Text.Trim()));
        if (lbllblGrantId.Text.Trim() != String.Empty)
        {
            cmd.Parameters.AddWithValue("@GrantID", Convert.ToInt32(lbllblGrantId.Text.Trim()));
        }
        else
        {
            cmd.Parameters.AddWithValue("@GrantID", 0);
        }
        if (txtAgency.Text.Trim() != String.Empty)
        {
            cmd.Parameters.AddWithValue("@GrantAgency", Convert.ToString(txtAgency.Text.Trim()));
        }
        else
        {
            cmd.Parameters.AddWithValue("@GrantAgency", String.Empty);
        }
        if (txtAward.Text.Trim() != String.Empty)
        {
            cmd.Parameters.AddWithValue("@GrantNumber", Convert.ToString(txtAward.Text.Trim()));
        }
        else
        {
            cmd.Parameters.AddWithValue("@GrantNumber", String.Empty);
        }

        cmd.ExecuteNonQuery();

        con.Close();
        GridView1.EditIndex = -1;
        LoadGridData();
    }
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (ViewState["SortDirection"] == null || ViewState["SortExpression"].ToString() != e.SortExpression)
        {
            ViewState["SortDirection"] = "ASC";
            GridView1.PageIndex = 0;

        }
        else if (ViewState["SortDirection"].ToString() == "ASC")
        {
            ViewState["SortDirection"] = "DESC";
        }

        else if (ViewState["SortDirection"].ToString() == "DESC")
        {
            ViewState["SortDirection"] = "ASC";
        }
        ViewState["SortExpression"] = e.SortExpression;
        DataSet ds = ViewState["ds"] as DataSet;
        if (ds.Tables.Count > 0)
        {
            DataView dv = ds.Tables[0].DefaultView;
            if (ViewState["SortDirection"] != null)
            {
                sortDirection = ViewState["SortDirection"].ToString();
            }
            if (ViewState["SortExpression"] != null)
            {
                sortExpression = ViewState["SortExpression"].ToString();
                dv.Sort = string.Concat(sortExpression, " ", sortDirection);
            }
            GridView1.DataSource = dv;
            GridView1.DataBind();
            //     LoadGridData();
        }
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        LoadGridData();
        GridView1.DataBind();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {

            Label lbllblGrantId = (Label)GridView1.Rows[i].Cells[0].FindControl("lblGrantId");

            Label lblSourceId = (Label)GridView1.Rows[i].Cells[0].FindControl("lblSourceId");
            TextBox txtAgency = (TextBox)GridView1.Rows[i].Cells[0].FindControl("txtAgency");
            TextBox txtAward = (TextBox)GridView1.Rows[i].Cells[0].FindControl("txtAward");
            string constring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandText = "sp_InsertUpdateSourceFunding";
            cmd.Parameters.AddWithValue("@SourceID" ,Convert.ToInt32(lblSourceId.Text.Trim()));
            if (lbllblGrantId.Text.Trim() != String.Empty)
            {
                cmd.Parameters.AddWithValue("@GrantID", Convert.ToInt32(lbllblGrantId.Text.Trim()));
            }
            else 
            {
                cmd.Parameters.AddWithValue("@GrantID",0);
            }
            if (txtAgency.Text.Trim() != String.Empty)
            {
                cmd.Parameters.AddWithValue("@GrantAgency", Convert.ToString(txtAgency.Text.Trim()));
            }
            else
            {
                cmd.Parameters.AddWithValue("@GrantAgency", String.Empty);
            }
            if (txtAward.Text.Trim() != String.Empty)
            {
                cmd.Parameters.AddWithValue("@GrantNumber", Convert.ToString(txtAward.Text.Trim()));
            }
            else
            {
                cmd.Parameters.AddWithValue("@GrantNumber", String.Empty);
            }
                      
            cmd.ExecuteNonQuery();
           
            con.Close();
        }
    }
}