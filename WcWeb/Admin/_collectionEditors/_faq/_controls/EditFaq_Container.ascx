<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditFaq_Container.ascx.cs" Inherits="wctMain.Admin._collectionEditors._faq._controls.EditFaq_Container" %>
<%@ Register src="~/Admin/_collectionEditors/_faq/_controls/EditFaq_Info.ascx" tagname="EditFaq_Info" tagprefix="uc1" %>
<%@ Register Src="~/Admin/_collectionEditors/_faq/_controls/EditFaq_Content.ascx" TagPrefix="uc2" TagName="EditFaq_Content" %>

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
                <uc1:EditFaq_Info ID="EditFaq_Info1" runat="server" />
            <asp:Literal ID="divInfoEnd" runat="server" EnableViewState="false" Text="</div>" />         
            <asp:Literal ID="divAnswer" runat="server" EnableViewState="false" />
                <uc2:EditFaq_Content ID="EditFaq_Content1" runat="server" />
            <asp:Literal ID="divAnswerEnd" runat="server" EnableViewState="false" Text="</div>" />
        </div><!-- end tab-content-->
    </div>
</div>
