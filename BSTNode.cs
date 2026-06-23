namespace Supermarketmanagementsystem
{
    public class BSTNode
    {
        public Product Data { get; set; }
        public BSTNode Left { get; set; }
        public BSTNode Right { get; set; }

        public BSTNode( Product item )
        {
            Data = item;
            Left = null;
            Right = null;
        }
    }
}
