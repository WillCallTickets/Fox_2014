using System;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;
//using wctMain.Controller;

namespace z2Main.View
{
    public partial class ContactView : System.Web.UI.UserControl
   {
       public string url { get; set; }

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

       protected z2Main.Controller.Z2Context _ztx = null;
       protected z2Main.Controller.Z2Context Ztx
       {
           get
           {
               if (_ztx == null)
                   _ztx = new Controller.Z2Context();

               return _ztx;
           }
       }

       protected void Page_Load(object sender, EventArgs e)
       {
           if (!IsPostBack)
               rptContact.DataBind();
       }

       protected void rptContact_DataBinding(object sender, EventArgs e)
       {
           Repeater rpt = (Repeater)sender;
           rpt.DataSource = Ztx.EmployeeListing;
       }

       protected void rptContact_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
       {
           Repeater rpt = (Repeater)sender;

           if (e.Item.ItemType == ListItemType.Item)
           {
               Employee ent = (Employee)e.Item.DataItem;
           }
       }
}
}
