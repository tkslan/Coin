using Coin.Core;
using NUnit.Framework;

namespace Coin.Tests
{
   [TestFixture]
   public class Blocks
   {
      [Test]
      public void CreateNewBlockNotNull()
      {
         var block = new Block(0);
         Assert.NotNull(block);
      }

      [Test]
      public void AddTxToNewBlock()
      {
         var key = new Key();
         var tx = new BlockTx()
         {
            Amount = 1,
            PubKeyFrom = key.Public,
            PubKeyTo = new Key("test").Public
         };
         tx.Sign(key);
         var block=new Block(0);
         block.AddTx(tx);
         Assert.NotNull(block);
         Assert.NotNull(block.Txes);
         Assert.AreEqual(1,block.Txes.Length);
      }
      [Test]
      public void AddTwoTxesToNewBlock()
      {
         var key = new Key();
         
         var tx1 = new BlockTx()
         {
            Amount = 1,
            PubKeyFrom = key.Public,
            PubKeyTo = new Key("test").Public
         };
       
         var tx2 = new BlockTx()
         {
            Amount = 5,
            PubKeyFrom = key.Public,
            PubKeyTo = new Key("test2").Public
         };
         
         tx1.Sign(key);
         tx2.Sign(key);
         
         var block=new Block(0);
         block.AddTx(tx1);
         block.AddTx(tx2);
         Assert.NotNull(block);
         Assert.NotNull(block.Txes);
         Assert.AreEqual(2,block.Txes.Length);
      }
   }
}