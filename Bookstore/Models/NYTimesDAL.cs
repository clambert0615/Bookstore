using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class NYTimesDAL
    {
        private readonly string APIKey;
        public NYTimesDAL(IConfiguration configuration)
        {
            APIKey = configuration.GetSection("ApiKeys")["NYTimesAPI"];
        }
        public string GetString()
        {
            string url = $"https://api.nytimes.com/svc/books/v3/lists/current/hardcover-fiction.json?api-key={APIKey}";
            System.Net.HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
         }

        public BestSeller getBestSeller()
        {
            string output = GetString();
            BestSeller s = JsonConvert.DeserializeObject<BestSeller>(output);
            return s;
        }
    }
}
