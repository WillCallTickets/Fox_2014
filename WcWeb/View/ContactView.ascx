<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ContactView.ascx.cs" Inherits="wctMain.View.ContactView" %>

<div class="displayer fade fade-slow contact-container static-container">     
    <div class="section-header">Contact Us</div>
    <asp:Panel ID="pnlContact" runat="server" ClientIDMode="Static" CssClass="main-inner">

        <ul id="contacttabnav" class="nav nav-pills nav-justified">
            <li class="active" id="li_contactus" runat="server" ClientIDMode="Static"><a href="#contactus" rel="/contact/contactus" data-toggle="tab" class="btn">Contact Us</a></li>
            <li id="li_boxoffices" runat="server" ClientIDMode="Static"><a href="#boxoffices" rel="/contact/boxoffices" data-toggle="tab" class="btn">Box Offices</a></li>
            <li id="li_officeinfo" runat="server" ClientIDMode="Static"><a href="#officeinfo" rel="/contact/officeinfo" data-toggle="tab" class="btn">Main Office</a></li>
        </ul>
    
        <div class="tab-content" style="overflow:visible;">
            <div class="tab-pane fade" id="contactus" runat="server" ClientIDMode="Static">
                
                <div>Please note that confirmations & e-mail communications often go to spam or junk mail folders, please check those folders before contacting customer service regarding missing email confirmations.</div>
                <br />
                <div>Need to contact our box office?<br />Call or text us at <span class="nowrap bo-phone"><a href="tel:+3034470095">303.447.0095</a>.</span></div>
                <br />
                <div>Need to manage your <a id="contact_subscribe" class="link-foxt" href="/subscribe">email subscriptions</a>?</div>
                <br />
                <div>If you are contacting us regarding an order, please be sure to include the date of your order as well as the order id for us to better serve you.</div>
                <br /><br />
                <a href="#contactmodal" role="button" data-toggle="modal" class="btn btn-foxt btn-contact">Send us an email!</a>

            </div>            
            
            <div class="tab-pane fade" id="boxoffices" runat="server" ClientIDMode="Static">
                <div class="leader">
                    <strong>Tickets for all Fox Theatre &amp; Boulder Theater shows available online at:</strong>
                    <div class="inset-wrapper">
                        <a target="_blank" class="link-foxt" href="http://foxtheatre.frontgatetickets.com">foxtheatre.frontgatetickets.com</a>
                        <span class="contact-and">&amp;</span>
                        <a target="_blank" class="link-foxt" href="http://bouldertheater.frontgatesolutions.com">bouldertheater.frontgatesolutions.com</a>
                    </div>
                </div>
                <div class="leader">
                    <strong>Tickets for all Fox Theatre &amp; Boulder Theater shows also available at the following locations:</strong>
                    <address>
                        <div class="locality">Fox Theatre Box Office</div>
                        <!--<div>**Now located at Will Call in the Theatre**</div>-->
                        <div>On The Hill</div>
                        <div>1135 13th street</div>
                        <div class="tele"><a href="tel:+3034470095">303.447.0095</a></div>
                        <div class="foxbohour">
                            We no longer have a day time box office at the Fox Theatre. Ticket sales will be available at the Boulder Theater, 
                            see below for details. You can also purchase tickets online 24 hours a day at <a style="display:inline;" href="http://foxtheatre.com">foxtheatre.com</a>. 
                            The Fox box office will open at 6pm on the night of a show.
                        </div>



                        <!-- roughly June -> Sept 1
                        <div>*Summer hours</div>
                        <table border="0" cellspacing="0" cellpadding="0" >
                            <tr>
                                <td style="white-space:nowrap;padding-right:24px;">Thurs &amp; Fri</td>
                                <td style="width:100%;">Noon - 7pm</td>
                            </tr>
                            <tr>
                                <td>Sat</td>
                                <td>3pm - 7pm</td>
                            </tr>
                            <tr>
                                <td>Sun - Wed</td>
                                <td>Closed</td>
                            </tr>
                        </table>-->

                        <!-- normal hours
                        <table border="0" cellspacing="0" cellpadding="0" >
                            <tr>
                                <td style="white-space:nowrap;padding-right:24px;">Tue - Fri</td>
                                <td style="width:100%;">3pm - 7pm</td>
                            </tr>
                            <tr>
                                <td>Sat</td>
                                <td>Noon - 5pm</td>
                            </tr>
                            <tr>
                                <td colspan="2"> Closed Sun &amp; Mon</td>
                            </tr>
                            <tr>
                            <td colspan="2">**also open during shows</td>
                            </tr>
                        </table>-->
                    </address>
                    <address>                        
                        <div class="locality">Boulder Theater Box Office</div>
                        <div>Downtown Boulder</div>
                        <div>2032 14th street</div>
                        <div class="tele"><a href="tel:+303.786.7030" >303.786.7030</a></div>
                        <table border="0" cellspacing="0" cellpadding="0" >
                            <tr>
                                <td style="white-space:nowrap;padding-right:24px;">Mon - Fri</td>
                                <td style="width:100%;">Noon - 6pm</td>
                            </tr>
                            <tr>
                                <td>Sat</td>
                                <td>12pm - 5pm</td>
                            </tr>
                            <tr>
                                <td>Sun</td>
                                <td>Closed</td>
                            </tr>
                        </table>
                    </address>
                </div>
            </div>
            
            
            
            <div class="tab-pane fade" id="officeinfo" runat="server" ClientIDMode="Static">
                <table border="0" cellspacing="0" cellpadding="0" class="table">
                    <caption><strong>Location and Main Phone</strong></caption>
                    <tr><th>Street Address</th><td><%=Wcss._Config._Site_Entity_PhysicalAddress %></td></tr>
                    <tr><th>Main Office</th><td><a href="tel:+<%=Wcss._Config._MainOffice_Phone %>" ><%=Wcss._Config._MainOffice_Phone %></a></td></tr>
                    <tr><th>Box Office</th><td ><a href="tel:+<%=Wcss._Config._BoxOffice_Phone %>" ><%=Wcss._Config._BoxOffice_Phone %></a></td></tr>
                </table>
                <table border="0" cellspacing="0" cellpadding="0" class="table">
                    <caption>Employee Contacts</caption>
                    <asp:Literal ID="litEmployeeInfo" runat="server" OnDataBinding="litEmployeeInfo_DataBinding" />                    
                </table>
            </div>
        </div>
    </asp:Panel>
</div> 

<script type="text/javascript">

    //register the contact subscribe link to address
    $(document).ready(function () {
        
        //(_fnService, _fnSuccess, _inputs, _fnParamValidate)
        $('#contactmodal').wctModal(
            'sendContactEmail',
            //define success
            wct_contactSuccess,
            //define inputs
            ['#contactName', '#contactEmail', '#contactSubject', '#contactMessage'],
            wct_contactParamBuilder            
            );

        $('#contact_subscribe').attr('data-bind', 'click: loadStaticControl').address();
        
        //attach tab navigation to address
        $('#contacttabnav LI A').click(function () {
            //this function lives in the index.js            
            anchorClientLink(this);
        });

        registerMailto();
    });

</script>