<%@ Page Language="C#" MasterPageFile="/View/Masters/Master_Main.master" AutoEventWireup="false" CodeFile="MobileMailManager.aspx.cs" Inherits="wctMain.MobileMailManager" Title="Mobile Mail Manager - The Fox Theatre Boulder, CO" %>


<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="mobilemailmanager">                       
        <div id="maincontentpanel" ></div>
    </div>
    <script type="text/javascript">
        var events = '';
        var staticPageList = '';
        var defaultImage = '';
    </script>
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="EventListingContent" runat="server" >
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="WidgetContent" runat="server">
</asp:Content>

<asp:Content ID="ContentJson" ContentPlaceHolderId="JsonObjects" runat="server">
</asp:Content>
