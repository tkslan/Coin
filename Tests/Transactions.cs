using Coin.Core;
using NUnit.Framework;

namespace Coin.Tests
{
   [TestFixture]
   public class Transactions
   {
      [Test]
      public void SingTxWithPublicKey()
      {
         var key = new Key();
         var tx = new BlockTx()
         {
            Amount = 1,
            PubKeyFrom = key.Public,
            PubKeyTo = new Key("test").Public
         };
         var serializedTx = tx.Serialize();
         tx.Sign(key);
         Assert.NotNull(tx.Fingerprint);
         Assert.NotZero(tx.Fingerprint.Length);
         Assert.IsTrue(key.Verify(serializedTx, ByteArray.HexToByte(tx.Fingerprint)));
      }

      [Test]
      public void ValidateTxFromPublicKey()
      {
         var key = new Key();
         var tx = new BlockTx()
         {
            Amount = 1,
            PubKeyFrom = key.Public,
            PubKeyTo = new Key("test").Public
         };
         var serializedTx = tx.Serialize();
         tx.Sign(key);
         var signKey = new Key(ByteArray.HexToByte(tx.PubKeyFrom));
         Assert.IsTrue(signKey.Verify(serializedTx, ByteArray.HexToByte(tx.Fingerprint)));
      }
      [Test]
      public void ValidateTxFailFromAlteredPublicKey()
      {
         var key = new Key();
         var tx = new BlockTx()
         {
            Amount = 1,
            PubKeyFrom = key.Public,
            PubKeyTo = new Key("test").Public
         };
         var serializedTx = tx.Serialize();
         tx.Sign(key);
         var signKey = new Key(ByteArray.HexToByte(tx.PubKeyFrom));
         var alteredPubKey = ByteArray.HexToByte(tx.Fingerprint.Replace("B", "C"));
         Assert.IsFalse(signKey.Verify(serializedTx, alteredPubKey));
      }
      [Test]
      public void ValidateTxFailFromAlteredTransactionAmount()
      {
         var key = new Key();
         var tx = new BlockTx()
         {
            Amount = 1,
            PubKeyFrom = key.Public,
            PubKeyTo = new Key("test").Public
         };
         tx.Sign(key);
         var signKey = new Key(ByteArray.HexToByte(tx.PubKeyFrom));
         tx.Amount = 2;
         Assert.IsFalse(signKey.Verify(tx.Serialize(), ByteArray.HexToByte(tx.Fingerprint)));
      }
   }
}