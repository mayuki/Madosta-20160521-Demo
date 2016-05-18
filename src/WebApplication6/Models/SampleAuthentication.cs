using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public static class SampleAuthentication
    {
        public static ConcurrentDictionary<string, UserRegistration> Users { get; } = new ConcurrentDictionary<string, UserRegistration>(StringComparer.CurrentCultureIgnoreCase);

        static SampleAuthentication()
        {
            Register("alice@example.com", "Password1", "Alice Cartelet");
            Register("karen@example.com", "Password1", "Karen Kujo");
        }

        public static ClaimsIdentity Authenticate(bool useFido, string userName, string password = null, string clientData = null, string authenticatorData = null, string signature = null, string challenge = null)
        {
            UserRegistration userRegistration;
            if (Users.TryGetValue(userName, out userRegistration))
            {
                if (useFido)
                {
                    if (FidoAuthenticator.ValidateSignature(userRegistration.PublicKey, clientData, authenticatorData, signature, challenge))
                    {
                        return userRegistration.Identity;
                    }
                }
                else if (CreateHash(password, userRegistration.Salt) == userRegistration.PasswordHash)
                {
                    return userRegistration.Identity;
                }
            }

            return null;
        }

        public static bool Register(string userName, string password, string displayName = null)
        {
            if (String.IsNullOrWhiteSpace(userName) && String.IsNullOrWhiteSpace(password)) return false;

            var salt = "HauhauHauhau"; // サンプルなので固定なのです
            var userRegistration = new UserRegistration()
            {
                Salt = salt,
                PasswordHash = CreateHash(password, salt),
                Identity = new ClaimsIdentity(
                    new[] {
                        new Claim(ClaimTypes.Name, displayName ?? userName),
                        new Claim(ClaimTypes.Email, userName),
                        new Claim(ClaimTypes.NameIdentifier, userName),
                        new Claim(ClaimTypes.Role, "User"),
                    },
                    Application.AuthenticationTypeCookieAuthentication,
                    ClaimTypes.Name,
                    ClaimTypes.Role
                )
            };

            return Users.TryAdd(userName, userRegistration);
        }

        private static string CreateHash(string value, string salt)
        {
            return String.Join("", SHA256.Create().ComputeHash(new UTF8Encoding(false).GetBytes((value ?? "") + "=" + salt)).Select(x => x.ToString("x2")));
        }
    }

    public class UserRegistration
    {
        public ClaimsIdentity Identity { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string PublicKey { get; set; }
    }
}
