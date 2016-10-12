using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using Wcss;

namespace wctMain.Model
{
    /// <summary>
    /// Summary description for GoogleProp
    /// http://tools.ranksider.com/rich-snippets/event
    /// </summary>
    public class GoogleProp
    {
        public class EventSchema
        {
            public StringBuilder sb = new StringBuilder();
            public string url { get; set; }
            public string summary { get; set; }
            public string description { get; set; }
            //public string photo { get; set; }
            public string startDate { get; set; }
            public string localDate { get; set; }
            //public string eventType { get; set; }//festival, concert, lecture
            public string price { get; set; }
            public string ticketurl { get; set; }
            public OrganizationSchema org { get; set; }

            public EventSchema(Show s)
            {
                url = s.FirstShowDate.ConfiguredUrl_withDomain;
                summary = s.Name ?? string.Empty;
                description = s.Display.DisplaySocial_Description.Trim();
                startDate = s.FirstShowDate.DateOfShow_ToSortBy.ToUniversalTime().ToString("yyyy-MM-ddTHHmmss");
                localDate = s.FirstShowDate.DateOfShow_ToSortBy.ToString("MMM dd, hh:mmtt");
                price = s.FirstShowDate.PricingText ?? string.Empty;
                ticketurl = (s.FirstShowDate.TicketUrl != null) ? s.FirstShowDate.TicketUrl.Trim() : string.Empty;                

                org = new OrganizationSchema(s.VenueRecord);
            }
            
            public string WriteToFormat(string format, bool includeHeader, bool includeOrg, bool includeOrgHeader)
            {
                sb.Length = 0;//reset

                if(includeHeader)
                    sb.Append("<div itemscope itemtype=\"http://data-vocabulary.org/Event\">").AppendLine();

                sb.AppendFormat("​<a href=\"{0}\" itemprop=\"url\" >", url);

                sb.AppendLine();
                if (summary.Trim().Length > 0)
                {
                    sb.AppendFormat("<span itemprop=\"name\">{0}</span>", summary.Trim());
                    sb.AppendLine();
                }
                sb.AppendLine("</a>");
                //if (photo.Trim().Length > 0)
                //{
                //    sb.AppendFormat("<img itemprop=\"photo\" src=\"{0}\" />", photo);
                //    sb.AppendLine();
                //}
                if (description.Trim().Length > 0)
                {
                    sb.AppendFormat("<span itemprop=\"description\">{0}</span>", description);
                    sb.AppendLine();
                }
                if (startDate.Trim().Length > 0)
                {
                    sb.AppendFormat("<time itemprop=\"startDate\" datetime=\"{0}\">{1}</time>", startDate, localDate);
                    sb.AppendLine();
                }

                if (includeOrg)
                    sb.AppendLine(org.WriteToFormat(format, includeOrgHeader));

                if (ticketurl.Trim().Length > 0 || price.Trim().Length > 0)
                {
                    //sb.AppendFormat("<span itemprop=\"eventType\">{0}</span>", eventType);
                    sb.AppendLine();
                    sb.AppendLine("<span itemprop=\"tickets\" itemscope itemtype=\"http://schema.org/Offer\">");
                    if (ticketurl.Trim().Length > 0)
                    {
                        sb.AppendFormat("<a href=\"{0}\" itemprop=\"url\">{1}</a>", ticketurl, localDate);
                        sb.AppendLine();
                    }
                    if (price.Trim().Length > 0)
                    {
                        sb.AppendFormat("<span itemprop=\"price\">{0}</span>", price);
                        sb.AppendLine();
                    }
                    sb.AppendLine("</span>");//end offer
                }

                if (includeHeader)
                    sb.AppendLine("</div>");//end Event
                
                return sb.ToString();
            }
        }

        public class OrganizationSchema
        {
            public StringBuilder sb = new StringBuilder();            
            public string name { get; set; }
            public string url { get; set; }
            public string streetAddress { get; set; }
            public string locality { get; set; }
            public string region { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string telephone { get; set; }
            //public string logo { get; set; }
            private bool includeHeader { get; set; }

            public OrganizationSchema(Venue v)
            {
                name = v.Name_Displayable.Trim();
                url = v.Website_Configured;
                streetAddress = v.Address ?? string.Empty;
                locality = v.City ?? string.Empty;
                region = v.State ?? string.Empty;
                latitude = v.Latitude ?? string.Empty;
                longitude = v.Longitude ?? string.Empty;

                telephone = string.Empty;                
                string boxoffice = v.BoxOfficePhone;
                if (boxoffice != null && boxoffice.Trim().Length > 0)
                    telephone = String.Format("box ofc: {0}{1}",
                        boxoffice.Trim(),
                        (v.BoxOfficePhoneExt != null && v.BoxOfficePhoneExt.Trim().Length > 0) ?
                            string.Format(" ext: {0}", v.BoxOfficePhoneExt.Trim()) : string.Empty);

                string mainoffice = v.MainPhone;
                if (mainoffice != null && mainoffice.Trim().Length > 0)
                    telephone += String.Format("{0}main ofc: {1}{2}",
                        (telephone.Length > 0) ? string.Format("  -  ") : string.Empty,
                        mainoffice.Trim(),
                        (v.MainPhoneExt != null && v.MainPhoneExt.Trim().Length > 0) ?
                            string.Format(" ext: {0}", v.MainPhoneExt.Trim()) : string.Empty);

                //logo = (v.PictureUrl != null && v.PictureUrl.Trim().Length > 0) ? v.ImageManager.Thumbnail_Max.Trim() : string.Empty;
            }

            public string WriteToFormat(string format, bool includeHeader)
            {
                sb.Length = 0;

                if (includeHeader)
                    sb.AppendLine("<div style=\"display:none;\" itemscope itemtype=\"http://data-vocabulary.org/Organization\">");

                if (url.Trim().Length > 0)
                    sb.AppendFormat("​<a href=\"{0}\" itemprop=\"url\" >", url).AppendLine();
                sb.AppendFormat("​<span itemprop=\"name\" >{0}</span>", name).AppendLine();
                if (url.Trim().Length > 0)
                    sb.AppendLine("</a>");

                //if (logo.Trim().Length > 0)
                //    sb.AppendFormat("<img itemprop=\"logo\" src=\"{0}\" />", logo).AppendLine();
                if (telephone.Trim().Length > 0)
                    sb.AppendFormat("<span itemprop=\"telephone\">{0}</span>", telephone).AppendLine();

                sb.AppendLine("<span itemprop=\"address\" itemscope itemtype=\"http://data-vocabulary.org/Address\">");
                if (streetAddress.Trim().Length > 0)
                    sb.AppendFormat("<span itemprop=\"streetAddress\">{0}</span>, ", streetAddress).AppendLine();
                if (locality.Trim().Length > 0)
                    sb.AppendFormat("<span itemprop=\"addressLocality\">{0}</span>, ", locality).AppendLine();
                if (region.Trim().Length > 0)
                    sb.AppendFormat("<span itemprop=\"addressRegion\">{0}</span>", region).AppendLine();
                
                sb.AppendLine("</span>");//end address

                if (latitude.Trim().Length > 0 || longitude.Trim().Length > 0)
                {
                    sb.AppendLine("<span itemprop=\"geo\" itemscope itemtype=\"http://data-vocabulary.org/​Geo\">");
                    if (latitude.Trim().Length > 0)
                        sb.AppendFormat("<meta itemprop=\"latitude\" content=\"{0}\" />", latitude).AppendLine();
                    if (longitude.Trim().Length > 0)
                        sb.AppendFormat("<meta itemprop=\"longitude\" content=\"{0}\" />", longitude).AppendLine();
                    sb.AppendLine("</span>");//end geo
                }

                if (includeHeader)
                    sb.AppendLine("</div>");//end organization

                return sb.ToString();
            }
        }

        public GoogleProp()
        {
            //
            // TODO: Add constructor logic here
            //
        }        
    }
}
