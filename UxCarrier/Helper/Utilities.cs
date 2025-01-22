using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace UxCarrier.Helper
{
    public static class Utilities
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public static TripleDES GetTripleDES => TripleDES.Create();
        public static bool IsNotNull([NotNullWhen(true)] object? obj) => obj != null;
        public static bool IsNull([NotNullWhen(false)] object? obj) => obj == null;

        public static bool TryFromBase64String(string bs64String)
        {
            return Convert.TryFromBase64String(bs64String, new Span<byte>(new byte[bs64String.Length]), out int bytesParsed);
        }

        public static string EncodeBase64(string inputText)
        {
            byte[] bytesEncode = Encoding.UTF8.GetBytes(inputText); //取得 UTF8 2進位 Byte
            return Convert.ToBase64String(bytesEncode); // 轉換 Base64 索引表
        }

        public static string DecodeBase64(string inputText)
        {
            byte[] bytesDecode = Convert.FromBase64String(inputText); // 還原 Byte
            return Encoding.UTF8.GetString(bytesDecode); // 還原 UTF8 字元
        }

        public static IEnumerable<KeyValuePair<string, string>> GetKeyValuePair(this object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            var keyValuePairList = new List<KeyValuePair<string, string>>();
            foreach (var x in properties)
            {
                KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(x.Name, x.GetValue(obj)!.ToString()!);
                keyValuePairList.Add(keyValuePair);

            }
            return keyValuePairList.AsEnumerable();
        }

        public static StringBuilder GetFormPostString(string url, NameValueCollection data)
        {
            StringBuilder s = new StringBuilder();
            s.Append("<html>");
            s.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
            s.AppendFormat("<form name='form' action='{0}' method='post'>", url);
            foreach (string key in data)
            {
                s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", key, data[key]);
            }
            s.Append("</form></body></html>");
            return s;
        }

        public static string EncryptKey(this byte[] data)
        {
            return Convert.ToBase64String(AppResource.Instance.EncryptSalted(data));
        }
        public static string EncryptData(this string data)
        {
            return Encoding.Unicode.GetBytes(data).EncryptKey();
        }

        public static string DecryptData(this string data)
        {
            return Encoding.Unicode.GetString(AppResource.Instance.DecryptSalted(Convert.FromBase64String(data)));
        }

        public static char[] DISCERNIBLE_CODE = {'2', '3', '4', '5', '6', '8',
                                    '9', 'A', 'B', 'C', 'D', 'E',
                                    'F', 'G', 'H', 'J', 'K', 'L',
                                    'M', 'N', 'P', 'R', 'S', 'T',
                                    'W', 'X', 'Y'};

        public static string CreateRandomStringCode(this int codeLength)
        {
            //驗證碼的字元集，去掉了一些容易混淆的字元
            Thread.Sleep(1);
            Random oRnd = new Random();
            char[] sCode = new char[codeLength];

            //生成驗證碼字串
            for (int n = 0; n < codeLength; n++)
            {
                sCode[n] = DISCERNIBLE_CODE[oRnd.Next(DISCERNIBLE_CODE.Length)];
            }
            return new string(sCode);
        }

        public static string ErrorMessage(this ModelStateDictionary modelState)
        {
            return string.Join("、", modelState.Keys.Where(k => modelState[k].Errors.Count > 0)
                    .Select(k => /*k + " : " +*/ string.Join("/", modelState[k].Errors.Select(r => r.ErrorMessage))));
        }

        public static string EmailMasking(string input)
        {
            string pattern = @"(?<=[\w]{1})[\w\-._\+%]*(?=[\w]{1}@)";
            return Regex.Replace(input, pattern, m => new string('*', m.Length));
        }
        public static string StringMask(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 3)
                return value;
            else if (value.Length <= 4)
                return value.Substring(0, 2) +
                       new string('*', value.Length - 2);
            else
                return value.Substring(0, 2) +
                       new string('*', value.Length - 3) +
                       value.Substring(value.Length - 1);
        }

        public static string HmacSHA256(string secret, string signKey)
        {
            string signRet = string.Empty;
            if (string.IsNullOrEmpty(secret)||string.IsNullOrEmpty(signKey)) return string.Empty;
            using (HMACSHA256 mac = new HMACSHA256(Encoding.UTF8.GetBytes(signKey)))
            {
                byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(secret));
                signRet = Convert.ToBase64String(hash);
                //signRet = ToHexString(hash); ;
            }
            return signRet;
        }

        public static string GetRandomCharacters(int n = 10, bool Number = true, bool Lowercase = false, bool Capital = false)  // 生成随机字符串
        {
            StringBuilder tmp = new StringBuilder();
            Random rand = new Random();
            string characters = (Capital ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ" : null) + (Number ? "0123456789" : null) + (Lowercase ? "abcdefghijklmnopqrstuvwxyz" : null);
            if (characters.Length < 1)
            {
                return (null);
            }
            for (int i = 0; i < n; i++)
            {
                tmp.Append(characters[rand.Next(0, characters.Length)].ToString());
            }
            return (tmp.ToString());
        }

        public static string ConvertToQueryString<T>(T entity, bool ifUrlEncode=true) where T : class
        {
            var props = typeof(T).GetProperties();

            if (ifUrlEncode)
                return $"{string.Join('&', props.Where(r => r.GetValue(entity) != null).Select(r => $"{HttpUtility.UrlEncode(r.Name)}={HttpUtility.UrlEncode(r.GetValue(entity).ToString())}"))}";
            else
                return $"{string.Join('&', props.Where(r => r.GetValue(entity) != null).Select(r => $"{r.Name}={r.GetValue(entity).ToString()}"))}";
        }

        //public static string GetJWTTokenClaim(string token, string claimName)
        //{

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
        //    var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
        //    return claimValue;
        //}
    }
}
