using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MatechEssential
{
    public class WebHelper
    {
        public static WebResponse Execute(string url, Dictionary<string, string> header, Dictionary<string, string> data, string method)
        {
            string d = "";
            if (data != null)
            {
                foreach (KeyValuePair<string, string> k in data)
                {
                    if (d.Length > 0)
                        d += "&";
                    d += k.Key + "=" + HttpUtility.UrlEncode(k.Value);
                }
            }
            return Execute(url, header, d, method, "application/x-www-form-urlencoded");
        }
        public static WebResponse Execute(string url, Dictionary<string, string> header, string data, string method, string contentType = "application/json")
        {
            WebResponse res = new WebResponse();
            try
            {
                if (method.ToLower() == "get")
                {
                    if (data.Length > 0)
                        url += "?" + data;
                }
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = method;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng;application/json";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";
                request.ContentType = contentType;
                if (method.ToLower() == "post")
                {
                    //request.ContentLength = data.Length;
                    StreamWriter sw = new StreamWriter(request.GetRequestStream());
                    sw.Write(data);
                    sw.Close();
                }
                if (header != null)
                {
                    foreach (KeyValuePair<string, string> k in header)
                    {
                        request.Headers.Add(k.Key, k.Value);
                    }
                }
                //if (method.ToLower() == "post")
                //    request.ContentLength = data.Length;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                res.Data = sr.ReadToEnd();
                res.Status = response.StatusCode;
                sr.Close();
                if (string.IsNullOrEmpty(res.Data))
                    res.Data = response.StatusDescription;
            }
            catch (Exception ex)
            {
                res.Data = ex.Message;
            }
            return res;
        }
    }

    public class WebResponse
    {
        public HttpStatusCode Status { get; set; }
        public string Data { get; set; }
    }
}
