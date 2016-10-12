<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Settings_Director.aspx.cs" MasterPageFile="/View/Masters/TemplateAdmin.master" ValidateRequest="false" 
Inherits="wctMain.Admin._settings.Settings_Director" Title="Admin - Settings" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="showmgr" >
                <asp:Panel ID="Content" runat="server" >
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>