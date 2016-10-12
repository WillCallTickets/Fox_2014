using System;

using Wcss;
using wctMain.Controller;

namespace wctMain.View
{
    public partial class SubscribeView : MainBaseControl
    {
        public string context { get; set; }

        protected string _profile
        {
            get
            {
                if (this.Page.User.Identity.Name.ToLower().IndexOf("anonymous") == -1 && 
                    this.Page.User.Identity.Name.Contains("@") && 
                    this.Page.User.Identity.IsAuthenticated)
                    return this.Page.User.Identity.Name;

                return string.Empty;
            }
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
            //MainContext Ctx = new MainContext();
            if (context == null)
                context = string.Empty;

            if (!IsPostBack)
                BindControls();
        }

        public void BindControls()
        {
            
        }      
    }
}


/*
 * 

<script type="text/javascript">


    $(document).ready(function () {

        //alert('modal');
        $('#subscribeModal').wctModal(
            'mailerSubscribe',
            //define success
            wct_subscribeSuccess,
            //define inputs
            ['#subscribeEmail'],
            wct_subscribeParamBuilder
            );
        
        
    });

</script>*/
