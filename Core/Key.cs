using System;
using System.Security.Cryptography;
using System.Text;

namespace Coin.Core
{
   public class Key
   {
      public string Public => ArrayToHex(_publicKey);
      public string Address => GetAdressFromPublicKey();
      public string Private => ArrayToHex(_privateKey);

      private readonly byte[] _publicKey, _privateKey;
      private Rebex.Security.Cryptography.Ed25519 _ed25519;

      private SHA256Managed _sha256;

      public Key(string seed = null)
      {
         _ed25519 = new Rebex.Security.Cryptography.Ed25519();
         _sha256 = new SHA256Managed();
         _ed25519.FromSeed(
            _sha256.ComputeHash(string.IsNullOrEmpty(seed) ? GetSeedKey() : Encoding.UTF8.GetBytes(seed)));

         _publicKey = _ed25519.GetPublicKey();
         _privateKey = _ed25519.GetPrivateKey();
      }

      public Key(byte[] fromPublicKey)
      {
         _ed25519 = new Rebex.Security.Cryptography.Ed25519();
         _sha256 = new SHA256Managed();
         _ed25519.FromPublicKey(fromPublicKey);
         _publicKey = _ed25519.GetPublicKey();
      }

      public string Sing(byte[] message)
      {
         return ArrayToHex(_ed25519.SignMessage(message));
      }

      public bool Verify(byte[] message, byte[] fingerprint)
      {
         return _ed25519.VerifyMessage(message, fingerprint);
      }

      string GetAdressFromPublicKey()
      {
         var ripemd160 = new RIPEMD160Managed();
         var bytes = new byte[21];
         bytes[0] = 0x41;
         ripemd160.ComputeHash(_sha256.ComputeHash(_publicKey)).CopyTo(bytes, 1);
         ripemd160.Dispose();
         var base58 = Base58Check.Base58CheckEncoding.Encode(bytes);
         return base58;
      }

      public bool ValidateAdress(string address)
      {
         var checksumSize = 4;
         var bytes = Base58Check.Base58CheckEncoding.DecodePlain(address);
         var source = new byte[checksumSize];
         var result = new byte[checksumSize];
         Buffer.BlockCopy(bytes, bytes.Length - checksumSize, source, 0, checksumSize);

         var shortBytes = new byte[21];
         for (int i = 0; i < bytes.Length - checksumSize; i++)
         {
            shortBytes[i] = bytes[i];
         }

         var checksum = _sha256.ComputeHash(_sha256.ComputeHash(shortBytes));

         Buffer.BlockCopy(checksum, 0, result, 0, checksumSize);

         var areEquals = true;
         for (int i = 0; i < checksumSize; i++)
         {
            if (source[i] != result[i])
               areEquals = false;
         }

         return areEquals;
      }

      string ArrayToHex(byte[] buff)
      {
         return ByteArray.ByteToHex(buff);
      }

      private byte[] GetSeedKey()
      {
         var secretkey = new Byte[64];
         //RNGCryptoServiceProvider is an implementation of a random number generator.
         using (var rng = new RNGCryptoServiceProvider())
         {
            // The array is now filled with cryptographically strong random bytes.
            rng.GetBytes(secretkey);
         }
         return secretkey;
      }
   }
}