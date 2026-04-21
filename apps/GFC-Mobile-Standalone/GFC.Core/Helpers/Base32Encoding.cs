// [NEW]
using System;
using System.Text;

namespace GFC.Core.Helpers
{
    public static class Base32Encoding
    {
        private const string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static string ToString(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var builder = new StringBuilder();
            var bits = 0;
            var bitCount = 0;

            foreach (var b in data)
            {
                bits = (bits << 8) | b;
                bitCount += 8;

                while (bitCount >= 5)
                {
                    var index = (bits >> (bitCount - 5)) & 31;
                    builder.Append(Base32Chars[index]);
                    bitCount -= 5;
                }
            }

            if (bitCount > 0)
            {
                var index = (bits << (5 - bitCount)) & 31;
                builder.Append(Base32Chars[index]);
            }

            return builder.ToString();
        }
    }
}
