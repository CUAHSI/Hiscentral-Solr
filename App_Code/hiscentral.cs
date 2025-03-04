using System;
using System.Diagnostics;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data.SqlClient;
using System.Data;
using System.Data.Sql;
using System.Configuration;
using System.Web.Caching;

using com.hp.hpl.jena.rdf.model;
using com.hp.hpl.jena.util.iterator;
using com.hp.hpl.jena.ontology;

using log4net;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;

using System.Net;
using System.Net.Http;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

using OdmCv;
using CsvHelper.Configuration;

/// <summary>
/// Summary description for hiscentral
/// </summary>

[WebService(Namespace = "http://hiscentral.cuahsi.org/20100205/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class hiscentral : System.Web.Services.WebService
{


    private static readonly ILog log
        = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private const string logFormat = "Method:{0}, {1}";

    private static readonly ILog queryLog = LogManager.GetLogger("QueryLog");
    private const string queryLogFormat = "{0}|{1}|{2}|{3}";

    private ServiceStatistics _ss = null;
    public ServiceStatistics ServiceStats
    {
        get
        {
            if (_ss != null) return _ss;
            //Try pulling from application cache if null
            var ss = Application["ServiceStatistics"];
            if (ss == null)
            {
                _ss = new ServiceStatistics();
                Application.Add("ServiceStatistics", _ss);
            }
            else
            {
                _ss = (ServiceStatistics)ss;


            }
            return _ss;
        }
    }

    public hiscentral()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    //     private const string media = "http://www.cuahsi.org/waterquality#medium";
    //     private const string mediumPropertyURI = "http://www.cuahsi.org/waterquality#hasMedium";


    public struct Box
    {
        public double xmin;
        public double xmax;
        public double ymin;
        public double ymax;
    }

    #region sites queries:
    public struct Site
    {
        public string SiteName;
        public string SiteCode;
        public double Latitude;
        public double Longitude;
        public string HUC;
        public int HUCnumeric;
        public string servCode;
        public string servURL;
    }

    public struct SiteInfo
    {
        public string SiteName;
        public string SiteCode;
        public double Latitude;
        public double Longitude;
        public string HUC;
        public int HUCnumeric;
        public string servCode;
        public string servURL;
        public int count;
    }


    //[WebMethod]
    [System.Web.Services.WebMethod(
       Description = "<br><p style='margin-left:25px;'>Get a list of site information within a specified lat/lon box, and other specified query parameters. </p>" +
                      "<p style='margin-left:25px;'> Typically used to subset sites dataset and plot the returned sites on a map </ p > ")]
    public Site[] GetSites(double xmin, double xmax, double ymin, double ymax,
                            string conceptKeyword, string networkIDs,
                            string beginDate, string endDate)
    {
        int Max_sites = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SOLRnsites"]);

        if (beginDate.Trim().Equals("")) beginDate = "01/01/1900";
        if (endDate.Trim().Equals("")) endDate = "01/01/2100";
        string baseUrl = requestUrl(xmin, xmax, ymin, ymax,
                                conceptKeyword, networkIDs,
                                beginDate, endDate);
        string url = baseUrl
                    + "&fl=SiteName,SiteCode,NetworkName,ServiceWSDL,Latitude,Longitude"
                    //&facet=true&facet.field=SiteCode"
                    + String.Format(@"&rows={0}", Max_sites);

        Site[] sites = null;
        XDocument xDocument;
        string response = null;

        //using (WebClient client = new WebClient())
        //{
        //    client.Encoding = Encoding.UTF8;
        //    response = client.DownloadString(url);
        //    TextReader xmlReader = new StringReader(response);
        //    xDocument = XDocument.Load(xmlReader);

        //    var xnode = xDocument.Descendants("lst").Where(o => (string)o.Attribute("name") == "SiteCode");

        //    sites =
        //    (from p in xnode.Descendants("int")
        //     let t = p.Attribute("name").Value.ToString()
        //     select new SiteInfo()
        //     {
        //         SiteCode = t,
        //         count = int.Parse(p.Value),
        //     }).ToArray();
        //}

        using (WebClient client = new WebClient())
        {
            client.Encoding = Encoding.UTF8;
            response = client.DownloadString(url);
            TextReader xmlReader = new StringReader(response);
            xDocument = XDocument.Load(xmlReader);

            //var sitecodeList = from o in xDocument.Descendants("doc")
            //        select o.Elements("str").Single(x => x.Attribute("name").Value == "SiteCode").Value.ToString();
            //var sitecodeDistinctList = sitecodeList.GroupBy(s => s).Select(s => s.First());

            var sitesUngrouped = (from o in xDocument.Descendants("doc")
                                  select new Site()
                                  {
                                      SiteCode = o.Elements("str").Single(x => x.Attribute("name").Value == "SiteCode").Value.ToString(),
                                      SiteName = o.Descendants("str").Where(e => (string)e.Attribute("name") == "SiteName").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "SiteName").Value.ToString(),
                                      servURL = o.Elements("str").Single(x => x.Attribute("name").Value == "ServiceWSDL").Value.ToString(),
                                      servCode = o.Elements("str").Single(x => x.Attribute("name").Value == "NetworkName").Value.ToString(),
                                      Latitude = double.Parse(o.Elements("double").Single(x => x.Attribute("name").Value == "Latitude").Value.ToString()),
                                      Longitude = double.Parse(o.Elements("double").Single(x => x.Attribute("name").Value == "Longitude").Value.ToString()),
                                  }).ToArray();

            sites = (from site in sitesUngrouped
                     group site by site.SiteCode into g
                     select new Site()
                     {
                         SiteCode = g.First().SiteCode,
                         SiteName = g.First().SiteName,
                         servURL = g.First().servURL,
                         servCode = g.First().servCode,
                         Latitude = g.First().Latitude,
                         Longitude = g.First().Longitude,

                     }).ToArray();
        }

        return sites;
    }

    /*
    * GetSitesInBox2
    * 
    * Input Parameters: 
    * 
    * 	Lat/Long Box, 
    * 	Ontology Concept (optional), 
    * 	// Begin Date (removed/ignored), 
    * 	// End Date (removed/ignored), 
    * 	// Number of Data Values (removed/ignored), 
    * 	A comma separated list of NetworkIDs (Optional)
    * 
    * Returns: A list of all sites that fall within the bounding box, have variables that are mapped 
    * to or fall under the Ontology Concept, overlap the date range of interest, have a minimum number 
    * of data values, and are within the list of services.  
    * Return Format: A list of WaterML siteInfo elements that includes enough information to identify 
    * the service from which the sites were extracted and the HUC Code and HUC Name for the HUC in which 
    * the sites are located (as a general rule, anywhere the siteInfo element is used it should contain  
    * the HUC Code and HUC Name).
    *
    * COUCH: HUC Code and HUC Name are no longer returned. 
    * COUCH: GetSitesInBox and GetSitesInBox2 differ only in input format. 
    */

    [WebMethod(
   Description = "<br><p style='margin-left:25px;'>Get a list of site information within a specified lat/lon box, and other specified query parameters. </p>" +
                 "<p style='margin-left:25px;'>Typically used to subset sites dataset and plot the returned sites on a map </ p > "
                    )]
    public Site[] GetSitesInBox2(
    double xmin, double xmax, double ymin, double ymax,
    string conceptKeyword, string networkIDs)
    {

        Box box = new Box();
        box.xmax = xmax;
        box.xmin = xmin;
        box.ymax = ymax;
        box.ymin = ymin;
        int[] ids = new int[0];
        if (networkIDs != "" && networkIDs != " ")
        {
            String[] sids = networkIDs.Split(',');
            ids = new int[sids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = int.Parse(sids[i]);
            }
        }
        return GetSitesInBox(box, conceptKeyword, ids);
    }

    /*
      * GetSitesInBox
      * 
      * Input Parameters: 
      * 
      * 	max latitude, 
      * 	min latitude, 
      * 	max longitude, 
      * 	min longitude,  
      * 	Ontology Concept (optional), 
      * 	// Begin Date (removed/ignored), 
      * 	// End Date (removed/ignored), 
      * 	// Number of Data Values (removed/ignored), 
      * 	A comma separated list of NetworkIDs (Optional)
      * 
      * Returns: A list of all sites that fall within the bounding box, have variables that are mapped 
      * to or fall under the Ontology Concept, and are within the list of services.  
      * Return Format: A list of WaterML siteInfo elements that includes enough information to identify 
      * the service from which the sites were extracted and the HUC Code and HUC Name for the HUC in which 
      * the sites are located (as a general rule, anywhere the siteInfo element is used it should contain  
      * the HUC Code and HUC Name).
      *
      * COUCH: HUC Code and HUC Name are no longer returned.     
      * COUCH: GetSitesInBox and GetSitesInBox2 differ only in input formats.  
      */



    [WebMethod(
     Description = "<br><p style='margin-left:25px;'> <strong>DEPRECATED</strong> Get a list of site information within a specified lat/lon box, and other specified query parameters. </p>" +
                   "<p style='margin-left:25px;'>Typically used to subset sites dataset and plot the returned sites on a map </ p > "
                      )]
    public Site[] GetSitesInBox(Box box, string conceptKeyword, int[] networkIDs)
    {

        return GetSites(box.xmin, box.xmax, box.ymin, box.ymax,
                        conceptKeyword, string.Join(" ", networkIDs.Select(i => i.ToString()).ToArray()),
                        " ", " ");
    }

    //public Site[] GetSitesInBox(Box box, string conceptKeyword, int[] networkIDs)
    //{


    //    string objecformat = "concept:{0},box({1},{2},{3},{4}),network({5}";
    //    string methodName = "GetSitesInBox";
    //    Stopwatch timer = new Stopwatch();
    //    timer.Start();

    //    String netString = "";
    //    if (networkIDs != null && networkIDs.Length != 0)
    //    {
    //        for (int i = 0; i < networkIDs.Length; i++)
    //        {
    //            if (i > 0) netString += ",";
    //            netString += networkIDs[i].ToString();
    //        }
    //    }

    //    log.InfoFormat(logFormat, methodName, "Start", 0,
    //       String.Format(objecformat,
    //           conceptKeyword ?? String.Empty,
    //           box.xmin, box.xmax, box.ymin, box.ymax,
    //           netString));//Marie - Network String

    //    string connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
    //    SqlConnection con = new SqlConnection(connect);
    //    // allow blank keywords through
    //    Site[] sites = new Site[0];

    //    String sql = "sp_getSitesInBox";


    //    using (con)
    //    {
    //        SqlDataAdapter da = new SqlDataAdapter(sql, con);
    //        da.SelectCommand.CommandTimeout = 300;
    //        da.SelectCommand.CommandType = CommandType.StoredProcedure;

    //        da.SelectCommand.Parameters.AddWithValue("@conceptName", conceptKeyword);
    //        da.SelectCommand.Parameters.AddWithValue("@latmax", box.ymax);
    //        da.SelectCommand.Parameters.AddWithValue("@latmin", box.ymin);
    //        da.SelectCommand.Parameters.AddWithValue("@longmax", box.xmax);
    //        da.SelectCommand.Parameters.AddWithValue("@longmin", box.xmin);
    //        da.SelectCommand.Parameters.AddWithValue("@networks", netString);
    //        DataSet ds = new DataSet();
    //        da.Fill(ds, "SearchCatalog");

    //        System.Data.DataRowCollection rows = ds.Tables["SearchCatalog"].Rows;
    //        sites = new Site[rows.Count];
    //        DataRow row;
    //        for (int i = 0; i < rows.Count; i++)
    //        {
    //            row = rows[i];
    //            sites[i] = new Site();
    //            sites[i].SiteCode = row["SiteCode"] != null ? row["SiteCode"].ToString() : "";
    //            sites[i].SiteName = row["SiteName"] != null ? row["SiteName"].ToString() : "";
    //            sites[i].servURL = row["ServiceWSDL"] != null ? row["ServiceWSDL"].ToString() : "";
    //            sites[i].servCode = row["NetworkName"] != null ? row["NetworkName"].ToString() : "";
    //            //sites[i].HUCnumeric = row["HUCnumeric"] != null ? (int)row["HUCnumeric"] : 0;
    //            sites[i].Latitude = (double)row["latitude"];
    //            sites[i].Longitude = (double)row["longitude"];


    //        }
    //    }
    //    log.InfoFormat(logFormat, methodName, "end", timer.ElapsedMilliseconds,
    //     String.Format(objecformat,
    //         conceptKeyword ?? String.Empty,
    //         box.xmin, box.xmax, box.ymin, box.ymax,
    //         netString));//marie-networkString
    //    timer.Stop();
    //    return sites;
    //}

    #endregion

    #region variable queries:
    [WebMethod(
   Description = "<br ><p  style='margin-left:25px;'><strong>DEPRECATED</strong> Input conceptid (as defined in ontology tree) for specified services, return the corresponding variable information including variablename, variablecode, valuetype, timeUnitID, datatype, etc.</p>")]
    public MappedVariable[] GetMappedVariables2(String conceptids, String Networkids)
    {
        String[] ceptsArray = conceptids.Split(',');
        String[] netsArray = Networkids.Split(',');
        return GetMappedVariables(ceptsArray, netsArray);
    }

    [WebMethod(
  Description = "<br ><p  style='margin-left:25px;'><strong>DEPRECATED</strong> Input conceptid (as defined in ontology tree) for specified services, return the corresponding variable information including variablename, variablecode, valuetype, timeUnitID, datatype, etc.</p>")]
    public MappedVariable[] GetMappedVariables(String[] conceptids, String[] Networkids)
    {
        string objecformat = "concept:{0},network({1}";
        string methodName = "GetMappedVariables";
        Stopwatch timer = new Stopwatch();
        timer.Start();

        log.InfoFormat(logFormat, methodName, "Start", 0,
                       String.Format(objecformat,
                        conceptids == null ? string.Empty : String.Join(",", conceptids),
            Networkids == null ? string.Empty : String.Join(",", Networkids))
        );

        string sql = "sp_getMappedVariables";

        // create string of comma-separated concept ids
        string conceptString = "";
        if (conceptids != null && conceptids.Length > 0)
        {
            if (!(conceptids.Length == 1 && conceptids[0].Length == 0))
            {
                int i = 0;
                foreach (string cept in conceptids)
                {
                    if (i > 0) conceptString += ",";
                    i++; conceptString += "'" + cept + "'";
                }
            }
        }

        // create string of comma-separated network ids
        String netString = "";
        if (Networkids != null && Networkids.Length != 0)
        {
            for (int i = 0; i < Networkids.Length; i++)
            {
                if (i > 0) netString += ",";
                netString += Networkids[i].ToString();
            }
        }


        string connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connect);
        MappedVariable[] mappedVars = new MappedVariable[0];
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@conceptIds", conceptString);
            da.SelectCommand.Parameters.AddWithValue("@networkIds", netString);

            DataSet ds = new DataSet();
            da.Fill(ds, "MappedVars");

            System.Data.DataRowCollection rows = ds.Tables["MappedVars"].Rows;
            mappedVars = new MappedVariable[rows.Count];
            DataRow row;
            for (int i = 0; i < rows.Count; i++)
            {
                row = rows[i];
                mappedVars[i] = new MappedVariable();
                mappedVars[i].variableName = row["AltVariableName"] != null ? row["AltVariableName"].ToString() : "";
                mappedVars[i].variableCode = row["AltVariableCode"] != null ? row["AltVariableCode"].ToString() : "";
                mappedVars[i].WSDL = row["ServiceWSDL"] != null ? row["ServiceWSDL"].ToString() : "";
                mappedVars[i].servCode = row["NetworkName"] != null ? row["NetworkName"].ToString() : "";
                mappedVars[i].conceptCode = row["ConceptID"] != null ? row["ConceptID"].ToString() : "";
                //mappedVars[i].conceptKeyword = getOntologyKeyword(mappedVars[i].conceptCode);
            }
        }

        log.InfoFormat(logFormat, methodName, "end", timer.ElapsedMilliseconds,
        String.Format(objecformat,
        conceptids == null ? string.Empty : String.Join(",", conceptids),
        Networkids == null ? string.Empty : String.Join(",", Networkids))
        );
        timer.Stop();
        return mappedVars;
    }
    // Interface code defines what is passed to aspx page. 
    public struct MappedVariable
    {
        public string variableName;
        public string variableCode;
        public string servCode;
        public string WSDL;
        public string conceptKeyword;
        public string conceptCode;
    }

    /// <summary>
    /// added by YX, Mar. 2017 to access solr database
    /// </summary>
    /// <param name="xmin"></param>
    /// <param name="xmax"></param>
    /// <param name="ymin"></param>
    /// <param name="ymax"></param>
    /// <param name="conceptKeyword"></param>
    /// <param name="networkIDs"></param>
    /// <param name="beginDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    [WebMethod(Description = "<br> <p style = 'margin-left:25px;' >Get a list of variable information within a specified lat/lon box, and other specified query parameters.</p >")]
    public MappedVariable[] GetVariables(double xmin, double xmax, double ymin, double ymax,
                            string conceptKeyword, string networkIDs,
                            string beginDate, string endDate)
    {
        int Max_sites = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SOLRnvariables"]);

        if (beginDate.Trim().Equals("")) beginDate = "01/01/1900";
        if (endDate.Trim().Equals("")) endDate = "01/01/2100";
        string baseUrl = requestUrl(xmin, xmax, ymin, ymax,
                                conceptKeyword, networkIDs,
                                beginDate, endDate);
        string url = baseUrl
                    + "&fl=VariableName,VariableCode,NetworkName,ServiceWSDL,ConceptKeyword"
                    + String.Format(@"&rows={0}", Max_sites);

        MappedVariable[] variables = null;
        XDocument xDocument;
        string response = null;

        using (WebClient client = new WebClient())
        {
            client.Encoding = Encoding.UTF8;
            response = client.DownloadString(url);
            TextReader xmlReader = new StringReader(response);
            xDocument = XDocument.Load(xmlReader);

            var varsUngrouped = (from o in xDocument.Descendants("doc")
                                 select new MappedVariable()
                                 {
                                     variableCode = o.Elements("str").Single(x => x.Attribute("name").Value == "VariableCode").Value.ToString(),
                                     variableName = o.Descendants("str").Where(e => (string)e.Attribute("name") == "VariableName").Count() == 0 ?
                                           "" : o.Elements("str").Single(x => x.Attribute("name").Value == "VariableName").Value.ToString(),
                                     WSDL = o.Elements("str").Single(x => x.Attribute("name").Value == "ServiceWSDL").Value.ToString(),
                                     servCode = o.Elements("str").Single(x => x.Attribute("name").Value == "NetworkName").Value.ToString(),
                                     conceptKeyword = o.Elements("str").Single(x => x.Attribute("name").Value == "ConceptKeyword").Value.ToString(),
                                 }).ToArray();

            variables = (from v in varsUngrouped
                         group v by v.variableCode into g
                         select new MappedVariable()
                         {
                             variableCode = g.First().variableCode,
                             variableName = g.First().variableName,
                             WSDL = g.First().WSDL,
                             servCode = g.First().servCode,
                             conceptKeyword = g.First().conceptKeyword,
                         }).ToArray();
        }

        return variables;
    }

    #endregion

    # region ServiceInfo struct and queries
    # region ServiceInfo wiki info
    /* 
     * GetWaterOneFlowServiceInfo 
     * Input Parameters: A comma separated list of ServiceIDs (Optional)
     * Returns: A list of all WaterOneFlow web services registered with HIS Central. 
     * We need a WaterML serviceInfo type to define this and should probably have the following elements.
     * Data Service Name
     * Data Service Title
     * Data Service WSDL URL
     * Data Service Description URL
     * Geographic Extent (xmin, xmax, ymin, ymax)
     * Abstract
     * Contact Name
     * Contact Email
     * Contact Phone
     * Organization
     * Organization Website
     * Supported Methods
     * Citation
     * Value Count
     * Variable Count
     * Site Count
     * Earliest Record DateTime
     * Latest Record DateTime
     * ServiceStatus
     */
    #endregion
    public struct ServiceInfo
    {
        public string servURL;
        public string Title, ServiceDescriptionURL;
        public string name, Email, phone;
        public string organization, orgwebsite, citation, aabstract;
        public int valuecount;
        public int variablecount, sitecount;
        public int ServiceID;
        public string NetworkName;
        public double minx, miny, maxx, maxy;
        public string serviceStatus;
    }
  

    [WebMethod(Description = "<br> <p style = 'margin-left:25px;' ><strong>DEPRECATED</strong> Get registered data services within a specified lat/lon box.</p>" +
                            "<p style = 'margin-left:25px;'>Typically used to subset the data services and use the result as an input for filtering</p>")]
    public ServiceInfo[] GetServicesInBox(Box box)
    {
        return GetServicesInBox2(box.xmin, box.ymin, box.xmax, box.ymax);
    }
    [WebMethod(Description = "<br> <p style = 'margin-left:25px;' >Get registered data services within a specified set of coordinates.</p>" +
                              "<p style = 'margin-left:25px;'>Typically used to subset the data services and use the result as an input for filtering</p>")]
    public ServiceInfo[] GetServicesInBox2(double xmin, double ymin, double xmax, double ymax)
    {
        ServiceStats.AddCount("GetServicesInBox2");

        string objecformat = "box({0},{1},{2},{3})";
        string methodName = "GetServicesInBox2";
        Stopwatch timer = new Stopwatch();
        timer.Start();

        queryLog.InfoFormat(queryLogFormat, methodName, "Start", 0,
            String.Format(objecformat, xmin, xmax, ymin, ymax)
    );

        //         String sql = "SELECT NetworkID, NetworkName, NetworkTitle, ServiceWSDL, ServiceAbs, ContactName, ContactEmail, ContactPhone, Organization, website,  " +
        //                     "   Citation, NetworkVocab, ProjectStatus, Xmin, Xmax, Ymin, Ymax, ValueCount, VariableCount, SiteCount, EarliestRec, LatestRec, ServiceStatus " +
        //                     "FROM hisnetworks WITH (NOLOCK) where ispublic='true' " +
        //          "AND (((xmin between @minx and @maxx) or (xmax between @minx and @maxx))AND((ymin between @miny and @maxy) or (ymax between @miny and @maxy))) OR " +
        //          "(((@minx between xmin and xmax) or (@maxx between xmin and xmax))AND((@miny between ymin and ymax) or (@maxy between ymin and ymax))) ";

        String sql = "sp_getServicesInBox";

        String connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;

        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(connect);
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@maxy", ymax);
            da.SelectCommand.Parameters.AddWithValue("@miny", ymin);
            da.SelectCommand.Parameters.AddWithValue("@maxx", xmax);
            da.SelectCommand.Parameters.AddWithValue("@minx", xmin);

            log.DebugFormat(logFormat, methodName, da.SelectCommand.CommandText);

            da.Fill(ds, "Service_LIST");
        }
        con.Close();
        //ds.Tables["URL"].Rows
        System.Data.DataRowCollection rows = ds.Tables["Service_LIST"].Rows;

        var r = getServiceInfoArray(rows);
        queryLog.InfoFormat(queryLogFormat, methodName, "end", timer.ElapsedMilliseconds,
        String.Format(objecformat, xmin, xmax, ymin, ymax)
    );
        timer.Stop();

        return r;

    }
   
  [WebMethod(Description = "<br> <p style = 'margin-left:25px;' > Get all registered data services from<a href= 'http://hiscentral.cuahsi.org/pub_services.aspx' > http://hiscentral.cuahsi.org/pub_services.aspx</a>. GetWaterOneFlowServiceInfo can be regarded as a special case of GetServicesInBox2, as the former requests the returns for the global area.</p>")]
    public ServiceInfo[] GetWaterOneFlowServiceInfo()
    {
        ServiceStats.AddCount("GetWaterOneFlowServiceInfo");
        string methodName = "GetWaterOneFlowServiceInfo";
        Stopwatch timer = new Stopwatch();
        timer.Start();

        queryLog.InfoFormat(queryLogFormat, methodName, "Start", 0, String.Empty);
        //SELECT     NetworkID, username, NetworkName, NetworkTitle, ServiceWSDL, ServiceAbs, ContactName, ContactEmail, ContactPhone, Organization, website, 
        //                  IsPublic, SupportsAllMethods, Citation, MapIconPath, OrgIconPath, LastHarvested, FrequentUpdates, logo, icon, IsApproved, NetworkVocab, 
        //                  ProjectStatus, CreatedDate, Xmin, Xmax, Ymin, Ymax, ValueCount, VariableCount, SiteCount, EarliestRec, LatestRec, ServiceStatus
        //FROM         HISNetworks
        //WHERE     (IsPublic = 'true')

        String sql = "SELECT NetworkID, NetworkName, NetworkTitle, ServiceWSDL, ServiceAbs, ContactName, ContactEmail, ContactPhone, Organization, website,  " +
                     "   Citation, NetworkVocab, ProjectStatus, Xmin, Xmax, Ymin, Ymax, ValueCount, VariableCount, SiteCount, EarliestRec, LatestRec, ServiceStatus " +
                     "FROM hisnetworks WITH (NOLOCK) where ispublic='true' ";
        String connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(connect);
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);

            log.DebugFormat(logFormat, methodName, da.SelectCommand.CommandText);

            da.Fill(ds, "Service_LIST");
        }
        con.Close();
        //ds.Tables["URL"].Rows
        System.Data.DataRowCollection rows = ds.Tables["Service_LIST"].Rows;

        var r = getServiceInfoArray(rows);
        queryLog.InfoFormat(queryLogFormat, methodName, "end", timer.ElapsedMilliseconds,
        String.Empty);
        timer.Stop();
        return r;

    }

    private ServiceInfo[] getServiceInfoArray(DataRowCollection rows)
    {

        ServiceInfo[] infos = new ServiceInfo[rows.Count];
        DataRow row;
        for (int i = 0; i < rows.Count; i++)
        {
            row = rows[i];
            infos[i] = new ServiceInfo();
            infos[i].ServiceID = int.Parse(row[0].ToString());
            infos[i].servURL = row["ServiceWSDL"] != null ? row["ServiceWSDL"].ToString() : "";
            infos[i].Title = row["NetworkTitle"] != null ? row["NetworkTitle"].ToString() : "";
            infos[i].NetworkName = row["NetworkName"] != null ? row["NetworkName"].ToString() : "";
            if (row["Xmin"] != null && row["Xmin"].ToString() != "")
            {
                infos[i].minx = double.Parse(row["Xmin"].ToString());
                infos[i].maxx = double.Parse(row["Xmax"].ToString());
                infos[i].miny = double.Parse(row["Ymin"].ToString());
                infos[i].maxy = double.Parse(row["Ymax"].ToString());
            }
            // infos[i].valuecount = (String.IsNullOrEmpty( (string)row["ValueCount"] )) ? Int32.Parse(row["ValueCount"].ToString()) : 0;
            if (row["ValueCount"] != DBNull.Value)
            {
                try
                {
                    infos[i].valuecount = Int32.Parse(row["ValueCount"].ToString());
                }
                catch (OverflowException ex)
                {
                    infos[i].valuecount = Int32.MaxValue;
                }
            }
            infos[i].variablecount = (row["VariableCount"] != null && row["VariableCount"].ToString() != "") ? int.Parse(row["VariableCount"].ToString()) : 0;
            infos[i].sitecount = (row["SiteCount"] != null && row["SiteCount"].ToString() != "") ? int.Parse(row["SiteCount"].ToString()) : 0;
            infos[i].citation = row["citation"] != null ? row["citation"].ToString() : "";
            infos[i].aabstract = row["ServiceAbs"] != null ? row["ServiceAbs"].ToString() : "";
            infos[i].organization = row["Organization"] != null ? row["Organization"].ToString() : "";
            infos[i].phone = row["ContactPhone"] != null ? row["ContactPhone"].ToString() : "";
            infos[i].Email = row["ContactEmail"] != null ? row["ContactEmail"].ToString() : "";
            infos[i].orgwebsite = row["website"] != null ? row["website"].ToString() : "";
            infos[i].ServiceDescriptionURL = "http://hiscentral.cuahsi.org/pub_network.aspx?n=" + infos[i].ServiceID;
            infos[i].serviceStatus = row["ServiceStatus"] != null ? row["ServiceStatus"].ToString() : "";
        }
        return infos;
    }
    #endregion

    #region Series methods

    public struct SeriesRecord
    {
        public string ServCode;
        public string ServURL;
        public string location;
        public string VarCode;
        public string VarName;
        public string beginDate;
        public string endDate;
        public string authtoken;
        public int ValueCount;

        public string Sitename;
        public double latitude;
        public double longitude;

        public string datatype;
        public string valuetype;
        public string samplemedium;
        public string timeunits;
        public string conceptKeyword;
        public string genCategory;
        public string TimeSupport;
    }

    public struct SeriesRecordFull
    {
        public string ServCode;
        public string ServURL;
        public string location;
        public string VarCode;
        public string VarName;
        public string beginDate;
        public string endDate;
        public string authtoken;
        public int ValueCount;

        public string Sitename;
        public double latitude;
        public double longitude;

        public string datatype;
        public string valuetype;
        public string samplemedium;
        public string timeunits;
        public string conceptKeyword;
        public string genCategory;
        public string TimeSupport;
        public string SeriesCode;

        public string QCLID;
        public string QCLDesc;
        public string Organization;
        public string TimeUnitAbbrev;
        public string TimeUnits;
        public string IsRegular;
        public string Speciation;
        public string SourceOrg;
        public string VariableUnitsAbbrev;
        public string SourceId;
        public string SourceDesc;
        public string MethodId;
        public string MethodDesc;
    }

    public class SeriesRecordFull2
    {
        public string ServCode { get; set; }
        public string ServURL { get; set; }
        public string location { get; set; }
        public string VarCode { get; set; }
        public string VarName { get; set; }
        public string beginDate { get; set; }
        public string endDate { get; set; }
        public string authtoken { get; set; }
        public string ValueCount { get; set; }
        public string Sitename { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string datatype { get; set; }
        public string valuetype { get; set; }
        public string samplemedium { get; set; }
        public string timeunits { get; set; }
        public string conceptKeyword { get; set; }
        public string genCategory { get; set; }
        public string TimeSupport { get; set; }
        public string SeriesCode { get; set; }
        public string QCLID { get; set; }
        public string QCLDesc { get; set; }
        public string Organization { get; set; }
        public string TimeUnitAbbrev { get; set; }
        //public string TimeUnits { get; set; }
        public string IsRegular { get; set; }
        public string Speciation { get; set; }
        public string SourceOrg { get; set; }
        public string VariableUnitsAbbrev { get; set; }
        public string SourceId { get; set; }
        public string SourceDesc { get; set; }
        public string MethodId { get; set; }
        public string MethodDesc { get; set; }
    }

    sealed class CSVFileDefinitionMap : CsvClassMap<SeriesRecordFull2>
    {
        public CSVFileDefinitionMap()
        {
            Map(m => m.ServURL).Name("ServiceWSDL");
            Map(m => m.MethodDesc).Name("MethodDesc");
            Map(m => m.QCLID).Name("QCLID");
            Map(m => m.ServCode).Name("NetworkName");
            //Map(m => m.endDate).Name("EndDateTimeUTC");
            Map(m => m.Organization).Name("Organization");
            Map(m => m.genCategory).Name("GeneralCategory");
            Map(m => m.conceptKeyword).Name("ConceptKeyword");
            Map(m => m.Sitename).Name("SiteName");
            Map(m => m.VarName).Name("VariableName");
            Map(m => m.latitude).Name("Latitude");
            Map(m => m.TimeUnitAbbrev).Name("TimeUnitAbbrev");
            //Map(m => m.s).Name("NetworkID");
            Map(m => m.ValueCount).Name("Valuecount");
            //Map(m => m.ServURL).Name("Timestamp");
            //Map(m => m.n).Name("NoDataValue");
            //Map(m => m.beginDate).Name("BeginDateTimeUTC");
            Map(m => m.timeunits).Name("TimeUnits");
            Map(m => m.endDate).Name("EndDateTime");
            Map(m => m.IsRegular).Name("IsRegular");
            Map(m => m.QCLDesc).Name("QCLDesc");
            Map(m => m.datatype).Name("DataType");
            Map(m => m.beginDate).Name("BeginDateTime");
            //Map(m => m.).Name("id");
            Map(m => m.Speciation).Name("Speciation");
            Map(m => m.SourceOrg).Name("SourceOrg");
            Map(m => m.VarCode).Name("VariableCode");
            Map(m => m.location).Name("SiteCode");
            Map(m => m.valuetype).Name("ValueType");
            Map(m => m.VariableUnitsAbbrev).Name("VariableUnitAbbrev");
            Map(m => m.SourceId).Name("SourceID");
            Map(m => m.SourceDesc).Name("SourceDesc");
            //Map(m => m.var).Name("VariableUnitsName");
            Map(m => m.longitude).Name("Longitude");
            Map(m => m.TimeSupport).Name("TimeSupport");
            Map(m => m.samplemedium).Name("SampleMedium");
            Map(m => m.MethodId).Name("MethodID");
            //Map(m => m.ServURL).Name("_version_");
            //Map(m => m.).Name("SourceCite");
            //Map(m => m.q).Name("QCLCode");
            //Map(m => m.ServURL).Name("VariableName_text");

        }
    }

    public class SeriesMetadataFromSolr
    {
        public string ServiceWSDL { get; set; }
        public string MethodDesc { get; set; }
        public string QCLID { get; set; }
        public string NetworkName { get; set; }
        public string EndDateTimeUTC { get; set; }
        public string Organization { get; set; }
        public string GeneralCategory { get; set; }
        public string ConceptKeyword { get; set; }
        public string SiteName { get; set; }
        public string VariableName { get; set; }
        public string Latitude { get; set; }
        public string TimeUnitAbbrev { get; set; }
        public string NetworkID { get; set; }
        public string Valuecount { get; set; }
        public string Timestamp { get; set; }
        public string NoDataValue { get; set; }
        public string BeginDateTimeUTC { get; set; }
        public string TimeUnits { get; set; }
        public string EndDateTime { get; set; }
        public string IsRegular { get; set; }
        public string QCLDesc { get; set; }
        public string DataType { get; set; }
        public string BeginDateTime { get; set; }
        public string id { get; set; }
        public string Speciation { get; set; }
        public string SourceOrg { get; set; }
        public string VariableCode { get; set; }
        public string SiteCode { get; set; }
        public string ValueType { get; set; }
        public string VariableUnitAbbrev { get; set; }
        public string SourceID { get; set; }
        public string SourceDesc { get; set; }
        public string VariableUnitsName { get; set; }
        public string Longitude { get; set; }
        public string TimeSupport { get; set; }
        public string SampleMedium { get; set; }
        public string MethodID { get; set; }
        public string _version_ { get; set; }
        public string SourceCite { get; set; }
        public string QCLCode { get; set; }
        public string VariableName_text { get; set; }

    }

    public struct FacetField
    {
        public string name;
        public long itemCount;
        public item[] items;
        //public FacetFieldValue[] facetValues;
    }

    public struct item
    {
        public string term;
        public string definition;
        public long count;
    }

    public struct CountOrData
    {
        public long nseries;
        public string message;
        public FacetField[] facet_fields;
        public SeriesRecordFull[] series;
    }

    public struct GetSeriesCountOrData
    {
        public long nseries;
        public string message;
        public FacetField[] facet_fields;
        public SeriesRecordFull2[] series;
    }


    [WebMethod(Description = "<br><p style = 'margin-left:25px;'><strong>DEPRECATED</strong> Returns metadata for timeseries that match the provided parameters.The returned object contains a subset of the available metadata sufficient for basic searches.</p> </br>" +
        "<p style = 'margin-left:25px;'>It does not contain data for e.g Quality control level or source</p>")]
     
    public SeriesRecord[] GetSeriesCatalogForBox(Box box, String conceptCode,
            int[] networkIDs, string beginDate, string endDate)
    {
        ServiceStats.AddCount("GetSeriesCatalogForBox");
        String networkString = "";
        if (networkIDs != null && networkIDs.Length > 0)
        {
            for (int i = 0; i < networkIDs.Length; i++)
            {
                if (i > 0) networkString += ",";
                networkString += (networkIDs[i]).ToString();
            }
        }
        return GetSeriesCatalogForBox2(box.xmin, box.xmax, box.ymin, box.ymax, conceptCode, networkString, beginDate, endDate);
    }

    [WebMethod(Description = "<br><p style = 'margin-left:25px;'>Returns metadata for timeseries that match the provided parameters.The returned object contains a subset of the available metadata sufficient for basic searches.</p> </br>" +
      "<p style = 'margin-left:25px;'>It does not contain data for e.g Quality control level or source</p>")]
    public SeriesRecord[] GetSeriesCatalogForBox2(double xmin, double xmax, double ymin, double ymax,
                              string conceptKeyword, String networkIDs,
                          string beginDate, string endDate)
    {
        SeriesRecord[] series = null;
        if (!validLatLonDateTime(xmin, xmax, ymin, ymax, beginDate, endDate)) return series;

        int Max_rows = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SOLRnrows"]);
        string url = requestUrl(xmin, xmax, ymin, ymax,
                                conceptKeyword, networkIDs,
                                Uri.UnescapeDataString(beginDate), Uri.UnescapeDataString(endDate))
                    + String.Format(@"&rows={0}", Max_rows);

        if (url == null) return series;

        XDocument xDocument;
        string response = null;
        using (WebClient client = new WebClient())
        {
            response = client.DownloadString(url);

            TextReader xmlReader = new StringReader(response);
            xDocument = XDocument.Load(xmlReader);

            //If using .Net 4.0 or above, better to use Linq to XML
            // Note: the following fields could be NULL
            //       SiteName, DataType, SampleMedium, TimeUnits, GeneralCategory
            series =
            (from o in xDocument.Descendants("doc")
             select new SeriesRecord()
             {
                 location = o.Elements("str").Single(x => x.Attribute("name").Value == "SiteCode").Value.ToString(), //???
                 //SiteCode like 'EPA:SDWRAP:LOUCOTTMC01',  Sitename==NULL
                 Sitename = o.Descendants("str").Where(e => (string)e.Attribute("name") == "SiteName").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "SiteName").Value.ToString(),
                 ServURL = o.Elements("str").Single(x => x.Attribute("name").Value == "ServiceWSDL").Value.ToString(),
                 ServCode = o.Elements("str").Single(x => x.Attribute("name").Value == "NetworkName").Value.ToString(),
                 latitude = double.Parse(o.Elements("double").Single(x => x.Attribute("name").Value == "Latitude").Value.ToString()),
                 longitude = double.Parse(o.Elements("double").Single(x => x.Attribute("name").Value == "Longitude").Value.ToString()),
                 ValueCount = int.Parse(o.Elements("long").Single(x => x.Attribute("name").Value == "Valuecount").Value.ToString()),
                 VarName = o.Elements("str").Single(x => x.Attribute("name").Value == "VariableName").Value.ToString(),
                 VarCode = o.Elements("str").Single(x => x.Attribute("name").Value == "VariableCode").Value.ToString(),
                 beginDate = o.Elements("date").Single(x => x.Attribute("name").Value == "BeginDateTime").Value.ToString(),
                 endDate = o.Elements("date").Single(x => x.Attribute("name").Value == "EndDateTime").Value.ToString(),
                 datatype = o.Descendants("str").Where(e => (string)e.Attribute("name") == "DataType").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "DataType").Value.ToString(),
                 valuetype = o.Elements("str").Single(x => x.Attribute("name").Value == "ValueType").Value.ToString(),
                 samplemedium = o.Descendants("str").Where(e => (string)e.Attribute("name") == "SampleMedium").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "SampleMedium").Value.ToString(),
                 timeunits = o.Descendants("str").Where(e => (string)e.Attribute("name") == "TimeUnits").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "TimeUnits").Value.ToString(),
                 conceptKeyword = o.Elements("str").Single(x => x.Attribute("name").Value == "ConceptKeyword").Value.ToString(),
                 genCategory = o.Descendants("str").Where(e => (string)e.Attribute("name") == "GeneralCategory").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "GeneralCategory").Value.ToString(),
                 TimeSupport = o.Descendants("long").Where(e => (string)e.Attribute("name") == "TimeSupport").Count() == 0 ? "0" : o.Elements("long").Single(x => x.Attribute("name").Value == "TimeSupport").Value.ToString(),
             }).ToArray();
        }

        //------------------------------------------------------------
        //modify endDateTime for the returned series for NASA networks
        //------------------------------------------------------------
        series = updateSeries_NasaEndDT(series);

        return series;
    }


    //Jan.2017, YX, abstracted getSeriesFull()
    /// <summary>
    /// Added by MS to return all 5 parameters (site, var, method, QC level source) for a timeseries to client 
    /// </summary>
    /// <param name="xmin"></param>
    /// <param name="xmax"></param>
    /// <param name="ymin"></param>
    /// <param name="ymax"></param>
    /// <param name="conceptKeyword"></param>
    /// <param name="networkIDs"></param>
    /// <param name="beginDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
  
     [WebMethod(Description = "<br> <p style = 'margin-left:25px;' >Returns metadata for timeseries that match the provided parameters. The returned object contains the full set of the available metadata sufficient for complex searches.</p>" +
        "<p style = 'margin-left:25px;'>It does contain data for e.g Qualitycontrol Level or Source</p>")]
    public SeriesRecordFull[] GetSeriesCatalogForBox3(double xmin, double xmax, double ymin, double ymax, string sampleMedium, string dataType, string valueType,
                             string conceptKeyword, string networkIDs,
                         string beginDate, string endDate)
    {
        SeriesRecordFull[] series = null;
        if (!validLatLonDateTime(xmin, xmax, ymin, ymax, beginDate, endDate)) return series;

        int Max_rows = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SOLRnrows"]);

        //rows is cut off to MAX_rows
        string url = requestUrl(xmin, xmax, ymin, ymax,
                                conceptKeyword, networkIDs,
                                beginDate, endDate)
                   + String.Format(@"&rows={0}", Max_rows);

        if (url == null) return series;

        series = getSeriesFull(url);

        //------------------------------------------------------------
        //modify endDateTime for the returned series for NASA networks
        //------------------------------------------------------------
        series = updateSeriesFull_NasaEndDT(series);

        return series;
    }


    //Jan.2019, YX, abstracted from GetSeriesCatalogForBox3()
    private SeriesRecordFull[] getSeriesFull(string url)
    {
        XDocument xDocument;
        SeriesRecordFull[] series = null;
        string response = null;
        using (WebClient client = new WebClient())
        {
            client.Encoding = Encoding.UTF8;

            response = client.DownloadString(url);

            TextReader xmlReader = new StringReader(response);
            xDocument = XDocument.Load(xmlReader);

            //If using .Net 4.0 or above, better to use Linq to XML
            // Note: the following fields could be NULL
            //       SiteName, DataType, SampleMedium, TimeUnits, GeneralCategory
            series =
            (from o in xDocument.Descendants("doc")
             select new SeriesRecordFull()
             {
                 location = o.Elements("str").Single(x => x.Attribute("name").Value == "SiteCode").Value.ToString(), //???
                 ////SiteCode like 'EPA:SDWRAP:LOUCOTTMC01',  Sitename==NULL
                 Sitename = o.Descendants("str").Where(e => (string)e.Attribute("name") == "SiteName").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "SiteName").Value.ToString(),
                 ServURL = o.Elements("str").Single(x => x.Attribute("name").Value == "ServiceWSDL").Value.ToString(),
                 ServCode = o.Elements("str").Single(x => x.Attribute("name").Value == "NetworkName").Value.ToString(),
                 latitude = double.Parse(o.Elements("double").Single(x => x.Attribute("name").Value == "Latitude").Value.ToString()),
                 longitude = double.Parse(o.Elements("double").Single(x => x.Attribute("name").Value == "Longitude").Value.ToString()),
                 ValueCount = int.Parse(o.Elements("long").Single(x => x.Attribute("name").Value == "Valuecount").Value.ToString()),
                 VarName = o.Elements("str").Single(x => x.Attribute("name").Value == "VariableName").Value.ToString(),
                 VarCode = o.Elements("str").Single(x => x.Attribute("name").Value == "VariableCode").Value.ToString(),
                 beginDate = o.Elements("date").Single(x => x.Attribute("name").Value == "BeginDateTime").Value.ToString(),
                 endDate = o.Elements("date").Single(x => x.Attribute("name").Value == "EndDateTime").Value.ToString(),
                 datatype = o.Descendants("str").Where(e => (string)e.Attribute("name") == "DataType").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "DataType").Value.ToString(),
                 valuetype = o.Elements("str").Single(x => x.Attribute("name").Value == "ValueType").Value.ToString(),
                 samplemedium = o.Descendants("str").Where(e => (string)e.Attribute("name") == "SampleMedium").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "SampleMedium").Value.ToString(),
                 timeunits = o.Descendants("str").Where(e => (string)e.Attribute("name") == "TimeUnits").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "TimeUnits").Value.ToString(),
                 conceptKeyword = o.Elements("str").Single(x => x.Attribute("name").Value == "ConceptKeyword").Value.ToString(),
                 genCategory = o.Descendants("str").Where(e => (string)e.Attribute("name") == "GeneralCategory").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "GeneralCategory").Value.ToString(),
                 TimeSupport = o.Descendants("long").Where(e => (string)e.Attribute("name") == "TimeSupport").Count() == 0 ? "0" : o.Elements("long").Single(x => x.Attribute("name").Value == "TimeSupport").Value.ToString(),
                 QCLID = o.Descendants("str").Where(e => (string)e.Attribute("name") == "QCLID").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "QCLID").Value.ToString(),
                 QCLDesc = o.Descendants("str").Where(e => (string)e.Attribute("name") == "QCLDesc").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "QCLDesc").Value.ToString(),
                 Organization = o.Descendants("str").Where(e => (string)e.Attribute("name") == "Organization").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "Organization").Value.ToString(),
                 TimeUnitAbbrev = o.Descendants("str").Where(e => (string)e.Attribute("name") == "TimeUnitAbbrev").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "TimeUnitAbbrev").Value.ToString(),
                 TimeUnits = o.Descendants("str").Where(e => (string)e.Attribute("name") == "TimeUnits").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "TimeUnits").Value.ToString(),
                 //IsRegular =  bool.Parse(o.Elements("str").Single(x => x.Attribute("name").Value == "IsRegular").Value.ToString()),
                 IsRegular = o.Descendants("bool").Where(e => (string)e.Attribute("name") == "IsRegular").Count() == 0 ? "" : o.Elements("bool").Single(x => x.Attribute("name").Value == "IsRegular").Value.ToString(),
                 //IsRegular = o.Elements("bool").Single(x => x.Attribute("name").Value == "IsRegular").Value.ToString(),
                 SeriesCode = o.Elements("str").Single(x => x.Attribute("name").Value == "id").Value.ToString(),
                 Speciation = o.Descendants("str").Where(e => (string)e.Attribute("name") == "Speciation").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "Speciation").Value.ToString(),
                 SourceOrg = o.Descendants("str").Where(e => (string)e.Attribute("name") == "SourceOrg").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "SourceOrg").Value.ToString(),
                 VariableUnitsAbbrev = o.Descendants("str").Where(e => (string)e.Attribute("name") == "VariableUnitAbbrev").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "VariableUnitAbbrev").Value.ToString(),
                 SourceId = o.Descendants("str").Where(e => (string)e.Attribute("name") == "SourceID").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "SourceID").Value.ToString(),
                 SourceDesc = o.Descendants("str").Where(e => (string)e.Attribute("name") == "SourceDesc").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "SourceDesc").Value.ToString(),
                 MethodId = o.Descendants("str").Where(e => (string)e.Attribute("name") == "MethodID").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "MethodID").Value.ToString(),
                 MethodDesc = o.Descendants("str").Where(e => (string)e.Attribute("name") == "MethodDesc").Count() == 0 ? "" : o.Elements("str").Single(x => x.Attribute("name").Value == "MethodDesc").Value.ToString(),
             }).ToArray();
        }

        return series;
    }

    private SeriesRecordFull2[] getSeriesMetadataFromSolr(string url)
    {
        string response = null;
        SeriesRecordFull2[] series = null;


        using (WebClient client = new WebClient())
        {
            client.Encoding = Encoding.UTF8;

            var returnType = "&wt=csv";
            response = client.DownloadString(url + returnType);

            //System.IO.File.WriteAllText("c:/cuahsi/response.dat", response);

            TextReader reader = new StringReader(response);

            var csvReader = new CsvHelper.CsvReader(reader);
            csvReader.Configuration.RegisterClassMap<CSVFileDefinitionMap>();
            try
            {
                series = csvReader.GetRecords<SeriesRecordFull2>().ToArray();
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //clean up ressources
                reader.Close();
                if (reader != null) reader.Close();
            }
        }

        return series;
    }


    private long GetCount(string url)
    {
        XDocument xDocument;
        string response = null;
        long nseries;
        using (WebClient client = new WebClient())
        {
            response = client.DownloadString(url);
            TextReader xmlReader = new StringReader(response);
            xDocument = XDocument.Load(xmlReader);
            var numFound = (from n in xDocument.Descendants("result")
                            where n.Attribute("name").Value == "response"
                            select n.Attribute("numFound").Value).FirstOrDefault().ToString();
            nseries = long.Parse(numFound);
        }
        return nseries;
    }

    private bool validLatLonDateTime(double xmin, double xmax, double ymin, double ymax, string beginDate, string endDate)
    {
        try
        {
            if (xmin > xmax || ymin > ymax)
                throw new InvalidOperationException("xmin should be less/equal than xmax | ymin should be less/equal than ymax");

            DateTime beginDT_query;
            DateTime.TryParse(beginDate, out beginDT_query);
            DateTime endDT_query;
            DateTime.TryParse(endDate, out endDT_query);
            if (DateTime.Compare(beginDT_query, endDT_query) > 0)
                throw new InvalidOperationException("beginDateTime shoud be prior to endDateTime");
        }
        catch (FormatException e)
        {
            throw new FormatException();
        }

        return true;
    }

    /// <summary>
    /// Jan. 2017, YX 
    /// required parameters:
    /// <param name="getData">true|false</param>
    /// <param name="getFacetOnCV">true|false</param>
    /// <param name="xmin">-180.</param>
    /// <param name="xmax">180.</param>
    /// <param name="ymin">-90</param>
    /// <param name="ymax">90</param>
    /// 
    /// default all: blank
    /// <param name="sampleMedium"></param>
    /// <param name="dataType"></param>
    /// <param name="valueType"></param>
    /// <param name="generalCategory"></param>
    ///
    /// allowed conceptKeyword input, case insensitive
    /// <param name="conceptKeyword"></param>
    /// <param name="conceptKeyword">*</param>
    /// <param name="conceptKeyword">all</param>
    ///    '|' separated string
    /// <param name="conceptKeyword">Precipitation | Temperature | Carbon, total</param>   
    /// 
    /// allowed networkID input, case insensitive
    /// <param name="networkIDs"></param>
    /// <param name="networkIDs">all</param>  
    /// <param name="networkIDs">*</param>
    /// <param name="networkIDs"></param>
    /// <param name="networkIDs">1, 3, 52</param>   
    /// <param name="networkIDs">1 3 52</param>
    ///  
    /// allowed datetime format
    /// <param name="beginDate">2011-05-21T00:00:00Z</param>
    /// <param name="beginDate">2011-05-21 09:30</param>
    /// <param name="beginDate">2011-05-21</param>
    /// <param name="beginDate">3/20/2000 8:20</param>
    /// <param name="beginDate">3/9/2000 8:20:30</param>
    /// <param name="beginDate">05/21/2011</param>
    /// <param name="beginDate">5/21/2011</param>
    /// 
    /// default beginDate/endDate
    /// <param name="beginDate"></param>  default: 1800-01-01
    /// <param name="endDate"></param>    default: 2100-01-01 
    /// </summary>
    ///  
    [WebMethod(Description = "<br> <p style = 'margin-left:25px;' ><strong>DEPRECATED</strong> use GetSeriesMetadataCountOrData) Provides information about metadata stored in the catalog.Tycally used to search the catalog. </p><br>" +
        "It can return " +
        "<ul>" +
         " <li> 1.The count of timeseries that match the provided parameters. </li>" +
           "<li> 2.the statistics for the distribution of all facets for timeseries that match the provided parameters.e.g how many timeseries have the datatype 'average', or the keyword 'precipitation'.</li>" +
           "<li> 3.the complete set of all metadata records for timeseries that match the provided parameters. </li >" +
        "</ul >" +
        "<p> The return can be defined by providing the appropriate parameters in the request.The return of this request can not exceed 25.000 timeseries. </p> ")]
    public CountOrData GetCountOrData(bool getData, bool getFacetOnCV, double xmin, double xmax, double ymin, double ymax,
                            string sampleMedium, string dataType, string valueType, string generalCategory,
                            string conceptKeyword, string networkIDs,
                            string beginDate, string endDate)
    {

        CountOrData countOrData = new CountOrData();

        //-------------------------
        //Validate input parameters
        //-------------------------
        if (!validLatLonDateTime(xmin, xmax, ymin, ymax, beginDate, endDate)) return countOrData;

        long nseries;
        int Max_rows = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SOLRnrows"]);

        string urlbase = requestUrlwithCV(xmin, xmax, ymin, ymax,
                     sampleMedium, dataType, valueType, generalCategory,
                     conceptKeyword, networkIDs,
                     beginDate, endDate);

        if (urlbase == null)
        {
            countOrData.message = "No data found! Please reset your search parameters!";
            return countOrData;
        }

        string url = urlbase;
        if (getData == false) url = urlbase + "&rows=0";

        //Get total nseries
        nseries = GetCount(url);
        countOrData.nseries = nseries;
        if (nseries == 0)
        {
            countOrData.message = "No data found! Please reset your search parameters!";
            return countOrData;
        }

        //returned series is limited by Max_rows
        if (nseries > Max_rows)
        {
            countOrData.message = "the number of series returned exceeds the maximum of " + Max_rows;
            if (getFacetOnCV == false) return countOrData;
        }

        if (getFacetOnCV == true)
        {
            string[] facetfields = { "DataType", "ValueType", "SampleMedium", "GeneralCategory",
                                    "NetworkID",  "ConceptKeyword", "SourceOrg"};
            bool getFacetDefinition = false;
            //FacetField[] 
            countOrData.facet_fields = GetFacetField(facetfields, url, getFacetDefinition);
        }

        if (getData == true && nseries <= Max_rows)
        {
            url = urlbase + String.Format("&rows={0}", Max_rows);
            SeriesRecordFull[] series = getSeriesFull(url);

            //------------------------------------------------------------
            //modify endDateTime for the returned series for NASA networks
            //------------------------------------------------------------
            countOrData.series = updateSeriesFull_NasaEndDT(series);
        }

        return countOrData;
    }


    [WebMethod(Description = "<br> <p style = 'margin-left:25px;' >Provides information about metadata stored in the catalog.Tycally used to search the catalog. <br>" +
        "It can return " +
        "<ul>" +
         " <li> 1.The count of timeseries that match the provided parameters. </li>" +
           "<li> 2.the statistics for the distribution of all facets for timeseries that match the provided parameters.e.g how many timeseries have the datatype 'average', or the keyword 'precipitation'.</li>" +
           "<li> 3.the complete set of all metadata records for timeseries that match the provided parameters. </li >" +
        "</ul >" +
        "<p> The return can be defined by providing the appropriate parameters in the request.The return of this request can not exceed 25.000 timeseries. </p> ")]



    public GetSeriesCountOrData GetSeriesMetadataCountOrData(bool getData, bool getFacetOnCV, double xmin, double xmax, double ymin, double ymax,
                           string sampleMedium, string dataType, string valueType, string generalCategory,
                           string conceptKeyword, string networkIDs,
                           string beginDate, string endDate)
    {

        var seriesCountOrData = new GetSeriesCountOrData();

        //-------------------------
        //Validate input parameters
        //-------------------------
        if (!validLatLonDateTime(xmin, xmax, ymin, ymax, beginDate, endDate)) return seriesCountOrData;

        long nseries;
        int Max_rows = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SOLRnrows"]);

        string urlbase = requestUrlwithCV(xmin, xmax, ymin, ymax,
                     sampleMedium, dataType, valueType, generalCategory,
                     conceptKeyword, networkIDs,
                     beginDate, endDate);

        if (urlbase == null)
        {
            seriesCountOrData.message = "No data found! Please reset your search parameters!";
            return seriesCountOrData;
        }

        string url = urlbase;
        if (getData == false) url = urlbase + "&rows=0";

        //Get total nseries
        nseries = GetCount(url);
        seriesCountOrData.nseries = nseries;

        //returned series is limited by Max_rows
        if (nseries > Max_rows)
        {
            seriesCountOrData.message = "the number of series returned exceeds the maximum of " + Max_rows;
            if (getFacetOnCV == false) return seriesCountOrData;
        }

        if (getFacetOnCV == true)
        {
            string[] facetfields = { "DataType", "ValueType", "SampleMedium", "GeneralCategory",
                                    "NetworkID",  "ConceptKeyword", "SourceOrg"};
            bool getFacetDefinition = false;
            //FacetField[] 
            seriesCountOrData.facet_fields = GetFacetField(facetfields, url, getFacetDefinition);
        }

        if (getData == true && nseries <= Max_rows)
        {
            url = urlbase + String.Format("&rows={0}", Max_rows);
            //SeriesRecordFull[] series = getSeriesFull(url);

            try
            {
                seriesCountOrData.series = getSeriesMetadataFromSolr(url);
            }
            catch (Exception ex)
            {
                throw;
            }

            //------------------------------------------------------------
            //modify endDateTime for the returned series for NASA networks
            //------------------------------------------------------------
            seriesCountOrData.series = updateSeriesFull2_NasaEndDT(seriesCountOrData.series);
        }

        return seriesCountOrData;
    }


    private SeriesRecord[] updateSeries_NasaEndDT(SeriesRecord[] series)
    {
        for (int i = 0; i < series.Length; i++)
        {
            if (series[i].ServCode.Contains("TRMM"))
            {
                series[i].endDate = NasaEndDT("TRMM").ToString("yyyy-MM-ddThh:mm:ssZ");
            }
            else if (series[i].ServCode.Contains("NLDAS"))
            {
                series[i].endDate = NasaEndDT("NLDAS").ToString("yyyy-MM-ddThh:mm:ssZ");
            }
            else if (series[i].ServCode.Contains("GLDAS"))
            {
                series[i].endDate = NasaEndDT("GLDAS").ToString("yyyy-MM-ddThh:mm:ssZ");
            }
        }

        return series;
    }

    private DateTime NasaEndDT(string networkname)
    {
        DateTime endDT = DateTime.Now;

        int deltdays_TRMM = int.Parse(System.Configuration.ConfigurationManager.AppSettings["EndDateTime_deltdays_TRMM"]);
        int deltdays_NLDAS = int.Parse(System.Configuration.ConfigurationManager.AppSettings["EndDateTime_deltdays_NLDAS"]);
        string endDateTime_GLDAS = System.Configuration.ConfigurationManager.AppSettings["EndDateTime_GLDAS"];

        switch (networkname)
        {
            case "GLDAS":
                DateTime.TryParse(endDateTime_GLDAS, out endDT);
                break;
            case "NLDAS":
                endDT = DateTime.Now.AddDays(deltdays_NLDAS);
                break;
            case "TRMM":
                endDT = DateTime.Now.AddDays(deltdays_TRMM);
                break;
        }

        return endDT;
    }

    private SeriesRecordFull[] updateSeriesFull_NasaEndDT(SeriesRecordFull[] series)
    {

        for (int i = 0; i < series.Length; i++)
        {
            if (series[i].ServCode.Contains("TRMM"))
            {
                series[i].endDate = NasaEndDT("TRMM").ToString("yyyy-MM-ddThh:mm:ssZ");
            }
            else if (series[i].ServCode.Contains("NLDAS"))
            {
                series[i].endDate = NasaEndDT("NLDAS").ToString("yyyy-MM-ddThh:mm:ssZ");
            }
            else if (series[i].ServCode.Contains("GLDAS"))
            {
                series[i].endDate = NasaEndDT("GLDAS").ToString("yyyy-MM-ddThh:mm:ssZ");
            }
        }

        return series;
    }

    private SeriesRecordFull2[] updateSeriesFull2_NasaEndDT(SeriesRecordFull2[] series)
    {

        for (int i = 0; i < series.Length; i++)
        {
            if (series[i].ServCode.Contains("TRMM"))
            {
                series[i].endDate = NasaEndDT("TRMM").ToString("yyyy-MM-ddThh:mm:ssZ");
            }
            else if (series[i].ServCode.Contains("NLDAS"))
            {
                series[i].endDate = NasaEndDT("NLDAS").ToString("yyyy-MM-ddThh:mm:ssZ");
            }
            else if (series[i].ServCode.Contains("GLDAS"))
            {
                series[i].endDate = NasaEndDT("GLDAS").ToString("yyyy-MM-ddThh:mm:ssZ");
            }
        }

        return series;
    }
    
    ///Jan.2017 YX, add query filter by: 
         ///   SampleMedium
         ///   DataType
         ///   ValueType
         ///   GeneralCategory  
    private string requestUrlwithCV(double xmin, double xmax, double ymin, double ymax,
                        string sampleMedium, string dataType, string valueType, string generalCategory,
                        string conceptKeyword, string networkIDs,
                        string beginDate, string endDate)
    {
        //get url without CV filter query
        string url = requestUrl(xmin, xmax, ymin, ymax, conceptKeyword, networkIDs, beginDate, endDate);
        if (url == null) return null;

        //------------------------------------------------------
        //Create query parameter for CV terms: serach logic AND
        //------------------------------------------------------
        //string qSampleMedium = getQueryString("SampleMedium", sampleMedium);
        //string qDataType = getQueryString("DataType", dataType);
        //string qValueType = getQueryString("ValueType", valueType);
        //string qGeneralCategory = getQueryString("GeneralCategory", generalCategory);
        //url = url + String.Format(@"&fq={0}&fq={1}&fq={2}&fq={3}", qSampleMedium, qDataType, qValueType, qGeneralCategory);

        //----------------------------------------------------
        //Create query parameter for CV terms: serach logic OR
        //----------------------------------------------------
        string[] cvFields = new string[] { "SampleMedium", "DataType", "ValueType", "GeneralCategory" };
        string[] cvInput = new string[] { sampleMedium, dataType, valueType, generalCategory };

        string qCVstring = String.Empty;
        for(int i=0; i < cvFields.Length; i++) {
            if(!cvInput[i].Equals("") && !cvInput[i].Equals(""))
            {
                string query = getQueryStringMulti(cvFields[i], cvInput[i]);
                qCVstring = qCVstring + String.Format(@"{0}+OR+", query);
            }

        }
        if (qCVstring.Equals(String.Empty)) return url;

        qCVstring = qCVstring.Substring(0, qCVstring.Length - 4);
        url = url + "&fq=" + qCVstring;

        return url;
    }

    private string getQueryStringMulti(string cvFieldName, string cvString)
    {
        string qString = String.Empty;
        string[] terms = null;

        cvFieldName = cvFieldName.Trim();

        //no space
        char[] delimiters = new char[] { ',', ';', '|' };
        terms = cvString.ToLower().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        foreach (var term in terms)
        {
            qString = qString +
                String.Format(@"({0}:%22{1}%22)+OR+", cvFieldName, HttpUtility.UrlEncode(term.Trim())); 
        }

        qString = qString.Substring(0, qString.Length - 4);


        return qString;
    }

    ///Dec.2015 YX, to adjust Concept search
    ///Sep.2016 YX, to take into accout the out-dated EndDateTime in the database for NASA networks
    private string requestUrl(double xmin, double xmax, double ymin, double ymax,
                              string conceptKeyword, string networkIDs,
                          string beginDate, string endDate)
    {
        string qNetworkIDs;
        string qConcept;
        string qLat, qLon;
        string keywordString = String.Empty;
        HashSet<string> keywordSet = new HashSet<string>();

        string endpoint = System.Configuration.ConfigurationManager.AppSettings["SOLRendpoint"];
        if (!endpoint.EndsWith("/")) endpoint = endpoint + "/";

        //--------------------------------------
        //Create query parameter for networkID
        //--------------------------------------
        if (networkIDs.Equals("") || conceptKeyword.Equals("*"))
        {
            qNetworkIDs = @"NetworkID:*";
        }
        else if (networkIDs.Length == 1)
        {
            qNetworkIDs = String.Format("NetworkID:{0}", networkIDs);
        }
        else
        {
            //allowing multiple networkIDs, select?q=NetworkID:(1 2 3)
            string[] parts = networkParser(networkIDs);
            qNetworkIDs = @"NetworkID:(";
            foreach (string part in parts)
            {
                qNetworkIDs += part + ' ';
            }
            qNetworkIDs += ')';
        }

        //----------------------------------------------------
        //Create query parameter for conceptKeyword
        //----------------------------------------------------
        if (conceptKeyword.Equals("") || conceptKeyword.Equals("*"))
        {
            qConcept = @"ConceptKeyword:*";
        }
        else if (conceptKeyword.Equals("all", StringComparison.InvariantCultureIgnoreCase))
        {
            qConcept = @"ConceptKeyword:*";
        }
        else
        {
            string[] keywords = conceptKeyword.ToLower().Split('|');
            foreach (var keyword in keywords)
            {
                //search in sql database first for any synonym 
                string keywordOntology = mapSearchableConcept(keyword);

                if (keywordOntology == null) keywordSet.Add(keyword);
                else keywordSet.Add(keywordOntology.Trim().ToLower());

                //Get leaf concepts for input conceptKeyword
                //May 2017, modified to get leaf keywords from partial Ontology tree only
                //                          if not found in partial ontology tree, return null
                string[] subconceptList = getLeafKeywordsPartial(keyword.Trim());

                if (subconceptList != null)
                {
                    foreach (var subKeyword in subconceptList)
                    {
                        keywordSet.Add(subKeyword.ToLower());
                    }
                }
            }

            foreach (var keyword in keywordSet)
            {
                keywordString += String.Format("ConceptKeyword:%22{0}%22+OR+", HttpUtility.UrlEncode(keyword));
            }
            qConcept = keywordString.Substring(0, keywordString.Length - 4);
        }

        //----------------------------------------------------
        //Create query parameter for Latitude/Longitude
        //----------------------------------------------------
        qLat = String.Format("Latitude:[{0:0.0000} TO {1:0.0000}]", ymin, ymax);
        qLon = String.Format("Longitude:[{0:0.0000} TO {1:0.0000}]", xmin, xmax);

        //-------------------------
        //query parameters to solr
        //-------------------------
        String reqType = "edismax";

        //----------------------------------------------------
        //Create query parameter for beginDateTime/endDateTime
        //----------------------------------------------------
        beginDate = validateDT(beginDate, "1800-01-01");
        endDate = validateDT(endDate, "2100-01-01");

        //----------------------------------------------------
        //Since NASA networks (262, 267, 274, 479) are not harvested frequenly, hence the EndDateTime is not updated in Solr Database, 
        //  query parameters for BeginDateTime&EndDateTime have to be modified in order to mimic the case where EndDateTime IS updated frequently 
        //----------------------------------------------------
        var qBeginDT = String.Format(@"BeginDateTime:[* TO {0}T00:00:00Z]", endDate);
        var qEndDT = String.Format(@"EndDateTime:[{0}T00:00:00Z TO *]", beginDate);
        var qEndDT_Exclude = String.Format(@"-EndDateTime:[* TO {0}T00:00:00Z]", beginDate);

        //exclude those with: stored beginDateTime > queried endDateTime 
        // However, include the day of endDate. T12:00:00 instead of T00:00:00
        var qBeginDT_Exclude = String.Format(@"-BeginDateTime:[{0}T12:00:00Z TO *]", endDate);

        string qDateTime_GLDAS = String.Format(@"(NetworkID:(262) {0} {1})", qEndDT_Exclude, qBeginDT_Exclude);

        string qDateTime_NLDAS = String.Format(@"({0} {1})", qNetworkIDs, qBeginDT_Exclude);
        if (qNetworkIDs.Equals("NetworkID:*")) qDateTime_NLDAS = String.Format(@"(NetworkID:(267+OR+274) {0})", qBeginDT_Exclude);

        string qDateTime_TRMM = String.Format(@"(NetworkID:(479) {0})", qBeginDT_Exclude);

        //For nasa networks, EndDateTime is modified, and exclude those BeginDateTime > endDate(defined in web.config)
        DateTime beginDate_query;
        DateTime.TryParse(beginDate, out beginDate_query);
        if (DateTime.Compare(beginDate_query, NasaEndDT("NLDAS")) > 0) qDateTime_NLDAS = String.Empty;
        if (DateTime.Compare(beginDate_query, NasaEndDT("TRMM")) > 0) qDateTime_TRMM = String.Empty;

        string parameters = String.Empty;
        string qDateTime_NASA = String.Empty;

        //GLDAS
        if (networkIDs.Contains("262"))
            parameters = parameters + qDateTime_GLDAS;
        //NLDAS      
        else if (networkIDs.Contains("267") || networkIDs.Contains("274"))
            parameters = parameters + qDateTime_NLDAS;
        //TRMM
        else if (networkIDs.Contains("479"))
            parameters = parameters + qDateTime_TRMM;
        else if (networkIDs.Equals("") || networkIDs.Contains("*"))
        {
            if (!String.IsNullOrEmpty(qDateTime_GLDAS)) qDateTime_NASA = qDateTime_NASA + qDateTime_GLDAS + "+OR+";
            if (!String.IsNullOrEmpty(qDateTime_NLDAS)) qDateTime_NASA = qDateTime_NASA + qDateTime_NLDAS + "+OR+";
            if (!String.IsNullOrEmpty(qDateTime_TRMM)) qDateTime_NASA = qDateTime_NASA + qDateTime_TRMM + "+OR+";

            if (String.IsNullOrEmpty(qDateTime_NASA))
                parameters = String.Format(@"(*:* -NetworkID:(262+OR+267+OR+274+OR+479)+AND+ _query_:%22{0}+AND+{1}%22)", qBeginDT, qEndDT);
            else
            {
                qDateTime_NASA = qDateTime_NASA.Substring(0, qDateTime_NASA.Length - 4);
                parameters = qDateTime_NASA + String.Format(@"+OR+(*:* -NetworkID:(262+OR+267+OR+274+OR+479)+AND+ _query_:%22{0}+AND+{1}%22)", qBeginDT, qEndDT);
            }
        }
        else
            parameters = String.Format(@"{0}&fq={1}&fq={2}", qNetworkIDs, qBeginDT, qEndDT);

        //beginDate_query > NasaEndDT("NLDAS") OR beginDate_query > NasaEndDT("TRMM")
        if (String.IsNullOrEmpty(parameters)) return null;

        //final query url to solr
        parameters = endpoint + "select?q=*:*"
                    + "&fq=" + parameters
                    + "&fq=" + String.Format(@"{0}&fq={1}&fq={2}&defType={3}", qConcept, qLat, qLon, reqType);

        return parameters;
    }


    private string validateDT(string dt, string defaultDT)
    {
        DateTime testDateTime;
        string msgDTformat = "Input datetime format is compatible with ISO_8601 standard \n"
                            + "Example format: \n"
                            + "5/21/2011 \n"
                            + "05/21/2011 \n"
                            + "3/9/2000 8:20:30 \n"
                            + "3/20/2000 8:20 \n"
                            + "2011-05-21 \n"
                            + "2011-05-21 09:30 \n"
                            + "2011-05-21T00:00:00Z \n";

        if (dt.Equals(""))
            dt = defaultDT;
        else if (DateTime.TryParse(dt, out testDateTime))
            dt = testDateTime.ToString("yyyy-MM-dd");
        else
            throw new FormatException("DateTime is not in the right format. " + msgDTformat);

        return dt;
    }


    ///Get leaf conceptKeywords in the small Ontology tree, with the available conceptkeywords only in our database 
    ///Added by Yaping, May 2017
    private string[] getLeafKeywordsPartial(string conceptKeyword)
    {
        //false: not returning the full tree
        conceptKeyword = conceptKeyword.Trim();
        OntologyNode ontNode = getOntologywithOption(conceptKeyword, false);

        if (ontNode.keyword == null) return null;

        //traverse the tree root node
        string[] leafKeywords = traverseTree(ontNode);

        //Get all synonums for conceptKeywords
        //leafKeywords = getSearchableConcept(leafKeywords).ToArray();

        return leafKeywords;
    }


    //Traverse the ontology subtree, Yaping, May 2017
    private string[] traverseTree(OntologyNode root)
    {
        List<string> keywordList = new List<string>();
        HashSet<string> keywordSet = new HashSet<string>();

        Queue<OntologyNode> queue = new Queue<OntologyNode>();
        queue.Enqueue(root);
        keywordSet.Add(root.keyword);

        while (queue.Any())
        {
            OntologyNode currentNode = queue.Dequeue();
            if (currentNode.childNodes != null)
            {
                foreach (var child in currentNode.childNodes)
                {
                    //since struct is never null, convert to object before comparing with null 
                    //object b = null;
                    //object childschild = child.childNodes;

                    //if leaf, add to keywordlist
                    //if ( childschild == b) keywordList.Add(child.keyword);
                    //otherwise, add to the queue
                    string key = child.keyword;
                    keywordSet.Add(child.keyword);

                    queue.Enqueue(child);
                }
            }
        }
        return keywordSet.ToArray(); ;
    }


    ///Get leaf conceptKeywords in Ontology tree for input notion 
    ///Added by Yaping, April 2016
    private string[] getLeafKeywords(string conceptKeyword)
    {
        XNamespace ns = System.Configuration.ConfigurationManager.AppSettings["ONTnamespace"];
        string[] leafKeywords = null;

        conceptKeyword = conceptKeyword.Trim();
        string endpointOntology = System.Configuration.ConfigurationManager.AppSettings["ONTendpoint"] + conceptKeyword + "?format=xml";

        try
        {
            XDocument xdoc = XDocument.Load(endpointOntology);
            XElement root = xdoc.Root;
            var keywordVar = (from o in root.Descendants(ns + "keyword")
                              select new string[]
                               {
                               o.Value
                               }).ToArray();

            leafKeywords = new string[keywordVar.Length];
            for (int i = 0; i < keywordVar.Length; i++)
            {
                leafKeywords[i] = keywordVar[i][0].ToString();
            }

            //Get all synonums for conceptKeywords
            //leafKeywords = getSearchableConcept(leafKeywords).ToArray();
        }
        catch (Exception e)
        {
            //no keyword found in ontology tree;
            return null;
        }

        return leafKeywords;
    }



    private FacetField[] GetFacetField(string[] facetfields, string urlBaseQuery, bool getFacetDefinition)
    {
        string[] cvsearchable = { "DataType", "ValueType", "SampleMedium", "GeneralCategory" };

        //no CV definition returned
        Dictionary<string, string> dictCvDefinition = new Dictionary<string, string>();
        if (getFacetDefinition == true)
        {
            //currently facetfields should have one element only when getFacetDefition=true
            string facetfield = facetfields[0];

            //archived under  Xml/cvdefinition.xml
            string XmlCvDefintion = Server.MapPath("~") + System.Configuration.ConfigurationManager.AppSettings["XmlCvDefinition"];

            //write Xml/cvdefinition.xml
            //if the first time run this program, make sure
            //     <add key="UpdateCvDefinition" value="true" />
            if (Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["UpdateCvDefinition"]))
                WriteXmlCvDefinition(XmlCvDefintion, cvsearchable);

            //read dictCv from Xml/cvdefinition.xml
            dictCvDefinition = ReadXmlCvDefinition(XmlCvDefintion, facetfield);
        }

        //03/30/2017, add rows=0
        // multiple facet fields
        string facetQuery = null;
        foreach (string q in facetfields)
        {
            facetQuery += String.Format(@"&facet.field={0}", q);
        }

        string url = urlBaseQuery + "&rows=0&facet=true" + facetQuery;

        XDocument xDocument;
        string response = null;
        item[] facetvalues = null;

        using (WebClient client = new WebClient())
        {
            client.Encoding = Encoding.UTF8;
            response = client.DownloadString(url);
            TextReader xmlReader = new StringReader(response);
            xDocument = XDocument.Load(xmlReader);
        }

        List<FacetField> facetList = new List<FacetField>();
        foreach (string facetfield in facetfields)
        {
            FacetField facet = new FacetField();

            var xnode = xDocument.Descendants("lst").Where(o => (string)o.Attribute("name") == facetfield);

            //only return the items with facet_cout !=0
            facetvalues =
            (from p in xnode.Descendants("int")
             let t = p.Attribute("name").Value.ToString().ToLower()
             where long.Parse(p.Value) != 0
             select new item()
             {
                 //term = t,
                 //Add synonym search, the comma and space in the multi-term word are replaced with '+' and '_', respectively, in the indexing process
                 //, which need to be transformed back in the faceting 
                 term = facetfield.Equals("ConceptKeyword") ? t.Replace('#', ',').Replace('_', ' ') : t,
                 definition = getFacetDefinition == false || !cvsearchable.Contains(facetfield) ?
                            null : (dictCvDefinition.ContainsKey(t) ? dictCvDefinition[t] : "undefined"),
                 count = long.Parse(p.Value),
             }).ToArray();

            facet.name = facetfield;
            facet.itemCount = facetvalues.Length;
            facet.items = facetvalues;

            facetList.Add(facet);
        }

        return facetList.ToArray();
    }


    //Yaping, May 2017
    //get from web service: http://his.cuahsi.org/odmcv_1_1/odmcv_1_1.asmx
    private Dictionary<string, string> GetCVfromWebservice(string cvField)
    {
        cvField = cvField.Trim();
        Dictionary<string, string> dictCvDefinition = new Dictionary<string, string>();
        string baseUrl = System.Configuration.ConfigurationManager.AppSettings["UrlCvWebservices"];
        string url = baseUrl + "?op=Get" + cvField.Trim() + "CV";

        ODMCVServiceSoapClient odmcv = new ODMCVServiceSoapClient();
        string response = null;
        XDocument xDocument;
        switch (cvField)
        {
            case "DataType":
                response = odmcv.GetDataTypeCV();
                break;
            case "ValueType":
                response = odmcv.GetValueTypeCV();
                break;
            case "SampleMedium":
                response = odmcv.GetSampleMediumCV();
                break;
            case "GeneralCategory":
                response = odmcv.GetGeneralCategoryCV();
                break;
            default:
                throw new ArgumentException("CV field should be one of [DataType, ValueType, SampleMedium, GeneralCategory]");
        }

        TextReader xmlReader = new StringReader(response);
        xDocument = XDocument.Load(xmlReader);

        dictCvDefinition = (from o in xDocument.Descendants("Record")
                            select new
                            {
                                term = o.Element("Term").Value.ToString().ToLower(),
                                defintion = o.Element("Definition").Value.ToString()
                            }).ToDictionary(o => o.term, o => o.defintion);

        return dictCvDefinition;
    }

    private Dictionary<string, string> ReadXmlCvDefinition(string filepath, string cvId)
    {
        Dictionary<string, string> dictCvDefinition = new Dictionary<string, string>();
        XDocument xDocument = XDocument.Load(filepath);

        var cvNode = (from o in xDocument.Descendants("vocabularyId")
                    .Where(x => (string)x.Attribute("name").Value == cvId)
                      select o);

        dictCvDefinition = (from o in cvNode.Descendants("item")
                            select new
                            {
                                term = o.Element("term").Value.ToString().ToLower(),
                                defintion = o.Element("definition").Value.ToString()
                            }).ToDictionary(o => o.term, o => o.defintion);

        return dictCvDefinition;
    }

    //called by WriteXmlCvDefinition
    private Dictionary<string, string> GetCVfromSql(string cvField)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        string tablename = cvField + "CV";


        String sql = "SELECT Term, Definition FROM " + tablename;
        DataSet ds = new DataSet();
        String connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connect);
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(ds, "rows");

            if (ds.Tables["rows"].Rows.Count >= 1)
            {
                //Could return more than one row
                for (int i = 0; i < ds.Tables["rows"].Rows.Count; i++)
                {
                    DataRow dataRow = ds.Tables["rows"].Rows[i];
                    dict.Add(dataRow[0].ToString(), dataRow[1].ToString());
                }
            }
            ds.Clear();
        }
        return dict;
    }

    private void WriteXmlCvDefinition(string filepath, string[] cvlist)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();

        XmlDocument doc = new XmlDocument();
        XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        XmlElement root = doc.DocumentElement;
        doc.InsertBefore(xmlDeclaration, root);

        XmlElement element1 = doc.CreateElement(string.Empty, "ControlledVocabularyList", string.Empty);
        doc.AppendChild(element1);
        for (int i = 0; i < cvlist.Length; i++)
        {

            //Get CV defintion from sql 
            //dict = GetCVfromSql(cvlist[i]);
            dict = GetCVfromWebservice(cvlist[i]);

            XmlElement element2 = doc.CreateElement(string.Empty, "vocabularyId", string.Empty);
            element2.SetAttribute("name", cvlist[i]);
            element2.SetAttribute("itemCount", dict.Count.ToString());
            element1.AppendChild(element2);

            XmlElement eleitems = doc.CreateElement(string.Empty, "items", string.Empty);
            element2.AppendChild(eleitems);

            foreach (var item in dict)
            {
                XmlElement eleitem = doc.CreateElement(string.Empty, "item", string.Empty); ;
                eleitems.AppendChild(eleitem);

                XmlElement eleterm = doc.CreateElement(string.Empty, "term", string.Empty);
                XmlText textkey = doc.CreateTextNode(item.Key);
                eleterm.AppendChild(textkey);

                XmlElement eledef = doc.CreateElement(string.Empty, "definition", string.Empty);
                XmlText textvalue = doc.CreateTextNode(item.Value);
                eledef.AppendChild(textvalue);

                eleitem.AppendChild(eleterm);
                eleitem.AppendChild(eledef);
            }

        }
        doc.Save(filepath);

        return;
    }


    /// <summary>
    /// YX Jan.2017 
    /// GetControlledVocabulary(string cvField)
    /// 
    /// input parameter: 
    ///     "DataType" | "ValueType" | "SampleMedium" | "GeneralCategory"
    /// 
    /// output:
    ///     struct vocabulary
    ///     <ControlledVocabularyList>
    ///         <vocabularyId itemCount="15" name="DataType">
    ///             <items>
    ///                 <item>
    ///                     <term>Derived Value</term>
    ///                     <defintion>Value that is directly derived from an observation or set of observations</defintion>
    ///                     <count>34000</count>
    ///                 </item>
    ///                 <item>
    ///                 ...
    ///                 </item>
    ///             </items>
    ///         </vocabularyId>
    ///     </ControlledVocabularyList>
    /// </summary>
    [WebMethod(Description = "<br> <p style = 'margin-left:25px;' >" +
        "Get the terms and definitions of controlled vocabulary (CV), which are dynamically updated from <a href='http://his.cuahsi.org/mastercvreg/cv11.aspx'>http://his.cuahsi.org/mastercvreg/cv11.aspx</a>"+
"Typically used when the user requires terms from these controlled vocabularies to provide additional filtering parameters.</ p >")]

    public FacetField GetControlledVocabulary(string cvField)
    {
        string endpoint = System.Configuration.ConfigurationManager.AppSettings["SOLRendpoint"];
        if (!endpoint.EndsWith("/")) endpoint = endpoint + "/";

        string urlBaseQuery = endpoint + "select?q=*:*";

        string[] cvStr = { cvField };
        FacetField[] cv = GetFacetField(cvStr, urlBaseQuery, true);

        return cv[0];
    }

    /// <summary>
    /// YX, May 2017
    /// map input keyword to a concept on the ontology tree, one to one mapping
    /// for example, "Streamflow" -> "Discharge, stream"
    /// </summary>
    public string mapSearchableConcept(string searchableConcept)
    {
        //In the table, SearchableConcept and ConceptName could be identical, thus HashSet is used here
        string synonym = null;
        String sql = "SELECT ConceptID,synonym,ConceptName,Path FROM v_SynonymLookup where LOWER(synonym) = @conceptName";
        DataSet ds = new DataSet();
        String connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connect);
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);

            da.SelectCommand.Parameters.Add("@conceptName", searchableConcept.ToLower());
            da.Fill(ds, "rows");

            if (ds.Tables["rows"].Rows.Count == 0) return null;

            if (ds.Tables["rows"].Rows.Count == 1)
            {
                //only one row is expected to return
                DataRow dataRow = ds.Tables["rows"].Rows[0];
                synonym = dataRow[2].ToString();
            }
            else
            {
                throw new Exception("more than one ConceptName is found for input searchableConcept! Please check the sql database");
            }

            ds.Clear();
            da.SelectCommand.Parameters.Clear();
        }
        return synonym;
    }


    /// <summary>
    /// YX Apr.2016
    /// Get all synonyms for input keywords
    /// </summary>
    /// 

    public HashSet<string> getSearchableConcept(string[] keywords)
    {
        //In the table, SearchableConcept and ConceptName could be identical, thus HashSet is used here
        HashSet<string> synonyms = new HashSet<string>();
        String sql = "SELECT ConceptID,synonym as SearchableConcept,ConceptName,Path FROM v_SynonymLookup where ConceptName = @conceptName";
        DataSet ds = new DataSet();
        String connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connect);
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);

            foreach (var keyword in keywords)
            {
                da.SelectCommand.Parameters.Add("@conceptName", keyword);
                da.Fill(ds, "rows");

                if (ds.Tables["rows"].Rows.Count >= 1)
                {
                    //Could return more than one row
                    for (int i = 0; i < ds.Tables["rows"].Rows.Count; i++)
                    {
                        DataRow dataRow = ds.Tables["rows"].Rows[i];
                        synonyms.Add(dataRow[1].ToString());
                    }
                }

                ds.Clear();
                da.SelectCommand.Parameters.Clear();
            }
        }
        return synonyms;
    }


    ///YX Jan.2017, called by requestUrlwithCV()
    private string getQueryString(string field, string query)
    {
        string parameters;
        query = query.Trim();

        if (query.Equals(""))
        {
            parameters = String.Format(@"{0}:*", field);
        }
        else if (query.Length == 1)
        {
            parameters = String.Format("{0}:{1}", field, query);
        }
        else
        {
            string[] parts = query.Split(',');
            parameters = String.Empty;

            for (int i = 0; i < parts.Length - 1; i++)
            {
                parameters = parameters + String.Format(@"{0}:%22{1}%22+OR+", field, parts[i].Trim());  // "%2B";
            }
            parameters += String.Format(@"{0}:%22{1}%22", field, parts[parts.Length - 1].Trim());
        }
        return parameters;
    }


    private HashSet<string> filterKeywords()
    {
        HashSet<string> keywordSet = new HashSet<string>();
        string filename = Server.MapPath("~") + System.Configuration.ConfigurationManager.AppSettings["conceptKeywordsNow"];
        var lines = File.ReadLines(filename);
        foreach (var line in lines)
        {
            keywordSet.Add(line.ToString().Trim());
        }

        return keywordSet;
    }


    //added by Yaping, Sep.2015
    //input: 1, 2-5, 8; 10..12
    //able to rmove duplicate networkIDs, e.g., 2, 1-4  =? 1, 2, 3, 4
    private string[] networkParser(String s)
    {
        char[] delimiters = new char[] { ',', ';', ' ' };
        String[] parts = s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        HashSet<string> networkSet = new HashSet<string>();
        //Regex reRange = new Regex(@"^\s*((?<from>\d+)|(?<from>\d+)(?<sep>(-|\.\.))(?<to>\d+)|(?<sep>(-|\.\.))(?<to>\d+)|(?<from>\d+)(?<sep>(-|\.\.)))\s*$");
        Regex reRange = new Regex(@"^\s*((?<from>\d+)|(?<from>\d+)(?<sep>(-|\.\.))(?<to>\d+))\s*$");
        foreach (String part in parts)
        {
            Match maRange = reRange.Match(part);
            if (maRange.Success)
            {
                Group gFrom = maRange.Groups["from"];
                Group gTo = maRange.Groups["to"];
                Group gSep = maRange.Groups["sep"];

                if (gSep.Success)
                {
                    Int32 from = -1;
                    Int32 to = -1;
                    if (gFrom.Success)
                        from = Int32.Parse(gFrom.Value);
                    if (gTo.Success)
                        to = Int32.Parse(gTo.Value);
                    for (Int32 page = from; page <= to; page++)
                        networkSet.Add(page.ToString());
                }
                else if (gFrom.Success)
                    networkSet.Add(Int32.Parse(gFrom.Value).ToString());
                else
                    throw new InvalidOperationException("Input NetworkID string is invalid!");
            }
        }
        return networkSet.ToArray();
    }

    //added by Yaping, Dec.2015
    public static void Write2LogFile(string message, string outputFile)
    {
        string line = DateTime.Now.ToString() + " | ";
        line += message;

        FileStream fs = new FileStream(outputFile, FileMode.Append, FileAccess.Write, FileShare.None);
        StreamWriter writer = new StreamWriter(fs);
        writer.WriteLine(line);
        writer.Flush();
        writer.Close();
    }

    [WebMethod(Description = "<br> <p style = 'margin-left:25px;' > <strong>DEPRECATED</strong> Returns metadata for timeseries that match the provided parameters in predefined subsets. The returned object contains a subset of the available metadata sufficient for basic searches. it does not contain dat for e.g Qualitycontrol Level or Source</p>" +
    "Typical use was to help with paginatin of results. Not supported anymore </ p >")]
    public SeriesRecord[] getSeriesCatalogInBoxPaged(
    double xmin, double xmax, double ymin, double ymax,
    string conceptKeyword, String networkIDs,
    string beginDate, string endDate, int pageno)
    {
        ServiceStats.AddCount("getSeriesCatalogInBoxPaged");

        string objecformat = "concept:{0},box({1},{2},{3},{4}),network({5},daterange{6}-{7}";
        string methodName = "getSeriesCatalogInBoxPaged";

        Stopwatch timer = new Stopwatch();
        timer.Start();
        string networksString = networkIDs ?? String.Empty;

        queryLog.InfoFormat(queryLogFormat, methodName, "Start", 0,
           String.Format(objecformat,
            conceptKeyword ?? String.Empty,
            xmin, xmax, ymin, ymax,
            networksString,
            beginDate ?? String.Empty, endDate ?? String.Empty)
           );

        string connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connect);

        SeriesRecord[] series = new SeriesRecord[0];

        bool filterNetwork = false;
        bool filterKeyword = false;


        using (con)
        {


            SqlCommand cmd = new SqlCommand("sp_SeriesSearch", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@minx", xmin);
            cmd.Parameters.AddWithValue("@miny", ymin);
            cmd.Parameters.AddWithValue("@maxx", xmax);
            cmd.Parameters.AddWithValue("@maxy", ymax);
            cmd.Parameters.AddWithValue("@beginDate", beginDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@pageno", pageno);
            cmd.CommandTimeout = 600;

            if (networkIDs != null && networkIDs != "")
            {
                cmd.Parameters.AddWithValue("@networkIDs", networkIDs);
                filterNetwork = true;
            }
            if (conceptKeyword != null && conceptKeyword != "")
            {
                //verify Keyword is valid, and replace synonyms
                conceptKeyword = ResolveSynonyms(conceptKeyword);
                if (conceptKeyword == "")
                {
                    throw (new Exception("concept keyword not found"));
                }
                cmd.Parameters.AddWithValue("@conceptName", conceptKeyword);
                filterKeyword = true;
            }
            if (filterNetwork && filterKeyword) cmd.CommandText = "sp_SeriesSearch_keyword_NetworkIDs";
            if (filterKeyword && !filterNetwork) cmd.CommandText = "sp_SeriesSearch_keyword";
            if (filterNetwork && !filterKeyword) cmd.CommandText = "sp_SeriesSearch_NetworkIDs";

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();

            da.Fill(ds, "SearchCatalog");

            System.Data.DataRowCollection rows = ds.Tables["SearchCatalog"].Rows;
            series = new SeriesRecord[rows.Count];
            DataRow row;
            for (int i = 0; i < rows.Count; i++)
            {
                row = rows[i];
                series[i] = new SeriesRecord();
                series[i].location = row[0] != null ? row[0].ToString() : "";
                series[i].Sitename = row[1] != null ? row[1].ToString() : "";
                series[i].ServURL = row["ServiceWSDL"] != null ? row["ServiceWSDL"].ToString() : "";
                series[i].ServCode = row["NetworkName"] != null ? row["NetworkName"].ToString() : "";
                series[i].latitude = (double)row["latitude"];
                series[i].longitude = (double)row["longitude"];
                series[i].ValueCount = (int)row["ValueCount"];
                series[i].VarName = row["VariableName"] != null ? row["VariableName"].ToString() : "";
                series[i].VarCode = row["VariableCode"] != null ? row["VariableCode"].ToString() : "";
                series[i].beginDate = row["BeginDateTime"] != null ? row["BeginDateTime"].ToString() : "";
                series[i].endDate = row["EndDateTime"] != null ? row["EndDateTime"].ToString() : "";
                /*  datatype; valuetype;samplemedium;timeunits; conceptKeyword; genCategory;*/
                series[i].datatype = row["DataType"] != null ? row["DataType"].ToString() : "";
                series[i].valuetype = row["ValueType"] != null ? row["ValueType"].ToString() : "";
                series[i].samplemedium = row["SampleMedium"] != null ? row["SampleMedium"].ToString() : "";
                series[i].timeunits = row["TimeUnits"] != null ? row["TimeUnits"].ToString() : "";
                series[i].conceptKeyword = row["conceptKeyword"] != null ? row["conceptKeyword"].ToString() : "";
                series[i].genCategory = row["GeneralCategory"] != null ? row["GeneralCategory"].ToString() : "";
                series[i].TimeSupport = row["TimeSupport"] != null ? row["TimeSupport"].ToString() : "";

            }
        }

        queryLog.InfoFormat(queryLogFormat, methodName, "end", timer.ElapsedMilliseconds,
            String.Format(objecformat,
            conceptKeyword ?? String.Empty,
            xmin, xmax, ymin, ymax,
            networksString,
            beginDate ?? String.Empty, endDate ?? String.Empty
        )
        );
        timer.Stop();
        return series;
    }
    #endregion

    # region ontology stuff

    public struct OntologyConcpt
    {
        string ConceptID;
        string ConceptText;
    }
    public struct OntologyPath
    {
        public string conceptID;
        public string SearchableKeyword;
        public string ConceptName;
        public string ConceptPath;
    }

    [WebMethod(Description = "<br> <p style = 'margin-left:25px;' ><strong>DEPRECATED</strong></p >")]
    public OntologyPath[] getSearchablePaths()
    {
        ServiceStats.AddCount("getSearchablePaths");

        const string methodName = "getSearchablePaths";
        String sql = "SELECT ConceptID,synonym as SearchableConcept,ConceptName,Path FROM v_SynonymLookup order by path";

        OntologyPath[] thetable = new OntologyPath[0];

        OntologyPath item;

        DataSet ds = new DataSet();
        String connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connect);
        int i = 0;
        using (con)
        {


            SqlDataAdapter da = new SqlDataAdapter(sql, con);

            log.DebugFormat(logFormat, methodName, da.SelectCommand.CommandText);

            da.Fill(ds, "rows");
            thetable = new OntologyPath[ds.Tables["rows"].Rows.Count];

            foreach (DataRow dataRow in ds.Tables["rows"].Rows)
            {
                item = new OntologyPath();
                item.conceptID = dataRow[0].ToString();
                item.SearchableKeyword = dataRow[1].ToString();
                item.ConceptName = dataRow[2].ToString();
                item.ConceptPath = dataRow[3].ToString();

                thetable[i] = item;

                i++;
            }

        }
        return thetable;
    }


    /*
     * Canonicalize a synonym as the concept name to which it refers. 
     * 	Inputs: Synonym or concept name
     * 	Outputs: Concept name to which synonym refers
     * 
     * This routine has the unfortunate behavior that 
     * if a keyword is both a synonym and a concept, 
     * the synonym wins over the concept. 
     * This should be changed to use real concepts 
     * even if a synonym has been defined in error.  
     */

    private string ResolveSynonyms(String keyword)
    {

        string returnval = "";
        DataSet ds = new DataSet();
        String connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connect);
        int i = 0;
        string sql = "select conceptName from v_synonymlookup where synonym = @conceptName";
        using (con)
        {

            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.Add("@conceptName", keyword);
            da.Fill(ds, "conceptlist");
        }
        if (ds.Tables["conceptlist"].Rows.Count >= 1)
        {
            DataRow dataRow = ds.Tables["conceptlist"].Rows[0];
            returnval = dataRow[0].ToString();
        }
        return returnval;
    }
    #region removed methods

    //[WebMethod]
    //public OntologyConcept[] GetSearchableConcepts() {
    //  WebRequest objWebClient = System.Net.HttpWebRequest.Create(url);
    //  WebResponse objResponse;
    //
    //  objResponse = objWebClient.GetResponse();
    //
    //  String strResult;
    //
    //  using (StreamReader sr =
    //      new StreamReader(objResponse.GetResponseStream()))
    //  {
    //    strResult = sr.ReadToEnd();
    //  }
    //
    //  Response.Write(strResult);
    //}
    //// need to come back to optimize these searches..
    //[WebMethod]
    //public String getOntologyKeyword(String conceptCode) { 
    //   OntologyClass[] ont = GetSearchableConcepts();
    //   OntologyClass o;
    //   for (int i=0;i<ont.Length;i++){
    //       o=ont[i];
    //       if (o.conceptcode.Equals(conceptCode)) return o.keyword;
    //   }
    //   return "ConceptCode not found";     
    //
    //}
    //[WebMethod]
    //public String getOntologyConceptCode(String keyword) { 
    //     OntologyClass[] ont = GetSearchableConcepts();
    //   OntologyClass o;
    //   for (int i=0;i<ont.Length;i++){
    //       o=ont[i];
    //       if (o.keyword.Equals(keyword)) return o.conceptcode;
    //   }
    //   return "ConceptCode not found"; 
    //}
    #endregion

    /* 
     * getOntologyKeywords
     * 	Input: none.
     * 	Output: all available keywords as strings. 
     *
     * Return a memcached list of ontology keywords with a cache timeout of three days. 
     * This means that ontology updates will not be seen for three days by HydroDesktop
     * COUCH: Changing this to one hour 5/29/2014
     */

    public String[] getOntologyKeywords()
    {

        // allowing blank keywords through
        // COUCH 2014/05/30: no more blank keywords exist
        String sql = "SELECT conceptName from v_searchableConcepts";
        //  appContext = HttpContext.Current;
        string cacheKey = "ajaxVocabulary";

        // deserialize a string cached version of the item
        string[] autoCompleteWordList = (string[])HttpRuntime.Cache.Get(cacheKey);
        if (autoCompleteWordList == null)
        {
            DataSet ds = new DataSet();
            String connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connect);
            int i = 0;
            using (con)
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.Fill(ds, "conceptlist");
                autoCompleteWordList = new String[ds.Tables["conceptlist"].Rows.Count];

                foreach (DataRow dataRow in ds.Tables["conceptlist"].Rows)
                {

                    //conceptid = dataRow["conceptid"].ToString();
                    String cptcode = dataRow["conceptName"].ToString();
                    //conceptKeyword = dataRow["conceptKeyword"].ToString();
                    autoCompleteWordList[i] = cptcode;
                    i++;
                }
            }
            //autoCompleteWordList = words.ToArray();
            Array.Sort(autoCompleteWordList, new CaseInsensitiveComparer());
            // this includes an implicit serialization autoCompleteWordList.toString()
            HttpRuntime.Cache.Add(cacheKey, autoCompleteWordList, null,
        DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }
        return autoCompleteWordList;
        //  autoCompleteWordList = temp;
        //  appContext.Cache.Insert("ajaxVocabulary", autoCompleteWordList);

    }


    /* 
     * Return a list of Searchable Keywords for use in autocompletion in HydroDesktop
     * Use a cached version of the ontology with a timeout of one hour. 
     */
    [WebMethod(Description = "<br><p>Get a list of searchable concept keywords from the HIS ontology</p>" +
                                 "<p> Typical use to retrieve list of concepts keywords that can be used as an input parameter for keyword searches or pre - populate fields in e.g. in HydroDesktop.</ p > "
        )]


    public string[] GetSearchableConcepts()
    {
        ServiceStats.AddCount("GetSearchableConcepts");
        return getOntologyKeywords();
    }

    [WebMethod(Description = "<br> <p style = 'margin-left:25px;' >Get the subnodes (both leaf and non-leaf) for input concept <strong>keyword</strong> in the Ontology Tree." +    
        "<ul>"+
            "<li style='font-weight: 400;'>Keyword is one of the search criteria used in HydroClient (refer to notes in this section). </li>"+
            "<li style='font-weight: 400;'>Keyword is also used when data provider uploads data and try to make the uploaded data comply with WaterOneFlow schema. Generally, the user is required to choose a leaf keyword in the ontology tree for each input variable name, so that the newly added variable name is searchable in HisCentral catalog.</li>"+
            "<li style='font-weight: 400;'>GetOntologyTree() returns nodes in the full ontology tree.</li>" +
        "</ul>"+
       " <br></p>")]

    public OntologyNode getOntologyTree(string conceptKeyword)
    {
        //get the full tree
        return getOntologywithOption(conceptKeyword, true);
    }

    //updated by Yaping, May 2016: eliminate access to SQL database
    //YX Feb.2016, get the tree for available conceptKeywords in current database
    [WebMethod(Description = "<br> <p style = 'margin-left:25px;' >Get the subnodes (both leaf and non-leaf) for input concept <strong>keyword</strong> in the Ontology Tree." +
            "<ul>" +
                "<li style='font-weight: 400;'>Keyword is one of the search criteria used in HydroClient (refer to notes in this section). </li>" +
                "<li style='font-weight: 400;'>Keyword is also used when data provider uploads data and try to make the uploaded data comply with WaterOneFlow schema. Generally, the user is required to choose a leaf keyword in the ontology tree for each input variable name, so that the newly added variable name is searchable in HisCentral catalog.</li>" +
                 "<li style = 'font-weight: 400;' > The current HisCentral catalog has timeseries data that utilize a subset of the total keywords in the < strong > full </ strong > ontology tree.Hereafter, the ontology tree composed of the nodes with existing variables in HisCentral catalog(i.e., those ~500 keywords) is referred as <strong> partial </strong > ontology tree.</li>"+
                "<li style = 'font-weight: 400;' > GetOntologyTree() returns nodes in the full ontology tree, while GetOntologyTreewithOption() adds the option of returning nodes in the partial ontology tree</li>"+
            "</ul>" +
           " <br></p>")]
    public OntologyNode getOntologywithOption(string conceptKeyword, bool fullTree)
    {
        ServiceStats.AddCount("getOntologyTree");

        if (conceptKeyword == null || conceptKeyword.Equals("")) conceptKeyword = "Hydrosphere";

        //Hydrosphere.xml is downloaded from hiscentral getOntologyTree web service, should exist before running the program
        XNamespace ns = System.Configuration.ConfigurationManager.AppSettings["ONTnamespace"];
        //need to adjust app living on azure
        string xmlOntology = Server.MapPath("~") + System.Configuration.ConfigurationManager.AppSettings["ONTxmlPath"];

        XElement root = XElement.Load(xmlOntology);

        OntologyNode wholeTree = new OntologyNode();
        HashSet<string> keywordAvail;

        //get OntologyNode for the entire tree
        if (fullTree == true)
        {
            //set available keyword set is set dummy as it is not needed in getChild() call
            keywordAvail = new HashSet<string>();
        }
        else
        {
            //get the available keyword set
            keywordAvail = filterKeywords();
        }

        wholeTree = getChild(ns, root, fullTree, keywordAvail);

        if (conceptKeyword.Equals("Hydrosphere", StringComparison.InvariantCultureIgnoreCase)) return wholeTree;

        //if not the entire tree, select OntologyNode for the given conceptKeyword
        int isFound = 0;

        OntologyNode selectedNode = new OntologyNode();
        selectChild(conceptKeyword, wholeTree, ref isFound, ref selectedNode);
        return selectedNode;
    }



    static void selectChild(String conceptKeyword, OntologyNode node, ref int isFound, ref OntologyNode selectedNode)
    {

        if (node.childNodes == null) return;
        if (isFound == 1) return;

        foreach (var childNode in node.childNodes)
        {
            if (isFound == 1) break;
            if (!conceptKeyword.Equals(childNode.keyword, StringComparison.InvariantCultureIgnoreCase))
            {
                selectChild(conceptKeyword, childNode, ref isFound, ref selectedNode);
                if (isFound == 1) break;
            }

            if (conceptKeyword.Equals(childNode.keyword, StringComparison.InvariantCultureIgnoreCase) && isFound == 0)
            {
                isFound = 1;
                selectedNode = childNode;
                break;
            }
        }

        return;
    }


    static OntologyNode getChild(XNamespace ns, XElement root, bool fullTree, HashSet<string> keywordAvail)
    {
        OntologyNode rootNode = new OntologyNode();
        rootNode.conceptid = int.Parse(root.Element(ns + "conceptid").Value);
        rootNode.keyword = (string)root.Element(ns + "keyword").Value;

        return getChildHelper(rootNode, ns, root.Element(ns + "childNodes"), rootNode, fullTree, keywordAvail);
    }

    static OntologyNode getChildHelper(OntologyNode rootNode, XNamespace ns, XElement node, OntologyNode parentNode,
                                       bool fullTree, HashSet<string> keywordAvail)
    {
        if (node != null)
        {
            XElement parent = node;
            var childNodeList = (from o in node.Elements(ns + "OntologyNode")
                                 select o).ToList();
            List<OntologyNode> childNodes = new List<OntologyNode>(); // new OntologyNode[childNodeList.Count];

            //loop through each child
            foreach (var childNode in childNodeList)
            {
                //if leaf node and the keyword is not found in the available keyword set, skip and not creating new OntologyNode
                if (fullTree == false &&
                     !keywordAvail.Contains(childNode.Element(ns + "keyword").Value) &&
                     childNode.Element(ns + "childNodes") == null)
                    continue;

                OntologyNode newNode = new OntologyNode();
                newNode.conceptid = int.Parse(childNode.Element(ns + "conceptid").Value);
                newNode.keyword = childNode.Element(ns + "keyword").Value;
                newNode = getChildHelper(rootNode, ns, childNode.Element(ns + "childNodes"), newNode, fullTree, keywordAvail);

                childNodes.Add(newNode);
            }

            //update its parent.childIDList
            parentNode.childNodes = childNodes.ToArray();
        }
        return parentNode;
    }


    public struct OntologyNode
    {
        public string keyword;
        public int conceptid;
        public OntologyNode[] childNodes;
    }
    /* 
     * This prefix search routine provides a word match list for HD. 
     * It is not documented in the main API returns for the catalog. 
     */
    [WebMethod(Description = "<br> <p style = 'margin-left:25px;' ><strong>DEPRECATED</strong> This prefix search routine provides a word match list for HD.</p >")]
    public string[] GetWordList(string prefixText, int count)
    {
        ServiceStats.AddCount("GetWordList");

        List<String> wordlist = new List<String>();

        int i = 0;
        string connect = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(connect);
        // String sql1 = "SELECT conceptName from v_searchableconcepts where conceptName = @prefixText order by conceptName";
        String sql = "SELECT ConceptName from FN_getWordList(@prefixText,@count)";
        DataSet ds = new DataSet();

        using (con)
        {
            // get the items that match prefix at the beginning of the text or in the middle. 
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.AddWithValue("prefixText", prefixText);
            da.SelectCommand.Parameters.AddWithValue("count", count);
            da.Fill(ds, "words");

            foreach (DataRow dataRow in ds.Tables["words"].Rows)
            {
                if (!wordlist.Contains(dataRow[0].ToString()))
                {
                    wordlist.Add(dataRow[0].ToString());
                    i++;
                }
                if (i >= count) return wordlist.ToArray();
            }
        }
        con.Close();
        return wordlist.ToArray();
    }

    # region commented out stuff

    //public class OntologyClass
    //{
    //    public string keyword;
    //    public string conceptcode;
    //}

    //private List<string> analyzeNodes(OntModel om)
    //{
    //  List<string> elements = new List<string>();
    //  List<string> mediumList = new List<string>();
    //  mediumList = getMediumList(om);
    //  for (ExtendedIterator allClassesItr = om.listClasses(); allClassesItr
    //      .hasNext(); )
    //  {
    //    OntClass k = (OntClass)allClassesItr.next();

    //    if (isCorrectElement(k))
    //    {
    //      elements.Add(k.getLabel("en"));
    //      if (checkMediumRestriction(om, k))
    //      {

    //        if (mediumList != null)
    //        {
    //          foreach (String ml in mediumList)
    //          {
    //            elements.Add(k.getLabel("en") + " (" + ml + ")");
    //          }
    //        }
    //      }
    //    }
    //  }


    //  return elements;
    //}

    //private bool isCorrectElement(OntClass c)
    //{
    //  bool iscor = false;
    //  //Make sure to handle exceptions!! 
    //  if (c != null)
    //  {
    //    if (c.getLocalName() != null && c.getNameSpace() != null )
    //    {
    //      iscor = c.getNameSpace().IndexOf("extended") < 0 && c.getNameSpace().IndexOf("navigation") < 0 && c.getNameSpace().IndexOf("gcmd") < 0 && !c.getLocalName().Equals("medium") && c.getLocalName().IndexOf("Axiom_") < 0 && (c.getLabel("en").ToUpper() != "OTHER");
    //    }

    //  }

    //  return iscor;
    //}
    //private bool checkMediumRestriction(OntModel m, OntClass k)
    //{
    //  bool hasMediumOptions = false;

    //  for (ExtendedIterator supClassesItr = k.listSuperClasses(); supClassesItr.hasNext(); )
    //  {
    //    OntClass s = (OntClass)supClassesItr.next();
    //    if (s.isRestriction())
    //    {

    //      Restriction r = s.asRestriction();
    //      if (r.isAllValuesFromRestriction())
    //      {

    //        AllValuesFromRestriction h = r.asAllValuesFromRestriction();
    //        if (h.getOnProperty().getLocalName() == "hasMedium")
    //        {
    //          hasMediumOptions = true;
    //        }

    //      }
    //    }

    //  }

    //  return hasMediumOptions;
    //}
    //private List<string> getMediumList(OntModel om)
    //{

    //  // Load the medium property to be used later
    //  //mediumProperty = om.getDatatypeProperty(mediumPropertyURI);

    //  // Here's the class where medium CV is located
    //  OntClass medium = om.getOntClass(media);

    //  // The List to store allowable media
    //  List<string> mediumList = new List<string>();
    //  if (medium != null)
    //  {
    //    // Get the medium instances
    //    for (ExtendedIterator mediumInst = medium.listInstances(); mediumInst.hasNext(); )
    //    {
    //      Individual i = (Individual)mediumInst.next();
    //      mediumList.Add(i.getLabel("en"));

    //    }
    //  }
    //  return mediumList;
    //}

    //private OntClass findMatchingConcept(OntModel m, SqlConnection con, string keyword)
    //  {
    //      OntClass k = null;
    //      OntClass pref = null;
    //      OntClass medium = m.getOntClass(media);
    //      string strippedName = null;
    //      string mediump = null;
    //      if (medium != null)
    //      {
    //          for (ExtendedIterator mediumInst = medium.listInstances(); mediumInst.hasNext(); )
    //          {

    //              Individual i = (Individual)mediumInst.next();
    //              mediump = i.getLabel("en");
    //              if (keyword.Contains("(" + mediump + ")"))
    //              {
    //                  strippedName = keyword.Replace(" (" + mediump + ")", "");
    //                  break;
    //              }
    //          }
    //      }

    //      for (ExtendedIterator allClassesItr = m.listClasses(); allClassesItr.hasNext(); )
    //      {

    //          k = (OntClass)allClassesItr.next();
    //          string classLabel = k.getLabel("en");

    #endregion

    #endregion
}

