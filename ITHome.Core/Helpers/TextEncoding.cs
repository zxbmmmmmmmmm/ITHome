using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHome.Helpers
{
    public class TextEncoding
    {
        public readonly static Encoding Utf8 = Encoding.UTF8;
        public readonly static Encoding Utf8Bom = new UTF8Encoding(true);
        public readonly static Encoding Utf8NoBom = new UTF8Encoding(false);
        public readonly static byte[] Utf8BomBytes = { 0xEF, 0xBB, 0xBF };

        public static bool HasBom(byte[] bytes)
        => bytes[0] == Utf8BomBytes[0] && bytes[1] == Utf8BomBytes[1] && bytes[2] == Utf8BomBytes[2];

        public static string GetUtf8RemovedBomString(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            if (HasBom(bytes))
                return Utf8.GetString(bytes, 3, bytes.Length - 3);
            return Utf8.GetString(bytes);
        }
    }

}
