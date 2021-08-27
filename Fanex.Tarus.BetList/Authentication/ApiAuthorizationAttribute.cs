using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Tarus.BetList.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ApiAuthorizationAttribute : AuthorizeAttribute
    {
        private readonly string APIKey = "kJCrpsMovfqAWrlY1E5sV1+ZpP5/vndJ9gE/QcSbYac=";

        public ApiAuthorizationAttribute()
        {
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string rawAuthzHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(rawAuthzHeader))
            {
                return false;
            }

            var autherizationHeaderArray = GetAutherizationHeaderValues(rawAuthzHeader);
            if (autherizationHeaderArray == null)
            {
                return false;
            }

            string incomingBase64Signature = autherizationHeaderArray[0].Split(' ')[1];
            string nonce = autherizationHeaderArray[1];
            string signatureRawData = string.Format("{0}:{1}", APIKey, nonce);
            using (HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(APIKey)))
            {
                return incomingBase64Signature.Equals(Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(signatureRawData))), StringComparison.Ordinal);
            }
        }

        private string[] GetAutherizationHeaderValues(string rawAuthzHeader)
        {

            var credArray = rawAuthzHeader.Split(':');

            if (credArray.Length == 2)
            {
                return credArray;
            }
            else
            {
                return null;
            }
        }
    }
}
