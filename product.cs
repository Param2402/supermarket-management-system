using System;

namespace SupplyChainManagementSystem
{
    public class product
    {
      public int ProductID { get; set; }
      public string ProductName { get; set; }
      public string Barcode { get; set; }
      public string Category { get; set; }
      public string Supplier { get; set; }
      public decimal Price { get; set; }
      public int StockQuantity { get; set; }
      public DateTime ExpiryDate { get; set; }
      public int QuantitySold { get; set; }
      public int QuantityRestocked { get; set; }

        public Product(int id, string name, string barcode, string category, string supplier, decimal price, int stock, DateTime expiryDate)
{
    ProductID = id;
    ProductName = name;
    Barcode = barcode;
    Category = category;
    Supplier = supplier;
    Price = price;
    StockQuantity = stock;
    ExpiryDate = expiryDate;
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
