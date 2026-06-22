namespace SupplyChainManagementSystem
{
    public class BSTNode
    {
        public FurnitureItem Data { get; set; }
        public BSTNode Left { get; set; }
        public BSTNode Right { get; set; }

        public BSTNode(FurnitureItem item)
        {
            Data = item;
            Left = null;
            Right = null;
        }
    }
}
