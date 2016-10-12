using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web;
using System.Text;
using System.Linq;

using Wcss;

// <%@ OutputCache Duration="500" VaryByParam="none" %>


namespace wctMain
{
    public partial class Json_UpcomingShows : wctMain.Controller.MainBasePage
    {
        protected string _JSONtext = string.Empty;

        protected override void OnPreInit(EventArgs e)
        {
            this.Theme = string.Empty;
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
            Response.ContentEncoding = Encoding.UTF8;
                        
            ConstructJSON();

            Response.Write(_JSONtext);
        }
        
        public void ConstructJSON()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("{");

            string jsonReduce = Utils.ParseHelper.ReturnJSONFormat("lastBuildDate", 
                Utils.ParseHelper.JavascriptDate_To_DateTime(Ctx.PublishVersion_Announced).ToString("MM-dd-yyyy HH:mm:ss"));

            sb.AppendFormat("{0}", jsonReduce);
          
            sb.Append("}");
            
            _JSONtext = sb.ToString();
        }
}
}
