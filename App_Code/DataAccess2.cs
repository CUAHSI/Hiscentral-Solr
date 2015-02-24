//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

///// <summary>
///// Summary description for DataAccess
///// </summary>
//public static class DataAccess
//{
//    public static List<String> SuperAdmins = new List<String> { 
//        "mmartinmass@gmail.com",    //For hiscentral2.cuahsi.org
//        "jtpollak@gmail.com",       //For hiscentral2.cuahsi.org
//        "alva.couch@gmail.com",     //For hiscentral2.cuahsi.org
//        "hiscentral3demo@gmail.com",  //For hiscentral3.cuahsi.org       
//        "xxx@gmail.com" };

//    public static void UpdateGmailAddress(string username, string gmailAddress)
//    {
//        var db = new hiscentralEntities();
//        var recordsToUpdate = db.HISNetworks.Where(x => x.username == username);
//        foreach (var record in recordsToUpdate)
//        {
//            if (record.GmailAddress != gmailAddress)
//            {
//                record.GmailAddress = gmailAddress;
//                record.username = gmailAddress;
//                db.Entry(record).State = System.Data.EntityState.Modified;
//            }
//        }
//        db.SaveChanges();
//    }

//    public static void GatherUniqueLoginsAndInitializeTokens()
//    {
//        var db = new hiscentralEntities();
//        //First delete existing data in the table if any
//        var rows = db.ExistingUserTokens;
//        foreach (var row in rows)
//        {
//            db.ExistingUserTokens.Remove(row);
//        }
//        db.SaveChanges();
//        var hisNetworkRecords = db.HISNetworks.Where(x => !String.IsNullOrEmpty(x.username)).ToList();
//        var logins = hisNetworkRecords.ConvertAll(x => x.username);
//        var distinctLogins = logins.Distinct();
//        distinctLogins.ToList().ForEach(x =>
//        {
//            var token = Guid.NewGuid().ToString();
//            var email = String.Empty;
//            var emailRecord = hisNetworkRecords.FirstOrDefault(y => y.username == x);
//            if (emailRecord != null) email = emailRecord.ContactEmail;
//            db.ExistingUserTokens.Add(new ExistingUserToken { ExistingUsername = x, GeneratedToken = token, EmailAddressOnFile = email });
//        });
//        db.SaveChanges();
//    }

//    public static string AssociateSuppliedTokenToExistingLogin(string token)
//    {
//        var login = String.Empty;
//        var db = new hiscentralEntities();
//        var existingLogin = db.ExistingUserTokens.SingleOrDefault(x => x.GeneratedToken == token);
//        if (existingLogin != null) login = existingLogin.ExistingUsername;
//        return login;
//    }

//    public static List<string> GetExistingNetworkNames()
//    {
//        return new hiscentralEntities().HISNetworks.Select(x => x.NetworkName).Distinct().ToList();
//    }

//    public static List<string> GetExistingWSDLNames()
//    {
//        return new hiscentralEntities().HISNetworks.Select(x => x.ServiceWSDL).Distinct().ToList();
//    }
//}
//public static class DataAccess_Logging
//{
//    public class DataAccess_StatisticModel
//    {
//        public string Date { get; set; }
//        public string DateString
//        {
//            get
//            {

//                DateTime d = new DateTime();
//                DateTime.TryParse(Date, out d);
//                return d.ToString("MMMM yyyy");
//            }
//        }

//        public string Count { get; set; }
//        public string Count_Com { get; set; }
//        public string Count_Edu { get; set; }
//        public string Count_Org { get; set; }
//        public DateTime GetActualDate()
//        {
//            DateTime d = new DateTime();
//            DateTime.TryParse(Date, out d);
//            return d;
//        }


//    }
//    private static List<DataAccess_StatisticModel> PadDates(bool incrementByDay, List<DataAccess_StatisticModel> inputList, string domainFilter, DateTime startingDate, DateTime endingDate)
//    {
//        var outputList = new List<DataAccess_StatisticModel>();
//        var dateCtr = startingDate;
//        while (dateCtr <= endingDate)
//        {
//            var transactionsOnDateCtr = inputList.Where(x => x.GetActualDate().Date == dateCtr);
//            if (transactionsOnDateCtr.Any())
//            {
//                outputList.AddRange(transactionsOnDateCtr);
//            }
//            else
//            {
//                var dummyItem = new DataAccess_StatisticModel { Date = dateCtr.ToShortDateString() };
//                switch (domainFilter.ToLower().Trim())
//                {
//                    case "":
//                        {
//                            dummyItem.Count = "0"; break;
//                        }
//                    case ".com":
//                        {
//                            dummyItem.Count_Com = "0"; break;
//                        }
//                    case ".org":
//                        {
//                            dummyItem.Count_Org = "0"; break;
//                        }
//                    case ".edu":
//                        {
//                            dummyItem.Count_Edu = "0"; break;
//                        }
//                }

//                //if (dummyItem.Count != "0") 
//                outputList.Add(dummyItem);
//            }
//            dateCtr = incrementByDay ? dateCtr.AddDays(1) : dateCtr.AddMonths(1);
//        }
//        return outputList;
//    }
//    private static hiscentral_loggingEntities _db = new hiscentral_loggingEntities();

//    #region FILTER BY DOMAIN NAME
//    public static List<string> GetStat1(string networkName, string domainFilter)
//    {
//        //    --he number of unique ip addresses that accessed the GetValues service for this network for each month. 
//        //--IP addresses originating from SDSC are not counted.

//        //select  YEAR(querytime) ydate, MONTH(querytime) AS mdate, COUNT(distinct (userhost))
//        //from log11service
//        //where method = 'GetValues_Start' and network = 'LittleBearRiver' and userhost not in ('xxx')
//        //group by YEAR(querytime), MONTH(querytime)



//        if (String.IsNullOrEmpty(networkName))
//        {
//            var records = MasterLogRecords.Where(x => x.network == networkName && x.method.Contains("GetValues") && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//            records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//            var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, x.Select(y => y.userhost).Distinct().Count())).ToList();
//            return result;
//        }
//        else
//        {
//            var records = MasterLogRecords.Where(x => x.network == networkName && x.method.Contains("GetValues") && x.network == networkName && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//            records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//            var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, x.Select(y => y.userhost).Distinct().Count())).ToList();
//            return result;
//        }




//    }

//    public static List<string> GetStat2(string networkName, string domainFilter)
//    {
//        //--Requests per Month: The total number of requests for this network for each month.
//        //select  YEAR(querytime) ydate, MONTH(querytime) AS mdate, COUNT(*)
//        //from log11service
//        //where method like '%_Start' and network = 'LittleBearRiver' 
//        //group by YEAR(querytime), MONTH(querytime)
//        if (String.IsNullOrEmpty(networkName))
//        {
//            var records = MasterLogRecords.Where(x => x.network == networkName && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//            records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//            var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, x.Count())).ToList();
//            return result;
//        }
//        else
//        {
//            var records = MasterLogRecords.Where(x => x.network == networkName && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//            records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//            var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, x.Count())).ToList();
//            return result;
//        }

//    }

//    public static List<string> GetStat3(string networkName, string domainFilter)
//    {

//        //--Values per Month: The total number of values retrieved from the Water One Flow webservices associated with this network. 
//        //--This chart's y axis uses a log scale. 
//        if (String.IsNullOrEmpty(networkName))
//        {
//            var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.network == networkName && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//            records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//            var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, Math.Round(Math.Log10(Convert.ToDouble(x.Select(y => y.reccount).Sum())), 2))).ToList();
//            return result;
//        }
//        else
//        {
//            var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.network == networkName && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//            records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//            var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, Math.Round(Math.Log10(Convert.ToDouble(x.Select(y => y.reccount).Sum())), 2))).ToList();

//            return result;
//        }
//    }

//    public static List<string> GetStat4(string networkName, string domainFilter)
//    {

//        //--Values per Month: The total number of values retrieved from the Water One Flow webservices associated with this network. 
//        //--This chart's y axis without a log scale. 
//        if (String.IsNullOrEmpty(networkName))
//        {
//            var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.network == networkName && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//            records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//            var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, Math.Round((Convert.ToDouble(x.Select(y => y.reccount).Sum())), 2))).ToList();
//            return result;
//        }
//        else
//        {
//            var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.network == networkName && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//            records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//            var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, Math.Round((Convert.ToDouble(x.Select(y => y.reccount).Sum())), 2))).ToList();

//            return result;
//        }
//    }

//    public static List<string> GetStat1_GroupByDay(string networkName, string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.network == networkName && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, x.querytime.Day, 0, 0, 0));

//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, x.Select(y => y.userhost).Distinct().Count())).ToList();

//        return result;
//    }

//    public static List<string> GetStat2_GroupByDay(string networkName, string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.userhost.Contains(domainFilter) && x.network == networkName).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, x.querytime.Day, 0, 0, 0));
//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, x.Count())).ToList();
//        return result;
//    }

//    public static List<string> GetStat3_GroupByDay(string networkName, string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.network == networkName && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, x.querytime.Day, 0, 0, 0));
//        //do the padding

//        //then turn them into statisticmodel lists

//        //then do the grouping
//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, Math.Round(Math.Log10(Convert.ToDouble(x.Select(y => y.reccount).Sum())), 2))).ToList();

//        return result;
//    }

//    public static List<string> GetStat4_GroupByDay(string networkName, string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.network == networkName && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, x.querytime.Day, 0, 0, 0));
//        //do the padding

//        //then turn them into statisticmodel lists

//        //then do the grouping
//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, Math.Round(Math.Log10(Convert.ToDouble(x.Select(y => y.reccount).Sum())), 2))).ToList();

//        return result;
//    }


//    public static List<string> GetStat1(string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, x.Select(y => y.userhost).Distinct().Count())).ToList();
//        return result;
//    }

//    public static List<string> GetStat2(string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, x.Count())).ToList();
//        return result;
//    }

//    public static List<string> GetStat3(string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, Math.Round(Math.Log10(Convert.ToDouble(x.Select(y => y.reccount).Sum())), 2))).ToList();
//        return result;
//    }

//    public static List<string> GetStat4(string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, 1, 0, 0, 0));
//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, Math.Round((Convert.ToDouble(x.Select(y => y.reccount).Sum())), 2))).ToList();
//        return result;
//    }


//    public static List<string> GetStat1_GroupByDay(string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, x.querytime.Day, 0, 0, 0));

//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, x.Select(y => y.userhost).Distinct().Count())).ToList();

//        return result;
//    }

//    public static List<string> GetStat2_GroupByDay(string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, x.querytime.Day, 0, 0, 0));
//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, x.Count())).ToList();
//        return result;
//    }

//    public static List<string> GetStat3_GroupByDay(string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, x.querytime.Day, 0, 0, 0));
//        //do the padding

//        //then turn them into statisticmodel lists

//        //then do the grouping
//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, Math.Round(Math.Log10(Convert.ToDouble(x.Select(y => y.reccount).Sum())), 2))).ToList();
//        return result;
//    }

//    public static List<string> GetStat4_GroupByDay(string domainFilter)
//    {
//        var records = MasterLogRecords.Where(x => x.method.Contains("GetValues") && x.userhost.Contains(domainFilter)).OrderBy(x => x.querytime).ToList();
//        records.ForEach(x => x.querytime = new DateTime(x.querytime.Year, x.querytime.Month, x.querytime.Day, 0, 0, 0));
//        //do the padding

//        //then turn them into statisticmodel lists

//        //then do the grouping
//        var result = records.GroupBy(x => x.querytime).Select(x => String.Format("{0},{1}", x.Key, Math.Round((Convert.ToDouble(x.Select(y => y.reccount).Sum())), 2))).ToList();
//        return result;
//    }
//    #endregion

//    public static List<DataAccess_StatisticModel> GetMultiColumnStat1_ByDay(string networkName)
//    {
//        #region STEP 1
//        var stat1 = DataAccess_Logging.GetStat1_GroupByDay(networkName, String.Empty);
//        var stat1_com = DataAccess_Logging.GetStat1_GroupByDay(networkName, ".com");
//        var stat1_org = DataAccess_Logging.GetStat1_GroupByDay(networkName, ".org");
//        var stat1_edu = DataAccess_Logging.GetStat1_GroupByDay(networkName, ".edu");
//        #endregion

//        #region STEP 2
//        var list1 = new List<DataAccess_StatisticModel>();
//        stat1.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1.Add(new DataAccess_StatisticModel { Count = splitString[1], Date = date[0] });
//        });
//        var list1_com = new List<DataAccess_StatisticModel>();
//        stat1_com.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_com.Add(new DataAccess_StatisticModel { Count_Com = splitString[1], Date = date[0] });
//        });
//        var list1_org = new List<DataAccess_StatisticModel>();
//        stat1_org.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_org.Add(new DataAccess_StatisticModel { Count_Org = splitString[1], Date = date[0] });
//        });
//        var list1_edu = new List<DataAccess_StatisticModel>();
//        stat1_edu.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_edu.Add(new DataAccess_StatisticModel { Count_Edu = splitString[1], Date = date[0] });
//        });
//        #endregion

//        #region STEP 3
//        var paddedList1 = PadDates(true, list1, String.Empty, list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_com = PadDates(true, list1_com, ".com", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_org = PadDates(true, list1_org, ".org", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_edu = PadDates(true, list1_edu, ".edu", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        for (var i = 0; i < paddedList1.Count(); i++)
//        {
//            paddedList1[i].Count_Com = paddedList1_com[i].Count_Com;
//            paddedList1[i].Count_Org = paddedList1_org[i].Count_Org;
//            paddedList1[i].Count_Edu = paddedList1_edu[i].Count_Edu;
//        }
//        #endregion

//        return paddedList1;

//    }
//    public static List<DataAccess_StatisticModel> GetMultiColumnStat2_ByDay(string networkName)
//    {
//        #region STEP 1
//        var stat1 = DataAccess_Logging.GetStat2_GroupByDay(networkName, String.Empty);
//        var stat1_com = DataAccess_Logging.GetStat2_GroupByDay(networkName, ".com");
//        var stat1_org = DataAccess_Logging.GetStat2_GroupByDay(networkName, ".org");
//        var stat1_edu = DataAccess_Logging.GetStat2_GroupByDay(networkName, ".edu");
//        #endregion

//        #region STEP 2
//        var list1 = new List<DataAccess_StatisticModel>();
//        stat1.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1.Add(new DataAccess_StatisticModel { Count = splitString[1], Date = date[0] });
//        });
//        var list1_com = new List<DataAccess_StatisticModel>();
//        stat1_com.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_com.Add(new DataAccess_StatisticModel { Count_Com = splitString[1], Date = date[0] });
//        });
//        var list1_org = new List<DataAccess_StatisticModel>();
//        stat1_org.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_org.Add(new DataAccess_StatisticModel { Count_Org = splitString[1], Date = date[0] });
//        });
//        var list1_edu = new List<DataAccess_StatisticModel>();
//        stat1_edu.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_edu.Add(new DataAccess_StatisticModel { Count_Edu = splitString[1], Date = date[0] });
//        });
//        #endregion

//        #region STEP 3
//        var paddedList1 = PadDates(true, list1, String.Empty, list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_com = PadDates(true, list1_com, ".com", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_org = PadDates(true, list1_org, ".org", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_edu = PadDates(true, list1_edu, ".edu", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        for (var i = 0; i < paddedList1.Count(); i++)
//        {
//            paddedList1[i].Count_Com = paddedList1_com[i].Count_Com;
//            paddedList1[i].Count_Org = paddedList1_org[i].Count_Org;
//            paddedList1[i].Count_Edu = paddedList1_edu[i].Count_Edu;
//        }
//        #endregion
//        return paddedList1;

//    }
//    public static List<DataAccess_StatisticModel> GetMultiColumnStat3_ByDay(string networkName)
//    {
//        #region STEP 1
//        var stat1 = DataAccess_Logging.GetStat3_GroupByDay(networkName, String.Empty);
//        var stat1_com = DataAccess_Logging.GetStat3_GroupByDay(networkName, ".com");
//        var stat1_org = DataAccess_Logging.GetStat3_GroupByDay(networkName, ".org");
//        var stat1_edu = DataAccess_Logging.GetStat3_GroupByDay(networkName, ".edu");
//        #endregion

//        #region STEP 2
//        var list1 = new List<DataAccess_StatisticModel>();
//        stat1.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1.Add(new DataAccess_StatisticModel { Count = splitString[1], Date = date[0] });
//        });
//        var list1_com = new List<DataAccess_StatisticModel>();
//        stat1_com.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_com.Add(new DataAccess_StatisticModel { Count_Com = splitString[1], Date = date[0] });
//        });
//        var list1_org = new List<DataAccess_StatisticModel>();
//        stat1_org.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_org.Add(new DataAccess_StatisticModel { Count_Org = splitString[1], Date = date[0] });
//        });
//        var list1_edu = new List<DataAccess_StatisticModel>();
//        stat1_edu.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_edu.Add(new DataAccess_StatisticModel { Count_Edu = splitString[1], Date = date[0] });
//        });
//        #endregion

//        #region STEP 3
//        var paddedList1 = PadDates(true, list1, String.Empty, list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_com = PadDates(true, list1_com, ".com", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_org = PadDates(true, list1_org, ".org", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_edu = PadDates(true, list1_edu, ".edu", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        for (var i = 0; i < paddedList1.Count(); i++)
//        {
//            paddedList1[i].Count_Com = paddedList1_com[i].Count_Com;
//            paddedList1[i].Count_Org = paddedList1_org[i].Count_Org;
//            paddedList1[i].Count_Edu = paddedList1_edu[i].Count_Edu;
//        }
//        #endregion
//        return paddedList1;

//    }
//    public static List<DataAccess_StatisticModel> GetMultiColumnStat4_ByDay(string networkName)
//    {
//        #region STEP 1
//        var stat1 = DataAccess_Logging.GetStat4_GroupByDay(networkName, String.Empty);
//        var stat1_com = DataAccess_Logging.GetStat4_GroupByDay(networkName, ".com");
//        var stat1_org = DataAccess_Logging.GetStat4_GroupByDay(networkName, ".org");
//        var stat1_edu = DataAccess_Logging.GetStat4_GroupByDay(networkName, ".edu");
//        #endregion

//        #region STEP 2
//        var list1 = new List<DataAccess_StatisticModel>();
//        stat1.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1.Add(new DataAccess_StatisticModel { Count = splitString[1], Date = date[0] });
//        });
//        var list1_com = new List<DataAccess_StatisticModel>();
//        stat1_com.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_com.Add(new DataAccess_StatisticModel { Count_Com = splitString[1], Date = date[0] });
//        });
//        var list1_org = new List<DataAccess_StatisticModel>();
//        stat1_org.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_org.Add(new DataAccess_StatisticModel { Count_Org = splitString[1], Date = date[0] });
//        });
//        var list1_edu = new List<DataAccess_StatisticModel>();
//        stat1_edu.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_edu.Add(new DataAccess_StatisticModel { Count_Edu = splitString[1], Date = date[0] });
//        });
//        #endregion

//        #region STEP 3
//        var paddedList1 = PadDates(true, list1, String.Empty, list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_com = PadDates(true, list1_com, ".com", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_org = PadDates(true, list1_org, ".org", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_edu = PadDates(true, list1_edu, ".edu", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        for (var i = 0; i < paddedList1.Count(); i++)
//        {
//            paddedList1[i].Count_Com = paddedList1_com[i].Count_Com;
//            paddedList1[i].Count_Org = paddedList1_org[i].Count_Org;
//            paddedList1[i].Count_Edu = paddedList1_edu[i].Count_Edu;
//        }
//        #endregion
//        return paddedList1;

//    }

//    public static List<DataAccess_StatisticModel> GetMultiColumnStat1_ByMonth(string networkName)
//    {
//        #region STEP 1
//        var stat1 = DataAccess_Logging.GetStat1(networkName, String.Empty);
//        var stat1_com = DataAccess_Logging.GetStat1(networkName, ".com");
//        var stat1_org = DataAccess_Logging.GetStat1(networkName, ".org");
//        var stat1_edu = DataAccess_Logging.GetStat1(networkName, ".edu");
//        #endregion

//        #region STEP 2
//        var list1 = new List<DataAccess_StatisticModel>();
//        stat1.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1.Add(new DataAccess_StatisticModel { Count = splitString[1], Date = date[0] });
//        });
//        var list1_com = new List<DataAccess_StatisticModel>();
//        stat1_com.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_com.Add(new DataAccess_StatisticModel { Count_Com = splitString[1], Date = date[0] });
//        });
//        var list1_org = new List<DataAccess_StatisticModel>();
//        stat1_org.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_org.Add(new DataAccess_StatisticModel { Count_Org = splitString[1], Date = date[0] });
//        });
//        var list1_edu = new List<DataAccess_StatisticModel>();
//        stat1_edu.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_edu.Add(new DataAccess_StatisticModel { Count_Edu = splitString[1], Date = date[0] });
//        });
//        #endregion

//        #region STEP 3
//        var paddedList1 = PadDates(false, list1, String.Empty, list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_com = PadDates(false, list1_com, ".com", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_org = PadDates(false, list1_org, ".org", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_edu = PadDates(false, list1_edu, ".edu", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        for (var i = 0; i < paddedList1.Count(); i++)
//        {
//            paddedList1[i].Count_Com = paddedList1_com[i].Count_Com;
//            paddedList1[i].Count_Org = paddedList1_org[i].Count_Org;
//            paddedList1[i].Count_Edu = paddedList1_edu[i].Count_Edu;
//        }
//        #endregion
//        return paddedList1;

//    }
//    public static List<DataAccess_StatisticModel> GetMultiColumnStat2_ByMonth(string networkName)
//    {
//        #region STEP 1
//        var stat1 = DataAccess_Logging.GetStat2(networkName, String.Empty);
//        var stat1_com = DataAccess_Logging.GetStat2(networkName, ".com");
//        var stat1_org = DataAccess_Logging.GetStat2(networkName, ".org");
//        var stat1_edu = DataAccess_Logging.GetStat2(networkName, ".edu");
//        #endregion

//        #region STEP 2
//        var list1 = new List<DataAccess_StatisticModel>();
//        stat1.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1.Add(new DataAccess_StatisticModel { Count = splitString[1], Date = date[0] });
//        });
//        var list1_com = new List<DataAccess_StatisticModel>();
//        stat1_com.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_com.Add(new DataAccess_StatisticModel { Count_Com = splitString[1], Date = date[0] });
//        });
//        var list1_org = new List<DataAccess_StatisticModel>();
//        stat1_org.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_org.Add(new DataAccess_StatisticModel { Count_Org = splitString[1], Date = date[0] });
//        });
//        var list1_edu = new List<DataAccess_StatisticModel>();
//        stat1_edu.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_edu.Add(new DataAccess_StatisticModel { Count_Edu = splitString[1], Date = date[0] });
//        });
//        #endregion

//        #region STEP 3
//        var paddedList1 = PadDates(false, list1, String.Empty, list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_com = PadDates(false, list1_com, ".com", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_org = PadDates(false, list1_org, ".org", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_edu = PadDates(false, list1_edu, ".edu", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        for (var i = 0; i < paddedList1.Count(); i++)
//        {
//            paddedList1[i].Count_Com = paddedList1_com[i].Count_Com;
//            paddedList1[i].Count_Org = paddedList1_org[i].Count_Org;
//            paddedList1[i].Count_Edu = paddedList1_edu[i].Count_Edu;
//        }
//        #endregion

//        paddedList1.ForEach(x =>
//        {
//            x.Count = Math.Ceiling(Convert.ToDouble(x.Count) / 2).ToString();
//            x.Count_Com = Math.Ceiling(Convert.ToDouble(x.Count_Com) / 2).ToString();
//            x.Count_Edu = Math.Ceiling(Convert.ToDouble(x.Count_Edu) / 2).ToString();
//            x.Count_Org = Math.Ceiling(Convert.ToDouble(x.Count_Org) / 2).ToString();
//        });
//        return paddedList1;

//    }
//    public static List<DataAccess_StatisticModel> GetMultiColumnStat3_ByMonth(string networkName)
//    {
//        #region STEP 1
//        var stat1 = DataAccess_Logging.GetStat3(networkName, String.Empty);
//        var stat1_com = DataAccess_Logging.GetStat3(networkName, ".com");
//        var stat1_org = DataAccess_Logging.GetStat3(networkName, ".org");
//        var stat1_edu = DataAccess_Logging.GetStat3(networkName, ".edu");
//        #endregion

//        #region STEP 2
//        var list1 = new List<DataAccess_StatisticModel>();
//        stat1.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1.Add(new DataAccess_StatisticModel { Count = splitString[1], Date = date[0] });
//        });
//        var list1_com = new List<DataAccess_StatisticModel>();
//        stat1_com.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_com.Add(new DataAccess_StatisticModel { Count_Com = splitString[1], Date = date[0] });
//        });
//        var list1_org = new List<DataAccess_StatisticModel>();
//        stat1_org.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_org.Add(new DataAccess_StatisticModel { Count_Org = splitString[1], Date = date[0] });
//        });
//        var list1_edu = new List<DataAccess_StatisticModel>();
//        stat1_edu.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_edu.Add(new DataAccess_StatisticModel { Count_Edu = splitString[1], Date = date[0] });
//        });
//        #endregion

//        #region STEP 3
//        var paddedList1 = PadDates(false, list1, String.Empty, list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_com = PadDates(false, list1_com, ".com", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_org = PadDates(false, list1_org, ".org", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_edu = PadDates(false, list1_edu, ".edu", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        for (var i = 0; i < paddedList1.Count(); i++)
//        {
//            paddedList1[i].Count_Com = paddedList1_com[i].Count_Com;
//            paddedList1[i].Count_Org = paddedList1_org[i].Count_Org;
//            paddedList1[i].Count_Edu = paddedList1_edu[i].Count_Edu;
//        }
//        #endregion
//        return paddedList1;

//    }
//    public static List<DataAccess_StatisticModel> GetMultiColumnStat4_ByMonth(string networkName)
//    {
//        #region STEP 1
//        var stat1 = DataAccess_Logging.GetStat4(networkName, String.Empty);
//        var stat1_com = DataAccess_Logging.GetStat4(networkName, ".com");
//        var stat1_org = DataAccess_Logging.GetStat4(networkName, ".org");
//        var stat1_edu = DataAccess_Logging.GetStat4(networkName, ".edu");
//        #endregion

//        #region STEP 2
//        var list1 = new List<DataAccess_StatisticModel>();
//        stat1.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1.Add(new DataAccess_StatisticModel { Count = splitString[1], Date = date[0] });
//        });
//        var list1_com = new List<DataAccess_StatisticModel>();
//        stat1_com.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_com.Add(new DataAccess_StatisticModel { Count_Com = splitString[1], Date = date[0] });
//        });
//        var list1_org = new List<DataAccess_StatisticModel>();
//        stat1_org.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_org.Add(new DataAccess_StatisticModel { Count_Org = splitString[1], Date = date[0] });
//        });
//        var list1_edu = new List<DataAccess_StatisticModel>();
//        stat1_edu.ForEach(x =>
//        {
//            var splitString = x.Split(',');
//            var date = splitString[0].Split(' ');
//            list1_edu.Add(new DataAccess_StatisticModel { Count_Edu = splitString[1], Date = date[0] });
//        });
//        #endregion

//        #region STEP 3
//        var paddedList1 = PadDates(false, list1, String.Empty, list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_com = PadDates(false, list1_com, ".com", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_org = PadDates(false, list1_org, ".org", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        var paddedList1_edu = PadDates(false, list1_edu, ".edu", list1.Any() ? list1.Min(x => x.GetActualDate()) : DateTime.MinValue, list1.Any() ? list1.Max(x => x.GetActualDate()) : DateTime.Now);
//        for (var i = 0; i < paddedList1.Count(); i++)
//        {
//            paddedList1[i].Count_Com = paddedList1_com[i].Count_Com;
//            paddedList1[i].Count_Org = paddedList1_org[i].Count_Org;
//            paddedList1[i].Count_Edu = paddedList1_edu[i].Count_Edu;
//        }
//        #endregion
//        return paddedList1;

//    }

//    //ADD ANY IP ADDRESSES TO EXCLUDE IN HERE
//    public static List<string> ExclusionList = new List<string> { "[SERVER_IP]", "::1", "137.135.87.5", "132.249.69.104", "183.182.84.27" };


//    private static List<log11Service> _masterLogRecords = new List<log11Service>();
//    public static List<log11Service> MasterLogRecords
//    {
//        get
//        {
//            //If already fetched from db, return the in-memory collection
//            if (_masterLogRecords.Any()) return _masterLogRecords;
//            //Else fetch from the db (except the ones in our exclusion list!)
//            _masterLogRecords = _db.log11Service.Where(x => !ExclusionList.Contains(x.userhost)).ToList();
//            //Apply userhost IP Address to Domain Name mapping
//            _masterLogRecords.ForEach(x => x.userhost = x.userhost.ConvertIpAddressToDomainName());
//            return _masterLogRecords;
//        }
//        set { _masterLogRecords = value; }
//    }

//    private static Dictionary<string, string> _globalIpDomainMappings = new Dictionary<string, string>();
//    public static Dictionary<string, string> GlobalIpDomainMappings { get { return _globalIpDomainMappings; } set { _globalIpDomainMappings = value; } }

//    public static string ConvertIpAddressToDomainName(this string value)
//    {
//        //Check if this value already exists in our dictionary? If so, avoid conversion to save time
//        if (GlobalIpDomainMappings.ContainsKey(value)) return GlobalIpDomainMappings[value];
//        //Else perform the conversion and add to dictionary only if DNS lookup is successful
//        System.Net.IPAddress parsedIp;
//        var isValidIp = System.Net.IPAddress.TryParse(value, out parsedIp);
//        try
//        {
//            if (isValidIp)
//            {
//                var domain = System.Net.Dns.GetHostEntry(parsedIp);
//                var result = domain.HostName.ToLower();
//                //Add this converted result to the global dictionary to avoid future conversions via the network
//                GlobalIpDomainMappings.Add(value, result);
//                return result;
//            }
//            return value;
//        }
//        catch (Exception ex)
//        {
//            return value;
//        }
//    }

//}