using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SubSonic;
using Wcss;
using wctMain.Controller;

//<input type="text" class="form-control typeahead" placeholder="Search..." id="vd-searchterms" autocomplete="off" />
//<a href="/search" id="vd-sitesearch"><i class="icon-search"></i></a>

namespace wctMain.Admin._venueData._controls
{
    public partial class VenueData_DataActQ : MainBaseControl, IPostBackEventHandler
    {
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            Dictionary<string, string> args = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventArgument);
            string commandName = args["commandName"].ToLower();
            
            switch (commandName)
            {
                case "selectcurrentact":

                    Atx.VDQueryActId = int.Parse(args["idx"]);
                    BindEditorControls();
                    break;
            }
        }

        ///
        ///Boilerplate and postbackevent handler
        ///
        #region Page-Control Overhead

        private int _numshows;
        private int _totalsold;
        private int _totalcomps;
        private int _totaltix;
        private decimal _gross;
        private decimal _bartotal;
        private decimal _barperhead;
        private decimal _concessions;
        private decimal _hospitality;
        private decimal _production;
        private decimal _security;
        private decimal _marketdays;
        private decimal _marketplays;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.Page.User.IsInRole("VenueDataViewer"))
                base.Redirect("/Admin/_venueData/VenueData_Director.aspx?p=vnuentry");

            vdsearchterms.Attributes.Add("placeholder", "Search...");
        }

        void frmMailer_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            workspace.Visible = e.NewMode != FormViewMode.Insert;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected void RefreshCurrent(object sender, EventArgs e)
        {
            BindEditorControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindEditorControls();
        }

        protected void BindEditorControls()
        {
            listCurrentActShows.DataBind();
        }

        #endregion

        #region ListView

        protected void listCurrentActShows_DataBinding(object sender, EventArgs e)
        {
            ListView cont = (ListView)sender;

            _numshows = 0;
            _totalsold = 0;
            _totalcomps = 0;
            _totaltix = 0;
            _gross = 0;
            _bartotal = 0;
            _barperhead = 0;
            _concessions = 0;
            _hospitality = 0;
            _production = 0;
            _security = 0;
            _marketdays = 0;
            _marketplays = 0;

            List<JShowAct> list = new List<JShowAct>();

            if (Atx.VDQueryActId > 0)
                list.AddRange(Atx.VD_CurrentQueryActRecord.JShowActRecords());

            if (list.Count > 1)
                //reverse x and y in the comparison for desc
                list.Sort(delegate(JShowAct x, JShowAct y) { return y.ShowDateRecord.DateOfShow_ToSortBy.CompareTo(x.ShowDateRecord.DateOfShow_ToSortBy); });

            cont.DataKeyNames = new string[] { "Id" };
            cont.DataSource = list;
        }

        protected void listCurrentActShows_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView cont = (ListView)sender;
            JShowAct ent = (e.Item.DataItem != null) ? (JShowAct)e.Item.DataItem : null;

            if (ent != null && cont.EditIndex != -1 && e.Item.DisplayIndex == cont.EditIndex)
            {
                //set the session value for the DataEntryShowRecord!
                //Atx.VD_CurrentDataEntryShowRecord = ent.ShowDateRecord.ShowRecord;

                //Literal litName = (Literal)e.Item.FindControl("litName");

                //if (litName != null)
                //    litName.Text = DisplayShowName(ent);

                //load a control
                DataEntry_Container dataEntry = (DataEntry_Container)e.Item.FindControl("DataEntry_Container1");
                dataEntry.ExplicitBind();

            }
            else if (ent != null && e.Item.ItemType == ListViewItemType.DataItem)
            {
                Literal litItem = (Literal)e.Item.FindControl("litItem");

                if (litItem != null)
                    litItem.Text = ent.SimpleDisplayName;

                Literal litInfo = (Literal)e.Item.FindControl("litInfo");

                if (litInfo != null)
                {
                    VdShowInfo showInfo = ent.ShowDateRecord.ShowRecord.VdShowInfoRecord;

                    if (showInfo == null)
                        litInfo.Text = "There is no show info available. todo: link to data entry.";
                    else
                        litInfo.Text =
                            string.Format("mgr:{0} tix:{1} comps:{2} ttl:{3} fac:{4} gross:{5} bar:{6}/{13} conc:{7} hosp:{8} prod:{9} sec:{10} mdays:{11} mplay:{12}",
                            showInfo.Mod ?? string.Empty,
                            showInfo.TicketsSold.ToString(),
                            showInfo.CompsIn.ToString(),
                            (showInfo.TicketsSold + showInfo.CompsIn).ToString(),
                            showInfo.FacilityFee.ToString("c"),
                            showInfo.TicketGross.ToString("c"),

                            showInfo.BarTotal.ToString("c"),

                            showInfo.Concessions.ToString("c"),
                            showInfo.Hospitality.ToString("c"),
                            showInfo.ProductionLabor.ToString("c"),
                            showInfo.SecurityLabor.ToString("c"),

                            showInfo.MarketingDays.ToString(),
                            showInfo.MarketPlays.ToString(),

                            showInfo.BarPerHead.ToString("c")//13

                            );

                    _numshows++;
                    _totalsold += showInfo.TicketsSold;
                    _totalcomps += showInfo.CompsIn;
                    _totaltix += (showInfo.TicketsSold + showInfo.CompsIn);
                    _gross = showInfo.TicketGross;
                    _bartotal += showInfo.BarTotal;
                    _barperhead += showInfo.BarPerHead;
                    _concessions += showInfo.Concessions;
                    _hospitality += showInfo.Hospitality;
                    _production += showInfo.ProductionLabor;
                    _security += showInfo.SecurityLabor;
                    _marketdays += showInfo.MarketingDays;
                    _marketplays += showInfo.MarketPlays;
                }
            }
        }

        protected void listCurrentActShows_DataBound(object sender, EventArgs e)
        {
            ListView cont = (ListView)sender;

            if (cont.EditIndex == -1)
            {

                Literal litName = (Literal)cont.FindControl("litName");
                Literal litAggs = (Literal)cont.FindControl("litAggs");

                if (litName != null && litAggs != null)
                {
                    litName.Text = Atx.VD_CurrentQueryActRecord.Name_Displayable;

                    litAggs.Text =
                        string.Format("shows:{22} tix:{0}/{1} comps:{2}/{3} ttl:{4}/{5} gross:{6}/{7} bar:{8}/{9}/{23}/{24} conc:{10}/{11} hosp:{12}/{13} prod:{14}/{15} sec:{16}/{17} mdays:{18}/{19} mplay:{20}/{21}",

                        _totalsold.ToString(),       //0                     
                        decimal.Round((_totalsold / _numshows), 2).ToString(),//1
                        _totalcomps.ToString(),//2
                        decimal.Round((_totalcomps / _numshows), 2).ToString(),//3

                        _totaltix.ToString(),//4
                        decimal.Round((_totaltix / _numshows), 2).ToString(),//5
                        _gross.ToString("c"),//6
                        decimal.Round((_gross / _numshows), 2).ToString(),//7
                        _bartotal.ToString("c"),//8
                        decimal.Round((_bartotal / _numshows), 2).ToString("c"),//9

                        _concessions.ToString("c"),
                        decimal.Round((_concessions / _numshows), 2).ToString("c"),
                        _hospitality.ToString("c"),
                        decimal.Round((_hospitality / _numshows), 2).ToString("c"), //13

                        _production.ToString("c"),
                        decimal.Round((_production / _numshows), 2).ToString("c"),
                        _security.ToString("c"),
                        decimal.Round((_security / _numshows), 2).ToString("c"),
                        _marketdays.ToString(),
                        decimal.Round((_marketdays / _numshows), 2).ToString(),
                        _marketplays.ToString(),
                        decimal.Round((_marketplays / _numshows), 2).ToString(),

                        _numshows.ToString(),
                        _barperhead.ToString("c"),//23
                        decimal.Round((_barperhead / _numshows), 2).ToString("c")//24
                        );
                }
            }
        }

        protected void listCurrentActShows_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView list = (ListView)sender;
            list.EditIndex = e.NewEditIndex;

            //get key and assign current
            int idx = (int)list.DataKeys[list.EditIndex].Value;
            JShowAct ent = Atx.VD_CurrentQueryActRecord.JShowActRecords().GetList()
                .Find(delegate(JShowAct match) { return (match.Id == idx); });
            Atx.VD_CurrentDataEntryShowRecord = (ent != null) ? ent.ShowDateRecord.ShowRecord : null;

            list.DataBind();
        }

        protected void listCurrentActShows_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            ListView list = (ListView)sender;

            if (list.EditIndex != -1)
                list.EditIndex = -1;

            list.DataBind();
        }

        #endregion
}
}
