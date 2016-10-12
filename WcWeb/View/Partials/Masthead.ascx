<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Masthead.ascx.cs" Inherits="wctMain.View.Partials.Masthead" %>

<header id="mastheader">

    <nav class="navbar navbar-fixed-top navbar-inverse" role="navigation">
        <div class="navbar-background"></div>
        <div class="container-fluid">
            <!-- Brand and toggle get grouped for better mobile display -->
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#navbar-content">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a id="linkhome" class="navbar-brand" href="/" title="The Fox Theatre"><img id="logo" src="/assets/images/foxthneon100.png" alt="Fox" /></a>
                <div id="brand-name" class="brandname">
                    <span class="grad"></span>
                    <span class="thewho">The Fox Theatre</span> <img src="/assets/images/globe55bw.png" border="0" alt="" /> Boulder CO
                </div>
            </div><!-- /.container -->
            <div class="collapse navbar-collapse" id="navbar-content">
                <div class="navbar-links">
                    <ul class="nav navbar-nav <%if //((true ) || 
                            (this.Page.User.IsInRole("Manifester") || this.Page.User.IsInRole("OrderFiller") ||
                            this.Page.User.IsInRole("Super") || this.Page.User.IsInRole("Master") || this.Page.User.IsInRole("Administrator") ||
                        this.Page.User.IsInRole("ContentEditor") || this.Page.User.IsInRole("MassMailer") || this.Page.User.IsInRole("ReportViewer"))
                                                //)
                                                { %> navbar-admin<%} %>">
                        <li class="dropdown">
                            <a href="#" id="aboutdropdown" class="dropdown-toggle" data-toggle="dropdown">About Us <b class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li><a href="/about.aspx">About Us</a></li>
                                
                                <li><a href="/history.aspx">History</a></li>                                
                                <li><a href="/accommodations.aspx">Accommodations</a></li>
                                
                                <li><a href="/privacy">Privacy Notice</a></li>
                                <!--
                                <li class="divider"></li>
                                
                                <li><a href="/parking.aspx">Parking</a></li>
                                <li><a href="/production.aspx">Production</a></li>
                                <li class="divider"></li>-->
                                
                            </ul>
                        </li>
                        <li data-bind="css: { active: $data.staticValue() == 'faq' }"><a href="/faq.aspx?p=general">FAQ</a></li>
                        <li data-bind="css: { active: $data.staticValue() == 'contact' }"><a href="/contact.aspx?p=officeinfo">Contact</a></li>
                        <li data-bind="css: { active: $data.staticValue() == 'directions' }"><a href="/directions.aspx">Directions</a></li>
                        <li data-bind="css: { active: $data.staticValue() == 'subscribe' || $data.staticValue() == 'unsubscribe' || $data.staticValue() == 'mailermanage' }">
                            <a href="/subscribe.aspx">Mailer</a>
                        </li>
                        <%if //((true ) || 
                                (this.Page.User.IsInRole("Manifester") || this.Page.User.IsInRole("OrderFiller") ||
                                this.Page.User.IsInRole("Super") || this.Page.User.IsInRole("Master") || this.Page.User.IsInRole("Administrator") ||
                            this.Page.User.IsInRole("ContentEditor") || this.Page.User.IsInRole("MassMailer") || this.Page.User.IsInRole("ReportViewer"))
                          //)
                          { %>
                        <li class="dropdown">
                            <a id="admindropdown" href="#" class="dropdown-toggle" data-toggle="dropdown">Admin <b class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li><a href="/admin">Admin</a></li>
                                <!--<li class="divider"></li>-->
                                <li><a class="logout" href="/logout">Logout</a></li>
                            </ul>
                        </li>
                        <%} %>
                    </ul>
                    
                    <div class="form-group form-search" role="search">
                        <input type="text" class="form-control typeahead" placeholder="Search..." id="ta-searchterms" autocomplete="off" />
                        <a href="/search" id="ta-sitesearch"><i class="icon-search"></i></a>
                    </div>
                </div>
                <div id="site-social">
                    <a href="http://www.facebook.com/foxtheatreboulder" ><img src="/assets/images/social/Facebook-64.png" alt="Facebook" /></a>                    
                    <a href="http://twitter.com/foxtheatreco" ><img src="/assets/images/social/Twitter-64.png" alt="Twitter" /></a>                    
                    <a href="http://plus.google.com/+TheFoxTheatreBoulder/about" ><img src="/assets/images/social/GooglePlus-64.png" alt="Google" /></a>                    
                    <a href="http://instagram.com/foxtheatreco" ><img src="/assets/images/social/Instagram-64.png" alt="Instagram" /></a>                    
                                      
                    <a href="/Rss.aspx" ><img src="/assets/images/social/_Misc_RSS-64.png" alt="Rss" /></a>
                </div>
            </div><!-- /.nav-collapse -->
        </div><!-- /.container -->
    </nav><!-- /.navbar  -->
    <div class="clearfix"></div>
</header>