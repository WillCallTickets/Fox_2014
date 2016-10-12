<%@ Page Language="C#" MasterPageFile="/View/Masters/Master_Main.master" AutoEventWireup="false" CodeFile="Index.aspx.cs" Inherits="wctMain.Index" Title="The Fox Theatre Boulder, CO" %>
<%@ Register src="/View/Partials/UpcomingShowList.ascx" tagname="UpcomingShowList" tagprefix="uc2" %>
<%@ Register src="/View/Partials/WidgetPanel.ascx" tagname="WidgetPanel" tagprefix="uc3" %>

<asp:Content ContentPlaceHolderID="EventListingContent" runat="server" >
    <uc2:UpcomingShowList ID="UpcomingShowList1" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderId="MainContent" runat="server">
    <div id="maincontentpanel"></div>
</asp:Content>

<asp:Content ContentPlaceHolderID="WidgetContent" runat="server">
    <uc3:WidgetPanel ID="WidgetPanel1" runat="server" />
</asp:Content>

<asp:Content ID="ContentJson" ContentPlaceHolderId="JsonObjects" runat="server">
</asp:Content>