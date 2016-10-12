<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sd.aspx.cs" Inherits="z2Main.Sd" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Site Director</title>
    <!-- analytic content -->
    <% if (Wcss._Config._DomainName.ToLower() != "local.announce.com" && 
           Wcss._Config._DomainName.ToLower() != "localhost")
       { %>
    <script>
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        if (window.location.hostname.indexOf('localhost') != -1 || window.location.hostname.indexOf('local.z2') != -1)
            ga('create', 'UA-XXXX-Y', { 'cookieDomain': 'none' });
        else
            ga('create', 'UA-53424680-1', 'z2ent.com');

        ga('send', 'pageview');

    </script>
    <%} //end of analytics%>
</head>
<body>
    <form id="form1" runat="server">
    </form>
</body>
</html>
