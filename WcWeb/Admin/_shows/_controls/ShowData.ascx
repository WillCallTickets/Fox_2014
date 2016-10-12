<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowData.ascx.cs" Inherits="wctMain.Admin._shows._controls.ShowData" %>
<%@ Register src="/Admin/_venueData/_controls/DataEntry_Container.ascx" tagname="DataEntry_Container" tagprefix="uc1" %>

<asp:Panel runat="server" ID="editz" ClientIDMode="Static" CssClass="show-mgmt">
    <uc1:DataEntry_Container ID="DataEntry_Container1" runat="server" />
</asp:Panel>