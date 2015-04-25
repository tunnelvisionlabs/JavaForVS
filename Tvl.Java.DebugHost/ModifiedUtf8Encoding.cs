namespace Tvl.Java.DebugHost
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Text;

    public class ModifiedUTF8Encoding
    {
        public static unsafe string GetString(byte* data)
        {
            Contract.Requires<ArgumentNullException>(data != null, "data");

            StringBuilder builder = new StringBuilder();
            for (byte* ptr = data; *ptr != 0; ptr++)
            {
                byte x = ptr[0];

                if (x >= 1 && x <= 0x7f)
                {
                    builder.Append((char)x);
                    continue;
                }
                else if ((x & 0xE0) == 0xC0)
                {
                    byte y = ptr[1];
                    if ((y & 0xC0) != 0x80)
                        throw new FormatException();

                    builder.Append((char)(((x & 0x1f) << 6) + (y & 0x3f)));
                    ptr++;
                    continue;
                }
                else if ((x & 0xF0) == 0xE0)
                {
                    byte y = ptr[1];
                    if ((y & 0xC0) != 0x80)
                        throw new FormatException();

                    byte z = ptr[2];
                    if ((z & 0xC0) != 0x80)
                        throw new FormatException();

                    builder.Append((char)(((x & 0xF) << 12) + ((y & 0x3F) << 6) + (z & 0x3F)));
                    ptr += 2;
                    continue;
                }
            }

            return builder.ToString();
        }

        public static string GetString(byte[] bytes, int index, int count)
        {
            Contract.Requires<ArgumentNullException>(bytes != null, "bytes");
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0);

            StringBuilder builder = new StringBuilder();
            for (int i = index; i < index + count; i++)
            {
                byte x = bytes[i];

                if (x >= 1 && x <= 0x7f)
                {
                    builder.Append((char)x);
                    continue;
                }
                else if ((x & 0xE0) == 0xC0)
                {
                    if (i >= index + count - 1)
                        throw new FormatException();

                    byte y = bytes[i + 1];
                    if ((y & 0xC0) != 0x80)
                        throw new FormatException();

                    builder.Append((char)(((x & 0x1f) << 6) + (y & 0x3f)));
                    i++;
                    continue;
                }
                else if ((x & 0xF0) == 0xE0)
                {
                    if (i >= index + count - 2)
                        throw new FormatException();

                    byte y = bytes[i + 1];
                    if ((y & 0xC0) != 0x80)
                        throw new FormatException();

                    byte z = bytes[i + 2];
                    if ((z & 0xC0) != 0x80)
                        throw new FormatException();

                    builder.Append((char)(((x & 0xF) << 12) + ((y & 0x3F) << 6) + (z & 0x3F)));
                    i+=2;
                    continue;
                }
            }

            return builder.ToString();
        }

        public static byte[] GetBytes(string s, bool nullTerminated = true)
        {
            List<byte> bytes = new List<byte>(s.Length + 1);
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c >= 1 && c <= 0x7F)
                {
                    bytes.Add((byte)c);
                }
                else if (c == 0 || (c >= 0x80 && c <= 0x07FF))
                {
                    bytes.Add((byte)(0xC0 | (c >> 6)));
                    bytes.Add((byte)(0x80 | (c & 0x3F)));
                }
                else
                {
                    bytes.Add((byte)(0xE0 | (c >> 12)));
                    bytes.Add((byte)(0x80 | ((c >> 6) & 0x3F)));
                    bytes.Add((byte)(0x80 | (c & 0x3F)));
                }
            }

            if (nullTerminated)
                bytes.Add(0);

            return bytes.ToArray();
        }
    }
}
