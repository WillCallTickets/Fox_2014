<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowDirector.aspx.cs" MasterPageFile="/View/Masters/TemplateAdmin.master" EnableEventValidation="false" ValidateRequest="false"  
    Inherits="wctMain.Admin._shows.ShowDirector" Title="Admin - Edit Shows" %>
<%@ Register src="~/Admin/_shows/_controls/ShowSelector.ascx" tagname="ShowSelector" tagprefix="uc1" %>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="showmgr" >
        <div class="panel panel-primary">
            <div class="panel-heading">
                <div class="badge-container">
                    <span class="badge"><%=controlTitle %></span> 
                </div>
                <div class="current-show-container">
                    <%if (Atx.CurrentShowRecord != null) { %>
                    <div class="admin-show-listing">
                        <div class="admin-show-upper">
                            <span class="admin-show-date">
                                <%= Atx.CurrentShowRecord.FirstDate.ToString("ddd MMM dd, yyyy") %>
                            </span>
                            <span class="admin-show-venue">
                                @ <%= Atx.CurrentShowRecord.VenueRecord.Name_Displayable %>
                            </span>
                        </div>
                        <span class="admin-show-lower">
                            <span class="admin-show-event">
                                <%= Atx.CurrentShowRecord.GetShowMainActPart %>
                            </span>
                        </span>
                    </div>
                    <%} %>
                </div>
                <div class="owner-container">
                    <%if (Atx.CurrentShowRecord != null && Atx.CurrentShowRecord.VcPrincipal.ToLower().IndexOf("fox") != -1)
                      { %>
                    Fox Show
                    <%}
                      else if (Atx.CurrentShowRecord != null && Atx.CurrentShowRecord.VcPrincipal.ToLower().IndexOf("bt") != -1)
                      {%>
                    BT Show
                    <%} else if (Atx.CurrentShowRecord != null) { %>
                        <%= Utils.ParseHelper.ConvertTo_ProperCase(Atx.CurrentShowRecord.VcPrincipal)%> Show
                    <%} %>
                </div>
            </div>
            <div class="clearfix"></div>
            <div class="panel-selector">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <uc1:ShowSelector ID="ShowSelector1" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:Panel ID="Content" runat="server"></asp:Panel>
        </div>
    </div>
</asp:Content>