using System;
using System.Web;

namespace z2Main
{
    public partial class index : z2Main.Controller.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //private void Output_EventDateModel_Json()
        //{
        //    string data = Ctx.WebData_Json;

        //    //output to content holder
        //    ContentPlaceHolder cont = (ContentPlaceHolder)this.Page.Master.FindControl("JsonObjects");
        //    if (cont != null)
        //    {
        //        cont.Controls.Clear();
        //        cont.Controls.Add(new LiteralControl(data));
        //    }
        //}

    }
}