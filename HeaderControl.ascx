<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HeaderControl.ascx.cs" Inherits="WebUserControl" %>
<link href="styles/his.css" rel="stylesheet" type="text/css" />


<div class="pageheader" style="left: 0px; top: 0px">
    <div class="whiteline"></div>
    <div class="hislogo">
        <img src="images/hiscentrallogo.jpg" alt="CUAHSI Hydrologic Information System - Sharing Hydrologic Data" align="left" height="125" width="700" tabindex="-1" />

    </div>

    <div class="whiteline"></div>
</div>

<asp:LoginView ID="LoginView1" runat="server" OnViewChanged="LoginView1_ViewChanged">
    <LoggedInTemplate>
        &nbsp;
        <asp:LoginName ID="LoginName1" runat="server" Font-Bold="True" FormatString="Welcome, {0}"
            Style="font-size: 13px !important; z-index: 100; left: 658px; position: absolute; top: 11px" ForeColor="White" TabIndex="-1" />
        <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="False" Font-Size="Smaller"
            Style="font-size: 13px !important; z-index: 101; left: 658px; position: absolute; top: 31px"
            Width="52px" OnClick="LinkButton1_Click" CausesValidation="false" ForeColor="White" TabIndex="-1">Log out</asp:LinkButton>
        &nbsp;
        <asp:HyperLink ID="HyperLink1" runat="server" BorderStyle="None" Font-Bold="True"
            Font-Underline="False" NavigateUrl="addnetwork.aspx" Style="font-size: 13px !important; z-index: 102; left: 533px; position: absolute; top: 110px"
            Width="125px" Font-Size="Smaller" ForeColor="White" TabIndex="-1">Add Data Service</asp:HyperLink>
        <asp:HyperLink ID="HyperLink7" runat="server" BorderStyle="None" Font-Bold="True"
            Font-Size="Smaller" Font-Underline="False" ForeColor="White" NavigateUrl="~/Default.aspx"
            Style=" font-size: 13px !important; z-index: 107; left: 352px; position: absolute; top: 110px" Width="45px" TabIndex="-1">Home</asp:HyperLink>
        <asp:HyperLink ID="HyperLink3" runat="server" BorderStyle="None" Font-Bold="True"
            Font-Underline="False" NavigateUrl="mynetworks.aspx" Style="font-size: 13px !important; z-index: 104; left: 404px; position: absolute; top: 110px"
            Width="124px" Font-Size="Smaller" ForeColor="White" TabIndex="-1">My Data Services</asp:HyperLink>
        <asp:HyperLink ID="HyperLink5" runat="server" BorderStyle="None" Font-Bold="True"
            Font-Underline="False" NavigateUrl="pub_services.aspx" Style="font-size: 13px !important; z-index: 105; left: 663px; position: absolute; top: 110px"
            Width="115px" Font-Size="Smaller" ForeColor="White" TabIndex="-1">All Data Services</asp:HyperLink>

        <asp:HyperLink ID="HyperLink8" runat="server" BorderStyle="None" Font-Bold="True"
            Font-Underline="False" Target="_blank" NavigateUrl="https://groups.google.com/forum/#!forum/cuahsi-wds-user-group-forum" Style="font-size: 13px !important; z-index: 105; left: 775px; position: absolute; top: 110px"
            Width="115px" Font-Size="Smaller" ForeColor="White">Forum</asp:HyperLink>

        <asp:HyperLink ID="HyperLink4" runat="server" BorderStyle="None" Font-Bold="True"
            Font-Underline="False" NavigateUrl="SearchGrants.aspx" Style="font-size: 13px !important; z-index: 105; left: 830px; position: absolute; top: 110px"
            Width="115px" Font-Size="Smaller" ForeColor="White">Search Grants</asp:HyperLink>



    </LoggedInTemplate>
    <AnonymousTemplate>
        <asp:HyperLink ID="HyperLink7" runat="server" BorderStyle="None" Font-Bold="True"
            Font-Size="Smaller" Font-Underline="False" ForeColor="White" NavigateUrl="~/Default.aspx"
            Style="font-size: 13px !important; z-index: 107; left: 352px; position: absolute; top: 110px" Width="45px">Home</asp:HyperLink>
        &nbsp;
        <asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="True" Font-Underline="False"
            NavigateUrl="login.aspx" Style="font-size: 13px !important; z-index: 100; left: 536px; position: absolute; top: 23px"
            CssClass="links" ForeColor="White" Width="45px">Login</asp:HyperLink>
        <asp:HyperLink ID="HyperLink5" runat="server" BorderStyle="None" Font-Bold="True"
            Font-Underline="False" NavigateUrl="pub_services.aspx" Style="font-size: 13px !important; z-index: 105; left: 402px; position: absolute; top: 110px"
            Width="136px" Font-Size="Smaller" ForeColor="White">All Data Services</asp:HyperLink>

        <asp:HyperLink ID="HyperLink8" runat="server" BorderStyle="None" Font-Bold="True"
            Font-Underline="False" Target="_blank" NavigateUrl="https://groups.google.com/forum/#!forum/cuahsi-wds-user-group-forum" Style="font-size: 13px !important; z-index: 105; left: 525px; position: absolute; top: 110px"
            Width="115px" Font-Size="Smaller" ForeColor="White">Forum</asp:HyperLink>

    </AnonymousTemplate>
    <RoleGroups>
        <asp:RoleGroup Roles="Administrator">
            <ContentTemplate>
                <asp:LoginName ID="LoginName1" runat="server" Font-Bold="True" FormatString="Welcome, {0}"
                    Style="font-size: 13px !important; z-index: 100; left: 658px; position: absolute; top: 11px" ForeColor="White" />
                &nbsp;
        <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="False" Font-Size="Smaller"
            Style="font-size: 13px !important; z-index: 101; left: 658px; position: absolute; top: 31px"
            Width="87px" OnClick="LinkButton1_Click" CausesValidation="false" ForeColor="White" TabIndex="-1">Log out</asp:LinkButton>
                <asp:HyperLink ID="HyperLink1" runat="server" BorderStyle="None" Font-Bold="True"
                    Font-Underline="False" NavigateUrl="addnetwork.aspx" Style="font-size: 13px !important; z-index: 102; left: 522px; position: absolute; top: 110px"
                    Width="125px" Font-Size="Smaller" ForeColor="White" TabIndex="-1">Add Data Service</asp:HyperLink>
                <asp:HyperLink ID="HyperLink7" runat="server" BorderStyle="None" Font-Bold="True"
                    Font-Size="Smaller" Font-Underline="False" ForeColor="White" NavigateUrl="~/Default.aspx"
                    Style="font-size: 13px !important; z-index: 107; left: 343px; position: absolute; top: 110px" Width="45px" TabIndex="-1">Home</asp:HyperLink>
                <asp:HyperLink ID="HyperLink3" runat="server" BorderStyle="None" Font-Bold="True"
                    Font-Underline="False" NavigateUrl="mynetworks.aspx" Style="font-size: 13px !important; z-index: 104; left: 393px; position: absolute; top: 110px"
                    Width="124px" Font-Size="Smaller" ForeColor="White" TabIndex="-1">My Data Services</asp:HyperLink>
                <asp:HyperLink ID="HyperLink5" runat="server" BorderStyle="None" Font-Bold="True"
                    Font-Underline="False" NavigateUrl="pub_services.aspx" Style="font-size: 13px !important; z-index: 105; left: 652px; position: absolute; top: 110px"
                    Width="136px" Font-Size="Smaller" ForeColor="White" TabIndex="-1">All Data Services</asp:HyperLink>

                <asp:HyperLink ID="HyperLink8" runat="server" BorderStyle="None" Font-Bold="True"
                    Font-Underline="False" Target="_blank" NavigateUrl="https://groups.google.com/forum/#!forum/cuahsi-wds-user-group-forum" Style="font-size: 13px !important; z-index: 105; left: 775px; position: absolute; top: 110px"
                    Width="115px" Font-Size="Smaller" ForeColor="White">Forum</asp:HyperLink>

                <asp:HyperLink ID="HyperLink4" runat="server" BorderStyle="None" Font-Bold="True"
                    Font-Underline="False" NavigateUrl="SearchGrants.aspx" Style="font-size: 13px !important; z-index: 105; left: 830px; position: absolute; top: 110px"
                    Width="115px" Font-Size="Smaller" ForeColor="White">Search Grants</asp:HyperLink>


                <asp:HyperLink ID="HyperLink6" runat="server" BorderStyle="None" Font-Bold="True"
                    Font-Underline="False" NavigateUrl="admin/admin.aspx" Style="font-size: 13px !important; z-index: 106; left: 674px; position: absolute; top: 65px"
                    Width="102px" Font-Size="Smaller" ForeColor="White" TabIndex="-1">Administration</asp:HyperLink>
            </ContentTemplate>
        </asp:RoleGroup>
    </RoleGroups>
</asp:LoginView>




