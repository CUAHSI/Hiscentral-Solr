<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admin.aspx.cs" Inherits="admin_harvesting" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../styles/his.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript"  src="../js/jquery-ui-1.8.20.min.js"></script>
    <link rel="stylesheet" href="../styles/jquery-ui-1.10.4.custom.min.css" type="text/css" />  
    <style type="text/css">
        #dvLoading{
         background: #fff url(../images/progressbar.gif) no-repeat center center;
            height: 80px;
            display:none;
            text-align:center;
            left: 43%;
            margin: -25px 0 0 -25px;
            position: fixed;
            top: 50%;
            width: 200px;
            z-index: 200;
        }
    </style>
</head>
<body>
 
    <form id="form1" runat="server">
        <uc1:HeaderControl ID="HeaderControl" runat="server" />
       <div class="pageheader" style="left: 0px; top: 0px">
    <div class="whiteline"></div>
        <div class="whiteline"></div>
</div>
        <div>
            &nbsp;<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CentralHISConnectionString %>"
                SelectCommand="SELECT [NetworkID], [username], [NetworkName], [ServiceWSDL], [Organization], [IsPublic], [LastHarvested], [FrequentUpdates], [CreatedDate], [ProjectStatus], [NetworkVocab], [IsApproved], [ValueCount], [VariableCount], [SiteCount], [ServiceStatus] FROM [HISNetworks] where networkID > 0 AND username != 'delete_me' ORDER BY [NetworkID]"></asp:SqlDataSource>
            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Large" Style="z-index: 100; left: 79px; position: absolute; top: 179px"
                Text="All Registered WaterOneFlow Web Services"
                Width="416px"></asp:Label>
            <asp:GridView OnRowDataBound="GridView1_RowDataBound" ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333"
                GridLines="None" OnRowCommand="GridView1_RowCommand"
                Style="z-index: 101; left: 0px; position: absolute; top: 222px; width: 100%;" Height="171px" Width="626px" DataKeyNames="NetworkID" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Font-Size="12px">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="NetworkID" HeaderText="ID" InsertVisible="False" SortExpression="NetworkID" />
                    <asp:BoundField DataField="NetworkName" HeaderText="NetworkName" SortExpression="NetworkName" />
                    <asp:BoundField DataField="ServiceWSDL" HeaderText="WSDL" SortExpression="ServiceWSDL" DataFormatString="&lt;a href={0}&gt;wsdl&lt;/a&gt;" HtmlEncode="False" />
                    <asp:BoundField DataField="username" HeaderText="Owner" SortExpression="username" />
                    <asp:CheckBoxField DataField="FrequentUpdates" HeaderText="Freq" SortExpression="FrequentUpdates" />
                    <asp:CheckBoxField DataField="IsApproved" HeaderText="IsApproved" SortExpression="IsApproved" />
                    <asp:CheckBoxField DataField="IsPublic" HeaderText="IsPublic" SortExpression="IsPublic" />
                    <asp:ButtonField CommandName="Details" Text="Details" />
                    <asp:BoundField NullDisplayText="Not Harvested" DataField="LastHarvested" HeaderText="LastHarvested" SortExpression="LastHarvested" />
                    <asp:BoundField DataField="ValueCount" HeaderText="Values" SortExpression="ValueCount" />
                    <asp:BoundField DataField="VariableCount" HeaderText="Variables" SortExpression="VariableCount" />
                    <asp:BoundField DataField="SiteCount" HeaderText="Sites" SortExpression="SiteCount" />
                    <asp:ButtonField CommandName="Delete" Text="Delete" />
                    <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>                                       
                 <asp:LinkButton runat="server" ID="lnkSuspend" OnClientClick='<%# String.Format("javascript:return fnConfirm(\"{0}\",\"{1}\")", Eval("IsPublic").ToString(),Eval("NetworkName").ToString()) %>' 
     CommandName="Suspend" CommandArgument='<%#Eval("NetworkID")  + ";" + Eval("IsPublic") + ";" + Eval("NetworkName")  %>'> <%# Convert.ToBoolean(Eval("IsPublic")) == false ? "<span style='color:red;'>Restore<span>" : "Suspend"%></asp:LinkButton>
                                    </ItemTemplate>
                    </asp:TemplateField>
                                      
                </Columns>
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="LightSteelBlue" Font-Bold="True" ForeColor="Black" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EditRowStyle BackColor="#999999" />
            </asp:GridView>
            &nbsp;
      

            <%--   Height="19px"  NavigateUrl="http://river.sdsc.edu/ssosite"  Style="z-index: 102;  --%>
            <%--<asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" ForeColor="#0000C0"
                Height="19px" NavigateUrl="10.200.2.14/ssosite" Style="z-index: 102; left: 568px; position: absolute; top: 163px"
                Width="136px">Manage Users</asp:HyperLink>--%>

          <b> <a id="lnkOpenPopup" href="javascript:void(0);" Style="z-index: 103; left: 550px; font-weight:bold; width:130px; color:#0000C0; position: relative; top: 110px;
               " onclick="showcatalogDetails();" >Catalog Details</a> </b>             


            <asp:HyperLink ID="HyperLink3" runat="server" Font-Bold="True" ForeColor="#0000C0"
              Width="130"  Height="19px" NavigateUrl="mailform.aspx" Style="z-index: 103; left: 575px; position: relative; top: 110px">
               Email</asp:HyperLink>


            <asp:HyperLink ID="HyperLink6" runat="server" Font-Bold="True" ForeColor="#0000C0"
               Width="130"   Height="19px" NavigateUrl="~/admin/unmappedvariables.aspx" Style="z-index: 105; left: 510px; position: relative; top: 110px"                
                >Unmapped Variables</asp:HyperLink>
            

            <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" ForeColor="#0000C0"
              Width="130"    Height="19px" NavigateUrl="~/admin/UsageReportAllNetworks.aspx" Style="z-index: 105; left: 495px; position: relative; top: 110px"
                >Usage Report  (All Networks)</asp:HyperLink> 
            
             <asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="True" ForeColor="#0000C0"
               Width="130"   Height="19px" NavigateUrl="../DeletedNetworks.aspx" Style="z-index: 105; left: 500px; position: relative; top: 110px">
                 Deleted Networks</asp:HyperLink>
            
            <asp:HyperLink ID="HyperLink4" runat="server" Font-Bold="True" ForeColor="#0000C0"
              Width="130"    Height="19px" NavigateUrl="~/admin/HarvestAll.aspx" Style="z-index: 105; left: 480px; position: relative; top: 110px"
                               >Harvest-All Log</asp:HyperLink>  
            
            <asp:HyperLink ID="HyperLink5" runat="server" Font-Bold="True" ForeColor="#0000C0"
              Width="130"    Height="19px" NavigateUrl="~/admin/EmailTemplate.aspx" Style="z-index: 105; left: 480px; position: relative; top: 110px"
                               >Email-Template</asp:HyperLink>                   

        </div>
        <div id="popup"></div>
        <div id="dvLoading">Loading<br /><br /> <p style="text-align:center;">Please Wait ...</p></div>
    </form>
    <script type="text/javascript">
        function fnConfirm(IsPublic,NetworkName) {
            //alert(IsPublic);
            var result = "";
            if (IsPublic == 'True') {
                result = confirm("Are you sure to suspend network '" + NetworkName + "' ?");
            } else {
                result = confirm("Are you sure to restore network '" + NetworkName + "' ?");
            }
            return result;
        }
        $(function () {
            $('#GridView1').contents().find('.hint').css('text-decoration', 'underline');
            $('#GridView1').contents().find('.hint').css('cursor', 'pointer');
            $('#GridView1').contents().find('.ChangeColor').css('color', 'red');

            $('#GridView1').contents().find('.hint').on('click', function () {
                alert('Insert hint about harvester here...');
            });
        });
        function showcatalogDetails() {           
            document.getElementById('dvLoading').style.display = 'block';
            $.ajax({
                type: "GET",
                //Set the url....
                url: 'CatalogDetailsPopup.aspx',               
                //When response comes then the success block will execute (If  required operation performed successfully then)......
                success: function (responseText, textStatus, XMLHttpRequest) {
                    $('#popup').html(responseText);
                    $('#popup').dialog({
                                        bgiframe: false,
                                        modal: true,
                                        width: 1000,
                                        height: 150,
                                        title: 'Catalog Detail'
                    });
                    document.getElementById('dvLoading').style.display = 'none';
                }
            });            
        }
    </script>
</body>
</html>
