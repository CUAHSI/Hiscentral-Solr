<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HealthMonitor.aspx.cs" Inherits="admin_HealthMonitor" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/jquery-ui-custom.css" rel="stylesheet" />
    <link href="../styles/demo.css" rel="stylesheet" />
    <link href="../styles/basic.css" rel="stylesheet" />
    <link href="http://cdn.datatables.net/1.10.3/css/jquery.dataTables.css" rel="stylesheet" />
    <script src="../Bootstrap/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.dataTables.min.js"></script>
    <script src="../js/jquery.simplemodal.js"></script>
    <script type="text/javascript">
        $('document').ready(function () {
            var oTable1 = $('#users').dataTable({
                "sPaginationType": "full_numbers",
                "order": [[3, "desc"]]
            });

        });

        function fnOpenPopup(errorMessage) {
            var array = new Array;
            array = errorMessage.split('~');
            for (i = 0; i < array.length; i++) {
                //alert(array[i]);
              
                    $('#basic-modal-content').append('<p><code>' + array[i] + '</code></p><br>');
               
            }
            $('#basic-modal-content').modal();
            return false;
        }

    </script>
    <style type="text/css">
        #users_paginate {
            /*margin-left: 5px;*/
            margin-right:50px;
        }

        #users_filter {
            float: right;
            margin-bottom: 20px;
            margin-right:50px;
        }
        .usersTd1 {
        text-align:center !important;
        width:10%  !important;
        }
        .usersTd2 {
        text-align:left !important;
        width:50%  !important;
        }
        .usersTd3 {
        text-align:center !important;
        width:10%  !important;
        }
         .usersTd4 {
        text-align:left !important;
        width:20%  !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:HeaderControl ID="HeaderControl" runat="server" />
        
            <div id="basic-modal-content" style="overflow-x: scroll;"></div>
            <div style="margin-top: 200px; width:90%; margin-left:100px;">
                <table id="users" class="table table-striped table-bordered table-hover display" cellspacing="0" width="90%">
                    <thead>
                        <tr>
                            <th>NetworkId</th>
                            <th>Network Name</th>
                            <th>Is Success</th>
                            <th>Last Test Date</th>
                        </tr>
                    </thead>
                    <tbody>
                        <%for (int i = 0; i < objDataTable.Rows.Count; i++) {                                   
                        %>
                        <tr>
                            <td class="usersTd1"><%Response.Write(objDataTable.Rows[i][0].ToString()); %></td>
                            <td class="usersTd2"><%Response.Write(objDataTable.Rows[i][1].ToString()); %></td>

                            <%if (Convert.ToBoolean(objDataTable.Rows[i][2]) == true) {
                                  if (objDataTable.Rows[i][4].ToString() == null || objDataTable.Rows[i][4].ToString() == "") { %>
                            <td class="usersTd3">
                                <img src="../images/sucess.jpg" height="20" width="20" /></td>
                            <%} else { %>
                            <td class="usersTd3"><a href='javascript:void(0);' onclick='fnOpenPopup("<%Response.Write(objDataTable.Rows[i][4].ToString());%>");' title='Click to see the error'>
                                <img src="../images/sucess.jpg" height="20" width="20" /></a></td>
                            <%}
                              } else {%>

                            <td class="usersTd3"><a href='javascript:void(0);' onclick='fnOpenPopup("<%Response.Write(objDataTable.Rows[i][4].ToString());%>");' title='Click to see the error'>
                                <img src="../images/error.jpg" height="20" width="20" /></a></td>
                            <%} %>

                            <td class="usersTd4"><%Response.Write(Convert.ToDateTime(objDataTable.Rows[i][3]).ToString("MM-dd-yyyy")); %></td>
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
