<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Cycle2Carousel.ascx.cs" Inherits="wctMain.View.Partials.Cycle2Carousel" %>

<div id="cycle-wrapper">
    <div id="cycle-border">
        <asp:Repeater ID="rptSlides" runat="server" EnableViewState="false" OnDataBinding="rptSlides_DataBinding" OnItemDataBound="rptSlides_ItemDataBound"><HeaderTemplate>
            <div id="cycle2carousel" class="cycle-slideshow" data-cycle-fx="fade" data-cycle-speed="1000" data-cycle-slides="> div" 
            data-cycle-pause-on-hover="true" ></HeaderTemplate><ItemTemplate>
                <asp:Literal ID="litSlideStart" runat="server" EnableViewState="false" /> 
                    <img src="<%# Eval("Banner_VirtualFilePath") %>" alt="<%# Eval("DisplayText") %>" />
                <asp:Literal ID="litSlideEnd" runat="server" EnableViewState="false" />  
            </ItemTemplate><FooterTemplate></div></FooterTemplate>
        </asp:Repeater>
    </div>
</div>