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
using System.Threading;

public partial class admin_Harvest : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

    }
    protected void Button1_Click(object sender, EventArgs e) {

        HarvesterRun.HarvesterRun objHarvesterRun = new HarvesterRun.HarvesterRun();

        var path = objHarvesterRun.HarvestRun();

        this.HyperLink1.NavigateUrl = path;
        this.HyperLink1.Visible = true;
        pnlWait.Visible = false;



    }
}