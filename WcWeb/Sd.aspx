<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sd.aspx.cs" Inherits="WillCallWeb.Sd" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Site Director</title>
    <!-- analytic content -->
    <% if (Wcss._Config._DomainName.ToLower() != "local.fox2014.com" && 
           Wcss._Config._DomainName.ToLower() != "localhost")
       { %>
    <script>
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        if (window.location.hostname.indexOf('localhost') != -1 || window.location.hostname.indexOf('local.fox2014') != -1)
            ga('create', 'UA-XXXX-Y', { 'cookieDomain': 'none' });
        else
            ga('create', 'UA-693894-1', 'foxtheatre.com');

        ga('send', 'pageview');

    </script>
    <%} //end of analytics%>
</head>
<body>
    <form id="form1" runat="server">
    </form>
</body>
</html>
