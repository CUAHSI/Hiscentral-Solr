using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SearchGrants : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void search_Click(object sender, EventArgs e)
    {
        string grantAgency = ddpGrantAgency.SelectedItem.Value;
        string grantNumber = txtGrantNumber.Text;
        if (!string.IsNullOrEmpty(grantNumber))
            Response.Redirect("~/ShowGrants.aspx?agency=" + grantAgency + "&number=" + grantNumber + "");
    }
}