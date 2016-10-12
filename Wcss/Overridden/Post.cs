using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SubSonic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wcss
{
    public partial class Post_Principal : _PrincipalBase.Principaled
    {
        public Post Post { get; set; }

        public Post_Principal() : base() { }

        public Post_Principal(Post post)
            : base(post)
        {
            Post = post;
        }

        public static void SortListByPrincipal(_Enums.Principal prince, ref List<Post> list)
        {
            //leave for testing ordering
            //foreach (SalePromotion sp in list)
            //{
            //    string name = sp.Name;
            //    int weight = new Post_Principal(sp).PrincipalWeight(prince);
            //    int order = new Post_Principal(sp).PrincipalOrder_Get(prince);
            //}

            list = new List<Post>(
                list
                .OrderBy(x => new Post_Principal(x).PrincipalWeight(prince))
                .ThenBy(x => new Post_Principal(x).PrincipalOrder_Get(prince))
                .ThenBy(x => x.Name));
        }

        public void SyncOrdinals()
        {
            this.Post.VcJsonOrdinal = JsonConvert.SerializeObject(base.SyncOrds());
        }

        private List<_PrincipalBase.PrincipalOrdinal> _principalOrdinalList = null;
        public override List<_PrincipalBase.PrincipalOrdinal> PrincipalOrdinalList
        {
            get
            {

                if (_principalOrdinalList == null)
                {
                    //determine that this collection is a MailerShow Collection
                    if (this.Post.VcJsonOrdinal != null && this.Post.VcJsonOrdinal.Trim().Length > 0)
                    {
                        try
                        {
                            _principalOrdinalList = JsonConvert.DeserializeObject<List<_PrincipalBase.PrincipalOrdinal>>(this.Post.VcJsonOrdinal);
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
                this.Post.VcJsonOrdinal = JsonConvert.SerializeObject(value);
                _principalOrdinalList = null;
            }
        }
    }

    public partial class Post : _PrincipalBase.IPrincipal
    {
        //vcPrincipal
        //bactive
        //slug - name with no spaces - a friendly version of name, but editable
        //value - valueX is html

        protected override void BeforeInsert()
        {
            this.Slug = Post.FormatSlug(Slug);
            base.BeforeInsert();
        }

        protected override void BeforeUpdate()
        {
            this.Slug = Post.FormatSlug(Slug);
            base.BeforeUpdate();
        }

        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }

        public static string FormatSlug(string s)
        {
            return Utils.ParseHelper.FriendlyFormat(s).ToLower();
        }

        #region DataSource methods

        /// <summary>
        /// 
        /// </summary>
        public static PostCollection GetPostsInContext(int startRowIndex, int maximumRows,
            string principal, string status, DateTime startDate, DateTime endDate, string searchTerms)
        {
            PostCollection coll = new PostCollection();

            coll.LoadAndCloseReader(SPs.TxGetPostsInContext(startRowIndex, maximumRows,
                principal, status, searchTerms).GetReader());

            return coll;
        }

        /// <summary>
        /// 
        /// </summary>
        public static int GetPostsInContextCount(
            string principal, string status, DateTime startDate, DateTime endDate, string searchTerms)
        {
            int count = 0;

            using (System.Data.IDataReader dr = SPs.TxGetPostsInContextCount(
                principal, status, searchTerms).GetReader())
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
