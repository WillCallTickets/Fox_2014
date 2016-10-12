using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WctControls.WebControls
{
    [ToolboxData("<{0}:DateTimeFormView runat='Server' SelectedDate='' ></{0}:DateTimeFormView>")]
    [DefaultPropertyAttribute("SelectedDate")]
    [ValidationProperty("SelectedDate")]
    public class DateTimeFormView : FormView
    {
        DateTime SelectedDate { get; set; }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            List<DateTime> list = new List<DateTime>();
            
            if(SelectedDate != null && SelectedDate != DateTime.MinValue && SelectedDate != DateTime.MaxValue)
                list.Add(SelectedDate);

            this.DataSource = list;
        }

        protected override void OnDataBound(EventArgs e)
        {
            base.OnDataBound(e);


        }
    }
}
