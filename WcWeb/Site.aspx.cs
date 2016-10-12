using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace wctMain
{
    /// <summary>
    /// This page exists to handle any legacy aspx requests. These types of requests should be gone by now.
    /// This will handle old link and no-js requests
    /// </summary>
    public partial class Site : wctMain.Controller.MainBasePage
    {
        private string _siteintention = null;
        public string SiteIntention
        {
            get
            {
                if (_siteintention == null)
                {
                    _siteintention = string.Empty;

                    object staticPage = this.RouteData.Values["staticpage"];
                    
                    if (staticPage != null)
                    {
                        _Enums.StaticPage sPage;

                        try
                        {
                            string stat = staticPage.ToString().ToLower();

                            //handle legacy login pages
                            switch (stat)
                            {
                                //case "index2":
                                case "register":
                                case "editprofile":
                                case "passwordrecovery":
                                    _siteintention = stat;
                                    break;
                                default:
                                    sPage = (_Enums.StaticPage)Enum.Parse(typeof(_Enums.StaticPage), stat, true);
                                    _siteintention = sPage.ToString();
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            //TODO: display 404 - redirect to default home page
                            throw new ArgumentOutOfRangeException(string.Format("{0} is not found", staticPage));
                        }
                    }
                }

                return _siteintention;
            }
        }

        #region Page Loop and Main Logic

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            QualifySsl(false);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        public override void Dispose()
        {
            this.Load -= new System.EventHandler(this.Page_Load);
            base.Dispose();
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (this.SiteIntention.Trim().Length > 0)
            {
                string query = string.Empty;

                foreach (string s in Request.QueryString.Keys)
                {
                    string[] val = Request.QueryString.GetValues(s);

                    //prevent
                    //p=..%2f..%2f..%2f..%2f..%2f..%2f..%2f..%
                    if (val[0] != null && (val[0].IndexOf("../") != -1 || val[0].IndexOf("/..") != -1
                        || val[0].ToLower().IndexOf("..%2f") != -1 || val[0].ToLower().IndexOf("%2f..") != -1)
                        )
                    {
                        Response.Redirect("/");
                    }

                    query += string.Format("{0}", val);
                }

                //handle legacy login
                if (SiteIntention == "register")
                {
                    Response.Redirect(string.Format("/Admin/{0}.aspx", SiteIntention), true);
                }
                else if(SiteIntention == "editprofile" || SiteIntention == "passwordrecovery")
                {
                    Response.Redirect(string.Format("/Admin/ControlsFT/{0}.aspx", SiteIntention, query), true);
                }
                else
                {
                    //convert to enum
                    _Enums.StaticPage sPage = (_Enums.StaticPage)Enum.Parse(typeof(_Enums.StaticPage), this.SiteIntention, true);

                    //handle legacy pages
                    //load control based on staticPage
                    switch (sPage)
                    {
                        case _Enums.StaticPage.contact:
                        case _Enums.StaticPage.directions:
                        case _Enums.StaticPage.faq:
                        case _Enums.StaticPage.mailerconfirm:
                        case _Enums.StaticPage.subscribe:
                        case _Enums.StaticPage.unsubscribe:
                        case _Enums.StaticPage.mailermanage:
                            Response.Redirect(string.Format("/{0}/{1}", sPage, query), true);
                            break;
                        default:

                            break;
                    }
                }
            }
        }

        #endregion
    }
}