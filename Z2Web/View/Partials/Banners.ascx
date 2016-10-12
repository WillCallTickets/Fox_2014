<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Banners.ascx.cs" Inherits="z2Main.View.Partials.Banners" %>

<div id="cycle-wrapper">
    <div id="cycle-border">
        <asp:Repeater ID="rptSlides" runat="server" EnableViewState="false" OnDataBinding="rptSlides_DataBinding" OnItemDataBound="rptSlides_ItemDataBound"><HeaderTemplate>
            <div id="cycle2carousel" class="cycle-slideshow" data-cycle-fx="fade" data-cycle-speed="1000" data-cycle-slides="> div" 
            data-cycle-pause-on-hover="true" ></HeaderTemplate><ItemTemplate>
                <asp:Literal ID="litSlideStart" runat="server" EnableViewState="false" /> 
                    <img src="/<%# z2Main.Controller.Z2Config.VIRTUAL_BANNER_DIR  + '/' + Eval("BannerUrl")%>" alt="<%# Eval("DisplayText") %>" />
                <asp:Literal ID="litSlideEnd" runat="server" EnableViewState="false" />  
            </ItemTemplate><FooterTemplate></div></FooterTemplate>
        </asp:Repeater>
    </div>
</div>