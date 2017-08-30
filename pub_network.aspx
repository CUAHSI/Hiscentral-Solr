<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pub_network.aspx.cs" Inherits="public_network" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CUAHSI HIS Central</title>
  <link href="styles/his.css" rel="stylesheet" type="text/css" />
     
    <script type="text/javascript" src="js/pub_networks.js"></script>

  
      

  <link href="styles/his.css" rel="stylesheet" type="text/css" />
  <link href="styles/his.css" rel="stylesheet" type="text/css" />
  <link href="styles/his.css" rel="stylesheet" type="text/css" />
  <link href="styles/his.css" rel="stylesheet" type="text/css" />
  <link href="styles/his.css" rel="stylesheet" type="text/css" />


</head>
<body>
    <form id="form1" runat="server">
    <div></div>    
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CentralHISConnectionString %>"
            SelectCommand="SELECT username, ServiceWSDL, ServiceAbs, NetworkName, ContactName, ContactEmail, ContactPhone, Organization, website, IsPublic, SupportsAllMethods, Citation, MapIconPath, OrgIconPath, LastHarvested,FrequentUpdates, Xmin, Xmax, Ymin, Ymax, ValueCount, VariableCount, SiteCount, EarliestRec, LatestRec, ServiceStatus, ProjectStatus, NetworkTitle, NetworkID, CreatedDate FROM HISNetworks WHERE (NetworkID = @NetworkID)" 
            InsertCommand="INSERT INTO HISNetworks(username, NetworkName, ServiceAbs, ServiceWSDL, ContactEmail, ContactName, ContactPhone, Organization, website) VALUES (,,,,,,,,) " 
            UpdateCommand="UPDATE HISNetworks SET NetworkName= @NetworkName, ServiceWSDL =@ServiceWSDL, ServiceAbs =@ServiceAbs, ContactName =@ContactName, ContactEmail =@ContactEmail, ContactPhone =@ContactPhone, Organization =@Organization, website =@website, Citation =@Citation, isPublic=@IsPublic WHERE [NetworkID] = @NetworkID">
            <SelectParameters>
              <asp:QueryStringParameter Name="NetworkID" QueryStringField="n" Type="Int32" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="NetworkName" />
                <asp:Parameter Name="ServiceWSDL" />
                <asp:Parameter Name="ServiceAbs" />
                <asp:Parameter Name="ContactName" />
                <asp:Parameter Name="ContactEmail" />
                <asp:Parameter Name="ContactPhone" />
                <asp:Parameter Name="Organization" />
                <asp:Parameter Name="website" />
                <asp:Parameter Name="Citation" />
                <asp:Parameter Name="IsPublic" />
                
                <asp:SessionParameter Name="NetworkID" SessionField="NetworkID" Type="Int32" />               
            </UpdateParameters>

        </asp:SqlDataSource>

        <uc1:HeaderControl ID="HeaderControl1" runat="server" />
     

 
                <asp:Label ID="lblServiceWSDLLabel" runat="server"  style="z-index: 100; left: 127px; position: absolute; top: 227px" Width="439px" Height="20px" Font-Italic="True" Font-Size="Small"></asp:Label>          
                <asp:Label ID="lblNetworkNameLabel" runat="server"  style="z-index: 101; left: 124px; position: absolute; top: 202px" Width="446px" Height="20px" Font-Bold="True" Font-Size="Small"></asp:Label>                
                <asp:Label ID="lblContactNameLabel" runat="server"  style="z-index: 102; left: 221px; position: absolute; top: 254px" Height="20px" Width="311px" Font-Size="Small"></asp:Label>
                <asp:Label ID="lblContactEmailLabel" runat="server"  style="z-index: 103; left: 221px; position: absolute; top: 276px" Height="20px" Width="311px" Font-Size="Small"></asp:Label>
                <asp:Label ID="lblContactPhoneLabel" runat="server" style="z-index: 104; left: 221px; position: absolute; top: 298px" Height="20px" Width="311px" Font-Size="Small"></asp:Label>                               
                   <asp:Label ID="lblSiteCount" runat="server" Font-Size="Small" Height="20px" Style="z-index: 105;
                     left: 92px; position: absolute; top: 348px" Width="105px"></asp:Label>
                   <asp:Label ID="lblVariableCount" runat="server" Font-Size="Small" Height="20px" Style="z-index: 106;
                     left: 92px; position: absolute; top: 366px" Width="105px"></asp:Label>
                   <asp:Label ID="lblValueCount" runat="server" Font-Size="Small" Height="20px" Style="z-index: 107;
                     left: 92px; position: absolute; top: 386px" Width="105px"></asp:Label>
         <asp:Label ID="lblChangesSiteCount" runat="server" Font-Size="Small" Height="20px" Style="z-index: 107;
                     left: 20px; position: absolute; top: 510px" Width="400px"></asp:Label>
          <asp:Label ID="lblChangesVariableCount" runat="server" Font-Size="Small" Height="20px" Style="z-index: 107;
                     left: 20px; position: absolute; top: 480px" Width="400px"></asp:Label>
          <asp:Label ID="lblChangesValueCounts" runat="server" Font-Size="Small" Height="20px" Style="z-index: 107;
                     left: 20px; position: absolute; top: 450px" Width="400px"></asp:Label>
        <asp:Image runat="server" ID="imgSuccess" Style="z-index: 109; left: 530px; position: absolute; top: 500px" Width="25px" height="25px"/>
        <asp:Label runat="server" ID="lblImgText" Font-Size="Small" Height="20px" Style="z-index: 109; left: 268px; position: absolute; top: 525px"></asp:Label>
                   <asp:Label ID="lblLastHarvested" runat="server" Font-Size="Small" Height="20px" Style="z-index: 107;
                     left: 20px; position: absolute; top: 420px" Width="500px"></asp:Label>                     
                <asp:Label ID="lblOrganizationLabel" runat="server"  style="z-index: 108; left: 124px; position: absolute; top: 177px" Height="20px" Width="670px" Font-Bold="True" Font-Size="Large"></asp:Label>                            
                <asp:Hyperlink ID="lblOrganizationLabel2" runat="server"  style="z-index: 108; left: 124px; position: absolute; top: 177px" Height="20px" Font-Bold="True" Font-Size="Large"></asp:Hyperlink>                            
                
                
                
 
                      
               <asp:Image ID="imgLogo" runat="server" Height="100px" 
                      ImageUrl="getLogo.aspx" 
                      Style="z-index: 109; left: 13px; position: absolute; top: 177px" Width="100px" />
               <asp:Image ID="imgIcon" runat="server"  
                      ImageUrl="getIcon.aspx"
                      Style="z-index: 110;left: 10px; position: absolute; top: 142px" Height="24px" Width="24px" />             &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                 <asp:Label ID="Label4" runat="server" Font-Bold="True" Style="z-index: 111; left: 141px;
                     position: absolute; top: 254px" Text="Contact:" Font-Size="Small" Width="67px"></asp:Label>
                &nbsp; &nbsp; &nbsp;&nbsp;
                   <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Small" Style="z-index: 112;
                     left: 20px; position: absolute; top: 350px" Text="Sites:" Width="67px"></asp:Label>
                   <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Small" Style="z-index: 113;
                     left: 20px; position: absolute; top: 390px" Text="Values:" Width="67px"></asp:Label>
                   <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Small" Style="z-index: 114;
                     left: 20px; position: absolute; top: 370px" Text="Variables:" Width="67px"></asp:Label>
                   <asp:Label ID="lblNorth" runat="server" Font-Size="X-Small" Style="z-index: 115;
                     left: 471px; position: absolute; top: 363px"></asp:Label>
                   <asp:Label ID="lblEast" runat="server" Font-Size="X-Small" Style="z-index: 116; left: 530px;
                     position: absolute; top: 379px"></asp:Label>
                   <asp:Label ID="lblWest" runat="server" Font-Size="X-Small" Style="z-index: 117; left: 418px;
                     position: absolute; top: 379px"></asp:Label>
                   <asp:Label ID="lblSouth" runat="server" Font-Size="X-Small" Style="z-index: 118; left: 472px;
                     position: absolute; top: 397px"></asp:Label>
                   <h3 Style="left: 16px; position: absolute; top: 302px; width:525px; z-index: 119;" >
                     Service Statistics:</h3>


      <asp:LinkButton ID="lnkEdit" runat="server" Font-Bold="True" PostBackUrl="network.aspx"
        Style="z-index: 120; left: 790px; position: absolute; top: 149px" Visible="False"
        Width="163px">Edit Service Details</asp:LinkButton>
                          <div id='myMap' style="position:relative; z-index:125; width:368px; height:329px; left: 592px; top: 196px;"></div>
        <button type="button" id="lnkContributingOrg" runat="server" onserverclick="lnkContributingOrg_ServerClick" style="margin-top:206px; color:blue; margin-left:594px;">Contributing Organizations</button>
        <%--<button type="button" onclick="window.open('Sources.aspx', 'newPage');" style="margin-top:206px; color:blue; margin-left:20px;">Funding Sources</button>--%>
      <asp:Label ID="lblNetworkTitle" runat="server" Font-Bold="True" Font-Size="20px" Height="20px"
        Style="z-index: 121; left: 42px; position: absolute; top: 142px" Width="740px"></asp:Label>
      <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="Small" Style="z-index: 126;
        left: 306px; position: absolute; top: 351px" Text="Geographic Extent:" Width="127px"></asp:Label>
          
    </form>
    <script type="text/javascript">
    getMap();
    
    </script>
    <%--<asp:LinkButton ID="LinkButton1" Style="z-index: 124; left: 15px; position: absolute; top: 427px" Text="Test URL" href="Grants.aspx" runat="server">Test URL</asp:LinkButton>     --%>
    <%--<button type="button" href="Grants.aspx">Test URL</button>--%>
  <asp:Panel ID="pnlLinks" runat="server" Style="z-index: 123; left: 591px;
    position: absolute; top: 580px" Width="361px">
    <asp:Literal ID="litCit_Pubs" runat="server"></asp:Literal></asp:Panel>
  <asp:Panel ID="pnlDesc" runat="server" Height="50px" Style="z-index: 124; left: 15px;
    position: absolute; top: 520px" Width="551px">
    <asp:Literal ID="litDesc" runat="server"></asp:Literal></asp:Panel>
    
    <%--<asp:LinkButton ID="LinkButton11" Text="Test URL" target="_blank" href="Sources.aspx" runat="server" Style="z-index: 142; left: 270px; position: absolute; top: 535px; border-right: black thin solid; border-top: black thin solid; font-weight: bold; border-left: black thin solid; color: blue; border-bottom: black thin solid; background-color: white; text-align: center; text-decoration: none;" Height="19px" Width="100px" Font-Size="12px"></asp:LinkButton>--%>
</body>
     <script type='text/javascript' src="http://maps.googleapis.com/maps/api/js?key=AIzaSyBv1tggGSW3-12h6vkbo8BL711KaUnG1w0&callback=initMap&v=3.28&libraries=places,geometry"></script>
  
</html>
