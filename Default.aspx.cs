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
using System.Text;

public partial class _Default : System.Web.UI.Page
{
   public string count = "";
  protected void Page_Load(object sender, EventArgs e)
  {
      string sql = "SELECT count(*) as count from HISNETWORKS where IsPublic = 1 and EarliestRec is not null and LatestRec is not  null";

    DataSet ds = new DataSet();
    SqlConnection con = new SqlConnection(this.SqlDataSource1.ConnectionString);

    using (con)
    {
      SqlDataAdapter da = new SqlDataAdapter(sql, con);
      da.Fill(ds, "count");
    }

    con.Close();
    if (ds.Tables["count"].Rows.Count > 0)
    {
      DataRow row = ds.Tables["count"].Rows[0];
      //NetworkTitle
       count = row[0].ToString();
      //this.lblCount.Text = count + " registered services";

    }
    DataTable table = new DataTable();
    using (var connect = new SqlConnection(ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString))
    using (var cmd = new SqlCommand("select TOP(1) * from catalogstatistics order by catalogstatisticsid  desc", connect))
    using (var da = new SqlDataAdapter(cmd))
    {
        cmd.CommandType = CommandType.Text;
        cmd.CommandTimeout = 0;
        da.Fill(table);
    }
    var data = ConvertDataTableToHTML(table,count);
    appendTable.InnerHtml = data;
  }

  public static string ConvertDataTableToHTML(DataTable dt,string count)
  {
      StringBuilder html = new StringBuilder();
      html.Append("<table>");
      //add header rows
      html.Append("<tr>");
      html.Append("<th>Number of Data Sources</th>");
      html.Append("<th>Properties Measured</th>");      
      html.Append("<th>Number of Time Series</th>");
      html.Append("<th>Number of Locations</th>");
      html.Append("<th>Number of Observations</th>");
      html.Append("<th>Last Updated on</th>");
      html.Append("</tr>");
      //add rows
      for (int i = 0; i < dt.Rows.Count; i++)
      {
          html.Append("<tr>");
          for (int j = 0; j < dt.Columns.Count; j++){
              
                  if (j == 0) {
                      html.Append("<td><a href='pub_services.aspx' style='cursor:pointer;'>" + count + "</a></td>");
                  }
                  if (j == 1 || j==3 || j== 4 || j==5) {
                      html.Append("<td>" + Convert.ToDecimal(dt.Rows[i][j]).ToString("#,##0") + "</td>");
                  }
                  if (j == 6) {
                      html.Append("<td>" + Convert.ToDateTime(dt.Rows[i][j]).ToString("MM/dd/yyyy") + "</td>");
                  }                 
                  
              }

          html.Append("</tr>");

      }
      html.Append("</table>");
      return html.ToString();
  }
}
