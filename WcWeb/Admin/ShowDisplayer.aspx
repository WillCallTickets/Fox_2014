<%@ Page Language="C#" MasterPageFile="/View/Masters/Master_Main.master" AutoEventWireup="false" CodeFile="ShowDisplayer.aspx.cs" Inherits="wctMain.Admin.ShowDisplayer" Title="ShowDisplay" %>

<asp:Content ContentPlaceHolderID="EventListingContent" runat="server" >
</asp:Content>

<asp:Content ContentPlaceHolderId="MainContent" runat="server">
    <asp:LinkButton ID="LinkButton1" runat="server" Text="refresh" onclick="LinkButton1_Click"></asp:LinkButton>
    <br />
    <asp:Panel ID="maincontent" runat="server" ClientIDMode="Static">
        <div id="maincontentpanel" >            
            <asp:Panel ID="pnlShowListingContent" runat="server" ClientIDMode="Static"></asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>

<asp:Content ContentPlaceHolderID="WidgetContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>