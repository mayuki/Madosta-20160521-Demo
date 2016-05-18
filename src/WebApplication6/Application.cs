using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6
{
    public class Application
    {
        /// <summary>
        /// 認証方法としてCookieを使う際に識別する文字列を取得します。
        /// </summary>
        public const string AuthenticationTypeCookieAuthentication = "CookieAuthentication";

        /// <summary>
        /// インメモリでFIDOの登録を保持するためのDictionaryを取得します。
        /// </summary>
        public static ConcurrentDictionary<string, string> FidoRegistration { get; } = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }
}
