using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wcss
{
    public partial class FaqItem_Principal : _PrincipalBase.Principaled
    {
        public FaqItem FaqItem { get; set; }

        public FaqItem_Principal() : base() { }

        public FaqItem_Principal(FaqItem faq)
            : base(faq)
        {
            FaqItem = faq;
        }

        public static void SortListByPrincipal(_Enums.Principal prince, ref List<FaqItem> list)
        {
            //leave for testing ordering
            //foreach (SalePromotion sp in list)
            //{
            //    string name = sp.Name;
            //    int weight = new Kiosk_Principal(sp).PrincipalWeight(prince);
            //    int order = new Kiosk_Principal(sp).PrincipalOrder_Get(prince);
            //}

            list = new List<FaqItem>(
                list
                .OrderBy(x => new FaqItem_Principal(x).PrincipalWeight(prince))
                .ThenBy(x => new FaqItem_Principal(x).PrincipalOrder_Get(prince))
                .ThenBy(x => x.Question));
        }

        public void SyncOrdinals()
        {
            this.FaqItem.VcJsonOrdinal = JsonConvert.SerializeObject(base.SyncOrds());
        }

        private List<_PrincipalBase.PrincipalOrdinal> _principalOrdinalList = null;
        public override List<_PrincipalBase.PrincipalOrdinal> PrincipalOrdinalList
        {
            get
            {

                if (_principalOrdinalList == null)
                {
                    //determine that this collection is a MailerShow Collection
                    if (this.FaqItem.VcJsonOrdinal != null && this.FaqItem.VcJsonOrdinal.Trim().Length > 0)
                    {
                        try
                        {
                            _principalOrdinalList = JsonConvert.DeserializeObject<List<_PrincipalBase.PrincipalOrdinal>>(this.FaqItem.VcJsonOrdinal);
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
                this.FaqItem.VcJsonOrdinal = JsonConvert.SerializeObject(value);
                _principalOrdinalList = null;
            }
        }
    }

    public partial class FaqItem : _PrincipalBase.IPrincipal
    {
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }

        public string AnswerWithMungedMailto
        {
            get
            {
                StringBuilder sb = new StringBuilder(this.Answer);

                Regex regex = new Regex(@"\<a href=\""mailto:(?<mailtolink>.*?)\""\>(?<innerhtml>.*?)\</a\>");
                return regex.Replace(sb.ToString(), new MatchEvaluator(EvaluateMailtos));

                
            }
        }

        private static string EvaluateMailtos(Match m)
        {
            
            string result = m.Value;

            if (result.Length > 0)
            {
                string mailto = m.Groups["mailtolink"].Value;
                string innerhtml = m.Groups["innerhtml"].Value;
                
                
                //look in mailto for pieces - emails, subject and body
                string emails = string.Empty;
                string subject = string.Empty;
                string body = string.Empty;


                string[] parts = mailto.Split('?');
                if (parts.Length > 0)
                {
                    emails = parts[0];

                    if (parts.Length > 1)
                    {
                        string parameters = parts[1];
                        if (parameters.Trim().Length > 0)
                        {
                            parameters = parameters.Replace("~~~", "&amp;");

                            string[] pieces = parameters.Split('&');
                            foreach (string s in pieces)
                            {
                                if (s.IndexOf("subject=", StringComparison.OrdinalIgnoreCase) != -1)
                                    subject = s.Replace("subject=", string.Empty).Replace("~~~", "&amp;");
                                if (s.IndexOf("body=", StringComparison.OrdinalIgnoreCase) != -1)
                                    body = s.Replace("body=", string.Empty).Replace("~~~", "&amp;");
                            }
                        }
                    }
                }

                Utils.MailtoHelper helper = new Utils.MailtoHelper(emails, subject, body, innerhtml);

                result = helper.FormattedMailtoLink;

            }

            return result;
        }

        public string ShortAnswer 
        {
            get
            {
                string returnVal = string.Empty;

                if(this.Answer != null)
                {
                    returnVal = Utils.ParseHelper.StripHtmlTags(this.Answer).Trim();

                    if (returnVal.Length > 200)
                        returnVal = System.Web.HttpUtility.HtmlEncode(string.Format("{0}...", returnVal.Substring(0, 196)));
                }

                return returnVal;
            }
        }

        #region DataSource methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <param name="principal"></param>
        /// <param name="isActive">all, active, notactive</param>
        /// <param name="startDate">ignored</param>
        /// <param name="endDate">ignored</param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static FaqItemCollection GetFaqsInContext(int startRowIndex, int maximumRows,
            string principal, string status, string category, DateTime startDate, DateTime endDate, string searchTerms)
        {
            category = (category != null && category.Trim().Length > 0) ? category : "all";

            FaqItemCollection coll = new FaqItemCollection();

            coll.LoadAndCloseReader(SPs.TxGetFaqsInContext(startRowIndex, maximumRows,
                principal, status, category, searchTerms).GetReader());

            return coll;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="status"></param>
        /// <param name="category"></param>
        /// <param name="startDate">ignored</param>
        /// <param name="endDate">ignored</param>
        /// <param name="searchTerms"></param>
        /// <returns></returns>
        public static int GetFaqsInContextCount(
            string principal, string status, string category, DateTime startDate, DateTime endDate, string searchTerms)
        {
            category = (category != null && category.Trim().Length > 0) ? category : "all";

            int count = 0;

            using (System.Data.IDataReader dr = SPs.TxGetFaqsInContextCount(
                principal, status, category, searchTerms).GetReader())
            {
                while (dr.Read())
                    count = (int)dr.GetValue(0);
                dr.Close();
            }

            return count;
        }

        #endregion
    }

    //public partial class FaqItemCollection : Utils._Collection.IOrderable<FaqItem>
    //{
    //    /// <summary>
    //    /// Adds an FaqItem to the collection
    //    /// </summary>
    //    /// <param name="faqCategorieId"></param>
    //    /// <param name="question"></param>
    //    /// <param name="answer"></param>
    //    /// <returns></returns>
    //    public FaqItem AddToCollection(int faqCategorieId, string question, string answer)
    //    {
    //        System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
    //        args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
    //        args.Add(new System.Web.UI.Pair("TFaqCategorieId", faqCategorieId)); //args.Add(new System.Web.UI.Pair("EmailAddress", email)); //newItem.TFaqCategorieId = faqCategorieId;
    //        args.Add(new System.Web.UI.Pair("IsActive", false)); //newItem.IsActive = false;
    //        args.Add(new System.Web.UI.Pair("Question", question)); //newItem.Question = question;
    //        args.Add(new System.Web.UI.Pair("Answer", answer)); //newItem.Answer = answer;

    //        return AddToCollection(args);
    //    }

    //    public FaqItem AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
    //    {
    //        return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
    //    }

    //    /// <summary>
    //    /// Delete an FaqItem from the collection by ID
    //    /// </summary>
    //    /// <param name="idx"></param>
    //    /// <returns></returns>
    //    public bool DeleteFromCollection(int idx)
    //    {
    //        return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
    //    }

    //    /// <summary>
    //    /// Reorder a FaqItem by ID and direction
    //    /// </summary>
    //    /// <param name="idx"></param>
    //    /// <param name="direction"></param>
    //    /// <returns></returns>
    //    public FaqItem ReorderItem(int idx, string direction)
    //    {
    //        return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
    //    }
    //}
}
