using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text.RegularExpressions;
using System.Net;

using Wcss;

namespace wctMain.Service.Admin
{
    /// <summary>
    /// Summary description for AdminServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AdminServices : System.Web.Services.WebService
    {

        public AdminServices() { }

        [WebMethod]
        public void PublishButton()
        {
            wctMain.Admin.AdminContext atx = new wctMain.Admin.AdminContext();

            atx.PublishSite();
        }

        [WebMethod]
        public string AddMailerShows(int mailerContentId, string foxShowList, string betShowList, string chqShowList)
        {
            string retVal = string.Empty;

            if (mailerContentId > 0)
            {
                MailerContent mc = new MailerContent(mailerContentId);

                if (mc.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.mailershow)
                {
                    //create a list of shows
                    List<MailerShow> list = new List<MailerShow>();

                    if (foxShowList != null && foxShowList.Trim().Length > 0)
                    {
                        string[] ids = foxShowList.Split(',');
                        foreach (string s in ids)
                        {
                            if (s == "0")
                                list.Add(new MailerShow(list.Count, "The Fox Theatre", "GENERIC FOX EVENT"));
                            else
                            {
                                MailerShow ms = new MailerShow(list.Count, new Show(int.Parse(s)));
                                list.Add(ms);
                            }
                        }
                    }

                    if (betShowList != null && betShowList.Trim().Length > 0)
                    {
                        wctMain.Admin.AdminContext _atx = new wctMain.Admin.AdminContext();

                        string[] ids = betShowList.Split(',');
                        foreach (string s in ids)
                        {
                            BT_EventItem bt = _atx.BT_EventItem_Listing.Find(delegate(BT_EventItem match) { return (match.Id.ToString() == s); });
                            //BT_EventItem bt = _atx.BT_EventItem_Listing.Find(delegate(BT_EventItem match) { return (match.DateAndTitleForListing == s); });

                            if (bt != null)
                            {
                                MailerShow ms = new MailerShow(list.Count, bt);
                                list.Add(ms);
                            }
                            else
                                list.Add(new MailerShow(list.Count, "The Boulder Theater", "GENERIC BT EVENT"));
                        }
                    }

                    if (chqShowList != null && chqShowList.Trim().Length > 0)
                    {
                        string[] ids = chqShowList.Split(',');
                        foreach (string s in ids)
                        {
                            if (s == "0")
                                list.Add(new MailerShow(list.Count, "Chautauqua Aditorium", "GENERIC CHAUTAUQUA EVENT"));
                            else
                            {
                                MailerShow ms = new MailerShow(list.Count, new Show(int.Parse(s)));
                                list.Add(ms);
                            }
                        }
                    }

                    //if we have a list - add it into the mailerContents content
                    if (list.Count > 0)
                    {
                        if (mc != null)
                        {
                            retVal = mc.AddBulkMailerShows(list);
                        }
                        else
                        {
                            retVal = string.Format("ERROR: MailerContent not found for id: {0}", mailerContentId.ToString());
                        }
                    }
                    else
                        retVal = string.Format("ERROR: no list items were selected");
                }
                else
                    retVal = string.Format("ERROR: MailerContent is not of asset type [{0}].", MailerTemplateContent.ContentAsset.mailershow.ToString());
            }

            if (retVal.Trim().Length == 0)
                retVal = string.Format("ERROR: invalid MailerContentId provided: {0}. Content not found", mailerContentId.ToString());

            return retVal;
        }

        [WebMethod]
        public string AddMailerBanners(int mailerContentId, string bannerList)
        {
            string retVal = string.Empty;

            if (mailerContentId > 0)
            {
                MailerContent mc = new MailerContent(mailerContentId);

                if (mc.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.bannerimages)
                {
                    //create a list of shows
                    List<MailerBanner> list = new List<MailerBanner>();

                    if (bannerList != null && bannerList.Trim().Length > 0)
                    {
                        string[] ids = bannerList.Split(',');
                        foreach (string s in ids)
                            list.Add(new MailerBanner(list.Count, new SalePromotion(int.Parse(s))));
                    }

                    //if we have alist - add it into the mailerContents content
                    if (list.Count > 0)
                    {
                        if (mc != null)
                        {
                            retVal = mc.AddBulkMailerBanners(list);
                            
                        }
                        else
                        {
                            retVal = string.Format("ERROR: MailerContent not found for id: {0}", mailerContentId.ToString());
                        }
                    }
                    else
                        retVal = string.Format("ERROR: no list items were selected");
                }
                else
                    retVal = string.Format("ERROR: MailerContent is not of asset type [{0}].", MailerTemplateContent.ContentAsset.bannerimages.ToString());
            }

            if (retVal.Trim().Length == 0)
                retVal = string.Format("ERROR: invalid MailerContentId provided: {0}. Content not found", mailerContentId.ToString());

            return retVal;
        }
    }
}