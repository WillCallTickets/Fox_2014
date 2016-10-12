using System;
using System.Diagnostics;
using System.Configuration;
using System.Collections;
using System.Data.SqlTypes;
using System.Web.UI;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Linq;

using Wcss;
//using SubSonic;

namespace TestApp
{
	/// <summary>
	/// This test app can be used to test your new Biz object.
	/// Please make sure that the test app is set as the startup Project.
	/// </summary>
	class TestApp
	{
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            

            try
            {
                Result r;

                r = TestApp.GetAnnounced(DateTime.Parse("2/14/2016 11:19am"));
                r = TestApp.GetAnnounced(DateTime.Parse("2/15/2016 9:19am"));
                r = TestApp.GetAnnounced(DateTime.Parse("2/15/2016 9:55am"));
                r = TestApp.GetAnnounced(DateTime.Parse("2/15/2016 9:59am"));
                r = TestApp.GetAnnounced(DateTime.Parse("2/15/2016 10:00am"));
                r = TestApp.GetAnnounced(DateTime.Parse("2/15/2016 10:01am"));
                r = TestApp.GetAnnounced(DateTime.Parse("2/15/2016 10:04am"));
                r = TestApp.GetAnnounced(DateTime.Parse("2/15/2016 10:05am"));
                r = TestApp.GetAnnounced(DateTime.Parse("2/15/2016 10:06am"));
                r = TestApp.GetAnnounced(DateTime.Parse("2/15/2016 10:10am"));
                r = TestApp.GetAnnounced(DateTime.Parse("2/15/2016 2pm"));


            }
            catch (Exception e) 
            {
                string d = e.Message;
            }
        }

        
        public static Result GetAnnounced(DateTime now)
        {
            List<ShowTest> listing = GetList();
            List<ShowTest> working = new List<ShowTest>();

            int leeway = 5;
            DateTime nextAnnounce = DateTime.MaxValue;

            //add shows where aannounce <= now + leeway
            foreach (ShowTest s in listing)
            {
                if (s.AnnounceDate_Virtual <= now.AddMinutes(-leeway))
                    working.Add(s);

                if (s.AnnounceDate_Virtual > now.AddMinutes(-leeway) && s.AnnounceDate_Virtual < nextAnnounce)
                    nextAnnounce = s.AnnounceDate_Virtual;
            }


            // do we need leeway here?
            // 
            if (nextAnnounce > now.AddHours(12))
                nextAnnounce = now.AddHours(12);



            return new Result(working, nextAnnounce);
        }




        public class Result
        {
            List<ShowTest> listing;
            DateTime nextAnnounce;

            public Result(List<ShowTest> list, DateTime next)
            {
                listing = list;
                nextAnnounce = next;
            }
        }

        public static List<ShowTest> GetList()
        {
            List<ShowTest> list = new List<ShowTest>();

            list.Add(new      ShowTest(DateTime.Parse("1/31/2106"), DateTime.Parse("1/1/2016 10am"), "2016/03/26 - Abba"));
            list.Add(new                            ShowTest(DateTime.Parse("1/31/2106 10am"), null, "2016/03/27 - Barnaby"));
            list.Add(new  ShowTest(DateTime.Parse("1/31/2106"), DateTime.Parse("1/10/2016 10am"), "2016/03/28 - Charlie"));
            list.Add(new ShowTest(DateTime.Parse("1/31/2106"), DateTime.Parse("2/15/2016 10am"), "2016/03/29 - David"));
            list.Add(new ShowTest(DateTime.Parse("1/31/2106"), DateTime.Parse("2/15/2016 10am"), "2016/03/30 - Elgin"));
            list.Add(new ShowTest(DateTime.Parse("1/31/2106"), DateTime.Parse("2/15/2016 10:04am"), "2016/03/31 - Fred"));
            list.Add(new ShowTest(DateTime.Parse("1/31/2106"), DateTime.Parse("2/15/2016 10:10am"), "2016/04/01 - George"));
            list.Add(new ShowTest(DateTime.Parse("1/31/2106"), DateTime.Parse("2/15/2016 2pm"), "2016/04/02 - Harry"));
            list.Add(new ShowTest(DateTime.Parse("1/31/2106"), DateTime.Parse("2/17/2016 10am"), "2016/04/03 - Inez"));

            return list;
        }

        class ShowTest 
        {
            public DateTime dtStamp { get; set; }
            public DateTime? AnnounceDate
            {
                get;
                set;
            }
            public DateTime AnnounceDate_Virtual
            {
                get
                {
                    return (AnnounceDate.HasValue && AnnounceDate.Value != Utils.Constants._MinDate) ? this.AnnounceDate.Value : this.dtStamp;
                }
            }
            public string Name { get; set; }

            public ShowTest(DateTime stamp, DateTime? announce, string name) 
            {
                this.dtStamp = stamp;
                this.AnnounceDate = announce;
                this.Name = name;
            }
        
        }

































        //private static _Enums.Principal prince = _Enums.Principal.all;


        //private static EmployeeCollection _masterCollection = null;
        ////private static EmployeeCollection _orderedCollection = null;
        //private static EmployeeCollection OrderedCollection
        //{
        //    get
        //    {
        //        if (_masterCollection == null)// || _masterCollection.Count == 0)
        //        {
        //            _masterCollection = new Wcss.EmployeeCollection().Load();
        //                //.Where(Employee.Columns.ApplicationId, _Config.APPLICATION_ID)
        //                //.OrderByAsc(Wcss.EmployeeToString()).Load();            
        //        }

        //        EmployeeCollection _orderedCollection = new EmployeeCollection();
        //        _orderedCollection.AddRange(_masterCollection.GetList()
        //            .FindAll(delegate(Employee match)
        //        {
        //            return (prince == _Enums.Principal.all ||
        //                new Employee_Principal(match).HasPrincipal(prince)
        //            );
        //        }));

                
        //        return _orderedCollection;
        //    }
        //}


        ////public static Regex regexUrlAllowableCharsPerRfc = new Regex(@"^[a-zA-Z0-9$-_\.+!*'()\s]+$", RegexOptions.IgnoreCase);

        //public static Regex regexUrlAllowableCharsPerRfc = new Regex(@"^[a-zA-Z0-9\s\$\-_\.\+\!\*\'\(\)\,]+$", RegexOptions.IgnoreCase);

        //public static bool IsAllowableAlphaOrderableName(string input)
        //{
        //    Match match = Regex.Match(input, regexUrlAllowableCharsPerRfc.ToString());

        //    return match.Success;
        //}

//        /// <summary>
//        /// The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main(string[] args)
//        {
//            try
//            {
//                bool allIsGood = false;

//                List<string> list = new List<string>();

//                //list.Add("()");
//                //list.Add("_");
//                //list.Add("-");
//                //list.Add("$");
//                //list.Add("");
//                //list.Add("  ");
//                //list.Add("*");
//                //list.Add("'");
//                //list.Add("+");
//                //list.Add("!");
//                //list.Add("=");
                
//                //list.Add(".");
//                //list.Add("jhgfdjhgfd()");



//                //list.Add("gfds ");
//                //list.Add(" jhgs  ");


//                //list.Add("$-_.+!*'(),");
//                ////list.Add("%#@^&");
//                //list.Add(".");//ok
//                //list.Add("#");
//                //list.Add("~");
//                //list.Add("`");
//                //list.Add("{}");
//                //list.Add("|");

//                list.Add(" ");
//                list.Add("\r");
//                list.Add("\n");
//                list.Add("\t");
//                list.Add("\\r");
//                list.Add("\\n");

//                //list.Add("%");//XXX                
//                //list.Add("@");//XXX
//                //list.Add("^");//XXX
//                //list.Add("&");//XXX                
//                //list.Add("=");//XXX
//                //list.Add("[]");//XXX                
//                //list.Add(@"\");//XXX
//                //list.Add("/");//XXX
//                //list.Add("<>");//XXX
//                //list.Add(":;");//XXX


//                foreach(string s in list)
//                {
//                    allIsGood = TestApp.IsAllowableAlphaOrderableName(s);


//                    string g = "l";

//                }

                


//                //string name = "sd!k...      j(hf  +kj)*'**h$-_";//$-_.+!*'(),sd";
//                ////string name = string.Empty;
//                ////   $-_.+!*'(),
//                //Match match = Regex.Match(name, @"^[a-zA-Z0-9$-_.+!*'()\s]+$");

//                //allIsGood= match.Success;

//                //string g = "l";




//                //prince = _Enums.Principal.fox;
//                //_masterCollection = null;

//                ////if (OrderedCollection.o.Count > 1)
//                ////{
//                ////    var result = Utils.LinqUtilities.OrderByDynamic<Employee>(IEnumerable<Employee> query, "");

//                ////}
//                //EmployeeCollection coll = new EmployeeCollection();
//                //coll.AddRange(OrderedCollection.OrderBy(delegate(Employee match) { return (new Employee_Principal(match).Principal); }));


//                //var ordered = OrderedCollection.OrderBy(x => new Employee_Principal(x).PrincipalWeight(prince))
//                //    .ThenBy(x => x.PrincipalOrder_Get(prince))
//                //    .ThenBy(x => x.LastName);


                


//                /*select vcjsonordinal, * from employee
//order by 
//case when charindex('fox',vcprincipal) >= 1 THEN 1 
//WHEN charindex('bt',vcprincipal) >= 1 THEN 2
//when charindex('z2',vcprincipal) >= 1 THEN 3 
//else 4 end 
//                 */

//                ////order in 2 ways - if all then 1 else do it by 2
//                //if(prince == _Enums.Principal.all)
//                //{
                    
//                //    ordered = ordered.ThenBy(

//                //    //var ordered = OrderedCollection.OrderBy(x => new Employee_Principal(x).HasPrincipal(_Enums.Principal.fox))
//                //    //    .ThenBy(x => new Employee_Principal(x).HasPrincipal(_Enums.Principal.bt))
//                //    //    .ThenBy(x => new Employee_Principal(x).HasPrincipal(_Enums.Principal.z2))                        
//                //    //    .ThenBy(x => x.LastName);
//                //}
//                //else
//                //{
//                //}


                
        
//            }
//            catch (Exception ex)
//            {
//                string s = ex.Message;
//            }
//        }
    }
}
