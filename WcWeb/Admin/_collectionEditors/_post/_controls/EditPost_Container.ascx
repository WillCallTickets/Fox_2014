<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditPost_Container.ascx.cs" Inherits="wctMain.Admin._collectionEditors._post._controls.EditPost_Container" %>
<%@ Register Src="~/Admin/_collectionEditors/_post/_controls/EditPost_Info.ascx" TagPrefix="uc1" TagName="EditPost_Info" %>
<%@ Register Src="~/Admin/_collectionEditors/_post/_controls/EditPost_Content.ascx" TagPrefix="uc1" TagName="EditPost_Content" %>

<div class="vd-dataentry-container">
    <div class="vd-data-entry entry-select">
        <!-- Nav tabs -->
        <asp:Repeater ID="rptNavTabs" runat="server" EnableViewState="true" 
            OnDataBinding="rptNavTabs_DataBinding" 
            OnItemDataBound="rptNavTabs_ItemDataBound"             
            OnItemCommand="rptNavTabs_ItemCommand">
            <HeaderTemplate><ul id="pnlNavTab" class="nav nav-tabs"></HeaderTemplate>
            <ItemTemplate><asp:Literal ID="litItem" runat="server" EnableViewState="false" /></ItemTemplate>
            <FooterTemplate>
                <li><asp:LinkButton ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" CommandName="Update" CssClass="btn btn-primary btn-command-tab"></asp:LinkButton></li>
                <li><asp:LinkButton ID="btnCancel" runat="server" ClientIDMode="Static" Text="Cancel" CommandName="Reset" CssClass="btn btn-primary btn-command-tab"></asp:LinkButton></li>
                <li><asp:LinkButton ID="btnClose" runat="server" ClientIDMode="Static" Text="Close" CommandName="Cancel" CssClass="btn btn-primary btn-command-tab"></asp:LinkButton></li>
                </ul></FooterTemplate>
        </asp:Repeater>
        <div class="tab-content" >
            <asp:Literal ID="divInfo" runat="server" EnableViewState="false" />
                <uc1:EditPost_Info runat="server" ID="EditPost_Info1" />
            <asp:Literal ID="divInfoEnd" runat="server" EnableViewState="false" Text="</div>" />
            <asp:Literal ID="divContent" runat="server" EnableViewState="false" />
                <uc1:EditPost_Content runat="server" ID="EditPost_Content1" />
            <asp:Literal ID="divContentEnd" runat="server" EnableViewState="false" Text="</div>" />
        </div><!-- end tab-content-->
    </div>
</div>