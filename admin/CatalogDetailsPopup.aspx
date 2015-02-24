<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CatalogDetailsPopup.aspx.cs" Inherits="admin_CatalogDetailsPopup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .wrap {
            width: 100%;
        }

            .wrap table {
                width: 100%;
                table-layout: fixed;
            }

       .wrap table tr td {
            padding: 5px;
            border: 1px solid #eee;
            width: 100px;
            word-wrap: break-word;
            text-align:center;
        }

       .wrap table th {
            background-color: #CFCFCF;
             text-align:center;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap" id="appendTable" runat="server">
    
    </div>
    </form>
</body>
</html>
