using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class MappingReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["NetworkID"] == null) Response.Redirect("default.aspx");
        int NetworkId = (int)Session["NetworkID"];
        DataTable dt = GetMappingdetail(NetworkId);
        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new SqlConnection(connectionstring);
        String sql = "select NetworkTitle from HISNetworks where networkid = " + Convert.ToInt32(NetworkId);
        objconnection.Open();
        SqlCommand cmd1 = new SqlCommand(sql, objconnection);
        string NetworkName = cmd1.ExecuteScalar().ToString();
        objconnection.Close();
        lblNetworkName.Text = NetworkName;
        rptmapping.DataSource = dt;
        rptmapping.DataBind();
    }
    private DataTable GetMappingdetail(int NetworkId) {
        DataTable objMappingData = new DataTable();
        string connectionstring = ConfigurationManager.ConnectionStrings["CentralHISConnectionString"].ConnectionString;
        SqlConnection objconnection = new SqlConnection(connectionstring);
        String sql = "select v.VariableId,v.AltvariableName,v.networkId,m.conceptId,m.ConceptKeyWord,Convert(Varchar(10),m.DateMapped,101) AS DateMapped , " +
                      " Convert(Varchar(10),m.dateApproved,101) AS DateApproved,m.RegisteringIndividual,m.OntologyVersion,v.WebservicevariableId " +
                      " from variables v Inner Join MappingsApproved m on v.variableId = m.variableId "+
                      " where v.AltvariableName Is Not Null and v.WebserviceVariableId is not null and networkid = "+NetworkId+""+
                      " order by v.NetworkId";
       
        SqlCommand cmd = new SqlCommand(sql, objconnection);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter objAdapter = new SqlDataAdapter(cmd);
        DataSet objDataSet = new DataSet();
        objconnection.Open();
        objAdapter.Fill(objDataSet);
        objconnection.Close();
        objMappingData = objDataSet.Tables[0];
        return objMappingData;
    }

   
}