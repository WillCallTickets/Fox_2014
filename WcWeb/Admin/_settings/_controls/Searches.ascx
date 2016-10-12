<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Searches.ascx.cs" Inherits="wctMain.Admin._settings._controls.Searches" %>
<%@ Register src="~/Admin/_customControls/CollectionContextPrincipalPicker.ascx" tagname="CollectionContextPrincipalPicker" tagprefix="uc2" %>

<div id="searches" class="show-mgmt">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="badge-container">
                        <span class="badge" >SITE SEARCHES</span> 
                    </div>
                </div>
                <div class="clearfix"></div>
                <div class="panel-actions">
                    <uc2:CollectionContextPrincipalPicker ID="CollectionContextPrincipalPicker1" Title="Searches" runat="server" />
                </div>
                <div class="panel-body">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" Width="100%" 
                        AutoGenerateColumns="true" DataKeyNames="Id" DataSourceID="SqlDataSource1"
                        PageSize="25" PagerSettings-Position="Top" CssClass="results" GridLines="None" >
                        <HeaderStyle CssClass="grid-header" />
                        <PagerStyle CssClass="grid-pager" /> 
                        <AlternatingRowStyle CssClass="grid-alt-row" /> 
                        <RowStyle CssClass="grid-row" />
                        <EmptyDataTemplate>
                            <div class="">No Searches</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:SqlDataSource ID="SqlDataSource1" runat="server"
    ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT DISTINCT * FROM [Search] WHERE CASE WHEN @vcPrincipal = 'all' THEN 1 ELSE CASE WHEN (CHARINDEX(@vcPrincipal, [vcPrincipal]) >= 1) THEN 1 ELSE 0 END END = 1 ORDER BY [dtStamp] DESC"
        OnSelecting="SqlDataSource1_Selecting">
    <SelectParameters>
        <asp:Parameter Name="vcPrincipal" DbType="string" />
    </SelectParameters>
</asp:SqlDataSource>