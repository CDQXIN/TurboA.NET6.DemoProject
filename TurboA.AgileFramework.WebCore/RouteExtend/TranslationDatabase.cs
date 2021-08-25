using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.AgileFramework.WebCore.RouteExtend
{
    //    /// <summary>
    //    /// 数据源
    //    /// </summary>
    //    public class TranslationDatabase
    //    {
    //        /// <summary>
    //        /// https://localhost:5001/en/home/index
    //        /// https://localhost:5001/ch/home1/index1
    //        /// https://localhost:5001/hk/home2/index2
    //        /// 
    //        /// </summary>
    //        private static Dictionary<string, Dictionary<string, string>> Translations
    //            = new Dictionary<string, Dictionary<string, string>>
    //        {
    //        {
    //            "en", new Dictionary<string, string>
    //            {
    //                { "home", "Home" },
    //                { "index", "Index" }
    //            }
    //        },
    //        {
    //            "ch", new Dictionary<string, string>
    //            {
    //                { "home1", "Home" },
    //                { "index1", "Index" }
    //            }
    //        },
    //        {
    //            "hk", new Dictionary<string, string>
    //            {
    //                { "home2", "Home" },
    //                { "index2", "Index" }
    //            }
    //        },
    //        };

    //        public async Task<string> Resolve(string lang, string value)
    //        {
    //            var normalizedLang = lang.ToLowerInvariant();
    //            var normalizedValue = value.ToLowerInvariant();
    //            if (Translations.ContainsKey(normalizedLang)
    //                && Translations[normalizedLang]
    //                    .ContainsKey(normalizedValue))
    //            {
    //                return Translations[normalizedLang][normalizedValue];
    //            }

    //            return null;
    //        }
    //    }
}
