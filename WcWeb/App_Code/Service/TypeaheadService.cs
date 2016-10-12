using System;
using System.Web.Services;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Newtonsoft;
using Newtonsoft.Json.Linq;

using wctMain.Controller;


/*https://github.com/twitter/typeahead.js/issues/166
I was able to show the progress spinner with the following:

css:

.typeahead {
  background-color: #fff;
  background-repeat: no-repeat;
  background-position: 98%;
}
javascript:

typeaheadCtrl = $('.typeahead').typeahead({ name: 'countries', prefetch: '../data/countries.json' });

typeaheadCtrl.on('typeahead:initialized', function (event, data) {
   // After initializing, hide the progress icon.
   $('.tt-hint').css('background-image', '');
});

// Show progress icon while loading.
$('.tt-hint').css('background-image', 'url("/assets/images/ajax-loader.gif")');
 * 
 * 
 * 
 * request for custom matcher
 * https://github.com/twitter/typeahead.js/issues/86
 * 
 * 
 * 
 */


namespace wctMain.Service
{
    public class TypeaheadService
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="accessLevel"></param>
        public TypeaheadService(string accessLevel)
        {
            AccessLevel = accessLevel;
        }

        #region Properties 

        /// <summary>
        /// normal or admin - allows restrictive searches
        /// </summary>
        protected string AccessLevel { get; set; }

        protected Wcss._Enums.Principal principal { get { return (AccessLevel == "admin" && Atx != null) ? Atx.CurrentEditPrincipal : Ctx.WebContextPrincipal; } }
        
        private MainContext _ctx = null;
        protected MainContext Ctx 
        { 
            get 
            {
                if (_ctx == null)
                    _ctx = new MainContext();

                return _ctx; 
            } 
        }

        private wctMain.Admin.AdminContext _atx = null;
        protected wctMain.Admin.AdminContext Atx
        {
            get
            {
                if (_atx == null && Wcss._Common.IsAuthdAdminUser())
                    _atx = new wctMain.Admin.AdminContext();

                return _atx;
            }
        }

        protected List<string> _approvedTableList = null;
        /// <summary>
        /// ACT, EMPLOYEE, FAQITEM, GENRE, KIOSK, POST, PROMOTER, SALEPROMOTION, SHOW, VENUE
        /// </summary>
        protected List<string> ApprovedTableList
        {
            get
            {
                if (_approvedTableList == null)
                {
                    _approvedTableList = new List<string>();
                    _approvedTableList.Add(Wcss.Act.Schema.TableName);
                    _approvedTableList.Add(Wcss.Employee.Schema.TableName);
                    _approvedTableList.Add(Wcss.FaqItem.Schema.TableName);
                    _approvedTableList.Add(Wcss.Genre.Schema.TableName);
                    _approvedTableList.Add(Wcss.Kiosk.Schema.TableName);
                    _approvedTableList.Add(Wcss.Post.Schema.TableName);
                    _approvedTableList.Add(Wcss.Promoter.Schema.TableName);
                    _approvedTableList.Add(Wcss.SalePromotion.Schema.TableName);
                    _approvedTableList.Add(Wcss.Show.Schema.TableName);
                    _approvedTableList.Add(Wcss.Venue.Schema.TableName);
                }

                return _approvedTableList;
            }
        }

        #endregion


        #region Methods 

        public static List<object> GetTypeahead(string accessLevel, string context, string query, int limit, int pagenum)
        {
            TypeaheadService svc = new TypeaheadService(accessLevel);            
            
            //todo - convert to json
            return svc.Name_Suggestions(context, query, limit, pagenum);
        }

        /// <summary>
        /// Should we ever need to reinstate page num....
        /// </summary>
        /// <param name="context">Table name to be queried</param>
        /// <param name="query"></param>
        /// <param name="limit"># of results</param>
        /// <param name="pagenum">Not currently implemented</param>
        /// <returns></returns>
        private List<object> Name_Suggestions(string context, string query, int limit, int pagenum)
        {
            List<object> suggestions = new List<object>();

            try
            {
                // REGISTER TABLE QUERIES
                // ACT, EMPLOYEE, FAQITEM, GENRE, KIOSK, POST, PROMOTER, SALEPROMOTION, SHOW, VENUE
                if (!ApprovedTableList.Contains(context))
                    throw new ArgumentOutOfRangeException("context", string.Format("Context: {0} is not a valid for typeahead.", context));

                //Active Required should be set for all normal searches
                //from admin: act and promoter, genre(ignores principal) should be all
                string prince = (this.AccessLevel == "admin" && (context == Wcss.Act.Schema.TableName || context == Wcss.Promoter.Schema.TableName || context == Wcss.Genre.Schema.TableName)) ? 
                    Wcss._Enums.Principal.all.ToString() : this.principal.ToString();                
                
                using (IDataReader dr = Wcss.SPs.TxGetTypeaheadSuggestions(prince, context, query, (this.AccessLevel == "normal"), limit).GetReader())
                {
                    while (dr.Read())
                    {
                        suggestions.Add(
                            new
                            {
                                Id = dr.GetValue(dr.GetOrdinal("Id")).ToString(),
                                Suggestion = dr.GetValue(dr.GetOrdinal("Suggestion")).ToString()
                            });
                    }

                    dr.Close();
                }
            }
            catch (System.Data.SqlClient.SqlException sex) { Wcss._Error.LogException(sex); }
            catch (Exception ex) { Wcss._Error.LogException(ex); }

            return suggestions;
        }

        #endregion
    }
}