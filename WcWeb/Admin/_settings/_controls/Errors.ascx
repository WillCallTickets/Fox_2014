<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Errors.ascx.cs" Inherits="wctMain.Admin._settings._controls.Errors" %>

<div id="errors" class="show-mgmt">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="badge-container">
                        <span class="badge badge-control" >SITE ERRORS</span>
                    </div>
                    <asp:LinkButton ID="btnArchive" CssClass="btn btn-default btn-add-new pull-right" runat="server" 
                        Text="Archive This" OnClick="btnArchive_Click" CausesValidation="false" 
                        OnClientClick='return confirm("Are you sure you want to archive all events? This will archive ALL events - not just the ones being viewed.")' />
                </div>
                <div class="clearfix"></div>
                <div class="panel-body" >
                    <div style="position:relative;float:left; width:100%;">
                        
		                <div style="position:relative;float:left; width:48%;height:900px;overflow-y:scroll;">
		                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" Width="100%" AutoGenerateColumns="False" Font-Size="12px" 
		                        DataKeyNames="Id" DataSourceID="SqlDataSource1" PageSize="25" PagerSettings-Position="Top" CssClass="results" GridLines="None" 
		                        OnDataBound="GridView1_DataBound" OnPageIndexChanged="GridView1_PageIndexChanged" >
                                <SelectedRowStyle CssClass="grid-selection" />
		                        <HeaderStyle CssClass="grid-header" />
                                <PagerStyle CssClass="grid-pager" /> 
                                <AlternatingRowStyle CssClass="grid-alt-row" /> 
                                <RowStyle CssClass="grid-row" />
		                        <Columns>
		                            <asp:TemplateField>
		                                <ItemTemplate>
		                                    <div>
		                                        <span style="width:80px;padding:0 8px;">
		                                            <asp:Button ID="btnSelect" runat="server" Text='<%#Eval("Id") %>' CommandName="Select" 
		                                                ToolTip="Select" cssClass="btn btn-primary" />
		                                        </span>
		                                        <span style="width:100%;padding:0 8px;"><%#Eval("Date", "{0:MM/dd/yyyy hh:mm:ss tt}") %></span>
		                                        <span style="float:right;padding:0 8px;text-align:right;"><%#Eval("ApplicationName") %></span>
		                                    </div>
		                                    <div style="clear:both;">
		                                        <span style="width:80px;padding:0 8px;">MSG:</span>
		                                        <div style="padding:0 8px;word-wrap:break-word;overflow-x:scroll;max-width:600px;"><%#System.Web.HttpUtility.HtmlEncode(Eval("Message").ToString()) %></div>
		                                    </div>                                            
		                                </ItemTemplate>
		                            </asp:TemplateField>
		                        </Columns>
		                    </asp:GridView>
		                </div>
                        <div style="position:relative;float:left; width:48%;margin-left:10px;height:900px;overflow-y:scroll;overflow-x:hidden;">
		                    <asp:FormView ID="FormView1" runat="server" DefaultMode="ReadOnly" DataKeyNames="Id" GridLines="None" CssClass="results results-error"
		                        DataSourceID="SqlDataSource2" Width="90%" Font-Size="12px" OnDataBound="FormView1_DataBound">
                                
		                        <ItemTemplate>
		                            <table border="0" cellspacing="0" cellpadding="0" >
		                                <tr>
		                                    <th valign="top" style="width:20%">ID:</th>
		                                    <td style="width:78%"><%# Eval("Id") %></td>
                                        </tr>
                                        <tr>
		                                    <th valign="top">Date:</th>
		                                    <td><%# Eval("Date") %></td>
		                                </tr>
                                        <tr>
                                            <th valign="top">IP:</th>
		                                    <td><%# Eval("IpAddress") %></td>
                                        </tr>
		                                <tr>
		                                    <th valign="top">UserInfo:</th>
		                                    <td><%# Eval("Email") %></td>
		                                </tr>
		                                <tr><th valign="top">Source:</th><td><%# System.Web.HttpUtility.HtmlEncode(Eval("Source").ToString()) %></td></tr>
		                                <tr><th valign="top">TargetSite:</th><td><%# System.Web.HttpUtility.HtmlEncode(Eval("TargetSite").ToString()) %></td></tr>
		                                <tr>
		                                    <th valign="top">Referrer:</th><td style="word-wrap:break-word;"><%# System.Web.HttpUtility.HtmlEncode(Eval("Referrer").ToString()) %></td>
                                        </tr>
                                        <tr>
		                                    <th valign="top">QueryString:</th><td style="word-wrap:break-word;"><%# System.Web.HttpUtility.HtmlEncode(Eval("QueryString").ToString()) %></td>
		                                </tr>
		                                <tr><th valign="top">Form:</th><td ><%# System.Web.HttpUtility.HtmlEncode(Eval("Form").ToString())%>&nbsp;</td></tr>
							            <tr><th valign="top">Message:</th><td colspan="3" style="overflow:auto;word-wrap:break-word;"><%# System.Web.HttpUtility.HtmlEncode(Eval("Message").ToString()) %></td></tr>
							            <tr><td colspan="2" ><div style="word-wrap:break-word;overflow-x:scroll;min-height:120px;"><%# System.Web.HttpUtility.HtmlEncode(Eval("StackTrace").ToString())%></div></td></tr>
                                    </table>
		                        </ItemTemplate>
		                    </asp:FormView>
		                    &nbsp;
		                </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:SqlDataSource ID="SqlDataSource1" runat="server"
    ConnectionString="<%$ ConnectionStrings:ErrorLogConnectionString %>" 
    SelectCommand="SELECT DISTINCT [Id], [Date], [Message], [ApplicationName] FROM [Log] WHERE [ApplicationName] = @ApplicationName ORDER BY [Date] DESC"
        OnSelecting="SqlDataSource1_Selecting">
    <SelectParameters>
        <asp:Parameter Name="ApplicationName" DbType="String" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ErrorLogConnectionString %>" 
    SelectCommand="SELECT [Id], [Date], [Source], [Message], [Referrer], [StackTrace], [TargetSite], [Querystring], [Form], 
    [IpAddress], [Email], [ApplicationName] FROM [Log] WHERE ([Id] = @Id)">
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="Id" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>