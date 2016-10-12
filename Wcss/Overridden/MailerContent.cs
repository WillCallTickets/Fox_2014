using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wcss
{
    #region MailerContent

    public partial class MailerContent
    {
        public string ContestText
        {
            get
            {
                if (this.MailerTemplateContentRecord.TemplateAsset == Wcss.MailerTemplateContent.ContentAsset.contest)
                {
                    return (this.VcContent != null) ? this.VcContent : string.Empty;
                }

                return null;
            }
        }

        public string PreheaderText
        {
            get
            {
                if (this.MailerTemplateContentRecord.TemplateAsset == Wcss.MailerTemplateContent.ContentAsset.preheader)
                {
                    return (this.VcJsonContent != null) ? this.VcJsonContent : string.Empty;
                }

                return null;
            }
        }

        private List<MailerBanner> _mailerBannerList = null;
        /// <summary>
        /// Note that this merely faciltates using a List for the property. Always make copy and save back to object to rewrite
        /// </summary>
        public List<MailerBanner> MailerBannerList
        {
            get
            {
                if (_mailerBannerList == null)
                {
                    //determine that this collection is a MailerShow Collection
                    if (this.MailerTemplateContentRecord.TemplateAsset == Wcss.MailerTemplateContent.ContentAsset.bannerimages &&
                        this.VcJsonContent != null && this.VcJsonContent.Trim().Length > 0)
                    {
                        try
                        {
                            _mailerBannerList = JsonConvert.DeserializeObject<List<MailerBanner>>(this.VcJsonContent);
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);

                            //create a default
                            _mailerBannerList = new List<MailerBanner>();
                        }
                    }
                    else
                        _mailerBannerList = new List<MailerBanner>();
                }

                return _mailerBannerList;
            }
            set
            {
                this.VcJsonContent = JsonConvert.SerializeObject(value);//, Formatting.Indented);
                _mailerBannerList = null;
            }
        }

        /// <summary>
        /// return SUCCESS or ERROR for evaluation by ajax call
        /// </summary>
        public string AddBulkMailerBanners(List<MailerBanner> list)
        {
            //merge any existing content
            List<MailerBanner> existing = this.MailerBannerList;

            //get highest ordinal and update new list
            int existingMemberCount = existing.Count;
            foreach (MailerBanner m in list)
            {
                m.Ordinal += existingMemberCount;
                existing.Add(m);
            }

            //write the content to the object
            this.MailerBannerList = existing;
            this.Save();

            return "SUCCESS";
        }

        public void DeleteMailerBanner(int idx)
        {
            List<MailerBanner> existing = new List<MailerBanner>();
            existing.AddRange(this.MailerBannerList);
            existing.RemoveAt(idx);

            foreach (MailerBanner m in existing)
                if (m.Ordinal > idx)
                    m.Ordinal -= 1;

            this.MailerBannerList = existing;
            this.Save();
        }



        private List<MailerShow> _mailerShowList = null;
        /// <summary>
        /// Note that this merely faciltates using a List for the property. Always make copy and save back to object to rewrite
        /// </summary>
        public List<MailerShow> MailerShowList
        {
            get
            {
                if (_mailerShowList == null)
                {
                    //determine that this collection is a MailerShow Collection
                    if (this.MailerTemplateContentRecord.TemplateAsset == Wcss.MailerTemplateContent.ContentAsset.mailershow &&
                        this.VcJsonContent != null && this.VcJsonContent.Trim().Length > 0)
                    {
                        try
                        {
                            _mailerShowList = JsonConvert.DeserializeObject<List<MailerShow>>(this.VcJsonContent);
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);

                            //create a default
                            _mailerShowList = new List<MailerShow>();
                        }
                    }
                    else
                        _mailerShowList = new List<MailerShow>();
                }

                return _mailerShowList;
            }
            set
            {
                this.VcJsonContent = JsonConvert.SerializeObject(value);//, Formatting.Indented);
                _mailerShowList = null;
            }
        }
        
        /// <summary>
        /// return SUCCESS or ERROR for evaluation by ajax call
        /// </summary>
        public string AddBulkMailerShows(List<MailerShow> list)
        {
            //merge any existing content
            List<MailerShow> existing = this.MailerShowList;

            //get highest ordinal and update new list
            int existingMemberCount = existing.Count;
            foreach (MailerShow m in list)
            {
                m.Ordinal += existingMemberCount;
                existing.Add(m);
            }

            //write the content to the object
            this.MailerShowList = existing;
            this.Save();

            return "SUCCESS";
        }

        public void DeleteMailerShow(int idx)
        {
            List<MailerShow> existing = new List<MailerShow>();
            existing.AddRange(this.MailerShowList);

            //delete corresponding images - thumbnails - leave any uploads
            MailerShow showToDelete = existing[idx];
            if (showToDelete.ImageUrl.ToLower().IndexOf("willcallresources") != -1)
            {
                try
                {
                    string mappedThumb = System.Web.HttpContext.Current.Server.MapPath(showToDelete.ImageUrl);
                    if (System.IO.File.Exists(mappedThumb))
                        System.IO.File.Delete(mappedThumb);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }
            }

            existing.RemoveAt(idx);

            foreach (MailerShow ms in existing)
                if (ms.Ordinal > idx)
                    ms.Ordinal -= 1;

            this.MailerShowList = existing;
            this.Save();
        }
    }
    #endregion

}
