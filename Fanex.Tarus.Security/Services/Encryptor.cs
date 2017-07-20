using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Tarus.Security.Services
{
    public class Encryptor
    {
        private const string HexDigit = "0123456789abcdef";

        protected Encryptor() { }
        
        public static string Code
        {
            get
            {
                var code = GetCaptchaString(4);

                return code;
            }
        }

        public static string GetCaptchaString(int length)
        {
            var varZero = '0';
            var varNine = '9';
            var varA = 'A';
            var varZ = 'Z';
            var varCount = 0;
            var varRandomNumber = 0;
            var strCaptchaString = string.Empty;
            var random = new Random(System.DateTime.Now.Millisecond);

            while (varCount < length)
            {
                varRandomNumber = random.Next(varZero, varZ);
                if ((varRandomNumber >= varZero && varRandomNumber <= varNine) || (varRandomNumber >= varA && varRandomNumber <= varZ))
                {
                    strCaptchaString = strCaptchaString + (char)varRandomNumber;
                    varCount = varCount + 1;
                }
            }

            return strCaptchaString;
        }

        public static string HexDecode(string data)
        {
            var b16_digits = HexDigit;
            var b16_map = new char[256];
            for (var i = 0; i < 256; i++)
            {
                b16_map[Convert.ToInt32(string.Concat(b16_digits[i >> 4], b16_digits[i & 15]), 16)] = Convert.ToChar(i);
            }

            var c = data.Length;
            if (c % 2 != 0)
            {
                data = '0' + data;
            }

            var result = new List<char>();
            for (var i = 0; i < c; i += 2)
            {
                result.Add(b16_map[Convert.ToInt32(data.Substring(i, 2), 16)]);
            }

            return new string(result.ToArray());
        }

        public static string RC4(string key, string value)
        {
            var ks = key.ToCharArray();
            var vs = value.ToCharArray();
            var c = key.Length;
            var d = value.Length;
            var s = new int[256];
            var i = 0;
            for (i = 0; i < 256; i++)
            {
                s[i] = i;
            }

            var j = 0;
            int x;
            for (i = 0; i < 256; i++)
            {
                j = (j + s[i] + ks[i % c]) % 256;
                x = s[i];
                s[i] = s[j];
                s[j] = x;
            }

            i = 0;
            j = 0;
            var ct = new StringBuilder();
            for (var y = 0; y < d; y++)
            {
                i = (i + 1) % 256;
                j = (j + s[i]) % 256;
                x = s[i];
                s[i] = s[j];
                s[j] = x;
                ct.Append(Convert.ToChar(vs[y] ^ s[(s[i] + s[j]) % 256]));
            }

            return ct.ToString();
        }

        public static string Decrypt(string encrypted, string code)
        {
            return RC4(code, HexDecode(encrypted));
        }
    }
}
