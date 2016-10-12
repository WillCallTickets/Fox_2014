<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Kiosk_Director.aspx.cs" MasterPageFile="/View/Masters/TemplateAdmin.master" ValidateRequest="false" EnableViewState="true" 
    Inherits="wctMain.Admin._collectionEditors._kiosk.Kiosk_Director" Title="Admin - Edit Kiosks" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="kioskdirector">              
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>                
                <asp:Panel ID="Content" runat="server" >
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>