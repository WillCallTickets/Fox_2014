<%@ Control Language="C#" AutoEventWireup="false" CodeFile="MailerConfirmView.ascx.cs" Inherits="z2Main.View.MailerConfirmView" %>

<div class="mailerconfirm-container view-container">
    <div class="main-inner">
        <form id="mailerconfirmform" action="mailerconfirm" class="main-inner" novalidate>
            <div id="mailerConfirmModal" class="wctmodal" aria-labelledby="mailerConfirmModalLabel">
                <div class="section-header">
                    <h2 id="mailerConfirmModalLabel">Z2ent Newsletter Confirmation</h2>                    
                </div>
            
                <h4 style="color:white;">You are confirming the email <%=_context %></h4>
                <p>&nbsp;</p>

                <div class="wctmodal-success wctmodal-response" style="font-size:20px;"></div>
                <div class="wctmodal-error wctmodal-response"></div>

                <div class="wctmodal-body">
                    <p>
                        <button type="button" class="btn btn-lg btn-danger wct-modal-action btn-pad-wide" data-toggle="button" 
                            data-loading-text="<span class='wct-modal-spinner'></span>Processing...">Click Here To Confirm Your Subscription</button>
                    </p>
                    <p>&nbsp;</p>
                    <p>If you are having problems, please <a id="unsubscribe_contact" href="mailto:<%=z2Main.Controller.Z2Config.CUSTOMERSERVICEEMAIL_Z2 %>">contact us</a>.</p>  
                </div>
            </div>
            <input id="hdnConfirmId" type="hidden" value="<%=_context %>" />
            <input id="hdnUserIp" type="hidden" value="<%=this.Request.UserHostAddress %>" />
        </form>
    </div>
</div>

<script type="text/javascript">

    $(document).ready(function () {

        $('#mailerConfirmModal').z2Modal(
            '/Handler/EmailSubscriptionRequest',
            //define success
            z2_mailerConfirmSuccess,
            //define inputs
            [''],
            z2_mailerConfirmParamBuilder
            );

        //automatically confirm request
        $('#mailerConfirmModal .wct-modal-action').click();
    });

</script>