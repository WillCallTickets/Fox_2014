<%@ Control Language="C#" AutoEventWireup="false" CodeFile="MailerManageView.ascx.cs" Inherits="wctMain.View.MailerManageView" %>

<div class="displayer fade fade-slow mailerconfirm-container static-container">
    <div class="section-header">Mailer Manager
        <% if(context.ToLower() == "mobile"){ %>
            <a class="btn btn-primary btn-xs pull-right lammie-close" id="laminate-close-top" href="/" title="Close">Close</a>
        <%} %>
    </div>
    <form id="mailermanagerform" action="mailermanager" class="main-inner" novalidate>
        <div id="mailerManagerModal" class="wctmodal" aria-labelledby="mailerManagerModalLabel">
            <div class="wctmodal-header">      
                <br />          
                <p>Select an option to manage your Fox Theatre email.</p>
            </div>
            
            <div class="wctmodal-success wctmodal-response"></div>
            <div class="wctmodal-error wctmodal-response"></div>

            <div class="wctmodal-body form-horizontal">
                <br />
                <p class="intent">
                    Did you intend to <a id="unsubscribe_subscribe" class="link-foxt" href="/subscribe">subscribe?</a>
                </p>
                <br />
                <p class="intent">
                    Or did you intend to <a id="subscribe_unsubscribe" class="link-foxt" href="/unsubscribe">unsubscribe?</a>
                </p>
                <br />
            </div>
        </div>
    </form>
</div>

<script type="text/javascript">

    $(document).ready(function () {

        $('#subscribe_unsubscribe').add('#unsubscribe_subscribe').attr('data-bind', 'click: loadStaticControl').address();
    });

</script>