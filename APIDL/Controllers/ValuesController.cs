using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public string Post([FromBody]PoloParam objPolo)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(objPolo.EndPoint);
                    StringContent myContent = new StringContent(objPolo.myParam);
                    myContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    client.DefaultRequestHeaders.Add("Key", objPolo.Key);
                    client.DefaultRequestHeaders.Add("Sign", genHMAC(objPolo.myParam,objPolo.Sign));
                    var result =  client.PostAsync(objPolo.EndPoint, myContent);
                    return result.Result.Content.ReadAsStringAsync().Result;
                }
               
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private string genHMAC(string message,string ApiSecret)
        {
            var hmac = new HMACSHA512(Encoding.ASCII.GetBytes(ApiSecret));
            var messagebyte = Encoding.ASCII.GetBytes(message);
            var hashmessage = hmac.ComputeHash(messagebyte);
            var sign = BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            return sign;
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
        public string myParam { get; set; }
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
