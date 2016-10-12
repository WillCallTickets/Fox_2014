<%@ Page Language="C#" MasterPageFile="/View/Masters/Master_Kiosk.master" AutoEventWireup="false" CodeFile="Kiosk.aspx.cs" Inherits="wctMain.Kiosk" Title="Upcoming Shows" %>

<asp:Content ID="ContentRotator" ContentPlaceHolderId="RotatedContent" runat="server">
    
<div id="cycle-kiosk" class="<%= classInfo %>">
    <asp:Repeater ID="rptKiosk" runat="server" ClientIDMode="Static" EnableViewState="false" 
        OnDataBinding="rptKiosk_DataBinding" 
        OnItemDataBound="rptKiosk_ItemDataBound"><HeaderTemplate>
        <div id="kioskcarousel" class="cycle-slidesho" data-cycle-fx="fadeout" data-cycle-speed="2000" data-cycle-slides="> div" 
            data-cycle-pause-on-hover="false" ></HeaderTemplate><ItemTemplate>
            <asp:Literal ID="litViewport" runat="server" EnableViewState="false" />
                <asp:Literal ID="litImage" runat="server" EnableViewState="false" />
                <asp:Literal ID="litDivText" runat="server" EnableViewState="false" />
            </div>
        </ItemTemplate><FooterTemplate></div></FooterTemplate>
    </asp:Repeater>
</div>

</asp:Content>

<asp:Content ID="ContentJson" ContentPlaceHolderId="JsonObjects" runat="server">
</asp:Content>