using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;

namespace openprocurement.api.client.Extensions
{
    public static class UriExtensions
    {
        public static Uri Append(this Uri uri, params string[] paths)
        {
            return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
        }


        public static Uri Query(this Uri uri, Dictionary<string, object> parameters)
        {
            string queryString =
                string.Join("&",
                    parameters.Select(kvp =>
                        string.Format("{0}={1}", kvp.Key, HttpUtility.UrlEncode(Convert.ToString(kvp.Value)))));

            UriBuilder baseUri = new UriBuilder(uri);
            baseUri.Query = queryString;
            return baseUri.Uri;
        }


        
    }
}
