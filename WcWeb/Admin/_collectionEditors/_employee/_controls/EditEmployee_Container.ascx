<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditEmployee_Container.ascx.cs" Inherits="wctMain.Admin._collectionEditors._employee._controls.EditEmployee_Container" %>
<%@ Register src="~/Admin/_collectionEditors/_employee/_controls/EditEmployee_Info.ascx" tagname="EditEmployee_Info" tagprefix="uc1" %>

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
                <li><asp:LinkButton ID="btnSave" runat="server" Text="Save" CommandName="Update" CssClass="btn btn-primary btn-command-tab" /></li>
                <li><asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CommandName="Reset" CssClass="btn btn-primary btn-command-tab" /></li>
                <li><asp:LinkButton ID="btnClose" runat="server" Text="Close" CommandName="Cancel" CssClass="btn btn-primary btn-command-tab" /></li>
                </ul></FooterTemplate>
        </asp:Repeater>
        <div class="tab-content" >
            <asp:Literal ID="divInfo" runat="server" EnableViewState="false" />
                <uc1:editemployee_info ID="EditEmployee_Info1" runat="server" />
            <asp:Literal ID="divInfoEnd" runat="server" EnableViewState="false" Text="</div>" />         
        </div><!-- end tab-content-->
    </div>
</div>
