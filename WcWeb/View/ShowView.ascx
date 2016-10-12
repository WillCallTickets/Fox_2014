<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" CodeFile="ShowView.ascx.cs" Inherits="wctMain.View.ShowView" %>

<div itemscope itemtype="http://schema.org/Event" class="displayer fade fade-slow show-container">
    <form id="showform" action="show">
    <%if(_show != null) {%>
        <link itemprop="url" href="<%= _show.FirstShowDate.ConfiguredUrl_withDomain %>" />
        <meta itemprop="location" content="<%= _show.VenueRecord.Name_Displayable %>" />

        <input id="hdnver" name="hdnver" type="hidden" value='<%= Ctx.PublishVersion_Announced.ToString() %>' />
        <input id="hdnid" name="hdnid" type="hidden" value='<%= _show.Id.ToString() %>' />
        <input id="hdnshowimg" name="hdnshowimg" type="hidden" itemprop="image" value='<%= _show.ShowImageUrl_Backstretch %>' />        
        <div class="section-header section-header-top section-header-show">
            <%= _show.Display.ShowHeader %>
        </div>
        <div class="main-inner">            
            <div class="event-detail-container">
                <div class="event-details">
                    <asp:Literal ID="litVenue" runat="server" EnableViewState="false" />
                    <asp:Literal ID="litPromoter" runat="server" EnableViewState="false" />
                    <asp:Literal ID="litHeader" runat="server" EnableViewState="false" />
                    <div itemprop="name" class="event-description">
                        <asp:Literal ID="litEventDescription" runat="server" EnableViewState="false" />
                    </div>
                </div>
            
                <asp:Literal ID="litNotes" runat="server" EnableViewState="false" />                

                <div itemprop="offers" class="event-info">
                    <div class="event-info-inner">
                        <asp:Literal ID="litInfo" runat="server" EnableViewState="false" />
                        <asp:Literal ID="litAdditionalInfo" runat="server" EnableViewState="false" />
                    </div>
                </div>
            </div>

            <div class="clearfix"></div>
            <div class="event-abstract">
                <asp:Literal ID="litWriteup" runat="server" EnableViewState="false" />
                <asp:Literal ID="litShowLinks" runat="server" EnableViewState="false" OnDataBinding="litShowLinks_DataBinding" />
            <!--</div>-->
            </div>            
        </div>
        <%} else {%>
        <h4 class="page-not-found">Sorry, page not found</h4>
        <%} %>
    </form>
</div>
<asp:Literal ID="litShowMore" runat="server" ClientIDMode="Static" />
