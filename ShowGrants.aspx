<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowGrants.aspx.cs" Inherits="ShowGrants" %>


<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search Grants</title>
     <style type="text/css">
        #users_paginate a
        {
            margin-left: 5px;
        }
        
        .auto-style1 {
            height: 36px;
        }
        #users1_paginate a
        {
            margin-left: 5px;
        }
        
        .auto-style1 {
            height: 36px;
        }
        #lnkSourceReport{
             display: block;
             font-weight: bold;
             color: #FFFFFF;
             background-color: #98bf21;
             width: 120px;
             text-align: center;
             padding: 4px;
             text-decoration: none;
         }
         #lnkSourceReport{
             background-color: #7A991A;
         }
        </style>
    <link href="styles/his.css" rel="stylesheet" type="text/css" />
    <link href="styles/his.css" rel="stylesheet" type="text/css" />
    <link href="styles/his.css" rel="stylesheet" type="text/css" />
    <link href="Bootstrap/Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" /
    <link href="Bootstrap/Content/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <script src="Bootstrap/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Bootstrap/Scripts/bootstrap.js" type="text/javascript"></script>
     
    <script type="text/javascript" src="js/jquery.dataTables.min.js"></script>
    <script src="js/jquery.dataTables.bootstrap.js" type="text/javascript"></script>
     <script src="zeroclipboard/ZeroClipboard.js" type="text/javascript"></script>
     <script src="main.js" type="text/javascript"></script>
   
    <script type="text/javascript">

        jQuery(document).ready(function () {
            $('#pleaseWaitDialog').show();
            $('#pleaseWaitDialog').fadeOut(2500);
            var oTable1 = $('#users').dataTable({
                "sPaginationType": "full_numbers"
            });
            var oTable2 = $('#users1').dataTable({
                "sPaginationType": "full_numbers"
            });

        });
       
        function func1() {

            var strUser = $('#DropDownList1 option:selected').val();
            if (strUser == 0) {
                $('#Panel1').hide();
                $('#Panel2').hide();
            }
            else if (strUser == 1) {
                
                $('#Panel1').show();
                $('#Panel2').hide();
                window.location.hash = '#Panel1';

            }
            else if (strUser == 2) {
               
                $('#Panel1').hide();
                $('#Panel2').show();
                window.location.hash = '#Panel2';

            }
        }
        var client = new ZeroClipboard($("#click-to-copy"), {
            moviePath: "zeroclipboard/ZeroClipboard.swf",
            debug: false
        });
        client.on("load", function (client) {
            $('#flash-loaded').fadeIn();

            client.on("complete", function (client, args) {
                client.setText("Set text copied.");
                $('#click-to-copy-text').fadeIn();
            });
        });

       
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <uc1:HeaderControl ID="HeaderControl1" runat="server" />
        <div style="z-index: 100; left: 2%; width: 96%; position: absolute; top: 178px; height: 192px">
            <% 
                if (objDataTableSearchGrants.Rows.Count > 0)
                {
                    int i = 0; 
                                                  
            %>
            <div>
                <h3 style="border-top-right-radius: 10px;"><%Response.Write(objDataTableSearchGrants.Rows[i][3].ToString()); %></h3>
            </div>
            <br />
            <a id="lnkSourceReport" title="Click to see source reports" href="SourceReports.aspx?<%="GrantNumber="+grantNumber+"&GrantAgency="+grantAgency %>">Report</a>
            <%--<input id="btn1" type="button" runat="server" value="Sources Report" title="Click to see source reports"/>--%>
           <asp:HyperLink ID="lnkNetworkName" runat="server"></asp:HyperLink>
                        <div>
                <h4><%Response.Write(objDataTableSearchGrants.Rows[i][0].ToString()); %></h4>
            </div>
            <table>
                <thead>
                    <tr>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <%for (i = 0; i < objDataTableSearchGrants.Rows.Count; i++)
                      {                                   
                    %>
                    <tr>
                        <td>Grant Number:</td>
                        <td><%Response.Write(objDataTableSearchGrants.Rows[i][1].ToString()); %></td>
                    </tr>
                    <tr>
                        <td>Start Date:</td>
                        <td><%Response.Write(objDataTableSearchGrants.Rows[i][4].ToString()); %></td>
                    </tr>
                    <tr>
                        <td>End Date:</td>
                        <td><%Response.Write(objDataTableSearchGrants.Rows[i][5].ToString()); %></td>
                    </tr>
                    <tr>
                        <td>Number of Time Series:</td>
                        <td><%Response.Write(objDataTableSearchGrants.Rows[i][7].ToString()); %></td>
                    </tr>
                    <tr>
                        <td>Total Number of Observations:</td>
                        <td><%Response.Write(objDataTableSearchGrants.Rows[i][6].ToString()); %></td>
                    </tr>
                    <tr>
                        <td>Total Number of Variables:</td>
                        <td><%Response.Write(objDataTableSearchGrants.Rows[i][8].ToString()); %></td>
                    </tr>


             

                    <%
                      }    
                    %>
                </tbody>

            </table>
            <br />
             <%--<select id="DropDownList1" onchange="func1()">
                <option value="0">--Select--</option>
                <option value="1">Sites</option>
                <option value="2">Variables</option>

            </select>--%>
            <br /><br />
            <% 
                }
                else
                { %>
            <div>No Record found!</div>
            <% }%>

         
           
           


           
            <div id="Panel2" style="display: none">
                <table id="users" class="table table-striped table-bordered table-hover">
                    <thead>
                        <tr>
                            <th class="auto-style1">Variable Code</th>
                            <th class="auto-style1">Variable Name</th>
                            <th class="auto-style1">Sample Medium</th>
                            <th class="auto-style1">Value Type</th>
                            <th class="auto-style1">Is Regular</th>
                            <th class="auto-style1">Data Type</th>
                            <th class="auto-style1">General Category</th>
                            <th class="auto-style1">Unit Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        <%for (int i = 0; i < objDataTableVariables.Rows.Count; i++)
                          {                                   
                        %>
                        <tr>
                            <td><%Response.Write(objDataTableVariables.Rows[i][0]); %></td>
                            <td><%Response.Write(objDataTableVariables.Rows[i][1]); %></td>
                            <td><%Response.Write(objDataTableVariables.Rows[i][2]); %></td>
                            <td><%Response.Write(objDataTableVariables.Rows[i][3]); %></td>
                            <td><%Response.Write(objDataTableVariables.Rows[i][4]); %></td>
                            <td><%Response.Write(objDataTableVariables.Rows[i][5]); %></td>
                            <td><%Response.Write(objDataTableVariables.Rows[i][6]); %></td>
                            <td><%Response.Write(objDataTableVariables.Rows[i][7]); %></td>
                        </tr>
                        <%
                          }    
                        %>
                    </tbody>
                </table>
            </div>
          

           
            <div id="Panel1" style="display: none;">
                <table id="users1" class="table table-striped table-bordered table-hover">
                    <thead>
                        <tr>
                            <th>Site Code</th>
                            <th>Site Name</th>
                            <th>Latitude</th>
                            <th>Longitude</th>
                        </tr>
                    </thead>
                    <tbody>
                        <%for (int i = 0; i < objDataTableSites.Rows.Count; i++)
                          {                                   
                        %>
                        <tr>
                            <td><%Response.Write(objDataTableSites.Rows[i][0]); %></td>
                            <td><%Response.Write(objDataTableSites.Rows[i][1]); %></td>
                            <td><%Response.Write(objDataTableSites.Rows[i][2]); %></td>
                            <td><%Response.Write(objDataTableSites.Rows[i][3]); %></td>
                        </tr>
                        <%
                          }    
                        %>
                    </tbody>
                </table>
            </div>
          
        </div>
    </form>
</body>
</html>
