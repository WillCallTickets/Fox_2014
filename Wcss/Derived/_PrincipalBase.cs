using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SubSonic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wcss
{
    public class _AlphaOrderedEntity
    {
        public interface IAlphaOrderedEntity
        {
            string Name { get; set; }
            string NameRoot { get; set; }
            string DisplayName { get; set; }
        }
    }


    public class _PrincipalBase
    {
        public static _Enums.Principal PrincipalFromUrlHost(HttpRequest request)
        {
            if (request != null)
            {
                string host = request.Url.Host.ToLower();

                if (host.IndexOf("fox") != -1)
                    return Wcss._Enums.Principal.fox;
                else if (host.IndexOf("bt.") != -1 || host.IndexOf("boulder") != -1 || host.IndexOf("bowl") != -1)
                    return Wcss._Enums.Principal.bt;
                else if (host.IndexOf("z2.") != -1 || host.IndexOf("z2ent") != -1)
                    return Wcss._Enums.Principal.z2;
            }

            return _Enums.Principal.all;
        }

        /// <summary>
        /// By looking at the Show's principal, determine the intended hostname
        /// </summary>
        public static string Host_IntendedForShow(HttpRequest request, Show s)
        {
            bool isLocal = (request.Url.Host.ToLower().IndexOf("local.") != -1);

            if (s != null)
            {
                //get principal from show's vcPrincipal string
                _Enums.Principal prince = PrincipalFromString(s.VcPrincipal);

                switch (prince)
                {
                    case _Enums.Principal.bt:
                        return (isLocal) ? "local.bt.com" : "bouldertheater.com";
                    case _Enums.Principal.fox:
                        return (isLocal) ? "local.fox2014.com" : "foxtheatre.com";
                    case _Enums.Principal.z2:
                        return (isLocal) ? "local.z2.com" : "z2ent.com";
                }

            }

            return string.Empty;
        }

        public static void chkPrincipal_DataBinding(object sender, EventArgs e, bool allowAllSelectionInList)
        {
            System.Web.UI.WebControls.CheckBoxList chk = (System.Web.UI.WebControls.CheckBoxList)sender;

            List<string> list = new List<string>(Enum.GetNames(typeof(_Enums.Principal)));

            if (!allowAllSelectionInList)
                list.Remove(_Enums.Principal.all.ToString());

            chk.DataSource = list;
        }


        public static _Enums.Principal PrincipalFromString(string s)
        {
            return (_Enums.Principal)Enum.Parse(typeof(_Enums.Principal), s, true);
        }

        public static string PrincipalListToString(List<_Enums.Principal> list)
        {
            return string.Join(",", list.ToArray());
        }

        public partial class PrincipalOrdinal
        {
            public string Owner { get; set; }
            public int Ordinal { get; set; }

            [JsonIgnore]
            public _Enums.Principal Principal
            {
                get
                {
                    return _PrincipalBase.PrincipalFromString(Owner);
                }
            }
        }

        public interface IPrincipal
        {
            int Id { get; }
            string VcPrincipal { get; set; }
        }

        
        //public interface IPrincipalOrder
        //{
        //    int PrincipalOrder(_Enums.Principal prince);
        //    void PrincipalOrder_Set(_Enums.Principal prince, int ordinal);
        //}

        public class Principaled
        {
            public int Id { get { return Prince.Id; } }
            public IPrincipal Prince { get; set; }

            public Principaled() { }

            public Principaled(IPrincipal prince)
            {
                Prince = prince;
            }

            public int PrincipalWeight(_Enums.Principal prince)
            {
                return (this.HasPrincipal(prince)) ? 1 :
                    (this.HasPrincipal(_Enums.Principal.fox)) ? 2 :
                    (this.HasPrincipal(_Enums.Principal.bt)) ? 3 :
                    (this.HasPrincipal(_Enums.Principal.z2)) ? 4 : 5;
            }

            /// <summary>
            /// implemented by Active Record?
            /// </summary>
            //public virtual void Save() {}

            /// <summary>
            /// 
            /// </summary>
            public virtual string VcPrincipal 
            {
                get
                {
                    return Prince.VcPrincipal;
                }
                set
                {
                    Prince.VcPrincipal = value;
                }
            }

            public _Enums.Principal Principal
            {
                get
                {
                    return PrincipalList[0];// _PrincipalBase.PrincipalFromString(Prince.VcPrincipal);
                }
                set
                {
                    Prince.VcPrincipal = value.ToString();
                }
            }

            private List<_Enums.Principal> _principalList = null;
            public List<_Enums.Principal> PrincipalList
            {
                get 
                { 
                    if(_principalList == null)
                        _principalList = Prince.VcPrincipal.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(c => _PrincipalBase.PrincipalFromString(c)).ToList<_Enums.Principal>();

                    return _principalList;
                }
            }

            public bool HasPrincipal(_Enums.Principal principal)
            {
                return this.PrincipalList.Contains(principal);
            }

            /// <summary>
            /// ONLY USE IF MULT PRINCIPAL!
            /// </summary>
            /// <param name="prince"></param>
            /// <param name="list"></param>
            public virtual void SortListByPrincipal(_Enums.Principal prince, List<IPrincipal> list) {}

            public virtual List<_PrincipalBase.PrincipalOrdinal> PrincipalOrdinalList { get; set; }

            public List<_PrincipalBase.PrincipalOrdinal> SyncOrds()
            {
                List<_PrincipalBase.PrincipalOrdinal> ords = new List<_PrincipalBase.PrincipalOrdinal>(this.PrincipalOrdinalList);
                List<_PrincipalBase.PrincipalOrdinal> removeOrds = new List<_PrincipalBase.PrincipalOrdinal>();

                foreach (_PrincipalBase.PrincipalOrdinal po in ords)
                    if (!this.HasPrincipal(po.Principal))
                        removeOrds.Add(po);

                foreach (_PrincipalBase.PrincipalOrdinal po in removeOrds)
                    ords.Remove(po);

                return ords;
            }

            /// <summary>
            /// if no ordinal is found-  return a high number
            /// </summary>
            /// <param name="prince"></param>
            /// <returns></returns>
            public int PrincipalOrder_Get(_Enums.Principal prince)
            {
                _PrincipalBase.PrincipalOrdinal po = this.PrincipalOrdinalList.Find(delegate(_PrincipalBase.PrincipalOrdinal match) { return (match.Principal == prince); });

                return (po != null) ? po.Ordinal : 9999999;
            }

            public void PrincipalOrder_Set(_Enums.Principal prince, int ordinal)
            {
                List<_PrincipalBase.PrincipalOrdinal> list = new List<_PrincipalBase.PrincipalOrdinal>();
                list.AddRange(this.PrincipalOrdinalList);

                //cleanup any mistaken all orders
                list.RemoveAll(delegate(PrincipalOrdinal match) { return (match.Principal == _Enums.Principal.all); });

                _PrincipalBase.PrincipalOrdinal po = list.Find(delegate(_PrincipalBase.PrincipalOrdinal match) { return (match.Principal == prince); });

                if (po != null)
                    po.Ordinal = ordinal;
                else
                {
                    po = new _PrincipalBase.PrincipalOrdinal();
                    po.Owner = prince.ToString();
                    po.Ordinal = ordinal;
                    list.Add(po);
                }

                this.PrincipalOrdinalList = list;
            }
        }


        #region Helpers

        public class Helpers
        {
            public class CollectionSearchCriteria
            {
                public _Enums.CollectionSearchCriteriaStatusType Status { get; set; }
                public DateTime StartDate { get; set; }
                public DateTime EndDate { get; set; }
                public string SearchTerms { get; set; }


                private void InitializeEmpty_SearchCriteria()
                {
                    Status = _Enums.CollectionSearchCriteriaStatusType.all;
                    StartDate = DateTime.MinValue;
                    EndDate = DateTime.MaxValue;
                    SearchTerms = string.Empty;
                }


                /// <summary>
                /// the default values
                /// </summary>
                public CollectionSearchCriteria()
                {
                    InitializeEmpty_SearchCriteria();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="status">_Enums.CollectionSearchCriteriaStatusType ToString()</param>
                /// <param name="startDate"></param>
                /// <param name="endDate"></param>
                /// <param name="terms"></param>
                public CollectionSearchCriteria(string status, string startDate, string endDate, string terms)
                {
                    Status = (_Enums.CollectionSearchCriteriaStatusType)Enum.Parse(typeof(_Enums.CollectionSearchCriteriaStatusType), status, true);

                    DateTime start = DateTime.MinValue;
                    DateTime end = DateTime.MaxValue;
                    DateTime.TryParse(startDate, out start);
                    DateTime.TryParse(endDate, out end);

                    StartDate = start;
                    EndDate = end;
                    SearchTerms = (terms != null && terms.Trim().Length > 0) ? terms.Replace("~", "-") : string.Empty;
                }

                /// <summary>
                /// In this case, we expect the 
                /// </summary>
                /// <param name="s"></param>
                public CollectionSearchCriteria(string cookieValue)
                {
                    if (cookieValue == null || cookieValue.Trim().Length == 0)
                    {
                        InitializeEmpty_SearchCriteria();
                        return;
                    }
                    
                    //specify max number of items - terms can contain commas - so handle separately
                    // do in parts to avoid stripping any commas out of the search terms
                    List<string> pieces = new List<string>(4);

                    try
                    {
                        int termIdx = cookieValue.IndexOf(",t~");

                        pieces.AddRange(cookieValue.Substring(0, termIdx).Split(new char[] { ',' }, 3).ToArray());
                        pieces.Add(cookieValue.Substring(termIdx + 1));//should be t~blah blah blah

                        //if we don't have any results here - something is wrong with they cookieVal
                        if (pieces.Count != 4)
                            throw new ArgumentOutOfRangeException();
                    }
                    catch(Exception ex)
                    {
                        InitializeEmpty_SearchCriteria();
                        return;
                    }


                    foreach (string s in pieces)
                    {
                        string[] parts = s.Split(new char[] { '~' });                        
                        string key = parts[0];
                        string val = parts[1];

                        if (key == "a")
                            Status = (_Enums.CollectionSearchCriteriaStatusType)int.Parse(val);
                        else if (key == "sd")
                            StartDate = (val != null && val.Trim().Length > 0) ? DateTime.Parse(val) : DateTime.MinValue;
                        else if (key == "ed")
                            EndDate = (val != null && val.Trim().Length > 0) ? DateTime.Parse(val) : DateTime.MaxValue;
                        else if (key == "t")
                            //SearchTerms = (val != null && val.Trim().Length > 0) ? val.Replace(",", string.Empty) : string.Empty;
                            SearchTerms = (val != null && val.Trim().Length > 0) ? val : string.Empty;
                    }
                }

                /// <summary>
                /// Cookies don't like = sign as a separator (Safari)
                /// </summary>
                /// <returns></returns>
                public string Serialized()
                {
                    //return string.Format("a={0},sd={1},ed={2},t={3}",
                    string s = string.Format("a~{0},sd~{1},ed~{2},t~{3}",
                        ((int)this.Status).ToString(),
                        (this.StartDate != DateTime.MinValue && this.StartDate != DateTime.MaxValue) ? this.StartDate.ToString("MM/dd/yyyy hh:mmtt") : string.Empty,
                        (this.EndDate != DateTime.MinValue && this.EndDate != DateTime.MaxValue) ? this.EndDate.ToString("MM/dd/yyyy hh:mmtt") : string.Empty,
                        (this.SearchTerms != null && this.SearchTerms.Trim().Length > 0) ? this.SearchTerms : string.Empty);

                    return s;
                }
            }
        }

     #endregion
    }
}