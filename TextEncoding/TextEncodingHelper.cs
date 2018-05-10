using System.Globalization;
using CgmInfo.Commands.Enums;

namespace CgmInfo.TextEncoding
{
    internal static class TextEncodingHelper
    {
        // returns a culture closest to what the clear text encoding looks like.
        // neutral English seems to work just fine.
        public static readonly CultureInfo Culture = new CultureInfo("en");

        // returns the amount of bits (multiples of a byte) required to store input
        public static int GetBitPrecision(int input)
        {
            if (input <= 0xFF)
                return 8;
            else if (input <= 0xFFFF)
                return 16;
            else if (input <= 0xFFFFFF)
                return 24;
            return 32;
        }
        public static int GetBitPrecision(int minValue, int maxValue)
        {
            // min is either 0 or negative, so subtracting it from max gives us roughly the number of values possible
            return GetBitPrecision(maxValue - minValue);
        }
        public static OnOffIndicator GetOnOffValue(string token)
        {
            if (token.ToUpperInvariant() == "ON")
                return OnOffIndicator.On;
            return OnOffIndicator.Off;
        }
    }
}
