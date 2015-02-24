<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmailTemplate.aspx.cs" Inherits="admin_EmailTemplate" %>


<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Email</title>
    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }

        .Grid th {
            color: #fff;
            background-color: #336699;
        }
        /* CSS to change the GridLines color */
        .Grid, .Grid th, .Grid td {
            border: 1px solid #336699;
        }
    </style>
    <script type="text/javascript">
        function validateEmail() {
            var email = document.getElementById('txt_Email');
            var filter = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if (!filter.test(email.value)) {
                alert('Please provide a valid email address');
                email.focus;
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:HeaderControl ID="HeaderControl" runat="server" />
        <div style="margin-top: 150px; margin-left: 220px; width: 500px;">
            <asp:GridView ID="GridView1" CssClass="Grid" runat="server" AutoGenerateColumns="false" CellPadding="4"
                ForeColor="#333333" ShowFooter="false" ShowHeader="true" GridLines="None"
                OnSelectedIndexChanging="GridView1_SelectedIndexChanging"
                OnPageIndexChanging="GridView1_PageIndexChanging"
                OnRowCreated="GridView1_RowCreated"
                OnRowDeleting="GridView1_RowDeleting"
                OnSorting="GridView1_Sorting"
                PageSize="2" AllowPaging="True" AllowSorting="True" Width="770px" BorderWidth="1px" BorderColor="White">
                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="White" />
                <%--<AlternatingRowStyle BackColor="#336699" ForeColor="White" />--%>
                <%--<FooterStyle BackColor="#336699" />--%>
                <PagerStyle BackColor="#336699" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#336699" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="White" ForeColor="#333333" />
                <PagerSettings FirstPageText="First" LastPageText="Last"
                    Mode="NextPreviousFirstLast" NextPageText="Next" PreviousPageText="Previous" />
                <Columns>
                    <asp:TemplateField SortExpression="Email" Visible="false" ControlStyle-BorderColor="White">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Email" ControlStyle-BorderColor="White">
                        <ItemTemplate>
                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="500px" />
                        <HeaderTemplate>
                            <%--<input type="text" id="txt_Email" style="width:410px; height:25px;" runat="server" placeholder="Write Email Address" />--%>
                            <asp:TextBox ID="txt_Email" ClientIDMode="Static" Width="490px" Height="25px" runat="server"></asp:TextBox>
                        </HeaderTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="500px" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="true">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtn1" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" ToolTip="Delete" ImageUrl="~/img/delete.png" Height="30px" Width="50px"></asp:ImageButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderTemplate>
                            <asp:ImageButton ID="LkB1" runat="server" Text="Add" CommandName="Select" ToolTip="Add" ImageUrl="~/img/Add.png" Height="30px" Width="50px" OnClientClick="return validateEmail();"></asp:ImageButton>
                        </HeaderTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
