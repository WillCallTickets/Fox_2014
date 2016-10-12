using System;
using System.Xml;

using Wcss;

public partial class Maintenance : System.Web.UI.Page
{
    protected override void OnPreInit(EventArgs e)
    {
        //QualifySsl(false);
        base.OnPreInit(e);
        this.Theme = string.Empty;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //get the page from the resources dir
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;

            try
            {
                string mappedPath = Server.MapPath(string.Format("/WillCallResources/Html/Maintenance.html"));
                doc.Load(mappedPath);
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                return;
            }

            XmlNodeList xlist = doc.GetElementsByTagName("title");
            if (xlist.Count > 0)
            {
                string title = xlist[0].InnerXml;
                this.Page.Title = title;
            }

            XmlNodeList xbody = doc.GetElementsByTagName("body");
            if (xbody.Count > 0)
            {
                string body = xbody[0].InnerXml;
                this.litBody.Text = body;
            }
        }
    }
}
