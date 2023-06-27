using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MatechEssential
{
    public class PushNotification
    {
        public static string GCMUrl = "https://fcm.googleapis.com/fcm/send";

        public static void Send(string AuthToken, string[] DeviceTokens, Notification notification)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Authorization", "key=" + AuthToken);
            WebResponse res = WebHelper.Execute(GCMUrl, header, notification.Serialize(), "POST");
            if (res.Status == HttpStatusCode.OK)
            {

            }
            else
            {
                throw new Exception(res.Data);
            }
        }
    }

    public class Notification
    {
        public string[] deviceTokens { get; set; }
        public Message message { get; set; }
        public object data { get; set; }
        public Notification()
        {
            message = new Message();
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class Message
    {
        public string title { get; set; }
        public string text { get; set; }
    }
}
