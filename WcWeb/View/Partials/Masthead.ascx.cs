using System;
using System.Web.UI;

using Wcss;

namespace wctMain.View.Partials
{
    [ToolboxData("<{0}:Masthead runat='server'></{0}:Masthead>")]
    public partial class Masthead : wctMain.Controller.MainBaseControl
    { }
}


/*
 * <a href="http://www.flickr.com/photos/foxtheatre/" ><img src="/assets/images/social/Flickr-64.png" alt="Flickr" /></a>  
 * 
 * 
<div class="navbar-form navbar-right">
                    <div class="form-group">
                        <input type="text" class="form-control typeahead" placeholder="Search..." id="ta-searchterms" />
                        <a href="/search" id="ta-sitesearch"><i class="icon-search"></i></a>
                    </div>
                </div>
 * 
 * 
 * <div class="form-group form-search" role="search">
                        <input type="text" class="form-control typeahead" placeholder="Search..." id="ta-searchterms" />
                        <a href="/search" id="ta-sitesearch"><i class="icon-search"></i></a>
                    </div>
*/