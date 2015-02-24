<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Forum.aspx.cs" Inherits="Forum" %>

<%@ Register Src="HeaderControl.ascx" TagName="HeaderControl" TagPrefix="uc1" %>
<script type="text/javascript">
    console.log("*test=" + encodeURIComponent(window.location.href));
</script>
<form runat="server">
    <uc1:HeaderControl ID="HeaderControl1" runat="server" />
    <div style="margin-left: auto; margin-right: auto; display: block">
        <div style="position: absolute; top: 150px;">
            <iframe id="forum_embed" src="javascript:void(0)" scrolling="no" frameborder="0" width="900" height="700"></iframe>
        </div>
    </div>
</form>

<script type="text/javascript">
    console.log("test2");
    var parentLoc = encodeURIComponent(window.location.href);
    document.getElementById("forum_embed").src = "https://groups.google.com/forum/embed/?place=forum/cuahsi-wds-user-group-forum&showsearch=true&showpopout=true&parenturl=" + parentLoc;
    document.getElementById("forum_embed").src = "https://groups.google.com/forum/embed/?place=forum/cuahsi-wds-user-group-forum&showsearch=true&showpopout=true&parenturl=http%3A%2F%2Fcuahsi.org%3A56608%2FForum.aspx";
    // document.getElementById("forum_embed").src = "https://groups.google.com/forum/embed/?place=forum/cuahsi-wds-user-group-forum&showsearch=true&showpopout=true";
</script>
