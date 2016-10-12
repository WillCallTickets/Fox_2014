<%@ Control Language="C#" AutoEventWireup="false" CodeFile="NewsletterView.ascx.cs" Inherits="z2Main.View.NewsletterView" EnableViewState="false" %>
    
<div class="newsletter-container view-container">
    <div class="main-inner">
        <div id="newsletterform" class="pageform">
            <div id="subscribeZ2Modal" class="wctmodal" aria-labelledby="subscribeModalLabel">
                <div class="section-header">
                    <div class="signup-cta">DON'T MISS OUT! We'd Miss You.</div>
                </div>
                                            
                <div class="newsletter-inner">                    

                    <p>Your 2 favorite venues, the Boulder and Fox Theatres, are combining their newsletters into 1. Please confirm your subscription to our new 2 &#61; 1 newsletter.</p>
                    <!--<p>Sign up by the end of October for your chance to win four tickets to the show of your choice.</p>-->
                    <p>Did you know we offer free tickets, special concert announcements and early bird ticket deals through our newsletter? Sign up today to find out about the 400+ 
                        events we have each year at your Boulder and Fox Theatres!</p>
                        
                    <h3>2 Venues in 1 Newsletter means More Music and Less Noise. Sign up today for 2 &#61; 1 </h3>
                                
                    <div class="wctmodal-success wctmodal-response" style="font-size:20px;"></div>
                    <div class="wctmodal-error wctmodal-response"></div>

                    <div class="wctmodal-body"> 
                        <form id="form-inputs" action="http://d9c4a.s76.it/frontend/subscribe.aspx" method="post" name="mailupSubscribeForm" class="pageform">
                            <p class="privacy-check" style="margin-left:10px;">
                                <input type="checkbox" id="chkPrivacy" class="chk-privacy"/>&nbsp; <label for="chkPrivacy">I have read and understand the</label> <a style="font-weight:bold;" id="newsprivacy">privacy policy</a>
                            </p>
                            <div class="input-group">
                                <label class="sr-only" for="contactEmail">Email Address</label>
                                <input type="email" class="form-control radius-helper-left" id="subscribeEmail" name="email" placeholder="Enter Email Address ..." />
                                <span class="input-group-btn">
                                     <input id="btnSubscribe" name="Submit" type="Submit" value="Subscribe!" class="btn btn-danger btn-cmd-signup wct-modal-action"
                                        onclick="return validateMailupSubmission(document.mailupSubscribeForm.chkPrivacy, document.mailupSubscribeForm.email.value);" >
                                </span>
                            </div>
                            <input type="hidden" name="list" value="2">
                            <input type="hidden" name="group" value="26">
                            <input type="hidden" name="confirm" value="off" />
                        </form>
                    </div>      
                    <p>Need to <a href="/mailermanage">unsubscribe</a>?</p>
                    <p>If you are having problems signing up, please <a id="unsubscribe_contact" 
                        href="mailto:<%=z2Main.Controller.Z2Config.CUSTOMERSERVICEEMAIL_Z2 %>">contact us</a>.</p>                     
                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        
        registerMailupSubscriptionProcess();
        
        $('#email').on('keydown', function (e) {
            if (e.keyCode >= 37 && e.keyCode <= 40) {
                e.stopPropagation();
            }
        });

        registerPrivacyPopup('#subscribeZ2Modal', '#newsprivacy');
        
    });
</script>