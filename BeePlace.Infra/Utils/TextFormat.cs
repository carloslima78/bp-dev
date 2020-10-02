using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Security.Cryptography;
using System.Globalization;

namespace BeePlace.Infra.Utils
{
    public class TextFormat
    {
        public static string RemoveAccentuation(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return String.Empty;

            byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(texto);

            string pattern = @"(?i)[^0-9a-záéíóúàèìòùâêîôûãõçÂÃ\s]";
            string replacement = "";
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(System.Text.Encoding.UTF8.GetString(bytes), replacement);

            return result;
        }



        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// CONVERTE STRING PARA DATETIME UNIVERSAL
        /// </summary>
        /// <param name="dt">DATA EM STRING</param>
        /// <returns></returns>
        public static DateTime DateTimeStrToUniversalDateTime(string dt)
        {
            string ret = "";

            dt = (String.IsNullOrEmpty(dt) ? "01-01-1900T00:00:00" : dt).Trim();

            if (dt.Length == 10) //20181202
                ret = dt.Substring(6, 4) + "-" + dt.Substring(3, 2) + "-" + dt.Substring(0, 2) + "T00:00:00";
            else if (dt.Length == 19)
                ret = dt.Substring(6, 4) + "-" + dt.Substring(3, 2) + "-" + dt.Substring(0, 2) + "T" + dt.Substring(11, 8);
            else if (dt.Length == 8)
                ret = dt.Substring(0, 4) + "-" + dt.Substring(4, 2) + "-" + dt.Substring(6, 2) + "T00:00:00";

            return Convert.ToDateTime(ret);
        }

        public static string DateTimeToGMTString()
        {
            DateTime a = DateTime.Now;

            string b = string.Format(
                a.DayOfWeek.ToString() +
                ", " +
                a.Day +
                " " +
                a.Month +
                " " +
                a.Year +
                " " +
                a.Hour +
                ":" +
                a.Minute +
                ":" +
                a.Second +
                " GMT"
                );

            return b;
        }


        public static string HashToString(string cnpj, string companyName, string secretKey) //GERAÇÃO DE HMACSHA512
        {
            foreach (var c in new string[] { "/", "-", " ", "." })
            {
                cnpj = cnpj.Replace(c, "");
            }

            string StrCodification = string.Format("{0}-{1}", cnpj, companyName);

            byte[] keyByte = Encoding.ASCII.GetBytes(secretKey);
            byte[] bytes = new HMACSHA512(keyByte).ComputeHash(Encoding.UTF8.GetBytes(StrCodification));

            var signature = ByteToString(bytes);

            return signature;
        }

        public static string HashToString(string strCodification, string secretKey) //GERAÇÃO DE HMACSHA256
        {
            byte[] keyByte = Encoding.ASCII.GetBytes(secretKey);
            byte[] bytes = new HMACSHA256(keyByte).ComputeHash(Encoding.UTF8.GetBytes(strCodification));


            // var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);




            var signature = System.Convert.ToBase64String(bytes);

            //ByteToString(bytes);

            return signature;
        }

        public static string ByteToString(byte[] buff) //GERAÇÃO DE HMAC
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); //hex format
            }
            return (sbinary);
        }
    }
}
