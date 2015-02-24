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

public partial class public_network : System.Web.UI.Page
{
    public string networkid;
    public long newSitesCounts;
    public long newVariablesCounts;
    public long newValuesCounts;
    public long ChangesSiteCounts;
    public long ChangesVariableCounts;
    public long ChangesValueCounts;

    protected void Page_Load(object sender, EventArgs e)
    {
       networkid = Request.Params.Get("n");
      if (networkid == null)
      {
        if (Session["NetworkID"] == null)
        {
          Response.Redirect("default.aspx");
        }
        else { 
          networkid = Session["NetworkID"].ToString();
        }
        
      }
      //DateTime maxd = getMaxEnddate(networkid);

      Session["NetworkID"] = networkid;
      string NETWORK = " ";
      string sql = "SELECT username, ServiceWSDL, ServiceAbs, NetworkName, ContactName, ContactEmail, ContactPhone, Organization, website, IsPublic, SupportsAllMethods, Citation, MapIconPath, OrgIconPath, DATEADD(hour, 5, LastHarvested) as LastHarvested,FrequentUpdates, Xmin, Xmax, Ymin, Ymax, ValueCount, VariableCount, SiteCount, EarliestRec, LatestRec, ServiceStatus, ProjectStatus, NetworkTitle, NetworkID, CreatedDate FROM HISNetworks WHERE (NetworkID = " + networkid + ")";


      DataSet ds = new DataSet();
      SqlConnection con = new SqlConnection(this.SqlDataSource1.ConnectionString);
      

      using (con)
      {
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        da.Fill(ds, "NETWORK");
      }

      con.Close();
      if (ds.Tables["NETWORK"].Rows.Count > 0) {
        DataRow row = ds.Tables["NETWORK"].Rows[0];
        //NetworkTitle
        string citation = row["Citation"].ToString();
        if (citation != null && citation != "") {
          this.litCit_Pubs.Text = "<H3>Citation</H3><BR>";
          this.litCit_Pubs.Text += citation; 
          this.litCit_Pubs.Text += "<BR><BR>";
        }
        this.lblContactEmailLabel.Text = row["ContactEmail"] == DBNull.Value ? String.Empty : row["ContactEmail"].ToString();
        this.lblContactNameLabel.Text = row["ContactName"] == DBNull.Value ? String.Empty : row["ContactName"].ToString();
        this.lblContactPhoneLabel.Text = row["ContactPhone"] == DBNull.Value ? String.Empty : row["ContactPhone"].ToString();
        string networkName = row["NetworkName"] == DBNull.Value ? String.Empty : row["NetworkName"].ToString();
        this.lblNetworkNameLabel.Text = networkName;
        this.imgIcon.ImageUrl = "getIcon.aspx?name=" + networkName;
        this.imgLogo.ImageUrl = "getLogo.aspx?name=" + networkName;
        this.lblNetworkTitle.Text = row["NetworkTitle"] == DBNull.Value ? String.Empty : row["NetworkTitle"].ToString();
        this.lblOrganizationLabel.Text = row["Organization"] == DBNull.Value ? String.Empty : row["Organization"].ToString();
        this.lblServiceWSDLLabel.Text = row["ServiceWSDL"] == DBNull.Value ? String.Empty : row["ServiceWSDL"].ToString();
        string updated = row["FrequentUpdates"] == DBNull.Value ? String.Empty : row["FrequentUpdates"].ToString();

        this.litDesc.Text += "<H3>Abstract</H3><BR>";
        this.litDesc.Text += row["ServiceAbs"] == DBNull.Value ? String.Empty : row["ServiceAbs"].ToString();
        this.litDesc.Text += "<BR><BR>";
        //String networkid = row["NetworkID"].ToString();
        //DateTime maxd = DateTime.Parse(row["LatestRec"].ToString());
        DateTime maxd = new DateTime();
        DateTime.TryParse(row["LatestRec"].ToString(), out maxd);
        DateTime lastHarvestDate = new DateTime();
         DateTime.TryParse(row["LastHarvested"].ToString(), out lastHarvestDate);
        TimeSpan dif = lastHarvestDate - maxd;
        bool isCurrent=false;
        if (dif.Days < 3 || networkid=="3") isCurrent = true;

        string lastHarvested = row["LastHarvested"].ToString();
        if (lastHarvested != null && lastHarvested != "") {
            this.lblLastHarvested.Text = "<b>Last Harvested on </b>" + lastHarvested + "(UTC)";
        }
        //if (updated == "True")
        //{
        //    this.lblLastHarvested.Text += "   <b>(Updated weekly,   ";
        //}
        //else {
        //    this.lblLastHarvested.Text += "   <b>(";
        //}
        //if (isCurrent)
        //{
        //    this.lblLastHarvested.Text += " assumed current)</b>";
        //}
        //else {
        //    this.lblLastHarvested.Text += " assumed static)</b>";
        //}
        // == DBNull.Value ? String.Empty : row["ServiceAbs"].ToString();
        //this.ServiceAbsLabel.Text = row["ServiceAbs"].ToString(); 
        this.lblSiteCount.Text = row["SiteCount"] == DBNull.Value ? "0" : Convert.ToDouble(row["SiteCount"]).ToString("#,##0");
        this.lblVariableCount.Text = row["VariableCount"] == DBNull.Value ? "0" : Convert.ToDouble(row["VariableCount"]).ToString("#,##0");
        this.lblValueCount.Text = row["ValueCount"] == DBNull.Value ? "0" : Convert.ToDouble(row["ValueCount"]).ToString("#,##0");
        //newSitesCounts = Convert.ToInt64(row["SiteCount"]);
        //newVariablesCounts = Convert.ToInt64(row["VariableCount"]);
        //newValuesCounts = Convert.ToInt64(row["ValueCount"]);

        this.lblNorth.Text = row["Ymax"] == DBNull.Value ? String.Empty : row["Ymax"].ToString();
        this.lblSouth.Text = row["Ymin"] == DBNull.Value ? String.Empty : row["Ymin"].ToString();
        this.lblEast.Text = row["Xmax"] == DBNull.Value ? String.Empty : row["Xmax"].ToString();
        this.lblWest.Text = row["Xmin"] == DBNull.Value ? String.Empty : row["Xmin"].ToString();

        string mapscript = "<script type='text/javascript'>xmin = " + (row["Xmin"].ToString()) + ";" +
          "xmax = " + (row["Xmax"].ToString()) + ";" +
          "ymin = " + (row["Ymin"].ToString()) + ";" +
          "ymax = " + (row["Ymax"].ToString()) + ";" +
          "</script>";
        RegisterClientScriptBlock("mapscript", mapscript);
        if (Page.User.IsInRole("Administrator") || Page.User.Identity.Name == row["username"].ToString()) {
          this.lnkEdit.Visible = true;
        }
        
      
      }
      AddLinks(networkid);
      AddDescriptions(networkid);
      OldStatistics(networkid);
      DailyHealthdetails(networkid);

    }

    protected void DailyHealthdetails(string networkId) {

        string sql = "SELECT IsSuccess,LastTestDate from HealthMonitor WHERE NetworkId = " + networkId;


        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(this.SqlDataSource1.ConnectionString);


        using (con) {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(ds, "HealthMonitor");
        }
        //Literal lit;
        //Label lbl;
        con.Close();
        if (ds.Tables["HealthMonitor"].Rows.Count >= 1) {
            DataRow row = ds.Tables["HealthMonitor"].Rows[0];
            bool IsSuccess = Convert.ToBoolean(row["IsSuccess"]);//LastTestDate

            if (!IsSuccess) {
                this.imgSuccess.ImageUrl = "~/images/error.jpg";
            } else {
                this.imgSuccess.ImageUrl = "~/images/sucess.jpg";
            }

            this.lblImgText.Text = "Download last tested on " + Convert.ToDateTime(row["LastTestDate"]).ToString("MM/dd/yyyy");
        } else {
            this.imgSuccess.ImageUrl = "~/images/nodata.png";
            this.lblImgText.Text = "Daily health monitor not yet harvested for this network";
        
        }
    }

    protected void FormView1_PageIndexChanging(object sender, FormViewPageEventArgs e)
    {

    }
    private DateTime getMaxEnddate(string networkid) {
        string NETWORK = " ";
        string sql = "SELECT max(enddatetime) from seriesCatalog WHERE (sourceiD = " + networkid + ")";


        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(this.SqlDataSource1.ConnectionString);


        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(ds, "maxdate");
        }
        //Literal lit;
        //Label lbl;
        con.Close();
        DateTime d = new DateTime(1800, 1, 1) ;
        if (ds.Tables["maxdate"].Rows.Count>=1){
            DataRow row = ds.Tables["maxdate"].Rows[0];
            d = DateTime.Parse(row[0].ToString());

        }
        return d;
       
    }
  private void AddDescriptions(string networkid){
          Session["NetworkID"] = networkid;
      string NETWORK = " ";
      string sql = "SELECT descTitle, descText FROM NetworkDesc WHERE (NetworkID = "+networkid+")";


      DataSet ds = new DataSet();
      SqlConnection con = new SqlConnection(this.SqlDataSource1.ConnectionString);
      

      using (con)
      {
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        da.Fill(ds, "NETWORK");
      }
      //Literal lit;
      //Label lbl;
      con.Close();
    string title, text;
    for (int i=0;i<ds.Tables["NETWORK"].Rows.Count;i++){
           DataRow row = ds.Tables["NETWORK"].Rows[i];
           title = row["DescTitle"].ToString();
           text = row["DescText"].ToString();
           this.litDesc.Text += "<H3>" + title + "</H3><BR>";
           this.litDesc.Text += text;
           this.litDesc.Text += "<BR><BR>";
            
        
     }
  }
    private void AddLinks(string networkid){
          Session["NetworkID"] = networkid;
      string NETWORK = " ";
      string sql = "SELECT linkText, linkURL FROM links WHERE (NetworkID = "+networkid+")";
      

      DataSet ds = new DataSet();
      SqlConnection con = new SqlConnection(this.SqlDataSource1.ConnectionString);
      

      using (con)
      {
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        da.Fill(ds, "NETWORK");
      }

      con.Close();
    string  text, url;
    string link;
    int count = ds.Tables["NETWORK"].Rows.Count;
    if (count > 0) {
      this.litCit_Pubs.Text += "<H3>Documents and Publications</H3><BR>";
      for (int i = 0; i < ds.Tables["NETWORK"].Rows.Count; i++)
      {
        DataRow row = ds.Tables["NETWORK"].Rows[i];
        text = row["linkText"].ToString();
        url = row["linkURL"].ToString();
        link = "<a href=" + url + " target=_new>" + text + "</a><br>";
        litCit_Pubs.Text += link;

      }
    }
  }
    private void OldStatistics(string networkid) {
        Session["NetworkID"] = networkid;
        string NETWORK = " ";
        string sql = "SELECT ChangeSiteCount,ChangeVariableCount,ChangeValueCount,LastCountsUpdated FROM OldStatistics WHERE (NetworkID = " + networkid + ")";


        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(this.SqlDataSource1.ConnectionString);


        using (con) {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(ds, "OldStatistics");
        }
        con.Close();
        DateTime UpdatedDate;
        int count = ds.Tables["OldStatistics"].Rows.Count;
        if (count > 0) {

            DataRow row = ds.Tables["OldStatistics"].Rows[0];

            UpdatedDate = row["LastCountsUpdated"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastCountsUpdated"]);

            ChangesSiteCounts =row["ChangeSiteCount"] == DBNull.Value ? 0: Convert.ToInt64(row["ChangeSiteCount"]);

            ChangesVariableCounts = row["ChangeVariableCount"] == DBNull.Value ? 0 : Convert.ToInt64(row["ChangeVariableCount"]);

            ChangesValueCounts = row["ChangeValueCount"] == DBNull.Value ? 0 : Convert.ToInt64(row["ChangeValueCount"]);

            if (ChangesSiteCounts > 0) {
                lblChangesSiteCount.Text = "<b>Updated Sites Count:</b> " + ChangesSiteCounts + "   <b> Updated on:</b> " + UpdatedDate + " ";
            } else { lblChangesSiteCount.Visible = false; }
            if (ChangesVariableCounts > 0) {
                lblChangesVariableCount.Text = "<b>Updated Variables Count:</b> " + ChangesVariableCounts + "   <b> Updated on:</b> " + UpdatedDate + " ";
            } else { lblChangesVariableCount.Visible = false; }
            if (ChangesValueCounts > 0) {
                lblChangesValueCounts.Text = "<b>Updated Values Count:</b> " + ChangesValueCounts + "    <b>Updated on:</b> " + UpdatedDate + " ";
            } else { lblChangesValueCounts.Visible = false; }
        }
    }
  protected void LinkButton1_Click(object sender, EventArgs e)
  {
    //SqlDataSource1.Select = "Select NetworkName from HISNetworks where NetworkID = 
    string sourceid = Session["NetworkID"].ToString();
    String sql = "SELECT Organization from Sources WHERE sourceID = " + sourceid;
    String orgname = " ";     
    Console.WriteLine(sql);
    DataSet ds = new DataSet();
    SqlConnection con = new SqlConnection(SqlDataSource1.ConnectionString);

    using (con)
    {
      SqlDataAdapter da = new SqlDataAdapter(sql, con);
      da.Fill(ds, "ID");
    }
    con.Close();
    //should be only one
    DataRow dataRow = ds.Tables["ID"].Rows[0];
    orgname = dataRow["Organization"].ToString();
    Session["OrgName"] = orgname;

  }

  protected void lnkContributingOrg_ServerClick(object sender, EventArgs e) {
      Response.Redirect("Grants.aspx?n=" + networkid);
  }
}
