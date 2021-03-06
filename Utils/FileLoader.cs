using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace Utils
{
	/// <summary>
	/// Summary description for FileLoader.
	/// </summary>
	[Serializable()]
	public class FileLoader
    {
        public static void WriteDownloadToContext(string fileContents, string attachment)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            context.Response.Clear();
            context.Response.ClearContent();
            context.Response.ClearHeaders();
            context.Response.ContentType = "application/x-download";//"text/csv";
            context.Response.AddHeader("Content-Disposition", attachment);

            try
            {
                context.Response.Write(fileContents);
                context.Response.End();//this may thread abort

                return;
            }
            catch (System.Threading.ThreadAbortException)
            {
                //we can safely ignore this error 
                return;
            }
            /*
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            Encoding encoding = Encoding.UTF8;
            var bytes = encoding.GetBytes(sb.ToString());
            MemoryStream stream = new MemoryStream(bytes);
            StreamReader reader = new StreamReader(stream);
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.AddHeader("content-disposition", attachment);
            context.Response.Charset = encoding.EncodingName;
            //context.Response.ContentType = "application/text";
            context.Response.ContentType = "application/x-download";//"text/csv";
            context.Response.ContentEncoding = Encoding.Unicode;
            context.Response.Output.Write(reader.ReadToEnd());
            context.Response.Flush();
            context.Response.End();
            */
        }


        public static void SaveToFile(string mappedPathAndName, string body)
        {
            SaveToFile(mappedPathAndName, body, false);
        }

        public static void SaveToFile(string mappedPathAndName, string body, bool overwriteExisting)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                if (System.Web.HttpContext.Current != null)
                {      
                    if(overwriteExisting)
                    {
                        fs = new FileStream(mappedPathAndName, FileMode.Create, FileAccess.Write);
                    }
                    else
                        fs = new FileStream(mappedPathAndName, FileMode.Append, FileAccess.Write);

                    sw = new StreamWriter(fs);

                    sw.Write(body);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (sw != null) sw.Close();
            if (fs != null) fs.Close();
        }

        public static string FileToString(string mappedFileName)
        {
            FileStream fs = null;
            StreamReader sr = null;

            try
            {
                using (fs = new FileStream(mappedFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    sr = new StreamReader(fs, Encoding.GetEncoding("ISO-8859-1"));

                    string fileData = sr.ReadToEnd();

                    return fileData;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();
            }
        }		
	}
}
