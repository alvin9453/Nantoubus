using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Nantou_bus.Model.TransportData
{
    public class News
    {
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string Updatetime { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public List<object> Stop { get; set; }

        public static List<News> RetrieveFromJson(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        string jsonString = client.DownloadString(url);
                        List<News> entities = JsonConvert.DeserializeObject<List<News>>(jsonString);
                        return entities;
                    }
                    catch(JsonReaderException jsonEx) { return new List<News>(); }
                }
            }
            catch (WebException ex) { return new List<News>(); }
        }
    }
}
