namespace Coin.Core
{
    public class Block
    {
        public uint Height;
        public long Timestamp;
        public uint Nonce;
        public Key PrevBlock;
        public Key ThisBlock;
        public Tx[] Txes;
    }
}