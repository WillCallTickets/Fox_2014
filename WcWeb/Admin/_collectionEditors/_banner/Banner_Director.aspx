<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Banner_Director.aspx.cs" MasterPageFile="/View/Masters/TemplateAdmin.master" ValidateRequest="false" EnableViewState="true" 
    Inherits="wctMain.Admin._collectionEditors._banner.Banner_Director" Title="Admin - Edit Banners" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="bannerdirector">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>                
                <asp:Panel ID="Content" runat="server" >
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>