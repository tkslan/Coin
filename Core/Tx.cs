using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Coin.Core
{
   [System.Serializable]
   public class Tx
   {
      public long Timestamp { get; private set; }
      public string PubKeyFrom;
      public string PubKeyTo;
      public uint Amount;

      public Tx()
      {
         this.Timestamp = Time.Timestamp();
      }

      public byte[] Serialize()
      {
         using (var ms = new MemoryStream())
         {
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(ms, this);
            return ms.GetBuffer();
         }
      }
   }

   [System.Serializable]
   public class BlockTx : Tx
   {
      public string Fingerprint { get; private set; }

      public void Sign(Key pubKey)
      {
         var serializedTx = Serialize();
         var fingerprint = pubKey.Sing(serializedTx);
         var verified = pubKey.Verify(serializedTx, ByteArray.HexToByte(fingerprint));
         
         if (verified == false)
            throw new OperationCanceledException("Transaction cannot be verified, this is an error");
         
         Fingerprint = fingerprint;

      }
   }
}