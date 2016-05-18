using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
//using System.Web;
//using System.Web.Script.Serialization;

namespace WebApplication6.Models
{
    public class FidoAuthenticator
    {
        public static string GetChallenge()
        {
            var buffer = new Byte[32];
            RandomNumberGenerator.Create().GetBytes(buffer);
            var a = String.Join("", buffer.Select(x => x.ToString("x2")));
            var hash = String.Join("", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("Hauhau37564" + a)).Select(x => x.ToString("x2")));
            return (a + "_" + hash);
        }

        public static bool ValidateSignature(string pk, string clientData, string authnrData, string signature, string challenge)
        {
            try
            {
                var c = rfc4648_base64_url_decode(clientData);
                var a = rfc4648_base64_url_decode(authnrData);
                var s = rfc4648_base64_url_decode(signature);

                // Make sure the challenge in the client data matches the expected challenge
                var cc = Encoding.ASCII.GetString(c);
                cc = cc.Replace("\0", "").Trim();
                var j = JsonConvert.DeserializeObject<Dictionary<string, object>>(cc);
                //var json = new JavaScriptSerializer();
                //var j = (System.Collections.Generic.Dictionary<string, object>)json.DeserializeObject(cc);
                if ((string)j["challenge"] != challenge) return false;

                // Hash data with sha-256
                var hash = SHA256.Create();
                var h = hash.ComputeHash(c);

                // Create data buffer to verify signature over
                var b = new byte[a.Length + h.Length];
                a.CopyTo(b, 0);
                h.CopyTo(b, a.Length);

                // Load public key
                //j = (System.Collections.Generic.Dictionary<string, object>)json.DeserializeObject(pk);
                j = JsonConvert.DeserializeObject<Dictionary<string, object>>(pk);
                var keyinfo = new RSAParameters();
                keyinfo.Modulus = rfc4648_base64_url_decode((string)j["n"]);
                keyinfo.Exponent = rfc4648_base64_url_decode((string)j["e"]);
                var rsa = RSA.Create();
                rsa.ImportParameters(keyinfo);

                // Verify signature is correct for authnrData + hash
                return rsa.VerifyData(b, s, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static byte[] rfc4648_base64_url_decode(string url)
        {
            url = url.Replace('-', '+');
            url = url.Replace('_', '/');

            switch (url.Length % 4)
            { // Pad with trailing '='s
                case 0:
                    // No pad chars in this case
                    break;
                case 2:
                    // Two pad chars
                    url += "==";
                    break;
                case 3:
                    // One pad char
                    url += "=";
                    break;
                default:
                    throw new Exception("Invalid string.");
            }
            return Convert.FromBase64String(url);
        }
    }
}