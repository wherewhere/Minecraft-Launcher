using System.Security.Cryptography;
using System.Text;
using System;

namespace UMCLauncher.Core.Helpers
{
    public static class DataHelper
    {
        public static string GetMD5(this string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            // Convert the input string to a byte array and compute the hash.
            byte[] data = MD5.HashData(Encoding.UTF8.GetBytes(input));
            string results = BitConverter.ToString(data).ToLowerInvariant();
            return results.Replace("-", "");
        }

        public static string GetBase64(this string input, bool israw = false)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            string result = Convert.ToBase64String(bytes);
            if (israw) { result = result.Replace("=", ""); }
            return result;
        }

        public static string Reverse(this string text)
        {
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string GetSizeString(this double size)
        {
            int index = 0;
            while (index <= 11)
            {
                index++;
                size /= 1024;
                if (size > 0.7 && size < 716.8) { break; }
                else if (size >= 716.8) { continue; }
                else if (size <= 0.7)
                {
                    size *= 1024;
                    index--;
                    break;
                }
            }
            string str = string.Empty;
            switch (index)
            {
                case 0: str = "B"; break;
                case 1: str = "KB"; break;
                case 2: str = "MB"; break;
                case 3: str = "GB"; break;
                case 4: str = "TB"; break;
                case 5: str = "PB"; break;
                case 6: str = "EB"; break;
                case 7: str = "ZB"; break;
                case 8: str = "YB"; break;
                case 9: str = "BB"; break;
                case 10: str = "NB"; break;
                case 11: str = "DB"; break;
                default:
                    break;
            }
            return $"{size:0.##}{str}";
        }
    }
}