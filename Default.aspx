<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CUAHSI HIS Central</title>
  <link href="styles/his.css" rel="stylesheet" type="text/css" />
  <link href="styles/his.css" rel="stylesheet" type="text/css" />
  <link href="styles/his.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .wrap {
            width: 100%;
        }

            .wrap table {
                width: 100%;
                table-layout: fixed;
            }

       .wrap table tr td {
            padding: 5px;
            border: 1px solid #336699;
            width: 100px;
            word-wrap: break-word;
            text-align:center;
            
        }

                .wrap table th {
                    background-color: #336699;
                    text-align: center;
                    color: #FFFFFF;
                    font-size: 14px;
                    font-weight: bold;
                    letter-spacing: 1px;
                    margin: 1.75em 0 2px;
                    padding: 2px 2px 2px 25px;
                    text-align: center;
                }

    </style>
</head>
<body>
    <form id="form1" runat="server"><uc1:HeaderControl ID="HeaderControl1" runat="server"/>
      <div style="z-index: 100; left: 2%; width: 96%; position: absolute; top: 178px; height: 192px">  
                     
               
          <H2 >
            <b>
              CUAHSI Water Data Center Catalog of Data Services</b></H2>
          <h2 ><b>Catalog Statistics</b></h2>
          <div class="wrap" id="appendTable" runat="server">
    
          </div>
              <h3>Publishing Point Observation Data Services</h3>
          This website supports the sharing of hydrologic data published using WaterOneFlow web services and the Observations Data Model (ODM). For more information on publishing hydrologic data, click <a href=https://www.cuahsi.org/DataServices target="_blank">here</a>.
          <br />
          1. To learn how to publish your data in the HIS Central catalog,<a href="UserGuide/HIS Central 2.0 User Guide October 2014.pdf" target="_blank">click here</a>  
          <br />
          2. To download data from the HIS Central catalog, <a href="http://www.hydrodesktop.org" target="_blank">click here</a> to download the latest version of HydroDesktop.
          <br />
                    
          
          <h3>Catalog Tools</h3>
          <ul>          
          <%--<li><a href="pub_services.aspx">Listing of registered public data services</a> (<asp:Label
            ID="lblCount" runat="server" Text=""></asp:Label>)</li>--%>
          <li><a href="startree.aspx">Hydrologic Concept Ontology</a> (requires java).</li>
          <%--<li><a href="login.aspx">Login</a></li>--%>
          <%--<li><a href="register.aspx">Sign up for an account as a contributer/data manager</a></li>--%>
          <li><a href="webservices/hiscentral.asmx">HIS Central Web Services API</a></li>


         </ul>
        <p>
          &nbsp;</p>
        <p>
          &nbsp;
        </p>


      <%--<p>Note: As January 27, 2014, you must use an OpenID (i.e. Gmail) to sign on HIS Central. If you have any questions please contact <a href="mailto:jpollak@cuahsi.org">Jon Pollak</a>.</p>--%>
    
    </div>
      <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CentralHISConnectionString %>"
        SelectCommand="SELECT COUNT(*) AS Expr1 FROM Sources"></asp:SqlDataSource>

    </form>
</body>
</html>
