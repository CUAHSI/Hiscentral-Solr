using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Report : System.Web.UI.Page {

    private static hiscentral_loggingEntities _db = new hiscentral_loggingEntities();

    public class StatisticModel {
        public string Date { get; set; }
        public string Count { get; set; }
        public string Count_Com { get; set; }
        public string Count_Edu { get; set; }
        public string Count_Org { get; set; }
        public string Count_Net{ get; set; }
        public string Count_Gov { get; set; }
        public string Others { get; set; }
    }

    public class CSVModel {
        public string Date { get; set; }
        public string Stat1 { get; set; }
        public string Stat2 { get; set; }
        public string Stat3 { get; set; }
        public string Stat4 { get; set; }
        public string Stat1_StatCom { get; set; }
        public string Stat1_StatOrg { get; set; }
        public string Stat1_StatEdu { get; set; }
        public string Stat1_StatNet { get; set; }
        public string Stat1_StatGov { get; set; }
        public string Stat2_StatCom { get; set; }
        public string Stat2_StatOrg { get; set; }
        public string Stat2_StatEdu { get; set; }
        public string Stat2_StatNet { get; set; }
        public string Stat2_StatGov { get; set; }
        public string Stat3_StatCom { get; set; }
        public string Stat3_StatOrg { get; set; }
        public string Stat3_StatEdu { get; set; }
        public string Stat3_StatNet { get; set; }
        public string Stat3_StatGov { get; set; }
        public string Stat4_StatCom { get; set; }
        public string Stat4_StatOrg { get; set; }
        public string Stat4_StatEdu { get; set; }
        public string Stat4_StatNet { get; set; }
        public string Stat4_StatGov { get; set; }
    }

    public string NetworkName = String.Empty;
    protected void LnkDownload_Click(object sender, EventArgs e) {
    

    }
    protected void Page_Load(object sender, EventArgs e) {
        var networkName = Server.UrlDecode(Request.QueryString["n"]);
        //Testing - remove the line below when project goes live!!!!
        //NetworkName = networkName = "LittleBearRiver";
        _db.Database.CommandTimeout = 0;
     var count = _db.log11Service.Where(x => x.network.Contains(networkName)).Count();

        lblNetworkName.Text = networkName;


        if (count == 0)
        {
            lblErrorMsg.Text = "No Data Available For Requested Network";
            return;
        }

        lnkReportDailyStatistics.NavigateUrl = "ReportDailyStatistics.aspx?n=" + networkName + "";

        //lnkChart.NavigateUrl = "ReportVisualization.aspx?n=" + networkName;
        if (!Page.IsPostBack) {
            hdnNetworkName.Value = Server.UrlDecode(Request.QueryString["n"]);
        }

        var s1 = DataAccess_Logging.GetMultiColumnStat1_ByMonth(networkName);
        var s2 = DataAccess_Logging.GetMultiColumnStat2_ByMonth(networkName);
        //var s3 = DataAccess_Logging.GetMultiColumnStat3_ByMonth(networkName);
        var s4 = DataAccess_Logging.GetMultiColumnStat4_ByMonth(networkName);


        #region CHART STUFF
        if (Request.QueryString["q"] == "getstats") {
            var result = new AjaxResponse { ErrorCode = 0 };
            try {
                result.Data = new MultiSeriesWrapper { Series1 = ConvertToFlot(s1), Series2 = ConvertToFlot(s2), Series4 = ConvertToFlot(s4) };
                result.Message = "OK";
            } catch (Exception ex) {
                result.ErrorCode = 9999;
                result.Message = ex.Message;

            } finally {
                Response.Clear();
                Response.Write(new JavaScriptSerializer().Serialize(result));
                Response.End();
            }

        }
        #endregion

        //var s1 = DataAccess_Logging.GetMultiColumnStat1_ByMonth(networkName);
        //var s2 = DataAccess_Logging.GetMultiColumnStat2_ByMonth(networkName);
        //var s3 = DataAccess_Logging.GetMultiColumnStat3_ByMonth(networkName);




        rptStat1.DataSource = s1;
        rptStat1.DataBind();
        rptStat2.DataSource = s2;
        rptStat2.DataBind();
        //rptStat3.DataSource = s3;
        //rptStat3.DataBind();
        rptStat4.DataSource = s4;
        rptStat4.DataBind();

        var csvList = new List<CSVModel>();
        csvList.Add(new CSVModel {
            Date = "Date",
            Stat1 = "Clients per Month",
            Stat1_StatCom = ".COM Clients per Month",
            Stat1_StatOrg = ".ORG Clients per Month",
            Stat1_StatEdu = ".EDU Clients per Month",
            Stat1_StatNet = ".NET Clients per Month",
            Stat1_StatGov = ".Gov Clients per Month",
            Stat2 = "Requests per Month",
            Stat2_StatCom = ".COM Requests per Month",
            Stat2_StatOrg = ".ORG Requests per Month",
            Stat2_StatEdu = ".EDU Requests per Month",
            Stat2_StatNet = ".NET Requests per Month",
            Stat2_StatGov = ".Gov Requests per Month",
            //Stat3 = "Values per Month",
            //Stat3_StatCom = ".COM Values per Month",
            //Stat3_StatOrg = ".ORG Values per Month",
            //Stat3_StatEdu = ".EDU Values per Month",
            //Stat3_StatNet = ".NET Values per Month",
            //Stat3_StatGov = ".Gov Values per Month",
            Stat4 = "Values per Month without log scale",
            Stat4_StatCom = ".COM Values per Month",
            Stat4_StatOrg = ".ORG Values per Month",
            Stat4_StatEdu = ".EDU Values per Month",
            Stat4_StatNet = ".NET Values per Month",
            Stat4_StatGov = ".Gov Values per Month"
        });

        s1.ForEach(x => {
            csvList.Add(new CSVModel { Date = x.Date, Stat1 = x.Count, Stat1_StatCom = x.Count_Com, Stat1_StatEdu = x.Count_Edu, Stat1_StatOrg = x.Count_Org,Stat1_StatNet = x.Count_Net,Stat1_StatGov = x.Count_Gov });
        });
        s2.ForEach(x => {
            var itemToUpdate = csvList.SingleOrDefault(y => y.Date == x.Date);
            if (itemToUpdate != null) {
                itemToUpdate.Stat2 = x.Count;
                itemToUpdate.Stat2_StatCom = x.Count_Com;
                itemToUpdate.Stat2_StatEdu = x.Count_Edu;
                itemToUpdate.Stat2_StatOrg = x.Count_Org;
                itemToUpdate.Stat2_StatNet = x.Count_Net;
                itemToUpdate.Stat2_StatGov = x.Count_Gov;
                //itemToUpdate.Stat2 = Math.Ceiling ( Convert.ToDouble(x.Count.Replace("-Infinity", "0")) / 2).ToString();
                //itemToUpdate.Stat2_StatCom = Math.Ceiling(Convert.ToDouble(x.Count_Com) /2).ToString();
                //itemToUpdate.Stat2_StatEdu = Math.Ceiling(Convert.ToDouble(x.Count_Edu) / 2).ToString();
                //itemToUpdate.Stat2_StatOrg = Math.Ceiling(Convert.ToDouble(x.Count_Org) / 2).ToString();
                //itemToUpdate.Stat2_StatNet = Math.Ceiling(Convert.ToDouble(x.Count_Net) / 2).ToString();
                //itemToUpdate.Stat2_StatGov = Math.Ceiling(Convert.ToDouble(x.Count_Gov) / 2).ToString();
            }
        });
        //s3.ForEach(x => {
        //    var itemToUpdate = csvList.SingleOrDefault(y => y.Date == x.Date);
        //    if (itemToUpdate != null) {
        //        itemToUpdate.Stat3 = x.Count.Replace("-Infinity", "0");
        //        itemToUpdate.Stat3_StatCom = x.Count_Com;
        //        itemToUpdate.Stat3_StatEdu = x.Count_Edu;
        //        itemToUpdate.Stat3_StatNet = x.Count_Net;
        //        itemToUpdate.Stat3_StatGov = x.Count_Gov;
        //    }
        //});
        s4.ForEach(x =>
        {
            var itemToUpdate = csvList.SingleOrDefault(y => y.Date == x.Date);
            if (itemToUpdate != null)
            {
                itemToUpdate.Stat4 = x.Count.Replace("-Infinity", "0");
                itemToUpdate.Stat4_StatCom = x.Count_Com;
                itemToUpdate.Stat4_StatEdu = x.Count_Edu;
                itemToUpdate.Stat4_StatOrg = x.Count_Org;
                itemToUpdate.Stat4_StatNet = x.Count_Net;
                itemToUpdate.Stat4_StatGov = x.Count_Gov;
            }
        });
        //Configuration conf = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);   //"/CentralHIS2"
        //if (conf.AppSettings.Settings["UsageReportFolder"] == null) throw new Exception("Unable to find UsageReportFolder in AppSettings");
        string folder = Server.MapPath("~/UsageReports");
            //conf.AppSettings.Settings["UsageReportFolder"].Value;
        string virtualDirectory = Request.ApplicationPath == "/" ? "/" : Request.ApplicationPath + "/";
        var fileName = "usagereport-" + Guid.NewGuid().ToString() + ".csv";

        System.IO.File.WriteAllLines(folder + "/ReportStat/" + fileName, csvList
            .Select(x => String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}", x.Date, x.Stat1, x.Stat1_StatCom, x.Stat1_StatOrg, x.Stat1_StatEdu,x.Stat1_StatNet,x.Stat1_StatGov, x.Stat2, x.Stat2_StatCom, x.Stat2_StatOrg, x.Stat2_StatEdu,x.Stat2_StatNet,x.Stat2_StatGov, x.Stat4, x.Stat4_StatCom, x.Stat4_StatOrg, x.Stat4_StatEdu,x.Stat4_StatNet,x.Stat4_StatGov)));
        lnkDownload.NavigateUrl = virtualDirectory + "UsageReports/ReportStat/" + fileName; 
           
    }
    #region CHART STUFF
    public class AjaxResponse {
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        public object Data { get; set; }
    }

    public class ChartObject {
        public double Time { get; set; }
        public double Value { get; set; }
    }

    public class MultiSeriesWrapper {
        public List<ChartObject> Series1 = new List<ChartObject>();
        public List<ChartObject> Series2 = new List<ChartObject>();
        //public List<ChartObject> Series3 = new List<ChartObject>();
        public List<ChartObject> Series4 = new List<ChartObject>();
    }

    private List<ChartObject> ConvertToFlot(List<DataAccess_Logging.DataAccess_StatisticModel> input) {
        var output = new List<ChartObject>();
        var startDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        input.ForEach(x => {
            DateTime cleanDate = new DateTime(1990, 1, 1, 0, 0, 0);
            if (DateTime.TryParse(x.Date, out cleanDate)) {
                output.Add(new ChartObject {
                    Time = (new DateTime(cleanDate.Year, cleanDate.Month, cleanDate.Day) - startDate).TotalMilliseconds,
                    Value = Convert.ToDouble(x.Count.Replace("-Infinity","0"))
                });
            } else throw new Exception("Unable to convert string to datetime");

        });
        return output;
    }
    #endregion
}