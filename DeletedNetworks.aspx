<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeletedNetworks.aspx.cs" Inherits="DeletedNetworks" %>
<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Deleted Networks</title>
    <link href="styles/his.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <style type="text/css">
        #GridView1 td{
            text-align:center;
            max-width:200px;
        }
        #GridView1 th{
            text-align:center;
            max-width:100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
   <uc1:HeaderControl ID="HeaderControl" runat="server" />
        <div>
            &nbsp;<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CentralHISConnectionString %>"
                SelectCommand="SELECT [NetworkID], [username], [NetworkName], [ServiceWSDL], [Organization], [IsPublic], [LastHarvested], [FrequentUpdates], [CreatedDate], [ProjectStatus], [NetworkVocab], [IsApproved], [ValueCount], [VariableCount], [SiteCount], [ServiceStatus] FROM [DeletedNetworks] where networkID > 0  ORDER BY [NetworkID]"></asp:SqlDataSource>
            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Large" Style="z-index: 100; left: 79px; position: absolute; top: 179px"
                Text="All Deleted WaterOneFlow Web Services"
                Width="416px"></asp:Label>
            <asp:GridView  ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333"
                GridLines="None"
                Style="z-index: 101; left: 0px; position: absolute; top: 222px; width: 100%;" Height="171px" Width="626px" DataKeyNames="NetworkID"  Font-Size="12px">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="NetworkID" HeaderText="ID" InsertVisible="False" SortExpression="NetworkID" />
                    <asp:BoundField DataField="NetworkName" HeaderText="NetworkName" SortExpression="NetworkName" />
                    <asp:BoundField DataField="ServiceWSDL" HeaderText="WSDL" SortExpression="ServiceWSDL" DataFormatString="&lt;a href={0}&gt;wsdl&lt;/a&gt;" HtmlEncode="False" />
                    <asp:BoundField DataField="username" HeaderText="Owner" SortExpression="username" />
<%--                    <asp:CheckBoxField DataField="FrequentUpdates" HeaderText="Freq" SortExpression="FrequentUpdates" />--%>
<%--                    <asp:CheckBoxField DataField="IsApproved" HeaderText="IsApproved" SortExpression="IsApproved" />--%>
<%--                    <asp:CheckBoxField DataField="IsPublic" HeaderText="IsPublic" SortExpression="IsPublic" />--%>
<%--                    <asp:ButtonField CommandName="Details" Text="Details" />--%>
<%--                    <asp:BoundField NullDisplayText="Not Harvested" DataField="LastHarvested" HeaderText="LastHarvested" SortExpression="LastHarvested" />--%>
                    <asp:BoundField DataField="ValueCount" HeaderText="Values" SortExpression="ValueCount" />
                    <asp:BoundField DataField="VariableCount" HeaderText="Variables" SortExpression="VariableCount" />
                    <asp:BoundField DataField="SiteCount" HeaderText="Sites" SortExpression="SiteCount" />
                    <%--<asp:ButtonField CommandName="Delete" Text="Delete" />--%>
                </Columns>
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="LightSteelBlue" Font-Bold="True" ForeColor="Black" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EditRowStyle BackColor="#999999" />
            </asp:GridView>
            </div>
    </form>
     <script type="text/javascript">
         $(function () {
             $('#GridView1').contents().find('.hint').css('text-decoration', 'underline');
             $('#GridView1').contents().find('.hint').css('cursor', 'pointer');

             $('#GridView1').contents().find('.hint').on('click', function () {
                 alert('Insert hint about harvester here...');
             });
         });
    </script>
</body>
</html>
