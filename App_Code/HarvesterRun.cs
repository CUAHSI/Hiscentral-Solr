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
using System.Web.Configuration;



/// <summary>
/// Summary description for HarvesterRun
/// </summary>
/// 
namespace HarvesterRun {
    public class HarvesterRun {

        public string HarvestRun() {

            string sourceid = HttpContext.Current.Session["NetworkID"].ToString();
            DateTime begdate = DateTime.Now;
            string datestring = begdate.Year.ToString();
            datestring += (begdate.Month < 10 ? "0" + begdate.Month.ToString() : begdate.Month.ToString());
            datestring += (begdate.Day < 10 ? "0" + begdate.Day.ToString() : begdate.Day.ToString());
            string logfilename = sourceid + "_" + datestring + ".htm";
            try {
                var ho = new HarvestorObject {
                    NetworkId = sourceid,
                    FilePath = HttpContext.Current.Server.MapPath("~/admin/harvester/HisCentralHarvester2.exe")
                };
               
                ThreadPool.QueueUserWorkItem(new WaitCallback(LaunchHarvester), ho);
                //Thread.Sleep(20000);
                //Configuration conf = WebConfigurationManager.OpenWebConfiguration("~/SVNCurrentHiscentealCode/");   //"/CentralHIS2"
                //if (conf == null) throw new Exception("Unable to read web config");
                //if (conf.AppSettings.Settings["HarvesterLogFolder"] == null) throw new Exception("Unable to find HarvestorLogFolder in AppSettings");
                string folder = HttpContext.Current.Server.MapPath("~/admin/harvester/logs");

                string virtualDirectory = HttpContext.Current.Request.ApplicationPath == "/" ? "/" : HttpContext.Current.Request.ApplicationPath + "/";

                var path = virtualDirectory + "admin/harvester/logs/" + logfilename;

                return path;
                
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }


        public class HarvestorObject {
            public string NetworkId { get; set; }
            public string FilePath { get; set; }
        }

        public static void LaunchHarvester(object networkId) {
            var ho = (HarvestorObject)networkId;
            System.Diagnostics.Process pr = new System.Diagnostics.Process();
            pr.StartInfo.FileName = ho.FilePath;
            pr.StartInfo.Arguments = ho.NetworkId;
            pr.Start();
            while (pr.HasExited == false)
                if ((DateTime.Now.Second % 5) == 0) {
                    System.Threading.Thread.Sleep(1000);
                }
        }
    }
}