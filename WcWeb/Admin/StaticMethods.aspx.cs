using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Linq;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin
{
    public partial class StaticMethods : MainBasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //List<BTEvent> _BTEventList = Atx.BTEventList;

            //foreach (BTEvent bte in _BTEventList)
            //{
            //}

        }









        protected void btnSendTestMail_Click(object sender, EventArgs e)
        {
            MailQueue.SendEmail("rob@robkurtz.net", "rob", "rkurtz@willcalltickets.com", "", "", "testing email from solution", "some body stuff", "a text version",
                null, true, null);

            //_Error.LogException(new Exception("blah blah blah"));
        }
        protected void btnTest_Click(object sender, EventArgs e)
        {
            _Error.LogException(new Exception("blah blah blah"));
        }
        protected void btnFB_Click(object sender, EventArgs e)
        {
            _Config.TestFacebook();
        }
        protected void btnRebuildShowThumbs_Click(object sender, EventArgs e)
        {
            //get a collection of past shows and loop thru
            ShowCollection shows = new ShowCollection();

            string sql = "SELECT TOP 500 s.* FROM [Show] s ORDER BY ID DESC ";
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            shows.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            System.Text.StringBuilder errors = new System.Text.StringBuilder();

            try
            {
                foreach (Show s in shows)
                {
                    if (s.DisplayUrl != null && s.DisplayUrl.Trim().Length > 0)
                    {
                        _ImageManager imp = s.ImageManager;
                        imp.CreateAllThumbs();
                    }
                }
            }
            catch (Exception ex)
            {
                errors.AppendFormat("{0}", ex.Message);
                errors.AppendLine();
            }


            if (errors.Length > 0)
                Literal4.Text = errors.ToString();
        }
        protected void btnPast_Click(object sender, EventArgs e)
        {
            
            //TODO: make sure a version of the image is saved in the show directory!!!!!!!
            //TODO: change order by after testing


            //get a collection of past shows and loop thru
            ShowCollection shows = new ShowCollection();
            
            string sql = "SELECT TOP 1000 s.* FROM [Show] s WHERE s.[DisplayUrl] IS NULL OR LEN(RTRIM(LTRIM(s.[DisplayUrl]))) = 0 AND ";
            sql += "s.[Name] < ( CAST(DATEPART(yyyy,@date) as varchar) + '/' +  CAST(DATEPART(mm,@date) as varchar) + '/' + CAST(DATEPART(dd,@date) as varchar) ) ORDER BY ID DESC ";
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@date", DateTime.Now.Date.AddDays(-5), DbType.DateTime);

            shows.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            foreach (Show s in shows)
            {

                //TODO reinstate show image
                string imageVersion = string.Empty;// s.ShowImageUrl;
                imageVersion = imageVersion.Replace("thumbsm", "thumblg");

                string mappedImage = System.Web.HttpContext.Current.Server.MapPath(imageVersion);
                string imageHtml = string.Empty;

                //if we are in venue mode, and we 
                //reverse logic at work here
                bool venuePass = true;

                if (venuePass)
                {
                    bool imagePass = true;

                    if (!System.IO.File.Exists(mappedImage))
                    {
                        //find the original and create a smaller version
                        //the original resides in the parent directory
                        string originalVersion = System.IO.Path.GetFileName(mappedImage);//.Replace("thumblg/", string.Empty);

                        //be sure the original exists
                        if (System.IO.File.Exists(originalVersion))
                        {
                            Utils.ImageTools.CreateAndSaveThumbnailImage(originalVersion, mappedImage, _Config._ShowThumbSizeSm);
                            //now mapped image should be valid
                        }
                        else
                        {
                            imagePass = false;
                            _Error.LogException(new Exception(string.Format("ShowId: {0} ShowId: {1} - {2} - originalImage version does not exist", 
                                s.Id.ToString(), "", s.Name)));
                        }
                    }

                    if(imagePass)
                    {
                        s.DisplayUrl = System.IO.Path.GetFileName(mappedImage);
                        //s.Save();
                    }



                }
            }
        }

        protected void btnRename_Click(object sender, EventArgs e)
        {
            ShowCollection coll = new ShowCollection();

            coll.LoadAndCloseReader(Show.FetchAll());

            foreach (Show s in coll)
            {
                JShowActCollection acts = new JShowActCollection();
                //int dates = s.ShowDateRecords().Count;
                //int sellouts = 0;

                foreach (ShowDate sd in s.ShowDateRecords())
                {
                    //if (sd.ShowStatus.Name.ToLower() == _Enums.ShowDateStatus.SoldOut.ToString().ToLower())
                    //    sellouts += 1;

                    foreach (JShowAct j in sd.JShowActRecords())
                        if (j.TopBilling_Effective)
                        {
                            JShowActCollection jColl = new JShowActCollection();
                            jColl.AddRange(acts.GetList().FindAll(delegate(JShowAct match) { return (match.TActId == j.TActId); }));

                            if (jColl.Count == 0)
                                acts.Add(j);
                        }
                }

                if (acts.Count > 1)
                    acts.Sort("IDisplayOrder", true);
                ActCollection sortedHeads = new ActCollection();
                foreach (JShowAct jsa in acts)
                    sortedHeads.Add(jsa.ActRecord);

                string rename = Show.CalculatedShowName(s.FirstDate, s.VenueRecord, sortedHeads);

                s.Name = rename;

                //if(sellouts == dates)
                //    s.BSoldOut

                try
                {
                    s.Save();
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }
            }
        }
        protected void btnPass_Click(object sender, EventArgs e)
        {
            
        }
        

        
        protected void btnCleanAct_Click(object sender, EventArgs e)
        {
            string actName = txtAct.Text.Trim();
            if (actName.Length > 0)
            {
                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("UPDATE Act SET PictureUrl = null, iPicWidth = 0, ipicwidth = 0 WHERE NameRoot = @actName ",
                    SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@actName", actName);

                try
                {
                    SubSonic.DataService.ExecuteQuery(cmd);
                    litCleanAct.Text = "Success";
                    txtAct.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    litCleanAct.Text = ex.Message;                    
                }
            }
        }
}
}