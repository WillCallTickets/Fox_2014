<%@ Control Language="C#" AutoEventWireup="false" CodeFile="MailerExampleView.ascx.cs" Inherits="z2Main.View.MailerExampleView" %>
    
<div class="mailer-example-container view-container">
    <div class="main-inner">
        <form id="mailerexampleform" action="/mailerexample" runat="server" class="pageform" novalidate>
            <div id="mailerExampleZ2Modal" class="wctmodal" aria-labelledby="mailerExampleModalLabel">
                <label class="sr-only" id="mailerExampleModalLabel">Signup for the Z2 Newsletter</label>
                <div class="newsletter-inner">
                    <h2>Check out our new look!</h2>     
                    <div>
                        <img src="/assets/images/mailtop.jpg" width="300" style="display:block;margin:0 auto;" />
                    </div>
                </div><!-- end inner -->
            </div>
        </form>
    </div>
</div>
