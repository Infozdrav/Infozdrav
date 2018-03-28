using System.Text;

namespace Infozdrav.Web.Helpers
{
    public static class StringHelpers
    {
        public static byte[] GetUTF8Bytes(this string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        public static int Dolzina(this string s)
        {
            return s.Length;
        }
    }
}