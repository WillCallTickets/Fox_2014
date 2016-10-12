<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowCreator.ascx.cs" Inherits="wctMain.Admin._shows._controls.ShowCreator" EnableViewState="false" %>
<%@ Register Src="/Admin/_editors/_controls/Editor_Act.ascx" TagName="Editor_Act" TagPrefix="uc2" %>
<%@ Register assembly="WctControls" namespace="WctControls.WebControls" tagprefix="cc1" %>

<div id="showcreator" class="show-mgmt">
    <div class="panel-body">
        <div class="alert alert-danger">
            <strong>Owner, Venue, ShowDate &amp; Act are required.</strong>
        </div>
        <cc1:ErrorDisplayLabel ID="ErrorDisplayLabel1" runat="server" />
                
        <div class="form-group">
            <div class="form-group col-xs-6" style="padding-left:0;">
                <div class="input-group">
                    <span class="input-group-addon">
                        Owner
                    </span>
                    <asp:RadioButtonList ID="rdoPrincipal" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" 
                        RepeatLayout="Table" EnableViewState="true" 
                        CssClass="form-control radio-horiz" 
                        AutoPostBack="true" OnSelectedIndexChanged="rdoPrincipal_SelectedIndexChanged" 
                        OnDataBinding="rdoPrincipal_DataBinding" OnDataBound="listControl_DataBound" />
                </div>
            </div>
            <div class="form-group col-xs-6" style="padding-right:0;">
                <div class="input-group">
                    <span class="input-group-addon">
                        Ages
                    </span>
                    <asp:DropDownList ID="ddlAges" runat="server" OnDataBinding="ddlAges_DataBinding" 
                        OnDataBound="ddlAges_DataBound" CssClass="form-control" />
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="input-group">
                <span class="input-group-addon">
                    Venue
                </span>                        
                <asp:DropDownList ID="ddlVenue" runat="server" ClientIDMode="Static" CssClass="form-control" 
                    DataSourceID="SqlVenue" DataTextField="Name" DataValueField="Id"
                    OnDataBound="listControl_DataBound" />
                <span class="input-group-btn">
                    <a class="btn btn-primary" href="/Admin/_editors/Editor_Director.aspx?p=venue">venue editor</a>
                </span>
            </div>
        </div>
        <div class="form-group">
            <div class="form-group col-xs-6" style="padding-left:0;">
                <cc1:BootstrapDateTimePicker ID="txtDateStart" Label="Show Date" Date='<%#Bind("DateStart") %>' 
                    DateCompareEmpty="min" IsRequired="false" CssClass="showeditor-dtpicker" runat="server" />
            </div>
            <div class="form-group col-xs-6" style="padding-right:0;">
                <ul class="list-group" style="margin-bottom:0;height:34px;">
                    <li class="list-group-item list-group-item-info"><span class="glyphicon glyphicon-info-sign"></span> Show Date references DOOR time.</li></ul>
            </div>
        </div>
        <div class="form-group">
            <div class="form-group col-xs-6" style="padding-left:0;">
                <cc1:BootstrapDateTimePicker ID="txtShowTime" Label="Show Time" Text="8:30PM"
                    FormatString='<%# WctControls.WebControls.Bootstrap.DateTimePicker.Time_FormatString %>' 
                    DateCompareEmpty="min" IsRequired="false" CssClass="showeditor-showtime-dtpicker fred" runat="server" />
            </div>
            <div class="form-group col-xs-6" style="padding-right:0;">
                <div class="input-group">
                    <span class="input-group-addon">
                        TBA
                    </span>
                    <asp:CheckBox ID="chkTba" runat="server" ClientIDMode="Static" CssClass="form-control" TextAlign="Right" Checked='<%#Bind("Tba") %>' />
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="form-group col-xs-6" style="padding-left:0;">
                <cc1:BootstrapDateTimePicker ID="txtDateAnnounce" Label="Announce" Date='<%#Bind("AnnounceDate") %>' 
                    DateCompareEmpty="min" IsRequired="false" CssClass="showeditor-dtpicker" runat="server" />
            </div>
            <div class="form-group col-xs-6" style="padding-right:0;">
                <cc1:BootstrapDateTimePicker ID="txtDateOnsale" Label="On Sale" Date='<%#Bind("OnSaleDate") %>' 
                    DateCompareEmpty="min" IsRequired="false" CssClass="showeditor-dtpicker" runat="server" />
            </div>
        </div>
        <div class="clearfix"></div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:Editor_Act ID="Editor_Actc" runat="server" HideSelectionPanel="false" HideDisplayWebsiteDelete="true" />
            </ContentTemplate>
        </asp:UpdatePanel>  
    </div>

    <div class="panel-footer">
        <div class="form-group">
            <asp:LinkButton ID="btnAdd" runat="server" CausesValidation="false" CssClass="btn btn-lg btn-primary btn-command" 
                Text="Create Show" 
                OnClick="btnAdd_Click"  />
            <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" Text="Cancel" 
                OnClick="btnCancel_Click"  CssClass="btn btn-lg btn-primary btn-command" />
        </div>
    </div>
</div>
<asp:SqlDataSource ID="SqlVenue" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT 0"
    onselecting="SqlVenue_Selecting">
</asp:SqlDataSource>