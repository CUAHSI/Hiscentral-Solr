<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportVisualization.aspx.cs" Inherits="ReportVisualization" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/flot/jquery.min.js"></script>
    <script src="js/flot/jquery.flot.min.js"></script>
    <script src="js/flot/jquery.flot.time.min.js"></script>
    <style>
        .scroll {
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
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hdnNetworkName" />
        <div>
            <uc1:HeaderControl ID="HeaderControl1" runat="server" />
            Usage Report for Network: [<asp:Label ID="lblNetworkName" runat="server"></asp:Label>]
        </div>

        <%--<div style="position: absolute; top: 150px;">--%>
        <div style="visibility: visible; position: relative; top: 170px; width: 100%; height: 400px;" id="chart"></div>
        <%--</div>--%>
    </form>

    <script type="text/javascript">
        $(document).ready(function () {
            $.get(location.pathname + '?q=getstats&n=' + $('#<%= hdnNetworkName.ClientID %>').val(), {}, function (rdata) {
                var data = JSON.parse(rdata);
                console.log(data);
                if (parseInt(data.ErrorCode) !== 0) {
                    alert(data.ErrorMessage);
                } else {
                    var series1 = [], series2 = [], series3 = [];
                    $.each(data.Data.Series1, function () {
                        series1.push([this.Time, this.Value]);
                    });
                    $.each(data.Data.Series2, function () {
                        series2.push([this.Time, this.Value]);
                    });
                    $.each(data.Data.Series3, function () {
                        series3.push([this.Time, this.Value]);
                    });
                    var options = {
                        xaxis: {
                            mode: 'time',
                            minTickSize: [1, 'month']
                        },
                        legend: {
                            show: true,
                            position: "ne"
                        }
                    };

                    $.plot($('#chart'), [
                        {
                            label: 'Unique IP Addresses',
                            data: series1,
                            lines: { show: true },
                            points: { show: true }
                        },
                        {
                            label: 'Total Requests',
                            data: series2,
                            lines: { show: true },
                            points: { show: true }
                        },
                        {
                            label: 'Values Retrieved',
                            data: series3,
                            lines: { show: true },
                            points: { show: true }
                        }
                    ], options);

                }
            });


        });

    </script>
</body>
</html>
