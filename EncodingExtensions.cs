using System.Text;

namespace CgmInfo
{
    internal static class EncodingExtensions
    {
        public static bool Supports(this Encoding encoding, string s)
        {
            // assume a null encoding is never supported
            if (encoding == null)
                return false;
            // assume every encoding supports an empty string
            if (string.IsNullOrEmpty(s))
                return true;

            byte[] originalBytes = encoding.GetBytes(s);
            string roundtrip = encoding.GetString(originalBytes);
            return s == roundtrip;
        }
    }
}
