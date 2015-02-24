<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Series.aspx.cs" Inherits="Series" %>

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
        <div style="float: left; margin-left:5px; margin-top: 110px; width:100%;">
            <fieldset>
                <legend>
                    <asp:Label runat="server" Font-Bold="true" Font-Size="22px" ID="lblNetworkName"></asp:Label></legend>                
            </fieldset>            
        </div>        
        <div style="top: 250px; left: 10px; margin-left:5px; float: left; width: 1339px" class="container">
            <table id="users" class="table table-striped table-bordered table-hover">
                <thead>
                    <tr>
                        <th>Series ID</th>
                        <th>Site ID</th>
                        <th>Site Code</th>
                        <th>Site Name</th>
                        <th>Variable ID</th>
                        <th>Value Count</th>
                        <th>Variable Code</th>
                    </tr>
                </thead>
                <tbody>
                      <%for (int i = 0; i < objDataTableSeries.Rows.Count; i++)
                      {                                   
                    %>
                    <tr>
                        <td><%Response.Write(objDataTableSeries.Rows[i][0]); %></td>
                        <td><%Response.Write(objDataTableSeries.Rows[i][1]); %></td>
                        <td><%Response.Write(objDataTableSeries.Rows[i][2]); %></td>
                        <td><%Response.Write(objDataTableSeries.Rows[i][3]); %></td>
                        <td><%Response.Write(objDataTableSeries.Rows[i][4]); %></td>
                        <td><%Response.Write(objDataTableSeries.Rows[i][5]); %></td>
                        <td><%Response.Write(objDataTableSeries.Rows[i][6]); %></td>
                    </tr>
                     <%
                      }    
                    %>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
