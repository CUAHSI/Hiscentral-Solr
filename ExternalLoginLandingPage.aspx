<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExternalLoginLandingPage.aspx.cs" Inherits="hiscentral.ExternalLoginLandingPage" %>

<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalLoginLandingPage.aspx.cs" Inherits="IntegrateSocialLogin.ExternalLoginLandingPage" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <hgroup class="title">
        <%--<h1>Register with your <%: ProviderDisplayName %> account</h1>--%>
        <%--<h2><%: ProviderUserName %>.</h2>--%>
    </hgroup>    
    <asp:ModelErrorMessage ID="ModelErrorMessage1" runat="server" ModelStateKey="Provider" CssClass="field-validation-error" />   
    <asp:PlaceHolder runat="server" ID="userNameForm">
        <fieldset>
            <legend></legend>
            <p style="font-size: 20px;">
                You've authenticated with <strong><%: ProviderName %></strong> as
                <strong><%:Context.User.Identity.Name  %></strong>
                Click the login in button to continue to the application...
            </p>
            <p style="color: red; font-weight: bold; font-size: 22px; border: 3px solid black;">
                Note: You will be registered as <%: Context.User.Identity.Name %>. If that is not the intended Google account you wish to associate with 
                your HisCentral account, please press the cancel button below. Please log out and log in with your desired gmail account. 
            </p>
            <ol>
                <li class="email" style="list-style: none;">                  
                    <asp:ModelErrorMessage ID="ModelErrorMessage2" runat="server" ModelStateKey="UserName" CssClass="field-validation-error" />                    
                </li>
            </ol>
            <asp:Button ID="Button1" runat="server" Text="Log in" ValidationGroup="NewUser" OnClick="LogIn_Click"  />
            <asp:Button ID="Button2" runat="server" Text="Cancel" CausesValidation="false" OnClick="cancel_Click"   />
            <script type="text/javascript">
                alert('Do you want to continue logging on as <%: Context.User.Identity.Name %>? Press the \'OK\' button to continue, followed by clicking the \'Login\' or \'Cancel\' button..');
            </script>
        </fieldset>
    </asp:PlaceHolder>
     <%--     <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <div class="form-horizontal">
            <h4>Association Form</h4>
            <hr />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
            <p class="text-info">
                You've authenticated with <strong><%: ProviderName %></strong>. Please enter a user name below for the current site
                and click the Log in button.
            </p>

            <div class="form-group">
                <asp:Label ID="Label1" runat="server" AssociatedControlID="userName" CssClass="col-md-2 control-label">User name</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="userName" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="userName"
                        Display="Dynamic" CssClass="text-danger" ErrorMessage="User name is required" />
                    <asp:ModelErrorMessage ID="ModelErrorMessage3" runat="server" ModelStateKey="UserName" CssClass="text-danger" />
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button ID="Button3" runat="server" Text="Log in" CssClass="btn btn-default" OnClick="LogIn_Click" />
                </div>
            </div>
        </div>
    </asp:PlaceHolder>--%>
    </form>

  <%--  <ol>
        <li class="email">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="userName">User name</asp:Label>
            <asp:TextBox runat="server" ID="userName" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="userName"
                Display="Dynamic" ErrorMessage="User name is required" ValidationGroup="NewUser" />
            <asp:ModelErrorMessage ID="ModelErrorMessage2" runat="server" ModelStateKey="UserName" CssClass="field-validation-error" />
        </li>
    </ol>
    <asp:Button ID="Button1" runat="server" Text="Log in" ValidationGroup="NewUser" OnClick="logIn_Click" />
    <asp:Button ID="Button2" runat="server" Text="Cancel" CausesValidation="false" OnClick="cancel_Click" />--%>
</body>
</html>
