using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace APIDL.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get(string EndPoint)
        {
            try
            {
                HttpClient _client = new HttpClient();
                using (var result = _client.PostAsync(EndPoint.Replace("[and]", "&"), null))
                {
                    return result.Result.Content.ReadAsStringAsync().Result;
                };
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public string Post(PoloParam objPolo)
        {
            try
            {
                objPolo.init();
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("key", objPolo.Key);
                _client.DefaultRequestHeaders.Add("Sign", objPolo.Sign);

                using (var result = _client.PostAsync(objPolo.EndPoint, null))
                {
                    return result.Result.Content.ReadAsStringAsync().Result;
                };
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    public class PoloParam
    {
        public string EndPoint { get; set; }
        public string Key { get; set; }
        public string Sign { get; set; }
        Dictionary<string, object> Param { get; set; }
        public void init()
        {
            Sign = PoloHelper.MD5Hash(Sign);
        }
    }
    public static class PoloHelper
    {
        //private HttpWebRequest CreateHttpWebRequest(string method, string relativeUrl)
        //{
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
        //    var request = WebRequest.CreateHttp(BaseUrl + relativeUrl);
        //    request.Method = method;
        //    request.UserAgent = "Poloniex API .NET v" + Helper.AssemblyVersionString;
        //    //request.Timeout = Timeout.Infinite;
        //    request.Credentials = CredentialCache.DefaultCredentials;

        //    request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
        //    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        //    return request;
        //}
        public static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                return Encoding.ASCII.GetString(result);
            }
        }
    }
}
