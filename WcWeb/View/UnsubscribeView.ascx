<%@ Control Language="C#" AutoEventWireup="false" CodeFile="UnsubscribeView.ascx.cs" Inherits="wctMain.View.UnsubscribeView" %>

<div class="displayer fade fade-slow unsubscribe-container static-container">
    <div class="section-header">Unsubscribe
        <% if(context.ToLower() == "mobile"){ %>
            <a class="btn btn-primary btn-xs pull-right lammie-close" id="laminate-close-top" href="/" title="Close">Close</a>
        <%} %>
    </div>
    <form id="unsubscribeform" action="unsubscribe" class="main-inner" novalidate>
        
        <div id="unsubscribeModal" class="wctmodal mailer-modal" tabindex="-1" role="dialog" aria-labelledby="unsubscribeModalLabel" aria-hidden="true">
            <div class="wctmodal-dialog">
                <div class="wctmodal-content">
            
                    <div class="wctmodal-header">Unsubscribing here will remove you from the Fox Theatre mailing list.<br /><br />
                        If you wish to unsubscribe from our 2 = 1 mailing list, please visit 
                        <a style="font-weight:bold;text-decoration:underline;" href="http://z2ent.com/unsubscribe">Z2ent.com/unsubscribe</a>
                    </div>
                    <br /><br />

                    <div class="wctmodal-success wctmodal-response"></div>
                    <div class="wctmodal-error wctmodal-response"></div>     
                    <br /><br />
                    <div class="wctmodal-footer">
                        <p class="wearefox">***Please note that we are the Fox Theatre located in Boulder, CO.</p>
                        <p>Allow up to 72 hours for your email address to be fully removed from our mailings.</p>
                        <p>If you are having problems unsubscribing, please ensure that you are using the correct email address. Sometimes email has been forwarded from an address other than which it was received.</p>
                        <p>If you continue to have problems, please <a id="unsubscribe_contact" class="link-foxt" href="/contact/contactus">contact us</a>.</p>
                        <p>
                            <small>Did you intend to <a class="link-foxt" href="http://z2ent.com/newsletter">subscribe</a>?</small>
                        </p>
                    </div>
                </div><!-- /.modal-content -->
            </div><!-- /.modal-dialog -->
        </div><!-- /#unsubscribeModal -->
    </form>
</div>


<script type="text/javascript">

    $(document).ready(function () {
        
        $('#unsubscribe_contact').attr('data-bind', 'click: loadStaticControl').address();
    });

</script>