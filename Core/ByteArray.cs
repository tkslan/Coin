using System;
using System.Globalization;

namespace Coin.Core
{
   public class ByteArray
   {
       /* From:
       https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/24343727#24343727
       */
      private static readonly uint[] _lookup32 = CreateLookup32();
      
      private static uint[] CreateLookup32()
      {
          var result = new uint[256];
          for (int i = 0; i < 256; i++)
          {
              string s=i.ToString("X2");
              result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
          }
          return result;
      }
      
      private static string ByteArrayToHexViaLookup32(byte[] bytes)
      {
          var lookup32 = _lookup32;
          var result = new char[bytes.Length * 2];
          for (int i = 0; i < bytes.Length; i++)
          {
              var val = lookup32[bytes[i]];
              result[2*i] = (char)val;
              result[2*i + 1] = (char) (val >> 16);
          }
          return new string(result);
      }

      public static string ByteToHex(byte[] bytes)
      {
          return ByteArrayToHexViaLookup32(bytes);
      }
      
      public static byte[] HexToByte(string hexString)
      {
          if (hexString.Length % 2 != 0)
          {
              throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
          }

          byte[] data = new byte[hexString.Length / 2];
          for (int index = 0; index < data.Length; index++)
          {
              string byteValue = hexString.Substring(index * 2, 2);
              data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
          }

          return data; 
      }
   }
}