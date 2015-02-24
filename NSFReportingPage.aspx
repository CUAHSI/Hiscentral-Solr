<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NSFReportingPage.aspx.cs" Inherits="NSF_Reporting_Page" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #users_paginate a
        {
            margin-left: 5px;
        }

        #popupGrant_paginate a
        {
            margin-left: 5px;
        }
         #popupSource_paginate a
        {
            margin-left: 5px;
        }
    </style>
    <link href="styles/his.css" rel="stylesheet" type="text/css" />
    <link href="Bootstrap/Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="Bootstrap/Content/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <script src="Bootstrap/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Bootstrap/Scripts/bootstrap.js" type="text/javascript"></script>

    <script type="text/javascript" src="js/jquery.dataTables.min.js"></script>
    <script src="js/jquery.dataTables.bootstrap.js" type="text/javascript"></script>

    <script type="text/javascript">

        jQuery(document).ready(function () {
            $('#pleaseWaitDialog').show();
            $('#pleaseWaitDialog').fadeOut(2500);

            var oTable1 = $('#users').dataTable({
                "sPaginationType": "full_numbers"
            });

            var oTable2 = $('#popupGrant').dataTable({
                "sPaginationType": "full_numbers"
            });

            var oTable3 = $('#popupSource').dataTable({
                "sPaginationType": "full_numbers"
            });
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="navbar">
                <uc1:HeaderControl ID="HeaderControl1" runat="server" />
            </div>
        </div>
        <%--<div class="modal hide" id="pleaseWaitDialog">
            <div class="modal-header">
                <h6>Processing...</h6>
            </div>
            <div class="modal-body">
                <div class="progress progress-striped active">
                    <div class="bar" style="width: 100%;"></div>
                </div>
            </div>
        </div>--%>
         <div style="float: left; margin-left:5px; margin-top: 110px;">
            <fieldset>
                <legend>
                    <asp:Label runat="server" Font-Bold="true" Font-Size="22px" ID="lblNetworkName"></asp:Label></legend>                
            </fieldset>            
        </div>        
        <div style="top: 250px; left: 10px; margin-left:5px; float: left; width: 1339px" class="container">
            <table id="users" class="table table-striped table-bordered table-hover">
                <thead>
                    <tr>
                        <th>Grant</th>
                        <th>Source</th>
                        <th>Sites</th>
                        <th>Variables</th>
                        <th>Series</th>
                        <th>Values</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>                        
                        <td><a target="_blank" href="Grants.aspx"><%Response.Write(objDataTableSource.Rows.Count); %></a></td>
                        <td><a target="_blank" href="Sources.aspx"><%Response.Write(objDataTableGrant.Rows.Count);%></a></td>
                        <td><a target="_blank" href="Sites.aspx"><%Response.Write(sites);%></a></td>                     
                        <td><a target="_blank" href="VariableData.aspx" ><%Response.Write(variables);%></a></td>
                        <td><a target="_blank" href="Series.aspx" ><%Response.Write(seriesCatalog);%></a></td>
                        <td style="color: #0A80B8"><%Response.Write(values);%></font></td>                                                
                    </tr>
                </tbody>
            </table>
        </div>       
    </form>
</body>
</html>
<script type="text/javascript">

    $(document).ready(function () {
        $('.plusTooltip').tooltip();
        $('.addTooltip').tooltip();
    });

</script>
