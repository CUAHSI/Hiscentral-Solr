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

public partial class admin_CatalogDetailsPopup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {   
        DataTable table = new DataTable();
        using (var connect = new SqlConnection(ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString))
        using (var cmd = new SqlCommand("select TOP(1) * from catalogstatistics order by catalogstatisticsid  desc", connect))
        using (var da = new SqlDataAdapter(cmd))
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            da.Fill(table);
        }
      var data = ConvertDataTableToHTML(table);
      appendTable.InnerHtml = data;
    }

    public static string ConvertDataTableToHTML(DataTable dt)
    {
        StringBuilder html = new StringBuilder();
        html.Append("<table>");
        //add header rows
        html.Append("<tr>");
        for (int i = 0; i < dt.Columns.Count; i++)
            if (i != 0)
            {
                html.Append("<th>" + dt.Columns[i].ColumnName + "</th>");
                
            }
        html.Append("</tr>");
        //add rows
        for (int i = 0; i < dt.Rows.Count; i++)
        {
                html.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                    if (j != 0)
                    {
                        html.Append("<td>" + dt.Rows[i][j].ToString() + "</td>");
                    }
                   
                html.Append("</tr>");
            
        }
        html.Append("</table>");
        return html.ToString();
    }
}