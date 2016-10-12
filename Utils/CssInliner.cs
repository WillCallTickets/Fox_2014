using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Utils
{
    /// <summary>
    /// This is reponsible for converting an html email, with styles, into an inlined html email without style - beyond what may be ignored
    /// by the premailer api call. Things such as media queries should be left in a style block
    /// https://github.com/milkshakesoftware/PreMailer.Net/blob/master/README.md
    /// 
    /// string htmlSource = File.ReadAllText(@"C:\Workspace\testmail.html");
    ///
    ///    var result = PreMailer.MoveCssInline(htmlSource);
    ///    result.Html         // Resultant HTML, with CSS in-lined.
    ///    result.Warnings     // string[] of any warnings that occurred during processing.
    ///
    /// The following options can be passed to the PreMailer.MoveCssInline method to configure it's behaviour:
    /// 
    ///     removeStyleElements(bool = false) - Removes elements that were used to source CSS (currently, only style is supported).
    ///     ignoreElements(string = null) - CSS selector of element(s) not to inline. Useful for mobile styles (see below).
    /// 
    /// 
    /// To ignore a style block, you need to specify an ignore selector when calling the MoveCssInline method, like this:
    ///
    ///     var result = PreMailer.MoveCssInline(input, false, ignoreElements: "#ignore");
    ///     And your mobile specific style block should have an ID of ignore:
    ///
    ///     <style type="text/css" id="ignore">.target { width: 1337px; }</style>
    ///    
    /// Pseudo classes/elements are not supported by CsQuery (which PreMailer.Net uses internally). Any that are encountered in 
    /// your HTML will be ignored and logged to the InlineResult.Warnings collection.
    /// </summary>
    public partial class CssInliner
    {
        public string HtmlAfter = null;
        PreMailer.Net.InlineResult Result = null;

        public CssInliner(string htmlDoc)
        {
            try
            {
                Result = PreMailer.Net.PreMailer.MoveCssInline(htmlDoc, true, ignoreElements: "#ignore");
                HtmlAfter = Result.Html;                
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                throw ex;
            }
        }
    }
}
