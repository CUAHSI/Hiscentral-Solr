using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class ReportVisualization : System.Web.UI.Page {
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
        public List<ChartObject> Series3 = new List<ChartObject>();
    }
    public string NetworkName = String.Empty;
    private List<ChartObject> ConvertToFlot(List<DataAccess_Logging.DataAccess_StatisticModel> input) {
        var output = new List<ChartObject>();
        var startDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        input.ForEach(x => {
            DateTime cleanDate = new DateTime(1990, 1, 1, 0, 0, 0);
            if (DateTime.TryParse(x.Date, out cleanDate)) {
                output.Add(new ChartObject {
                    Time = (new DateTime(cleanDate.Year, cleanDate.Month, cleanDate.Day) - startDate).TotalMilliseconds,
                    Value = Convert.ToDouble(x.Count)
                });
            } else throw new Exception("Unable to convert string to datetime");

        });
        return output;
    }

    protected void Page_Load(object sender, EventArgs e) {
       if(!Page.IsPostBack) {
           hdnNetworkName.Value = Server.UrlDecode(Request.QueryString["n"]);
       }

        if (Request.QueryString["q"] == "getstats") {
            var result = new AjaxResponse { ErrorCode = 0 };
            var networkName = Server.UrlDecode(Request.QueryString["n"]);
            lblNetworkName.Text = networkName;
            //Testing - remove the line below when project goes live!!!!
            NetworkName = networkName = "LittleBearRiver";
            var s1 = DataAccess_Logging.GetMultiColumnStat1_ByMonth(networkName);
            var s2 = DataAccess_Logging.GetMultiColumnStat2_ByMonth(networkName);
            var s3 = DataAccess_Logging.GetMultiColumnStat3_ByMonth(networkName);

            //var items = (from t in db.Transactions group t by new { t.transactionDate.Month, t.transactionDate.Year } into g
            //             select new { Month = g.Key.Month, Year = g.Key.Year, Day = 1, moneyMade = g.Sum(x => x.moneyMade) }).ToList();
            //var groupS1 = s1.Select(x => new { Month = Convert.ToDateTime(x.Date).Month, Year = Convert.ToDateTime(x.Date).Year, Day = 1, 

            try {
                result.Data = new MultiSeriesWrapper { Series1 = ConvertToFlot(s1), Series2 = ConvertToFlot(s2), Series3 = ConvertToFlot(s3) };
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

    }
}