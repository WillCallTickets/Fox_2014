<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Employee_Director.aspx.cs" MasterPageFile="/View/Masters/TemplateAdmin.master" ValidateRequest="false" EnableViewState="true" 
    Inherits="wctMain.Admin._collectionEditors._employee.Employee_Director" Title="Admin - Edit Employees" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="employeedirector">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>                
                <asp:Panel ID="Content" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>