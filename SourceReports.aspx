<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SourceReports.aspx.cs" Inherits="SourceReports" %>


<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Source Reports</title>
    <link href="styles/his.css" rel="stylesheet" type="text/css" />
    <link href="Bootstrap/Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="Bootstrap/Content/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
     <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="Bootstrap/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Bootstrap/Scripts/bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jquery.dataTables.min.js"></script>   
    <script src="js/jquery.dataTables.bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jquery.zclip.min.js"></script>
    <script type="text/javascript">
       $(document).ready(function () {
            $("a#copy-link").zclip({
                path: "zeroclipboard/ZeroClipboard.swf",
                copy: function () {
                    var pathname = document.URL;
                    //alert(pathname);
                    return pathname;                 
                },
                afterCopy: function () {
                    alert("Url Copied Succesfully !");
                }
           });
        });
    </script>
    <style>
        table th {
                    background-color: #336699;
                    text-align: center;
                    color: #FFFFFF;
                    font-size: 14px;
                    font-weight: bold;
                    letter-spacing: 1px;
                    margin: 1.75em 0 2px;
                    padding: 2px 2px 2px 25px;

        }
        table tr td {
            padding: 5px;
            border: 1px solid #336699;
            width: 100px;
            word-wrap: break-word;
            text-align:center;
            
        }
       table tr td:first-child { width: 300px; text-align: left; }
        
         .lnkCopyUrl{
             display: block;
             font-weight: bold;
             color: #FFFFFF;
             background-color: #98bf21;
             width: 120px;
             text-align: center;
             padding: 4px;
             text-decoration: none;
         }
         .lnkCopyUrl a:visited{
             background-color: #98bf21;
             color:#FFFFFF;
         }
        
    </style>

</head>
<body>
    <form id="form1" runat="server">
     <div class="container">
            <div class="navbar">
                <uc1:HeaderControl ID="HeaderControl1" runat="server" />
            </div>
        </div>
        <div style="float: left; margin-left: 5px; margin-top: 110px;">
            <fieldset>
                <legend>
                    <asp:Label runat="server" Font-Bold="true" Font-Size="20px" ID="lblNetworkName"></asp:Label></legend>
            </fieldset>
             <fieldset id="fldset1" runat="server">
                <legend>
                    <asp:Label runat="server" Font-Bold="true" Font-Size="15px" ID="lblGrantAgency"></asp:Label>&nbsp;&nbsp;&nbsp;
                     <asp:Label runat="server" Font-Bold="true" Font-Size="15px" ID="lblGrantNumber"></asp:Label>
                    <asp:Label runat="server" Font-Bold="true" ID="lblErrorMessage" Font-Size="15px" ForeColor="Red"></asp:Label>
                                        </legend>
            </fieldset>
            <label>
               
                     <a href="#copy" id="copy-link" class="lnkCopyUrl" title="Click to copy url">Copy Url</a>
              
            </label>
             
        </div>
          
        <div style="margin-top: 9px; margin-left: 5px; float: left; width: 1339px" class="container">
            <table id="user" class="table table-striped table-bordered table-hover">
                <thead>
                    <tr>
                        <th style="width:100px; text-align: center;">NetworkId</th>
                        <th style="width:100px; text-align: center;">Sources</th>
                        <th style="width:100px; text-align: center;">Sites</th>
                        <th style="width:100px; text-align: center;">Variables</th>
                        <th style="width:100px; text-align: center;">Series</th>
                        <th style="width:100px; text-align: center;">Values</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="width:100px; text-align: center;" ><%=NetworkId%></td>
                        <td  style="width:100px; text-align: center;"><%Response.Write(objDataTableSource.Rows.Count);%></td>
                        <td  style="width:100px; text-align: center;" ><%Response.Write(Convert.ToDecimal(sites).ToString("#,##0"));%></td>
                        <td  style="width:100px; text-align: center;"><%Response.Write(Convert.ToDecimal(variables).ToString("#,##0"));%></td>
                        <td style="width:100px; text-align: center;"><%Response.Write(Convert.ToDecimal(seriesCatalog).ToString("#,##0"));%></td>
                        <td  style="width:100px; text-align: center;"><%Response.Write(values.ToString("#,##0"));%></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div style="float: left; margin-left: 5px; margin-top: 70px;  width: 1339px"">
             <asp:GridView ID="gvSourceDetails" runat="server" AutoGenerateColumns="false" ShowFooter="false" Width="1350px" >
                    <Columns>                     
                       
                        <asp:BoundField DataField="Source" ControlStyle-CssClass="SourceWidth" HeaderText="Source" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="left" />
                         <asp:BoundField DataField="SourceId" HeaderText="SourceId" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <%--<asp:BoundField DataField="TotalSources" HeaderText="Number of Sources" HeaderStyle-HorizontalAlign="Center" 
                            ItemStyle-HorizontalAlign="Center" />--%>
                         <asp:BoundField DataField="TotalVariables" HeaderText="Number of Variables" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TotalSites" HeaderText="Number of Sites" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TotalSeries" HeaderText="Number of Series" HeaderStyle-HorizontalAlign="Center" 
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TotalValues" HeaderText="Total Values" HeaderStyle-HorizontalAlign="Center" 
                            ItemStyle-HorizontalAlign="Center" dataformatstring="{0:#,0}"  />                        
                    </Columns>
                </asp:GridView>
        </div>
    </form>
</body>
</html>
