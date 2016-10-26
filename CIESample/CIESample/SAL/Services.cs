using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CIESample.SAL
{
    class Services
    {

        public static async Task<string> Get(string url, Dictionary<string, string> queryStringParams = null, int timeout = 30000)
        {
            string retVal = string.Empty;

            try
            {
                
                if (queryStringParams != null && queryStringParams.Count > 0)
                {
                    bool first = true;
                    foreach (string key in queryStringParams.Keys)
                    {
                        if (first)
                        {
                            url = String.Format("{0}?{1}={2}", url, key, queryStringParams[key]);
                            first = false;
                        }
                        else
                        {
                            url = String.Format("{0}&{1}={2}", url, key, queryStringParams[key]);
                        }
                    }
                }

                Console.WriteLine(url);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                request.Timeout = timeout;

                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        {
                            using (Stream stream = response.GetResponseStream())
                            {
                                retVal = new StreamReader(stream).ReadToEnd();
                                return retVal;
                            }
                        }
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (httpResponse != null)
                            Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        if (response != null)
                            using (Stream data = response.GetResponseStream())
                            {
                                using (var reader = new StreamReader(data))
                                {
                                    retVal = httpResponse.StatusCode.ToString();
                                }
                            }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }

        public static async Task<string> GetNowPlayingJSON()
        {
            return await Get(String.Format("http://api.themoviedb.org/3/movie/now_playing?api_key=ab41356b33d100ec61e6c098ecc92140&amp;sort_by=popularity.des"));
        }

        public static async Task<string> GetTopRatedJSON()
        {
            return await Get(String.Format("http://api.themoviedb.org/3/movie/top_rated?api_key=ab41356b33d100ec61e6c098ecc92140&amp;sort_by=popularity.des"));
        }

        public static async Task<string> GetPopularJSON()
        {
            return await Get(String.Format("http://api.themoviedb.org/3/movie/popular?api_key=ab41356b33d100ec61e6c098ecc92140&amp;sort_by=popularity.des"));
        }

        public static async Task<string> GetSimilarJSON(string idPassed)
        {
            var tempUrl = "http://api.themoviedb.org/3/movie/" + idPassed + "?api_key=ab41356b33d100ec61e6c098ecc92140&append_to_response=similar_movies";
            return await Get(String.Format(tempUrl));
        }


    }
}