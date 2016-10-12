<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_DefaultShow.ascx.cs" Inherits="wctMain.Admin._editors._controls.Editor_DefaultShow" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc2" %>

<div id="defaulshowteditor" class="show-mgmt">
    <div class="panel-spacer"></div>
    <div class="panel-body">    
        <div class="form-group"> 
            <ul class="list-group" style="margin-bottom:0;">
                <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Select an item from the list and publish to activate.</li>
                <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Select &quot;Normal Flow&quot; or click on the reset button below and <b>publish</b> to reset to normal activity.</li>
                <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> When the selected show date has passed, normal flow will automatically resume.</li>
            </ul>   
            <br />                
        </div>     
        <div class="form-group">
            <div class="input-group">
                <span class="input-group-addon" style="width:150px;">
                    <b>Fox Theatre</b>
                </span>
                <asp:DropDownList ID="ddlFox" EnableViewState="true" runat="server" AppendDataBoundItems="true" AutoPostBack="True" 
                    DataSourceID="SqlFoxEventList" DataTextField="EventName" DataValueField="Id" CssClass="form-control"
                    OnDataBound="ddlShowDates_DataBound"
                    OnSelectedIndexChanged="ddlShowDates_SelectedIndexChanged" >
                    <asp:ListItem Text="...Normal Flow..." Value="0" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                <span class="input-group-btn">
                    <asp:LinkButton ID="btnResetFox" runat="server" OnClick="btnReset_Click" CssClass="btn btn-primary btn-command" Text="Reset" />
                </span>
            </div>
        </div>
        <div class="form-group">
            <div class="input-group">
                <span class="input-group-addon" style="width:150px;">
                    <b>Boulder Theater</b>
                </span>
                <asp:DropDownList ID="ddlBT" EnableViewState="true" runat="server" AppendDataBoundItems="true" AutoPostBack="True" 
                    DataSourceID="SqlBTEventList" DataTextField="EventName" DataValueField="Id" CssClass="form-control"
                    OnDataBound="ddlShowDates_DataBound" 
                    OnSelectedIndexChanged="ddlShowDates_SelectedIndexChanged" >
                    <asp:ListItem Text="...Normal Flow..." Value="0" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                <span class="input-group-btn">
                    <asp:LinkButton ID="btnResetBT" runat="server" OnClick="btnReset_Click" CssClass="btn btn-primary btn-command" Text="Reset" />
                </span>
            </div>
        </div>
        <div class="form-group">
            <br />
            <ul class="list-group" style="margin-bottom:0;">
                <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Publish to save changes!</li>                            
            </ul>                        
        </div>
    </div>
    <div class="panel-footer">
        &nbsp;
    </div>
</div>
<asp:SqlDataSource ID="SqlFoxEventList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT DISTINCT TOP 1000 sd.[Id], sd.[dtDateOfShow], CONVERT(varchar(25), sd.[dtDateOfShow], 111) + ' ' + 
        LTRIM(SUBSTRING(CONVERT(varchar(25), sd.[dtDateOfShow]), LEN(CONVERT(varchar(25), sd.[dtDateOfShow])) -6, 7)) + ' - ' + 
        SUBSTRING(s.[Name], (CHARINDEX(' - ', s.[Name]) + 3), LEN(s.[Name])) as 'EventName' FROM [ShowDate] sd, [Show] s 
        WHERE sd.[tShowId] = s.[Id] AND sd.[dtDateOfShow] >= getDate() AND (CHARINDEX(@prince, s.[vcPrincipal]) > 0)  ORDER BY sd.[dtDateOfShow] " 
    >
    <SelectParameters>
        <asp:Parameter Name="prince" DbType="String" DefaultValue="fox" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlBTEventList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT DISTINCT TOP 1000 sd.[Id], sd.[dtDateOfShow], CONVERT(varchar(25), sd.[dtDateOfShow], 111) + ' ' + 
        LTRIM(SUBSTRING(CONVERT(varchar(25), sd.[dtDateOfShow]), LEN(CONVERT(varchar(25), sd.[dtDateOfShow])) -6, 7)) + ' - ' + 
        SUBSTRING(s.[Name], (CHARINDEX(' - ', s.[Name]) + 3), LEN(s.[Name])) as 'EventName' FROM [ShowDate] sd, [Show] s 
        WHERE sd.[tShowId] = s.[Id] AND sd.[dtDateOfShow] >= getDate() AND (CHARINDEX(@prince, s.[vcPrincipal]) > 0)  ORDER BY sd.[dtDateOfShow] "        
    >
    <SelectParameters>        
        <asp:Parameter Name="prince" DbType="String" DefaultValue="bt" />
    </SelectParameters>
</asp:SqlDataSource>