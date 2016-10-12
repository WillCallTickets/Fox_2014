<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Editor_Director.aspx.cs" MasterPageFile="/View/Masters/TemplateAdmin.master" ValidateRequest="false" 
Inherits="wctMain.Admin._editors.Editor_Director" Title="Admin - Editor" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">    
    <%if(!IsCollectionEditor) {%> 
    <div id="showmgr" >
        <div class="panel panel-primary">
            <div class="panel-heading">
                <div class="badge-container">
                    <span class="badge"><%=controlTitle %></span> 
                </div>
            </div>
            <div class="clearfix"></div>
    <%} %>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="Content" runat="server"></asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
    <%if(!IsCollectionEditor) {%> 
        </div>
    </div>
    <%} %>
</asp:Content>