using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    /// <summary>
    /// Allows us to obfuscate email addresses. This class handles the server side "encoding"
    /// Use Jquery to respond to the clicks and parse/form the proper link
    /// </summary>
    public class MailtoHelper
    {
        public List<string> mailto { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string innerHtml { get; set; }
        public string linkClass { get; set; }
        public string toolTip { get; set; }

        public MailtoHelper()
        {
            mailto = new List<string>();
            subject = string.Empty;
            body = string.Empty;
            innerHtml = string.Empty;
            linkClass = string.Empty;
            toolTip = string.Empty;
        }

        public MailtoHelper(string _mailtoAddress, string _subject, string _body, string _innerHtml) :
            this(_mailtoAddress, _subject, _body, _innerHtml, string.Empty)
        {
        }

        public MailtoHelper(string _mailtoAddress, string _subject, string _body, string _innerHtml, string _linkClass) :
            this(_mailtoAddress, _subject, _body, _innerHtml, _linkClass, string.Empty)
        {
        }

        public MailtoHelper(string _mailtoAddress, string _subject, string _body, string _innerHtml, string _linkClass, string _toolTip)
        {
            mailto = new List<string>();
            string[] pieces = _mailtoAddress.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in pieces)
                mailto.Add(s.Trim());

            subject = _subject;
            body = _body;
            innerHtml = _innerHtml;
            linkClass = _linkClass;
            toolTip = _toolTip;
        }

        public string FormattedMailtoLink
        {
            get
            {
                //string mail = (this.mailto.Trim().Length > 0) ?
                //    this.mailto.Replace("@", "[atsign]").Replace(".", "[dotsign]").Trim() : string.Empty;

                StringBuilder sb = new StringBuilder();                

                sb.AppendFormat("<a href=\"\" class=\"{0}ml-mailer\" {1} >", 
                    (this.linkClass.Trim().Length > 0) ? string.Format("{0} ", this.linkClass.Trim()) : string.Empty,
                    (this.toolTip.Trim().Length > 0) ? string.Format("title=\"{0}\" ", this.toolTip.Trim()) : string.Empty
                    );
                sb.AppendLine(); 
                sb.AppendLine(innerHtml);

                if (mailto.Count > 0)
                {
                    sb.AppendFormat("<span style=\"display:none;\" class=\"ml-rec\">");
                    sb.AppendLine();

                    int i = 0;
                    foreach(string s in mailto)
                    {
                        sb.AppendFormat("{0}{1}", s.Replace("@", "[atsign]").Replace(".", "[dotsign]").Trim(),
                            (i++ < mailto.Count - 1) ? "," : "");
                        sb.AppendLine();
                    }
                    sb.AppendLine("</span>");
                    sb.AppendLine();
                }
                if (subject.Trim().Length > 0)
                {
                    sb.AppendFormat("<span style=\"display:none;\" class=\"ml-subject\">{0}</span>", this.subject);
                    sb.AppendLine();
                }
                if (body.Trim().Length > 0)
                {
                    sb.AppendFormat("<span style=\"display:none;\" class=\"ml-body\">{0}</span>", body);
                    sb.AppendLine();
                }

                sb.AppendLine("</a>");

                return sb.ToString();
            }
        }
    }
}
