<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FundingNetwork.aspx.cs" Inherits="FundingNetwork" %>


<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>





<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        #users_paginate a {
            margin-left: 5px;
        }

        .datepicke {
            z-index: 1151 !important;
        }

        .datepicke1 {
            z-index: 1151 !important;
        }

        /*.row-fluid
        {
            float: left;
        }*/
    </style>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="styles/his.css" rel="stylesheet" type="text/css" />
    <link href="Bootstrap/Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Bootstrap/Content/bootstrap-responsive.css" rel="stylesheet" />
    <link href="Bootstrap/Content/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="Bootstrap/Content/bootstrap.css" rel="stylesheet" />
    <link href="Bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="js/fundingnetwork.js" type="text/javascript"></script>
    <script src="Bootstrap/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Bootstrap/Scripts/bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jquery.dataTables.min.js"></script>
    <script src="js/jquery.dataTables.bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript" src="Bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <script type="text/javascript">

        jQuery(document).ready(function () {           
            $('#pleaseWaitDialog').show();
            var oTable1 = $('#users').dataTable({
                "sPaginationType": "full_numbers"
            });
            
            var nowTemp = new Date();
           
            var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
            var now1 = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
           
            $('.datepicke').datepicker({
                onRender: function(date) {
                    //return date.valueOf() < now.valueOf() ? 'disabled' : '';
                }
            });
            

            $('.datepicke1').datepicker({
                onRender: function(date) {
                    // return date.valueOf() <= now1.valueOf() ? 'disabled' : '';
                }
            });
           
                       
               

        });
        
    </script>
</head>
<body>

    <form id="form1" runat="server">
        <div style="margin-left: 5px;" class="container">
            <div class="navbar">
                <uc1:HeaderControl ID="HeaderControl1" runat="server" />
            </div>
        </div>

        <div style="float: left; margin-left: 5px; margin-top: 110px; width:95%;">
            <fieldset>
                <legend>
                    <asp:Label Font-Bold="true" Font-Size="22px" runat="server" ID="lblNetworkName"></asp:Label></legend>
            </fieldset>
        </div>

        <div style="top: 250px; margin-left: 5px; left: 10px; float: left; width: 1339px" class="container">
            <div class="table-responsive">
                <table id="users" class="table table-striped table-bordered table-hover">
                    <thead>
                        <tr>
                            <th>Source ID</th>
                            <th>Organization</th>
                            <th>Agency Name</th>
                            <th>Award ID</th>
                            <th>Action</th>
                        </tr>

                    </thead>
                    <tbody>
                        <%for (int i = 0; i < objDataTable.Rows.Count; i++)
                          {
                        %>
                        <tr>
                            <td><%Response.Write(objDataTable.Rows[i][0].ToString()); %></td>
                            <td id="new<%Response.Write(objDataTable.Rows[i][0].ToString()); %>">
                                <%Response.Write(objDataTable.Rows[i][1].ToString()); %></td>
                            <td id="AG-<%Response.Write(objDataTable.Rows[i][0].ToString()); %>"><%Response.Write(objDataTable.Rows[i][2].ToString()); %></td>
                            <td id="AID-<%Response.Write(objDataTable.Rows[i][0].ToString()); %>"><%Response.Write(objDataTable.Rows[i][3].ToString()); %></td>
                            <td>
                                <div class="controls">
                                    <button type="button" style="width: 60px; margin-left: 2px;" id="Button11" title="Add Grants" onclick="emptyPopup(<% Response.Write(objDataTable.Rows[i][0].ToString()); %>)" href="#myModal<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" data-toggle="modal" class="btn btn-primary">
                                        Add</button>
                                    &nbsp; 
                                <button type="button" style="width: 60px; display: none; margin-left: -1px;" onclick="addtodatabase1(<% Response.Write(objDataTable.Rows[i][0].ToString()); %>)" id="savebutton<% Response.Write(objDataTable.Rows[i][0].ToString()); %>"
                                    class="btn btn-success">
                                    Save
                                </button>
                                    &nbsp;                             
                                <button type="button" style="width: 60px; margin-left: -1px; <%if (objDataTable.Rows[i][2].ToString() == "") Response.Write("display:none"); %>" onclick="EditMain(<% Response.Write(objDataTable.Rows[i][0].ToString()); %>)" data-toggle="modal" id="Button1<% Response.Write(objDataTable.Rows[i][0].ToString()); %>"
                                    title="Edit Grants" class="greybutton">
                                    Edit</button>
                                </div>
                                <%--MODAL box for each source--%>
                                <div class="modal hide" style="width: 900px" id="myModal<% Response.Write(objDataTable.Rows[i][0].ToString()); %>">
                                    <div class="modal-header">
                                        <button type="button" onclick="labelhide()" class="close" data-dismiss="modal">×</button>
                                        <h4><% Response.Write(objDataTable.Rows[i][1].ToString()); %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="table-responsive">
                                            <table class="table table-striped table-bordered table-hover" id="table-<% Response.Write(objDataTable.Rows[i][0].ToString()); %>">
                                                <thead>
                                                    <tr>
                                                        <th>Agency Name</th>
                                                        <th>Award ID</th>
                                                        <th>Start Date</th>
                                                        <th>End Date</th>
                                                        <th>Grant URL</th>
                                                        <th>Complete?</th>
                                                        <th><a data-original-title="Tooltip on left" data-placement="left" data-toggle="tooltip" href="javascript:;" title="Add row" onclick="addRow(<% Response.Write(objDataTable.Rows[i][0].ToString()); %>)" class="icon-plus-sign addTooltip" id="a1"></a></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr class="mode1">
                                                        <td style="width: 30px">
                                                            <%--<input style="width: 110px" type="text" onkeydown="return (event.keyCode!=188);" placeholder='Agency Name' class="clsAG-<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" name="name" value="" />--%>
                                                            <% string clsAG = objDataTable.Rows[i][0].ToString(); %>
                                                            <select class="clsAG<%Response.Write(clsAG);%>" style="width: 150px">
                                                                <option selected="selected">Agency Name</option>
                                                                 <option value="National Aeronautics and Space Administration">National Aeronautics and Space Administration</option>
                                                                <option value="National Science Foundation">National Science Foundation</option>
                                                                <option value="Pennsylvania Department of Environmental Protection">Pennsylvania Department of Environmental Protection</option>                                                                
                                                                <option value="U.S. Department Of Energy">U.S. Department Of Energy</option>
                                                                <option value="U.S. Environmental Protection Agency">U.S. Environmental Protection Agency</option>
                                                                <option value="U.S. Geological Survey">U.S. Geological Survey</option>
                                                                
                                                               
                                                            </select>
                                                        </td>
                                                        <td style="width: 30px">
                                                            <input type="text" onkeydown="return (event.keyCode!=188);" style="width: 110px" placeholder='Award ID' class="clsAID-<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" name="name" value="" /></td>

                                                        <td style="width: 30px">

                                                            <input type="text" id="dateTxt1" style="width: 110px" onkeydown="return ((((event.keyCode >= 48) && (event.keyCode <= 57)) || (event.keyCode == 191)) || (event.keyCode == 8));" placeholder='MM/DD/YYYY' class="datepicke startDate<% Response.Write(objDataTable.Rows[i][0].ToString()); %>"  name="startDate1<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" value="" />
                                                        </td>
                                                        <td style="width: 30px">
                                                            <input type="text" id="dateTxt2" style="width: 110px" onkeypress="" onkeydown="return ((((event.keyCode >= 48) && (event.keyCode <= 57)) || (event.keyCode == 191)) || (event.keyCode == 8));" placeholder='MM/DD/YYYY' class="datepicke1 endDate<% Response.Write(objDataTable.Rows[i][0].ToString()); %>"  name="endDate1<% Response.Write(objDataTable.Rows[i][0].ToString()); %>"value="" />
                                                        </td>
                                                        <td style="width: 30px">
                                                            <input type="text" style="width: 110px" onkeydown="return (event.keyCode != 188);" placeholder='Grant URL' class="url-<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" name="name" value="" />
                                                        </td>
                                                        <td style="width: 30px">
                                                            <input type="checkbox" class="clscomplete-<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" style="width: 76px" />
                                                        </td>
                                                        <td style="width: 30px"></td>
                                                    </tr>

                                                </tbody>

                                            </table>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <label class="require" style="text-align: left; display: none; color: red">Required*</label>
                                        <label class="require1" style="text-align: left; display: none; color: red">Required*</label>
                                        <label class="endDate" style="text-align: left; display: none; color: red">End Date must be later than start date!</label>

                                        <a class="btn btn-primary" onclick="addDataToTable(<% Response.Write(objDataTable.Rows[i][0].ToString()); %>, event)">OK</a>
                                    </div>
                                </div>
                                <%--MODAL box for update and delete source--%>
                                <div class="modal hide" style="width: 950px" id="myModaledit<% Response.Write(objDataTable.Rows[i][0].ToString()); %>">
                                    <div class="modal-header">
                                        <button type="button" onclick="labelhide()" class="close" data-dismiss="modal">×</button>
                                        <h4><% Response.Write(objDataTable.Rows[i][1].ToString()); %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="table-responsive">
                                            <table class="table table-striped table-bordered table-hover" id="table1<% Response.Write(objDataTable.Rows[i][0].ToString()); %>">
                                                <thead>
                                                    <tr>
                                                        <th>Agency Name</th>
                                                        <th>Award ID</th>
                                                        <th>Start Date</th>
                                                        <th>End Date</th>
                                                        <th>Grant Url</th>
                                                        <th>Complete?</th>
                                                        <th>Delete</th>





                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <%string[] separators = { "<br/>" };
                                                      string Agencyedit = objDataTable.Rows[i][2].ToString();
                                                      string Awardedit = objDataTable.Rows[i][3].ToString();
                                                      string GIDedit = objDataTable.Rows[i][4].ToString();
                                                      string iscompleteEdit = objDataTable.Rows[i][5].ToString();
                                                      string startEdit = objDataTable.Rows[i][6].ToString();
                                                      string endEdit = objDataTable.Rows[i][7].ToString();
                                                      string urlEdit = objDataTable.Rows[i][8].ToString();
                                                      string[] AgencyNameedit = Agencyedit.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                                      string[] AgencyIDedit = Awardedit.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                                      string[] GrantIDedit = GIDedit.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                                      string[] iscomplete = iscompleteEdit.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                                      string[] start = startEdit.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                                      string[] end = endEdit.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                                      string[] url = urlEdit.Split(separators, StringSplitOptions.None);
                                                    %>
                                                    <%for (int k = 0; k < AgencyNameedit.Length; k++)
                                                      {%>
                                                    <input type="hidden" id="hiddenField<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" class="hiddenAG<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" value="<% Response.Write(GrantIDedit[k].ToString()); %>">
                                                    <tr class="mode">

                                                        <td style="width: 30px">
                                                            <%--<input type="text" style="width: 110px" onkeydown="return (event.keyCode!=188);" class="editAG-<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" name="name" value="<% Response.Write(AgencyNameedit[k].ToString()); %>" /></td>--%>
                                                            <% string editAG = objDataTable.Rows[i][0].ToString();
                                                               string dbVal = AgencyNameedit[k].ToString();
                                                            %>
                                                            <select class="editAG<%Response.Write(editAG);%>" style="width: 150px">
                                                                <option selected="selected" value="<%Response.Write(dbVal);%>"><%Response.Write(dbVal);%></option>
                                                                <option value="National Aeronautics and Space Administration">National Aeronautics and Space Administration</option>
                                                                <option value="National Science Foundation">National Science Foundation</option>
                                                                <option value="Pennsylvania Department of Environmental Protection">Pennsylvania Department of Environmental Protection</option>                                                                
                                                                <option value="U.S. Department Of Energy">U.S. Department Of Energy</option>
                                                                <option value="U.S. Environmental Protection Agency">U.S. Environmental Protection Agency</option>
                                                                <option value="U.S. Geological Survey">U.S. Geological Survey</option>
                                                                
                                                                
                                                            </select>
                                                            <%--  <%
                                                          switch (dbVal)
                                                          {
                                                              case "abc":%>
                                                                <option selected="selected" value="abc">abc</option>
                                                                <% break;%>
                                                                <% case "def": %>
                                                                <option selected="selected" value="def">def</option>
                                                                <% break;%>
                                                                <% case "ghi": %>
                                                                <option selected="selected" value="ghi">ghi</option>
                                                                <% break;%>
                                                                <% case "jkl": %>
                                                                <option selected="selected" value="jkl">jkl</option>
                                                                <% break;%>
                                                                <% default: %>
                                                                <option value="abc">abc</option>
                                                                <option value="def">def</option>
                                                                <option value="ghi">ghi</option>
                                                                <option value="jkl">jkl</option>
                                                                <% break;%>

                                                                <% }%>--%>
                                                           
                                                        </td>
                                                        <td style="width: 30px">
                                                            <input type="text" style="width: 110px" onkeydown="return (event.keyCode!=188);" class="editAID-<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" name="name" value="<% Response.Write(AgencyIDedit[k].ToString()); %>" /></td>
                                                        <td style="width: 30px">
                                                            <input type="text" style="width: 110px" onkeydown="return ((event.keyCode == 8) || (event.keyCode == 46));" class="editstart-<% Response.Write(objDataTable.Rows[i][0].ToString()); %>  editstartDate<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" name="startDate<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" <% if (start.Length > 0) { if (start[k].ToString() == "no" || start[k].ToString() == "") { Response.Write("placeholder='-'"); } else { Response.Write("value='" + start[k].ToString() + "'"); } } else { Response.Write("placeholder='-'"); } %> />
                                                        </td>
                                                        <td style="width: 30px">
                                                            <input type="text" style="width: 110px" onkeydown="return ((event.keyCode == 8) || (event.keyCode == 46));" class="editend-<% Response.Write(objDataTable.Rows[i][0].ToString()); %>  editendDate<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" name="endDate<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" <% if (end.Length > 0) { if (end[k].ToString() == "no") { Response.Write("placeholder='-'"); } else { Response.Write("value='" + end[k].ToString() + "'"); } } else { Response.Write("placeholder='-'"); }%> /></td>
                                                        <td style="width: 30px">
                                                            <input type="text" style="width: 110px" onkeydown="return (event.keyCode != 188);" class="editurl-<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" name="name" <%if (url[k].ToString() == "no") { Response.Write("placeholder='-'"); } else { Response.Write("value='" + url[k].ToString() + "'"); } %> /></td>
                                                        <td style="width: 30px">
                                                            <input type="checkbox" style="float: right; width: 76px" class="completeEdit<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" <% if (Convert.ToInt32(iscomplete[k]) == 1) { Response.Write(("checked='checked'")); }%> />
                                                        </td>
                                                        <td style="width: 30px">
                                                            <input type="checkbox" onclick="deleteRowdatabase(<% Response.Write(GrantIDedit[k].ToString()); %> , <% Response.Write(objDataTable.Rows[i][0].ToString()); %>)" class="check-<% Response.Write(GrantIDedit[k].ToString()); %>" value="<% Response.Write(GrantIDedit[k].ToString()); %>" style="float: right; width: 76px" /></td>
                                                    </tr>
                                                    <%} %>
                                                </tbody>

                                            </table>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" id="updatebutton<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" onclick="update(<% Response.Write(objDataTable.Rows[i][0].ToString()); %>); return false;" class="btn btn btn-primary">Update</button>
                                        <%--<button id="A2<% Response.Write(objDataTable.Rows[i][0].ToString()); %>" class="greybutton" disabled="disabled" onclick="deleteRowdatabase(<% Response.Write(objDataTable.Rows[i][0].ToString()); %>,event); return false;">Delete</button>--%>
                                        <label class="requiredel" style="text-align: left; display: none; color: red">Required*</label>
                                        <label class="requiredel1" style="text-align: left; display: none; color: red">Required*</label>
                                           
                                        <label class="endDate1" id="error" style="text-align: left; display: none; color: red">End Date must be later than start date!</label>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <%
                          }    
                        %>
                    </tbody>
                </table>
                <div>
                    <button type="button" style="width: 70px; margin-left: 1082px" id="Button55" title="Save data" onclick="if(confirm('Reset may cause delete of unsaved data. Want to continue ?')) location.reload(true);" class="btn btn-primary">Reset</button>
                    <br />
                    <br />
                </div>
            </div>
            <%--<button type="button" onclick="yes();return false;">Click</button>--%>
            <%--<input type="text" id="picker"/>--%>
        </div>


        <div class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Modal title</h4>
                    </div>
                    <div class="modal-body">
                        <p>One fine body…</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-primary">Save changes</button>
                    </div>
                </div>
            </div>
        </div>

        <div id="myModalDelete" class="modal hide">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4 id="h4Header">Delete</h4>
            </div>
            <div class="modal-body">
                <p id="pBody">Data deleted successfully!</p>
            </div>
            <div class="modal-footer">
                <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">OK</button>
            </div>
        </div>



        <div id="alert" style="width: 500px; left: 50%; height: 158px;" class="modal hide alert alert-warning fade in">
            <div class="modal-header">

                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4>Message</h4>
            </div>
            <div class="modal-header">
                <p id="pBody1">Data deleted successfully!</p>
                <div class="modal-body">
                </div>
            </div>
            <div class="modal-body">

                <button type="button" id="cancel" style="float: right; margin-left: 12px" class="btn btn-info">Cancel</button>
                <button type="button" id="ok" style="float: right;" class="btn btn-info">OK</button>
            </div>
        </div>
        <div id="alert1" style="width: 500px; left: 50%; height: 158px;" class="modal hide alert alert-danger fade in">
            <div class="modal-header">

                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4>Message</h4>
            </div>
            <div class="modal-header">
                <p id="pBody11">Data deleted successfully!</p>
                <div class="modal-body">
                </div>
            </div>
            <div class="modal-body">

                <button type="button" id="cancel1" style="float: right; margin-left: 12px" class="btn btn-danger">Cancel</button>
                <button type="button" id="ok1" style="float: right;" class="btn btn-danger">OK</button>
            </div>
        </div>

        <div class="footer">
            <p>2014 - CUAHSI</p>
        </div>

    </form>


</body>
</html>

