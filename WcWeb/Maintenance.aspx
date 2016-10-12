<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Maintenance.aspx.cs" Inherits="Maintenance" Title="Maintenance" EnableViewState="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">    
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252" />
    <title></title>
</head>
<body>
    <form id="Main" runat="server">
        <div id="about">
            <asp:Literal ID="litBody" runat="server" />
        </div>        
    </form>

    <%if (this.Request.UserHostName != "local.fox2014.com" && this.Request.UserHostName != "localhost" && this.Request.UserHostName != "127.0.0.l")
      {%>    
    <script src="https://ssl.google-analytics.com/urchin.js" type="text/javascript">
    </script>
    <script type="text/javascript">
            try
            {
                _uacct = "UA-2338476-1";
                urchinTracker();
            }
            catch(ex) {}
    </script>
    <%} %>
</body>
</html>
