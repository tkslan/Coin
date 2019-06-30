using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Coin.Core
{
   public class Block
   {
      public uint Height;
      public long Timestamp;
      public uint Nonce;
      public Key PrevBlock;
      public Key ThisBlock;
      public BlockTx[] Txes;

      public Block(uint height)
      {
         Height = height;
         Timestamp = Time.Timestamp();
      }

      public void AddTx(BlockTx blockTx)
      {
         if (Txes == null)
         {
            Txes = new[] {blockTx};
            return;
         }

         var temp = new BlockTx[Txes.Length + 1];
         Txes.CopyTo(temp, 0);
         temp[Txes.Length] = blockTx;
         Txes=new BlockTx[temp.Length];
         temp.CopyTo(Txes, 0);
         temp = null;
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
}