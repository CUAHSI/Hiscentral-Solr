<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SourceFunding.aspx.cs" Inherits="SourceFunding" EnableEventValidation="false" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Source Funding</title>
    <script src="js/flot/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".copyTextAgency").focusout(function () {
                if (document.getElementById('chkSelectAll').checked) {
                    //   alert("test")
                    $(".copyTextAgency").val($(this).val());
                }
            });

            $(".copyTextAward").focusout(function () {
                if (document.getElementById('chkSelectAll').checked) {
                    //   alert("test")
                    $(".copyTextAward").val($(this).val());
                }
            });
        });
        

        function validate() {
            if (document.getElementById('chkSelectAll').checked) {

                $('#GridView1 tr').css("background", "darkgrey");
                $('#GridView1 th').css("background", "#000084");

            } else {

                $('#GridView1 tr').css("background", "#EEEEEE");
                $('#GridView1 th').css("background", "#000084");

            }
        }
        function fnCopyText() {
            if (document.getElementById('chkSelectAll').checked) {
                $(".copyTextAgency").focusout(function () {
                    //   alert("test")
                    $(".copyTextAgency").val($(this).val());
                })
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        +
    <div>
        <uc1:HeaderControl ID="HeaderControl1" runat="server" />
        <div style="margin-top: 160px; margin-left: 30px; color: black;">
            <asp:Label runat="server" ID="lblNetworkName"></asp:Label>
        </div>
        <div style="margin-left: 25px; margin-top: 10px; position: relative;">
            <input type="checkbox" id="chkSelectAll" onclick="validate();" />
            <label for="chkSelectAll" id="lblChk">Select All</label>
        </div>
        <div>
            <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#999999"
                BorderStyle="None" BorderWidth="1px" CellPadding="3"
                GridLines="Vertical" Style="z-index: 100; left: 10px; position: relative; top: 15px" Height="1px" Width="594px" Font-Size="12px" AutoGenerateColumns="False"
                OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" OnPageIndexChanging="GridView1_PageIndexChanging"
                OnSorting="GridView1_Sorting"
                PageSize="10" AllowPaging="True" AllowSorting="True" ShowFooter="true">

                <%--<FooterStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />--%>
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                <%--<AlternatingRowStyle BackColor="Gainsboro" />--%>
                <Columns>

                    <asp:TemplateField HeaderText="S No." HeaderStyle-CssClass="tdwidthSno" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Grant ID" InsertVisible="False" SortExpression="GrantID" Visible="false">
                        <EditItemTemplate>
                            <asp:Label ID="lblGrantId" runat="server" Text='<%# Eval("GrantID") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGrantId" runat="server" Text='<%# Bind("GrantID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sources" SortExpression="SourceID">

                        <ItemTemplate>
                            <asp:Label ID="lblSourceId" runat="server" Text='<%# Bind("SourceID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Agency" SortExpression="GrantAgency">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtAgency" onkeypress="fnCopyText();" ClientIDMode="Static" class="copyTextAgency" runat="server" Text='<%# Bind("GrantAgency") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtAgency" onkeypress='fnCopyText();' ClientIDMode="Static" class="copyTextAgency"  runat="server" Text='<%# Bind("GrantAgency") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Award No." SortExpression="GrantNumber">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtAward" ClientIDMode="Static" class="copyTextAward" runat="server" Text='<%# Bind("GrantNumber") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtAward" ClientIDMode="Static" class="copyTextAward" runat="server" Text='<%# Bind("GrantNumber") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--  <asp:TemplateField HeaderText="IsCompleted" SortExpression="IsCompleted" Visible="false">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("IsCompleted") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("IsCompleted") %>' Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="State" SortExpression="State" Visible="false">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("State") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("State") %>' Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>              --%>
                    <asp:TemplateField ShowHeader="true" HeaderText="Action">
                        <EditItemTemplate>
                            <asp:ImageButton ID="imgbtn3" class="imgbtn3" runat="server" CausesValidation="True" CommandName="Update" Text="Update" ToolTip="Save" ImageUrl="images/save.png" Height="25px" Width="25px"></asp:ImageButton>
                            &nbsp;<asp:ImageButton ID="imgbtn4" class="imgbtn4" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" ToolTip="Cancel" ImageUrl="images/cancel.png" Height="25px" Width="25px"></asp:ImageButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtn1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ToolTip="Edit" ImageUrl="images/edit.png" Height="30px" Width="30px"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>


                </Columns>
            </asp:GridView>
        </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CentralHISConnectionString %>" SelectCommand="SELECT * FROM [SourceFunding]"></asp:SqlDataSource>
    </div>
        <div style="margin-left: 220px; margin-top: 50px; width: auto; position: relative;">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
            <input type="reset" id="btnReset" />
        </div>
    </form>
</body>
</html>
