<%@ Control Language="C#" AutoEventWireup="false" CodeFile="MailerManageView.ascx.cs" Inherits="z2Main.View.MailerManageView" %>

<div class="unsubscribe-container view-container">
    <div class="main-inner">
        <form id="unsubscribeform" action="/unsubscribe" class="pageform" novalidate>
            <div id="unsubscribeZ2Modal" class="wctmodal" aria-labelledby="unsubscribeModalLabel">
                <div class="section-header">                        
                    <h2 id="unsubscribeModalLabel">Manage Your Newsletter Subscription</h2>
                </div>

                <div class="wctmodal-success wctmodal-response"></div>
                <div class="wctmodal-error wctmodal-response"></div>
                    
                <div class="wctmodal-body form-horizontal">                        
                    <div>
                        <div class="input-group">
                            <label class="sr-only" for="contactEmail">Email Address</label>
                            <input type="email" class="form-control radius-helper-left" id="unsubscribeEmail" name="email" placeholder="Enter Email Address ..." />
                            <span class="input-group-btn">
                                <button type="button" class="btn btn-primary wct-modal-action btn-pad-wide" data-toggle="button" 
                                    data-loading-text="<span class='wct-modal-spinner'></span>Processing...">Unsubscribe</button>
                            </span>
                        </div>
                        
                        <p>&nbsp;</p>
                        <p>I would like to <a id="" class="mailersubscriber" href="/newsletter">subscribe</a></p>
                    </div>
                </div>                                     
                <div class="wctmodal-footer">
                    <p>Please allow up to 72 hours for your email to be fully removed from our mailings.</p>
                    <p>If you are having problems unsubscribing, please ensure that you are using the correct email address. Sometimes email has been forwarded to an address from other than which it was received.</p>
                    <p>If you continue to have problems, please &nbsp;<a id="unsubscribe_contact" href="mailto:<%=z2Main.Controller.Z2Config.CUSTOMERSERVICEEMAIL_Z2 %>">contact us</a>.</p>
                </div>
            </div><!-- /#unsubscribeModal -->
        </form>
    </div>
</div>

<script type="text/javascript">

    $(document).ready(function () {
        
        $('#unsubscribeZ2Modal').z2Modal(
            '/Handler/MailupApi',
            //define success
            z2_mailupApiSuccess,
            //define inputs
            ['#unsubscribeEmail'],
            z2_mailupApiManageParamBuilder
            );
        
        $('#unsubscribeEmail').on('keydown', function (e) {
            if (e.keyCode >= 37 && e.keyCode <= 40) {
                e.stopPropagation();
            }
        });

        $('.mailersubscriber').address();

        //$('#unsubscribe_contact').add('#unsubscribe_subscribe').attr('data-bind', 'click: loadStaticControl').address();
    });

</script>