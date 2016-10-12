<%@ Control Language="C#" AutoEventWireup="false" CodeFile="WidgetPanel.ascx.cs" Inherits="wctMain.View.Partials.WidgetPanel" %>
<%@ Register src="/View/Partials/CachedTwitterFeed.ascx" tagname="CachedTwitterFeed" tagprefix="uc4" %>

<section id="widgetpanel">

    <% if (Ctx.JustAnnounced != null && Ctx.JustAnnounced.Count > 0)
       { %>
    <section id="justannounced">
        <div class="section-header section-header-top section-header-announced">
            Just Announced
        </div>

        <div class="als-container" id="als-announced">
            <div class="justannounced-listing als-viewport">
            
                <ul class="als-wrapper">
                    <% foreach (Wcss.Show s in Ctx.JustAnnounced)
                       { %>
                        <li class="als-item"><% = 
                            string.Format("<a href=\"/{0}\" onclick=\"setSelection(event)\" ><span class=\"dater\">{1}</span> <span class=\"announcer\">{2}</span></a><div class=\"clearfix\"></div>", 
                            s.FirstShowDate.ConfiguredUrl,
                            s.FirstShowDate.DateOfShow.ToString("MM/dd"),
                            (s.FirstShowDate.MenuBilling != null && s.FirstShowDate.MenuBilling.Trim().Length > 0) ? 
                            Utils.ParseHelper.StripHtmlTags( s.FirstShowDate.MenuBilling.Trim()) : Utils.ParseHelper.StripHtmlTags(s.FirstShowDate.Display.Heads_NoFeatures.Trim()))
                                %>
                        </li>
                    <% } %>
                </ul>
            </div>
        </div>
        <div class="clearfix"></div>

    </section>
    <% } %>
    
    <section id="videoplayer">
        <div class="section-header">
            Featured Artist Videos
        </div>
        <div id="ytplayer" class="iframe-wrapper main-inner">
            <iframe id="Iframe2" type="text/html" width="400" class="img-rounded"                
                src="https://www.youtube.com/embed/?listType=playlist&list=<%=Wcss._Config._YouTubePlaylist %>&fs=1&color=white&theme=dark&modestbranding=1&autohide=1&wmode=transparent" 
                frameborder="0" allowfullscreen></iframe>
        </div>    
    </section>
    
    <section id="widgetbuttons">
        <a id="textuslink" class="section-header" href="/textus.aspx" title="Text us at 720.645.2467 for show info.">            
            <span><strong>Text Us! 720.645.2467</strong></span>
            <span>For show info</span>
        </a>
        
        <a id="maileractivate" class="section-header" href="http://z2ent.com/?utm_source=foxsubscribepage_confirm&utm_medium=website&utm_campaign=subscribeview" title="Join our mailer or manage your email settings.">            
            <span><strong>Join our mailer!</strong></span>
            <span >Get the latest news</span>
        </a>
    </section>

    <section id="radioplayer"> 
        
        <!-- begin DeliRadio pop-out button -->

	    <iframe class="drbutton" src="http://deliradio.net/e/1929?c=1" frameborder="0" width="100%" height="91" scrolling="no"></iframe>
            
	    <!-- begin DeliRadio pop-out button -->
               
    </section>
    
    <section id="twitter">        
        <div class="section-header">
            <a class="follow-link" href="http://www.twitter.com/foxtheatreco/" title="Follow Us On Twitter"><span class="tweet-icon"></span>Follow Us On Twitter</a>
        </div>
        <uc4:CachedTwitterFeed ID="CachedTwitterFeed1" runat="server" Account="foxtheatreco" tweets="16" cachettl="10" />        
    </section>

    <section id="sponsors">
        <a href="http://www.bouldertheater.com" target="_blank" title="Visit The Boulder Theater" ><span class="boulder-theater">Boulder Theater</span></a>
    </section>
</section>

