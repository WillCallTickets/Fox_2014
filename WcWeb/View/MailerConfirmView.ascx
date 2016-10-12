<%@ Control Language="C#" AutoEventWireup="false" CodeFile="MailerConfirmView.ascx.cs" Inherits="wctMain.View.MailerConfirmView" %>

<div class="displayer fade fade-slow mailerconfirm-container static-container">
    <div class="section-header">Mailer Confirmation</div>
    <form id="mailerconfirmform" action="mailerconfirm" class="main-inner" novalidate>
        <div id="mailerConfirmModal" class="wctmodal" aria-labelledby="mailerConfirmModalLabel">
            <div class="wctmodal-header">      
                <br />          
                <p>You are confirming a subscription to Fox Theatre Boulder, CO.</p>
            </div>
            
            <div class="wctmodal-success wctmodal-response"></div>
            <div class="wctmodal-error wctmodal-response"></div>

            <div class="wctmodal-body form-horizontal">
                <div class="form-group">
                    <button type="button" class="btn btn-foxt wct-modal-action" data-toggle="button" 
                        data-loading-text="<span class='wct-modal-spinner'></span>Processing...">Confirm</button>
                </div>
            </div>
        </div>
        <input id="hdnConfirmId" type="hidden" value="<%=_context %>" />
        <input id="hdnUserIp" type="hidden" value="<%=this.Request.UserHostAddress %>" />
    </form>
</div>

<script type="text/javascript">

    $(document).ready(function () {

        $('#mailerConfirmModal').wctModal(
            'mailerConfirm',
            //define success
            wct_mailerConfirmSuccess,
            //define inputs
            [''],
            wct_mailerConfirmParamBuilder
            );

        $('#mailerConfirmModal .wct-modal-action').click();
    });

</script>