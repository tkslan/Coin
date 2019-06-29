using System;
using System.Security.Cryptography;
using System.Text;

namespace Coin.Core
{
    public class Key
    {
        public string Public => ByteToString(_publicKey);
        public string Adress => GetAdressFromPublicKey();
        public string Private => ByteToString(_privateKey);

        private readonly byte[] _publicKey, _privateKey;
        private Rebex.Security.Cryptography.Ed25519 _ed25519;

        public Key(string seed = null)
        {
            _ed25519 = new Rebex.Security.Cryptography.Ed25519();
            var sha256 = new SHA256Managed();
            sha256.Initialize();
            _ed25519.FromSeed(
                sha256.ComputeHash(string.IsNullOrEmpty(seed) ? GetSeedKey() : Encoding.UTF8.GetBytes(seed)));

            _publicKey = _ed25519.GetPublicKey();
            _privateKey = _ed25519.GetPrivateKey();
        }

        public string Sing(byte[] message)
        {
            return ByteToString(_ed25519.SignMessage(message));
        }

        string GetAdressFromPublicKey()
        {
            return Base58Check.Base58CheckEncoding.Encode(_publicKey);
        }

        string ByteToString(byte[] buff)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < buff.Length; i++)
            {
                builder.Append(buff[i].ToString("X2")); // hex format
            }

            return builder.ToString();
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