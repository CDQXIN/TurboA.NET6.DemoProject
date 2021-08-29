using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.NET6.AuthenticationCenter.Utility
{
    public class JWTTokenDeserialize
    {
        public static string AnalysisToken(string token)
        {
            string info = token.Split('.')[1];
            string escapeInfo = Escape(info);
            string sInfo = FromBase64(escapeInfo);
            Newtonsoft.Json.JsonConvert.DeserializeObject(sInfo);
            return sInfo;
        }

        public static string ToBase64(string content)
        {
            byte[] byteContent = System.Text.Encoding.Default.GetBytes(content);
            return Convert.ToBase64String(byteContent);
        }

        public static string FromBase64(string result)
        {
            byte[] byteResult = Convert.FromBase64String(result);
            return System.Text.Encoding.Default.GetString(byteResult);
        }

        public static string Escape(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                sb.Append((Char.IsLetterOrDigit(c)
                || c == '-' || c == '_' || c == '\\'
                || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
            }
            return sb.ToString();
        }

        public static string UnEscape(string str)
        {
            StringBuilder sb = new StringBuilder();
            int len = str.Length;
            int i = 0;
            while (i != len)
            {
                if (Uri.IsHexEncoding(str, i))
                    sb.Append(Uri.HexUnescape(str, ref i));
                else
                    sb.Append(str[i++]);
            }
            return sb.ToString();
        }
    }
}
