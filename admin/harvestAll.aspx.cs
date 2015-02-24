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
using System.Threading;
using System.IO;

public partial class HarvestAll : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string fileName = "HARVEST_ALL_LOG.htm";
        string sourcePath = @"C:\HealthQuery";
        //C:\ScheduleHarvester\HarvestAll\logs
        string targetPath = Server.MapPath("~/admin/harvester/harvestalllog");

        // Use Path class to manipulate file and directory paths. 
        string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
        string destFile = System.IO.Path.Combine(targetPath, fileName);

        // To copy a folder's contents to a new location: 
        // Create a new target folder, if necessary. 
        
        if (!System.IO.Directory.Exists(targetPath))
        {
            System.IO.Directory.CreateDirectory(targetPath);
        }

        // To copy a file to another location and  
        // overwrite the destination file if it already exists.
        System.IO.File.Copy(sourceFile, destFile, true);

        HyperLink1.NavigateUrl =  "harvester/harvestalllog/HARVEST_ALL_LOG.htm";
        
        pnlWait.Visible = false;
    }
    //protected void Button1_Click(object sender, EventArgs e)
    //{ //log("Begin Harvest for service ID:" + SourceID);
    //    //log(System.DateTime.Now.ToString());
    //    //  String sql = "SELECT NetworkID, NetworkName, FrequentUpdates FROM HISNetworks WHERE (FrequentUpdates = 1) AND (IsPublic = 1)";
    //    //    String URL = " ";
    //    //   // Console.WriteLine(sql);
    //    //    String Organization = " ";
    //    //    DataSet ds = new DataSet();
    //    //    SqlConnection con = new SqlConnection(this.SqlDataSource1.ConnectionString);

    //    //    using (con)
    //    //    {
    //    //      SqlDataAdapter da = new SqlDataAdapter(sql, con);
    //    //      da.Fill(ds, "URL");
    //    //    }
    //    //    con.Close();
    //    //    //should be only one
    //    //    String args = "";
    //    //    String name, networkid;
    //    //    foreach (DataRow dataRow in ds.Tables["URL"].Rows)
    //    //    {
    //    //      //URL = dataRow["Link"].ToString();
    //    //      networkid = dataRow[0].ToString();
    //    //      name = dataRow[1].ToString();
    //    //      args += networkid + " ";


    //    //    }

    //    //    //string logfilename = sourceid + "_"+datestring + ".txt";

    //    //  //string harvesterPath = "C:/Documents and Settings/whitenac/My Documents/Visual Studio 2005/Projects/HISCentralHarvester/HISCentralHarvester/bin/Debug/HISCentralHarvester.exe";
    //    //  string harvesterPath = "c:\\hydroseek\\Harvester\\HISCentralHarvester.exe";
    //    //  System.Diagnostics.Process.Start(harvesterPath, args);

    //    //}

    //        //string sourceid = Session["NetworkID"].ToString();
    //    //Harvester h = new Harvester(sourceid);
    //    //h.HarvestNetwork();

    //    //Response.Redirect("../network.aspx      string datestring = begdate.Year.ToString();
    //    DateTime begdate = DateTime.Now;
    //    string datestring = begdate.Year.ToString();
    //    datestring += (begdate.Month < 10 ? "0" + begdate.Month.ToString() : begdate.Month.ToString());
    //    datestring += (begdate.Day < 10 ? "0" + begdate.Day.ToString() : begdate.Day.ToString());
    //    string logfilename = "HarvestAll" + "_" + datestring + ".htm";
    //    try {
    //        var ho = new HarvestorObject {
    //            //NetworkId = sourceid,
    //            FilePath = Server.MapPath("~/admin/harvester/HisCentralHarvester2.exe")
    //            //FilePath = @"C:\inetpub\wwwroot\hiscentral\admin\harvester\HisCentralHarvester2.exe"
    //           // FilePath = @"C:\inetpub\wwwroot\hiscentralstaging.cuahsi.org\admin\harvester\HisCentralHarvester2.exe"

    //        };
    //        ThreadPool.QueueUserWorkItem(new WaitCallback(LaunchHarvester), ho);
    //        Configuration conf = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);   //"/CentralHIS2"
    //        if (conf == null) throw new Exception("Unable to read web config");
    //        if (conf.AppSettings.Settings["HarvesterLogFolder"] == null) throw new Exception("Unable to find HarvestorLogFolder in AppSettings");
    //        string folder = conf.AppSettings.Settings["HarvesterLogFolder"].Value;
    //        string virtualDirectory = Request.ApplicationPath == "/" ? "/" : Request.ApplicationPath + "/";
    //        //this.HyperLink1.NavigateUrl = virtualDirectory + "admin/" + folder + "/" + logfilename;
    //        //this.HyperLink1.NavigateUrl = virtualDirectory + "admin/" + folder + "/" + "HARVEST_ALL_LOG_20140423";
    //        //this.HyperLink1.Visible = true;HARVEST_ALL_LOG_20140423
    //        pnlWait.Visible = false;
    //    } catch (Exception ex) {
    //        throw new Exception(ex.Message);
    //    }
    //}
    
    //public class HarvestorObject {
    //    public string NetworkId { get; set; }
    //    public string FilePath { get; set; }
    //}

    //public static void LaunchHarvester(object networkId) {
    //    var ho = (HarvestorObject)networkId;
    //    System.Diagnostics.Process pr = new System.Diagnostics.Process();
    //    pr.StartInfo.FileName = ho.FilePath;
    //    pr.StartInfo.Arguments = "RUNALL";
    //    //pr.StartInfo.FileName = Server.MapPath("harvester/HisCentralHarvester2.exe");
    //    //pr.StartInfo.FileName = @"E:\Marie\WebApp\hiscentral3.cuahsi.org\hiscentral3.cuahsi.org\admin\harvester\HisCentralHarvester2.exe";
    //    pr.Start();
    //    while (pr.HasExited == false)
    //        if ((DateTime.Now.Second % 5) == 0) {
    //            System.Threading.Thread.Sleep(1000);
    //        }

    //}
 
}
