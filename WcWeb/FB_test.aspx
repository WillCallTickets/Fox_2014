<%@ Page Language="C#" AutoEventWireup="false" CodeFile="FB_test.aspx.cs" Inherits="wctMain.FB_test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Facebook Scraper Testing</title>
    <style type="text/css">
        .agent-class {
            margin:1em 1em;
            padding:5px;
            border:solid #444 3px;
            color:#666;
        }
        .fb-class {
            color:purple;
            font-weight:bold;
            border: solid green 3px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>A crawling I will go</h1>
        <div id="divAgent" class="agent-class" runat="server"></div>
        <div id="divFacebook" class="agent-class fb-class" runat="server"></div>
    </div>
    </form>
</body>
</html>
