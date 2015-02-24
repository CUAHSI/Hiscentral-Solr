<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportDailyStatistics.aspx.cs" Inherits="ReportDailyStatistics" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        /*.scroll {
            max-height: 200px;
            height: 200px;
            overflow-y: auto;
        }
        table {
            border: 1px solid black;
        }

        tr {
            border: 1px solid black;
        }

        td {
            width: 150px;
            text-align: center;
        }

        h3 {
            font-weight: normal !important;
        }

            h3 span {
                font-weight: bold;
                color: black;
            }*/
        .wrap {
            width: 100%;
        }

            .wrap table {
                width: 100%;
                table-layout: fixed;
            }

        table tr td {
            padding: 5px;
            border: 1px solid #eee;
            width: 100px;
            word-wrap: break-word;
        }

        table th {
            background-color: #CFCFCF;
        }

        .inner_table {
            height: 150px;
            overflow-y: auto;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:HeaderControl ID="HeaderControl1" runat="server" />
            Usage Report for Network: [<asp:Label ID="lblNetworkName" runat="server"></asp:Label>]
        </div>

        <div style="position: absolute; top: 150px;">
            <asp:HyperLink ID="lnkDownload" runat="server" Text="[Download Usage CSV]"></asp:HyperLink>

            <h3><span>Clients per Day: </span>The number of unique ip addresses that accessed the GetValues service for this network for each day. IP addresses originating from SDSC are not counted</h3>
            <div class="wrap">
                <asp:Repeater ID="rptStat1" runat="server">
                    <HeaderTemplate>
                        <table class="head">
                            <tr>
                                <th>Date</th>
                                <th>Total</th>
                                <th>.Com</th>
                                <th>.Org</th>
                                <th>.Edu</th>
                                <th>.Net</th>
                                <th>.Gov</th>
                                <th>Others</th>
                            </tr>
                        </table>
                        <div class="inner_table">
                            <table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("Date") %> 
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Com")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Org")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Edu")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Net")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Gov")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Convert.ToInt32(Eval("Count")) -(Convert.ToInt32(Eval("Count_Com")) + Convert.ToInt32(Eval("Count_Org")) + Convert.ToInt32(Eval("Count_Edu")) + Convert.ToInt32(Eval("Count_Net")) + Convert.ToInt32(Eval("Count_Gov")))).ToString("#,##0") %> 
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                            </div>  
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <h3><span>Requests per Day: </span>The total number of requests for this network for each day.</h3>
            <div class="wrap">
                <asp:Repeater ID="rptStat2" runat="server">
                    <HeaderTemplate>
                        <table class="head">
                            <tr>
                                <th>Date</th>
                                <th>Total</th>
                                <th>.Com</th>
                                <th>.Org</th>
                                <th>.Edu</th>
                                <th>.Net</th>
                                <th>.Gov</th>
                                <th>Others</th>
                            </tr>
                        </table>
                        <div class="inner_table">
                            <table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("Date") %> 
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Com")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Org")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Edu")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Net")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Gov")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Convert.ToInt32(Eval("Count")) -(Convert.ToInt32(Eval("Count_Com")) + Convert.ToInt32(Eval("Count_Org")) + Convert.ToInt32(Eval("Count_Edu")) + Convert.ToInt32(Eval("Count_Net")) + Convert.ToInt32(Eval("Count_Gov")))).ToString("#,##0") %> 
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                            </div>  
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <h3><span>Values per Day without log scale: </span>The total number of values retrieved from the Water One Flow webservices associated with this network.</h3>
            <div class="wrap">
                <asp:Repeater ID="rptStat4" runat="server">
                    <HeaderTemplate>
                        <table class="head">
                            <tr>
                                <th>Date</th>
                                <th>Total</th>
                                <th>.Com</th>
                                <th>.Org</th>
                                <th>.Edu</th>
                                <th>.Net</th>
                                <th>.Gov</th>
                                <th>Others</th>
                            </tr>
                        </table>
                        <div class="inner_table">
                            <table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("Date") %> 
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Com")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Org")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Edu")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Net")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Eval("Count_Gov")).ToString("#,##0") %>
                            </td>
                            <td>
                                <%# Convert.ToDouble(Convert.ToInt32(Eval("Count")) -(Convert.ToInt32(Eval("Count_Com")) + Convert.ToInt32(Eval("Count_Org")) + Convert.ToInt32(Eval("Count_Edu")) + Convert.ToInt32(Eval("Count_Net")) + Convert.ToInt32(Eval("Count_Gov")))).ToString("#,##0") %> 
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                            </div>  
                    </FooterTemplate>
                </asp:Repeater>
            </div>

            <br />
            <br />
            <p style="font-size: 10px;">
                We use reporting services to generate reports from the logging database. 
For each registered network we calculate monthly totals on the number of clients, requests, and values for the GetValues requests. We filter out requests that originate from SDSC. Since each Water One Flow request is logged twice, we divide the total number of requests and number of values by two. We generate CSV files of these reports every night.
            </p>
        </div>

    </form>
</body>
</html>
