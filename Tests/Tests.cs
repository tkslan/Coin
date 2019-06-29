using Coin.Core;
using NUnit.Framework;

namespace Coin.Tests
{
   [TestFixture]
   public class KeyTests
   {
      private const string seed1 = "1";
      private const string seed1PrivateKey = "CD03FBDDCAAA2703C251656D5CCDD99F5635B1E0653C0636B951A3A3DB21DAD4";
      private const string seed1Adress = "TW1LwnxAs9T6YDgF937c2eJLw6jD5cWKbP";

      [Test]
      public void KeyIsNotNull()
      {
         var key = new Key();
         Assert.NotNull(key);
      }

      [Test]
      public void KeyFromSeedIsNotNull()
      {
         var key = new Key(seed1);
         Assert.NotNull(key);
      }

      [Test]
      public void KeyFromSeedPublicKeyLengthIs64()
      {
         var key = new Key(seed1);
         Assert.AreEqual(64, key.Public.Length);
      }

      [Test]
      public void KeyFromSeedPrivateKeyLengthIs128()
      {
         var key = new Key(seed1);
         Assert.AreEqual(128, key.Private.Length);
      }

      [Test]
      public void KeyFromSeedPublicKeyEqualsKey()
      {
         var key = new Key(seed1);
         Assert.AreEqual(seed1PrivateKey, key.Public);
      }

      [Test]
      public void KeyFromSeedPublicKeyNotEqualsKey()
      {
         var key = new Key(seed1 + "1");
         Assert.AreNotEqual(seed1PrivateKey, key.Public);
      }

      [Test]
      public void PubKeyConvertToB58IsValidAdress()
      {
         var key = new Key(seed1);
         var adress = key.Adress;
         Assert.NotNull(key);
         Assert.NotNull(adress);
         Assert.AreEqual(34, adress.Length);
         Assert.AreEqual(seed1Adress, adress);
      }

      [Test]
      public void PubKeyConvertToB58IsNotValidWhenSeedChanged()
      {
         var key = new Key(seed1 + "test");
         var adress = key.Adress;
         Assert.NotNull(key);
         Assert.NotNull(adress);
         Assert.AreNotEqual(seed1Adress, adress);
      }
   }
}