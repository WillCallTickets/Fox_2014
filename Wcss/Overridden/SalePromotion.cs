using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SubSonic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wcss
{
    public partial class SalePromotion_Principal : _PrincipalBase.Principaled
    {
        public SalePromotion SalePromotion { get; set; }

        public SalePromotion_Principal() : base() { }

        public SalePromotion_Principal(SalePromotion promo)
            : base(promo)
        {
            SalePromotion = promo;
        }

        public static void SortListByPrincipal(_Enums.Principal prince, ref List<SalePromotion> list)
        {
            //leave for testing ordering
            //foreach (SalePromotion sp in list)
            //{
            //    string name = sp.Name;
            //    int weight = new SalePromotion_Principal(sp).PrincipalWeight(prince);
            //    int order = new SalePromotion_Principal(sp).PrincipalOrder_Get(prince);
            //}

            list = new List<SalePromotion>(
                list
                .OrderBy(x => new SalePromotion_Principal(x).PrincipalWeight(prince))
                .ThenBy(x => new SalePromotion_Principal(x).PrincipalOrder_Get(prince))
                .ThenBy(x => x.Name));
        }

        public void SyncOrdinals()
        {
            this.SalePromotion.VcJsonOrdinal = JsonConvert.SerializeObject(base.SyncOrds());
        }

        private List<_PrincipalBase.PrincipalOrdinal> _principalOrdinalList = null;
        public override List<_PrincipalBase.PrincipalOrdinal> PrincipalOrdinalList
        {
            get
            {

                if (_principalOrdinalList == null)
                {
                    //determine that this collection is a MailerShow Collection
                    if (this.SalePromotion.VcJsonOrdinal != null && this.SalePromotion.VcJsonOrdinal.Trim().Length > 0)
                    {
                        try
                        {
                            _principalOrdinalList = JsonConvert.DeserializeObject<List<_PrincipalBase.PrincipalOrdinal>>(this.SalePromotion.VcJsonOrdinal);
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);

                            //create a default
                            _principalOrdinalList = new List<_PrincipalBase.PrincipalOrdinal>();
                        }
                    }
                    else
                        _principalOrdinalList = new List<_PrincipalBase.PrincipalOrdinal>();
                }

                return _principalOrdinalList;
            }
            set
            {
                this.SalePromotion.VcJsonOrdinal = JsonConvert.SerializeObject(value);
                _principalOrdinalList = null;
            }
        }
    }

    public partial class SalePromotion : _PrincipalBase.IPrincipal
    {
        #region Table Properties

        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }

        /// <summary>
        /// If the promotion is a banner....then this will hold the timeout - the length of time to display the particular banner
        /// </summary>
        [XmlAttribute("BannerTimeout")]
        public int BannerTimeout
        {
            get { return this.IBannerTimeoutMsecs; }
            set { this.IBannerTimeoutMsecs = value; }
        }

        [XmlAttribute("DateStart")]
        public DateTime DateStart
        {
            get { return (!this.DtStartDate.HasValue) ? DateTime.MinValue : this.DtStartDate.Value; }
            set { this.DtStartDate = (value == DateTime.MinValue) ? (DateTime?)null : value; }
        }

        //dtEndDate
        [XmlAttribute("DateEnd")]
        public DateTime DateEnd
        {
            get { return (!this.DtEndDate.HasValue) ? DateTime.MaxValue : this.DtEndDate.Value; }
            set { this.DtEndDate = (value == DateTime.MaxValue) ? (DateTime?)null : value; }
        }

        #endregion

        #region Derived Properties

        private bool _hasStarted
        {
            get
            {
                return this.DateStart < DateTime.Now;
            }
        }
        private bool _hasEnded
        {
            get
            {
                return this.DateEnd < DateTime.Now;
            }
        }
        /// <summary>
        /// Ensures that entity is active, has started and has not ended
        /// </summary>
        public bool IsCurrentlyRunning
        {
            get
            {
                return (this.IsActive && this._hasStarted && (!this._hasEnded));
            }
        }

        public string DisplayNameWithAttribs
        {
            get
            {
                return this.DisplayText;
            }
        }

        #endregion

        public static string Banner_VirtualDirectory
        {
            get
            {
                //ensure directories exist
                string path = string.Format("/{0}/Images/Banners/", _Config._VirtualResourceDir);
                string mappedPath = System.Web.HttpContext.Current.Server.MapPath(path);

                if (!System.IO.Directory.Exists(mappedPath))
                    System.IO.Directory.CreateDirectory(mappedPath);

                return path;
            }
        }
        public string Banner_VirtualFilePath
        {
            get
            {
                if (this.BannerUrl != null && this.BannerUrl.Trim().Length > 0)
                {
                    string filePath = string.Format("{0}{1}", Banner_VirtualDirectory, this.BannerUrl);
                    string mappedFile = System.Web.HttpContext.Current.Server.MapPath(filePath);

                    if (System.IO.File.Exists(mappedFile))
                        return filePath;
                }

                return string.Empty;
            }
        }

        #region DataSource methods

        public static List<SalePromotion> GetCurrentRunningBannerList(_Enums.Principal prince, bool ignoreStartDate, bool randomize)
        {
            string sql = "SELECT * FROM [SalePromotion] k WHERE k.[bActive] = 1 AND ";
            if(!ignoreStartDate)
                sql += "(k.[dtStartDate] IS NULL OR k.[dtStartDate] < getDate()) AND ";
            sql += "(k.[dtEndDate] IS NULL OR k.[dtEndDate] > getDate()) AND ";
            sql += "CASE WHEN @principal = 'all' THEN 1 WHEN CHARINDEX(@principal, k.[vcPrincipal]) >= 1 THEN 1 ELSE 0 END = 1 ";

            _DatabaseCommandHelper dch = new _DatabaseCommandHelper(sql);

            dch.AddCmdParameter("@principal", prince.ToString(), System.Data.DbType.String);

            SalePromotionCollection coll = new SalePromotionCollection();
            dch.PopulateCollectionByReader<SalePromotionCollection>(coll);

            List<SalePromotion> list = new List<SalePromotion>(coll);

            //randomize the list
            int count = list.Count;
            if (count > 0 && randomize)
            {
                List<SalePromotion> rnd = new List<SalePromotion>();
                int startAt = Utils.ParseHelper.GenerateRandomNumber(0, count - 1);

                //add end of list
                for (int i = startAt; i < count; i++)
                    rnd.Add(list[i]);
                //add items at beginning
                for (int i = 0; i < startAt; i++)
                    rnd.Add(list[i]);

                list.Clear();
                list.AddRange(rnd);

                rnd = null;
            }
            else
                SalePromotion_Principal.SortListByPrincipal(prince, ref list);


            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">all, active, notactive</param>
        public static SalePromotionCollection GetSalePromotionsInContext(int startRowIndex, int maximumRows,
            string principal, string status, DateTime startDate, DateTime endDate, string searchTerms)
        {
            SalePromotionCollection coll = new SalePromotionCollection();

            coll.LoadAndCloseReader(SPs.TxGetSalePromotionsInContext(startRowIndex, maximumRows,
                principal, status,
                Utils.ParseHelper.SanitizeDateTimeToSqlDateTime(startDate),
                Utils.ParseHelper.SanitizeDateTimeToSqlDateTime(endDate),
                searchTerms).GetReader());

            return coll;
        }

        public static int GetSalePromotionsInContextCount(
            string principal, string status, DateTime startDate, DateTime endDate, string searchTerms)
        {
            int count = 0;

            using (System.Data.IDataReader dr = SPs.TxGetSalePromotionsInContextCount(
                principal, status,
                Utils.ParseHelper.SanitizeDateTimeToSqlDateTime(startDate),
                Utils.ParseHelper.SanitizeDateTimeToSqlDateTime(endDate),
                searchTerms).GetReader()) 
            {
                while (dr.Read())
                    count = (int)dr.GetValue(0);
                dr.Close();
            }
            
            return count;
        }

        #endregion

    }
}
