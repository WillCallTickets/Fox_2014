<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditBanner_Container.ascx.cs" Inherits="wctMain.Admin._collectionEditors._banner._controls.EditBanner_Container" %>
<%@ Register Src="~/Admin/_collectionEditors/_banner/_controls/EditBanner_Info.ascx" TagPrefix="uc1" TagName="EditBanner_Info" %>
<%@ Register Src="~/Admin/_collectionEditors/_banner/_controls/EditBanner_Image.ascx" TagPrefix="uc2" TagName="EditBanner_Image" %>
 
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
                <uc1:EditBanner_Info ID="EditBanner_Info1" runat="server" />
            <asp:Literal ID="divInfoEnd" runat="server" EnableViewState="false" Text="</div>" />         
            <asp:Literal ID="divImage" runat="server" EnableViewState="false" />
                <uc2:EditBanner_Image ID="EditBanner_Image1" runat="server" />
            <asp:Literal ID="divImageEnd" runat="server" EnableViewState="false" Text="</div>" />
        </div><!-- end tab-content-->
    </div>
</div>