<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UsageReportAllNetworks.aspx.cs" Inherits="admin_UsageReportAllNetworks" %>

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
            }

        .float-right {
            float: right;
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
    text-align:center;
}
    </style>
     <script src="../js/flot/jquery.min.js"></script>
    <script src="../js/flot/jquery.flot.min.js"></script>
    <script src="../js/flot/jquery.flot.time.min.js"></script>
    <script src="../js/flot/jquery.flot.stack.min.js"></script>
    <script src="../js/flot/jquery.flot.symbol.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:HeaderControl ID="HeaderControl1" runat="server" />
            Usage Report - All Networks
        </div>
        
        <div style="position: absolute; top: 150px;">
            <asp:HyperLink ID="lnkDownload" runat="server" Text="[Download Usage CSV]"></asp:HyperLink>
            <%--<asp:HyperLink CssClass="float-right" NavigateUrl="ReportDailyStatistics.aspx" ID="lnkReportDailyStatistics" runat="server" Text="Click here for daily aggregates"></asp:HyperLink>--%>
            <div style="visibility: visible; position: relative;  width: 95%; height: 400px; margin-top: 75px"  id="chart"></div>
            <h3><span>Clients per Month: </span>The number of unique ip addresses that accessed the GetValues service for all networks for each month. IP addresses originating from CUAHSI are not counted</h3>
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
                                <%# ("DateString") %> 
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
            <h3><span>Requests per Month: </span>The total number of requests for all networks for each month.</h3>
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
                                <%# Eval("DateString") %> 
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
             <h3><span>Values per Month Without log scale: </span>The total number of values retrieved from the Water One Flow webservices associated with all networks.</h3>
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
                                <%# Eval("DateString") %> 
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
For each registered network we calculate monthly totals on the number of clients, requests, and values for the GetValues requests. We filter out requests that originate from CUAHSI. Since each Water One Flow request is logged twice, we divide the total number of requests and number of values by two. We generate CSV files of these reports every night.
            </p>
        </div>

    </form>
    <script type="text/javascript">
        
            $.get(location.pathname + '?q=getstats',{}, function (rdata) {
                var data = JSON.parse(rdata);
                if (parseInt(data.ErrorCode) !== 0) {
                    alert(data.ErrorMessage);
                } else {
                    var series1 = [], series2 = [], series4 = [];
                    $.each(data.Data.Series1, function () {
                        series1.push([this.Time, this.Value]);
                    });
                    $.each(data.Data.Series2, function () {
                        series2.push([this.Time, this.Value]);
                    });
                    $.each(data.Data.Series4, function () {
                        series4.push([this.Time, this.Value]);
                    });
                    
                    var options = {
                        xaxis: {
                        mode: "time",
                        tickSize: [1, "month"],
                        tickLength: 10,
                        color: "black",
                        //axisLabel: "Date",
                        axisLabelUseCanvas: true,
                        axisLabelFontSizePixels: 12,
                        axisLabelFontFamily: 'Verdana, Arial',
                        axisLabelPadding: 10
                    },
                        legend: {
                            show: true,
                            position: "ne"
                        },
                        series: {
                            stack: true,
                            bars: {
                                show: true,                                
                            }
                        },
                        bars: {
                            align: "center",
                            barWidth:24 * 60 * 60 * 600
                        },
                        grid: {
                            hoverable: true,
                            borderWidth: 2,
                            backgroundColor: { colors: ["#EDF5FF", "#ffffff"] }
                        },

                    };
                    

                    $.plot($('#chart'), [
                        {                       
                            label: 'Clients per Month',
                            data: series1,
                            lines: { show: false },
                            points: { show: false }
                        },
                        {
                            label: 'Requests per Month',
                            data: series2,
                            lines: { show: false },
                            points: { show: false }
                        },
                        {
                            label: 'Values per Month Without log scale',
                            data: series4,
                            lines: { show: false },
                            points: { show: false }
                        }
                    ], options);
                    //$('#chart').UseTooltip();
                }
            });
            //$('#chart').UseTooltip();
    </script>
</body>
</html>
