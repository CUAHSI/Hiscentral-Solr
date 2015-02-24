<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchGrants.aspx.cs" Inherits="SearchGrants" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search Grants</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="styles/his.css" rel="stylesheet" type="text/css" />
    <link href="Bootstrap/Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Bootstrap/Content/bootstrap-responsive.css" rel="stylesheet" />
    <link href="Bootstrap/Content/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="Bootstrap/Content/bootstrap.css" rel="stylesheet" />

    <%--    <link rel="stylesheet" href="//code.jquery.com/ui/1.10.4/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>--%>


    <script type="text/javascript" src="Bootstrap/Scripts/bootstrap.js"></script>
    <%--<a href="Bootstrap/datepicker/less/datepicker.less">Bootstrap/datepicker/less/datepicker.less</a>--%>
    <script src="js/fundingnetwork.js" type="text/javascript"></script>
    <script src="Bootstrap/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Bootstrap/Scripts/bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jquery.dataTables.min.js"></script>
    <script src="js/jquery.dataTables.bootstrap.js" type="text/javascript"></script>
     <script type="text/javascript">
         function displayLoadingImage() {
             var imgLoading = document.getElementById("dvLoading");
             imgLoading.style.display = "block";
         }
		</script>
    <style type="text/css">
        
        #dvLoading {
            background: #fff url(Images/Loading.gif) no-repeat center center;
            height: 80px;
            display:none;
            text-align:center;
            left: 43%;
            margin: -25px 0 0 -25px;
            position: fixed;
            top: 50%;
            width: 200px;
            z-index: 200;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:HeaderControl ID="HeaderControl1" runat="server" />
        <div id="dvLoading"></div>
        <div style="z-index: 100; left: 35%; width: 96%; position: absolute; top: 178px; height: 192px">

            <table>
                <thead>
                    <tr>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Granting Agency: </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddpGrantAgency">
                                <asp:ListItem Value="National Aeronautics and Space Administration" Text="National Aeronautics and Space Administration" />
                                 <asp:ListItem Value="National Science Foundation" /> 
                                 <asp:ListItem Value="Pennsylvania Department of Environmental Protection " /> 
                                <asp:ListItem Value="U.S. Department Of Energy" Text="U.S. Department Of Energy" />
                                <asp:ListItem Value="U.S. Environmental Protection Agency" Text="U.S. Environmental Protection Agency" />                               
                                <asp:ListItem Value="U.S. Geological Survey" Text="U.S. Geological Survey" />
                               
                                
                                
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Award ID: </td>
                        <td>
                            <asp:TextBox ID="txtGrantNumber" runat="server"  />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator" Text="Please enter Granting Number" ControlToValidate="txtGrantNumber" SetFocusOnError="True" ValidationGroup="search"></asp:RequiredFieldValidator>
                        </td>
                    </tr>

                </tbody>

            </table>
            <div style="float: left; margin-left: 6%; margin-top: 3%;">
                <asp:Button Text="Search" runat="server" OnClientClick="displayLoadingImage();" CssClass="btn btn-primary" OnClick="search_Click" ValidationGroup="search" />
            </div>

        </div>
        <div class="footer">
            <p>2014 - CUAHSI</p>
        </div>

    </form>
</body>
</html>
