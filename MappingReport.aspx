<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MappingReport.aspx.cs" Inherits="MappingReport" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mapping Report</title>
    <style>
        .wrap {
            width: 95%;
        }

            .wrap table {
                width: 100%;
                table-layout: fixed;
            }

        table tr td {
            padding: 5px;
            border: 1px solid #eee;
            width: auto;
            word-wrap: break-word;
        }

        table th {
            background-color: #336699;
            text-align: center;
            color: #FFFFFF;
            font-size: 14px;
            font-weight: bold;
            letter-spacing: 1px;
            margin: 1.75em 0 2px;
            padding: 2px 2px 2px 25px;
            text-align: center;
        }

        .inner_table {
            height: 500px;
            overflow-y: auto;
            text-align: center;
        }

        .disabled {
            background: red;
            color:white;
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            var table = $(".tblValues")
            table.find('tr').each(function (key, val) {
                var v1 = $(this).find('td')[0].innerHTML;
                var v2 = $(this).find('td')[2].innerHTML
                if (v1.trim() != v2.trim()) {
                     $(this).addClass('disabled');
                    //alert($(this).find('td')[0].innerHTML + '-------------' + $(this).find('td')[2].innerHTML);
                }                
            });           
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:HeaderControl ID="HeaderControl1" runat="server" />
            <div style="position: absolute; top: 150px;">
                <h2>
                    <p>Mapping Details for Network "<asp:Label runat="server" ID="lblNetworkName"></asp:Label>"</p>
                </h2>
                <div class="wrap">
                    <asp:Repeater ID="rptmapping" runat="server">
                        <HeaderTemplate>
                            <table class="head">
                                <tr>
                                    <th id="variableHeader">Variable Name</th>
                                    <th>Concept Id</th>
                                    <th id="conceptHeader">Concept KeyWord</th>
                                    <th>Date Mapped</th>
                                    <th>Date Approved</th>
                                    <th>Registering Individual</th>
                                    <th>Ontology Version</th>
                                    <th>Webservice VariableId</th>
                                </tr>
                            </table>
                            <div class="inner_table">
                                <table class="tblValues">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="<%#Eval("VariableId") %>">
                                <td id="tdVariable">
                                    <%# Eval("AltVariableName") %> 
                                </td>
                                <td>
                                    <%# Eval("ConceptId")%>
                                </td>
                                <td id="tdConcept">
                                    <%# Eval("ConceptKeyword") %>
                                </td>
                                <td>
                                    <%# Eval("DateMapped") %>
                                </td>
                                <td>
                                    <%# Eval("DateApproved") %>
                                </td>
                                <td>
                                    <%# Eval("RegisteringIndividual") %>
                                </td>
                                <td>
                                    <%# Eval("OntologyVersion") %>
                                </td>
                                <td>
                                    <%# Eval("WebServiceVariableId") %> 
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                            </div>                                 
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
