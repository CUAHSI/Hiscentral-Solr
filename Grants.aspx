<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Grants.aspx.cs" Inherits="Grants" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #users_paginate a {
            margin-left: 5px;
        }
    </style>
    <link href="Bootstrap/Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Bootstrap/Content/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <link href="styles/his.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    
    <script src="Bootstrap/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Bootstrap/Scripts/bootstrap.js" type="text/javascript"></script>

    <script type="text/javascript" src="js/jquery.dataTables.min.js"></script>
    <script src="js/jquery.dataTables.bootstrap.js" type="text/javascript"></script>

    <script type="text/javascript">

        jQuery(document).ready(function () {
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
        <div style="float: left; margin-left: 5px; margin-top: 110px; width:100%">
            <fieldset>
                <legend>
                    <asp:Label runat="server" Font-Bold="true" Font-Size="22px" ID="lblNetworkName"></asp:Label></legend>
            </fieldset>
        </div>

        <div style="margin-top: 9px; margin-left: 5px; float: left; width: 1339px" class="container">
            <table id="user" class="table table-striped table-bordered table-hover">
                <thead>
                    <tr>
                        <th>Sources</th>
                        <th>Sites</th>
                        <th>Variables</th>
                        <th>Series</th>
                        <th>Values</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><a target="_blank" href="Sources.aspx?n=<%=NetworkId %>"><%Response.Write(objDataTableSource.Rows.Count.ToString("#,##0"));%></a></td>
                        <td><a target="_blank" href="Sites.aspx?n=<%=NetworkId %>"><%Response.Write(sites.ToString("#,##0"));%></a></td>
                        <td><a target="_blank" href="VariableData.aspx?n=<%=NetworkId %>"><%Response.Write(variables.ToString("#,##0"));%></a></td>
                        <td><a target="_blank" href="Series.aspx?n=<%=NetworkId %>"><%Response.Write(seriesCatalog.ToString("#,##0"));%></a></td>
                        <td style="color: #0A80B8"><%Response.Write(values.ToString("#,##0"));%></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <%--<div style="margin-left:-1339px; margin-top: 160px; float: left;">
            
                <u><b>Grants</b></u>
            
        </div>--%>

        <a name="sec"></a>
        <div style="margin-top: 10px; margin-left: 5px; left: 10px; float: left; width: 1339px" class="container">
            <table id="users" class="table table-striped table-bordered table-hover">
                <thead>
                    <tr>
                        <%--<th>Source ID</th>--%>
                        <th>Organization</th>
                        <th>Agency Name</th>
                        <th>Award ID</th>

                        <th>Start Date</th>
                        <th>End Date</th>
                        <th>Grant Url</th>
                        <th>Complete?</th>
                    </tr>
                </thead>
                <tbody>                    
                    <%for (int i = 0; i < objDataTableGrant.Rows.Count; i++)
                      {                                   
                    %>
                    <tr>
                        <%--<td><%Response.Write(objDataTableGrant.Rows[i][0]); %></td>--%>
                        <td><%Response.Write(objDataTableGrant.Rows[i][1]); %></td>
                        <td><%Response.Write(objDataTableGrant.Rows[i][2]); %></td>
                        <td><%Response.Write(objDataTableGrant.Rows[i][3]); %></td>

                        <td><%if (objDataTableGrant.Rows[i][5].ToString() == "no") { Response.Write("-"); } else { Response.Write(objDataTableGrant.Rows[i][5].ToString()); } %></td>
                        <td><%if (objDataTableGrant.Rows[i][6].ToString() == "no") { Response.Write("-"); } else { Response.Write(objDataTableGrant.Rows[i][6].ToString()); } %></td>
                        <td><%if (objDataTableGrant.Rows[i][7].ToString() == "no") { Response.Write("-"); } else { Response.Write(objDataTableGrant.Rows[i][7].ToString()); } %></td>
                        <td><%if (objDataTableGrant.Rows[i][4].ToString() == "True") { Response.Write("Yes"); } else { Response.Write("No"); } %></td>

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
