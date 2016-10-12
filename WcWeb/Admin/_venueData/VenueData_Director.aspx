<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VenueData_Director.aspx.cs" MasterPageFile="/View/Masters/TemplateAdmin.master" ValidateRequest="false" EnableViewState="true" 
    Inherits="wctMain.Admin._venueData.VenueData_Director" Title="Venue Data" %>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="venuedirector">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Content" runat="server" >
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>