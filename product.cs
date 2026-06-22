using System;

namespace SupplyChainManagementSystem
{
    public class product
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int StockLevel { get; set; }
        public int QuantitySold { get; set; } = 0;
        public int QuantityRestocked { get; set; } = 0;
        public string Barcode { get; set; }
        public string SupplierName { get; set; }
        public DateTime ExpiryDate { get; set; }

        public product(int id, string name, string category, double price, int stock, string zone)
        {
            ItemID = id;
            Name = name;
            Category = category;
            Price = price;
            StockLevel = stock;
            WarehouseZone = zone;
        }

        public void DisplayItem()
        {
            Console.WriteLine($"ID: {ItemID} | {Name} ({Category}) | Price: £{Price:F2}");
            string status = StockLevel == 0 ? "OUT OF STOCK" : (StockLevel <= 5 ? "LOW STOCK" : "IN STOCK");
            ConsoleColor color = StockLevel == 0 ? ConsoleColor.Red : (StockLevel <= 5 ? ConsoleColor.Yellow : ConsoleColor.Green);
            Console.Write($"Current Stock: {StockLevel} [");
            Console.ForegroundColor = color;
            Console.Write(status);
            Console.ResetColor();
            Console.WriteLine($"] | Location: {WarehouseZone}");
            Console.WriteLine($"Total Sold: {QuantitySold} | Total Restocked: {QuantityRestocked}");
        }
    }
}
