<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" CodeFile="UpcomingShowList.ascx.cs" Inherits="wctMain.View.Partials.UpcomingShowList" %>
<div id="upcoming-shows" >
    <div class="section-header section-header-top section-header-upcoming">
        <%=Wcss._Config._DisplayMenu_Title %>        
    </div>
    <div id="upcoming-inner" >
        <div data-bind="foreach: events" id="upcoming-loop">
            <!-- ko if: $data.Displayable() -->
            
            <div data-toggle="collapse" data-parent="#upcoming-loop" data-bind="
    attr: { 'class': $data.EventOrigin + ' panel', 'data-target': '#ecollapse' + $data.Id },
        css: { activio: $parent.displayEventToggle() == true && $data.Id == $parent.selected().Id }" >
                
                <a class="list-url" data-bind="attr: { href: $data.Url, title: ($data.MonthName + ' ' + $data.Day + ' - ' + $data.Billing) }, click: setSelected">
        
                    <span class="datepart-container">
                        <span class="month" data-bind="text: $data.MonthName"></span>
                        <span class="day" data-bind="text: $data.Day"></span>
                    </span>

                    <span class="upcoming-container">
                        <span class="billing" data-bind="text: $data.Billing"></span>
                        <!-- ko if: $data.BillingOpens != '' -->
                        <span class="billingopens" data-bind="text: $data.BillingOpens"></span>      
                        <!-- /ko -->                                         
                        <span class="status-container">
                            <!-- ko if: $data.Alert != '' -->
                            <span class="status-alert" data-bind="text: $data.Alert"></span>
                            <!-- /ko -->
                            <!-- ko if: $data.Status != '' -->
                            <span class="status-status" data-bind="text: $data.Status"></span>
                            <!-- /ko -->
                        </span>                    
                    </span>
                    <span class="mobile-info">
                        <!-- ko if: $data.ParsedStatus() != '' -->
                        <span class="status-status"><span data-bind="text: $data.ParsedStatus()"></span></span>
                        <!-- /ko -->                            
                        <!-- ko if: $data.ParsedStatus() == '' && $data.DisplaySaleDate() != '' -->
                        <span class="status-future-sale"><span style="margin-bottom:5px;">On Sale</span><span data-bind="text: $data.DisplaySaleDate()"></span></span>
                        <!-- /ko -->
                        <!-- ko if: ($data.ParsedStatus() == '' && $data.DisplaySaleDate() == '' && $data.TicketUrl != '') -->
                        <span class="status-buy"><span data-bind="click: navTickets">Buy Tix! <i class="glyphicon glyphicon-new-window"></i></span></span>
                        <!-- /ko -->
                    </span>
                </a>
                <div class="clearfix"></div>   
                <div data-bind="attr: { 'id': 'ecollapse' + $data.Id }" class="collapse m-container">
                         
                    <div class="m-inner">
                        <span class="m-times">
                            <!-- ko if: ($data.DoorTime != '') -->
                            <span data-bind="text: 'Doors: ' + $data.DoorTime"></span>
                            <!-- /ko -->
                            <!-- ko if: ($data.DoorTime != '' && $data.ShowTime != '') -->
                            <span class="m-separator">/</span>
                            <!-- /ko -->
                            <!-- ko if: ($data.ShowTime != '') -->
                            <span data-bind="text: 'Show: ' + $data.ShowTime"></span>
                            <!-- /ko -->                            
                        </span>
                        <!-- ko if: ($data.Ages != '') -->
                        <span class="m-ages" data-bind="text: $data.Ages"></span>
                        <!-- /ko -->
                        <!-- ko if: ($data.Pricing != '') -->
                        <span class="m-pricing" data-bind="text: $data.Pricing"></span>
                        <!-- /ko -->
                        <div class="m-social-space" >Loading...</div>
                    </div>
                </div>
            </div>
            <!-- /ko -->
        </div><!--end for each event -->
        <footer>
            <div class="copyright">
                ©
                <%=DateTime.Now.Year%>
                Fox Theatre
            </div>
            <div class="address">
                <%=Wcss._Config._Site_Entity_PhysicalAddress%>
            </div>
        </footer>
    </div><!--upcoming-inner -->
</div>
<div class="clearfix"></div>