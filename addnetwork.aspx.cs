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
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

public partial class addnetwork : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        var abc = HttpContext.Current.Request.Url.AbsoluteUri;
        if (!Page.User.Identity.IsAuthenticated) Response.Redirect("login.aspx");
        //this.SubmitButton.Attributes.Add("disabled", "true");
        //LoadControl//HeaderControl1_LoginView1_LoginName1
        var name = Page.User.Identity.Name;
        
    }


    protected void InsertCancelButton_Click(object sender, EventArgs e) {
        Response.Redirect("mynetworks.aspx");
    }

    protected void SubmitButton_Click(object sender, EventArgs e) {
         var validationResult = CheckIfNetworkAndWSDLNameInUse(txtNetworkName.Text, txtServiceWSDL.Text);
        if (String.IsNullOrEmpty(validationResult)) {
        ParameterCollection parms = SqlDataSource1.InsertParameters;
        parms.Clear();
        parms.Add("username", Page.User.Identity.Name);
        parms.Add("NetworkName", txtNetworkName.Text);
        parms.Add("NetworkTitle", txtNetworkTitle.Text);
        parms.Add("ServiceWSDL", txtServiceWSDL.Text);
        //parms.Add("ContactName", txtContactName.Text);
        //parms.Add("ContactEmail", txtContactEmail.Text);
        //parms.Add("ContactPhone", txtContactPhone.Text);
        //parms.Add("Organization", txtOrganization.Text);
        //parms.Add("website", txtwebsite.Text);
        //parms.Add("Citation", CitationTextBox.Text);
        parms.Add("IsPublic", "False");
        parms.Add("IsApproved", "False");
        parms.Add("CreatedDate", DateTime.Now.ToString());
        parms.Add("ServiceGroup", "3");
        parms.Add("GmailAddress", Page.User.Identity.Name);
        SqlDataSource1.Insert();
        getNetworkID();
        Response.Redirect("mynetworks.aspx");
        } else {
        lblInUse.Visible = true;
        lblInUse.Text = validationResult;
        }
    }
    private string CheckIfNetworkAndWSDLNameInUse(string networkName, string serviceWsdl) {
        var validationResult = String.Empty;
        var existingNetworks = DataAccess.GetExistingNetworkNames();
        if (existingNetworks.Contains(networkName.ToUpper())) {
            validationResult += String.Format("The network name [{0}] is already in use! ", networkName);
        }
        var existingWSDLs = DataAccess.GetExistingWSDLNames();
        if (serviceWsdl.Contains("?")) {
            if (existingWSDLs.Contains(serviceWsdl.ToUpper())) {
                validationResult += String.Format("The service WSDL URL [{0}] is already in use! ", serviceWsdl);
            }
        } else {
            var NewServiceWSDL = serviceWsdl + "?WSDL";
            if (existingWSDLs.Contains(NewServiceWSDL.ToUpper())) {
                validationResult += String.Format("The service WSDL URL [{0}] is already in use! ", serviceWsdl);
            }

        }

        return validationResult;
    }

    private void getNetworkID()
    {
        String sql = " select top(1) networkId,networkName from hisnetworks where username = '" + Page.User.Identity.Name + "' order by networkid desc";
        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(SqlDataSource1.ConnectionString);
        int netid = 0;
        string netname = String.Empty;
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(ds, "network");
        }
        con.Close();
        //should be only one
        foreach (DataRow dataRow in ds.Tables["network"].Rows)
        {
            netid = (int)dataRow["networkId"];
            netname = dataRow["networkName"].ToString();
        }
        if (netid != 0)
        {
            //mailAdmins(netid);           
            Session["NetworkID"] = netid;

            InsertNewNetworktable(netid);
            sendEmail(netname);

            Response.Redirect("network.aspx");
        }
        else
        {
            Response.Redirect("mynetworks.aspx");
        }



    }

    private void InsertNewNetworktable(int networkId)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ToString());
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO NewNetworkHarvestJob (NetworkId) VALUES (@NetworkId)";
        //Uses the FirstName variable and creates a new sql variable for use in the sql commandtext
        cmd.Parameters.Add("@NetworkId", SqlDbType.Int).Value = networkId;
        cmd.Connection = conn;
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    /// <summary>
    /// Method to send Email
    /// </summary>
    private void sendEmail(string networkname)
    {
        MailMessage m = new MailMessage();
        SmtpClient sc = new SmtpClient();
        m.From = new MailAddress("hydroseek@gmail.com", "Cuahsi HIS Central Administration");
        m.To.Add(new MailAddress(Page.User.Identity.Name));
        m.CC.Add(new MailAddress("Help@cuahsi.org"));
        m.Bcc.Add(new MailAddress("mmartin@cuahsi.org", "Marie Martin"));
        m.Subject = "Data Service is added";
        StringBuilder Sb = new StringBuilder();
        Sb.Append("<h3>Hello,</h3><br />");
        Sb.Append("<p>New data service (" + networkname + ") is registered with HIS Central. </p>");
        Sb.Append("<p>you will be notify once the harvester is started for this data service.</p>");
        Sb.Append("<p>Thank you</p>");
        m.Body = Convert.ToString(Sb);
        m.IsBodyHtml = true;
        sc.Host = "smtp.gmail.com";
        sc.Port = 587;
        sc.Credentials = new System.Net.NetworkCredential("hydroseek@gmail.com", "his4cuahsi");
        sc.EnableSsl = true;
        sc.Send(m);
    }

}