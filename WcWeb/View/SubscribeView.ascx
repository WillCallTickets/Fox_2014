<%@ Control Language="C#" AutoEventWireup="false" CodeFile="SubscribeView.ascx.cs" Inherits="wctMain.View.SubscribeView" %>

<div class="displayer fade fade-slow subscribe-container static-container">
    <div class="section-header">Subscribe
        <% if(context.ToLower() == "mobile"){ %>
            <a class="btn btn-primary btn-xs pull-right lammie-close" id="laminate-close-top" href="/" title="Close">Close</a>
        <%} %>
    </div>
    <div class="main-inner">
        <form id="subscribeform" action="subscribe" class="pageform" onsubmit="return false;" novalidate>
            <!-- Modal -->
            <div id="subscribeModal" class="wctmodal mailer-modal" tabindex="-1" role="dialog" aria-labelledby="subscribeModalLabel" aria-hidden="true">
               <div class="wctmodal-dialog">
                   <div class="wctmodal-content">
                       <div class="wctmodal-header">                                            
                           <div style="padding-top:10px;padding-BOTTOM: 5px; FONT-SIZE: 18px;font-weight:bold;white-space:nowrap;">
                                DON'T MISS OUT! We'd Miss You.
                            </div>
                            <div style="FONT-SIZE: 14px;padding-top:10px;">
                                <p>Your 2 favorite venues, the Boulder and Fox Theatres, are combining their newsletters into 1.  
                                    <a style="font-weight:bold;" href="http://z2ent.com/?utm_source=foxsubscribepage_confirm&utm_medium=website&utm_campaign=subscribeview" target="_blank">Sign up</a> 
                                    to our new 2 &#61; 1 newsletter.</p>
                                <p>
                                    Did you know we offer free tickets, special concert announcements and early bird ticket deals through our newsletter? 
                                    <a style="font-weight:bold;" href="http://z2ent.com/?utm_source=foxsubscribepage_signup&utm_medium=website&utm_campaign=subscribeview" target="_blank">Sign up</a> 
                                    today to find out about the 400+ events we have each year at your Boulder and Fox Theatres!
                                </p>
                            </div>
                        </div>

                                                                
                        <div class="wctmodal-footer">
                            <br /><br />
                            <p><small>
                                Need to <a class="link-foxt" href="http://z2ent.com/unsubscribe">unsubscribe</a>?</small>
                            </p>
                        </div>
                    </div><!-- /.modal-content -->
                </div><!-- /.modal-dialog -->
            </div><!-- /#subscribeModal -->

            <input id="hdnProfile" type="hidden" value="<%=_profile %>" />
            <input id="hdnUserIp" type="hidden" value="<%=this.Request.UserHostAddress %>" />
        </form>
    </div>
</div>