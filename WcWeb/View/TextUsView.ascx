<%@ Control Language="C#" AutoEventWireup="false" CodeFile="TextUsView.ascx.cs" Inherits="wctMain.View.TextUsView" %>

<div class="displayer fade fade-slow textus-container static-container">
    <div class="section-header">Text Us!</div>
    <div class="main-inner">
        <div class="locality" style="text-align:center;">Do you have a quick question?</div>
        <br />
        <div style="font-size:32px;text-align:center;font-weight:normal;">
            Click Here To
        </div>
        <div style="text-align:center;">
            <a id="widget-subnav" href="#" title="Text Us!" style="border:none;outline:none;display:inline-block;">
                <img src="/assets/images/Fox-TextUsButton-380.png" width="100%" style="border:none;outline:none;max-width:360px;" />
            </a>
        </div>
        <br /><br />
        <div>Text us at <a href="tel:+7206452467">720.645.2467</a> during normal <a class="contact-link" href="/contact/boxoffices">box office hours.</a></div>
        <br />
        <div>Please be patient, we may be busy tending to customers waiting in line, but will get back to you as soon as we can.</div>
        
        <div id="text-us-placeholder" style="visibility:hidden;height:10px;"></div>
    </div>
</div>
<script src="http://app.textus.biz/widget/1/1ulrQ_BOl5lN5pF9qxptvTwbdvU/embedded.js?textus-type=button"></script>
<script type="text/javascript">


    $(document).ready(function () {
        $('#widget-subnav').on('click', function () {
            $('#text-us-placeholder').trigger('click');
        });
    });

</script>