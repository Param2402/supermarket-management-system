using System;

namespace Supermarketmanagementsystem
{
    public class Product
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
      public string WarehouseZone { get; set; }

      public Product(int id, string name, string barcode, string category, string supplier, decimal price, int stock, DateTime expiryDate, string warehouseZone)
    {
        ProductID = id;
        ProductName = name;
        Barcode = barcode;
        Category = category;
        Supplier = supplier;
        Price = price;
        StockQuantity = stock;
        ExpiryDate = expiryDate;
        WarehouseZone = warehouseZone;
    }
      public void DisplayItem()
        
    {
        Console.WriteLine($"ID: {ProductID}");
        Console.WriteLine($"Name: {ProductName}");
        Console.WriteLine($"Barcode: {Barcode}");
        Console.WriteLine($"Category: {Category}");
        Console.WriteLine($"Supplier: {Supplier}");
        Console.WriteLine($"Price: £{Price:F2}");
        Console.WriteLine($"Expiry Date: {ExpiryDate:dd/MM/yyyy}");

        string status =
        StockQuantity == 0 ? "OUT OF STOCK" :
        StockQuantity <= 5 ? "LOW STOCK" :
        "IN STOCK";

        Console.WriteLine($"Stock Quantity: {StockQuantity} ({status})");
        Console.WriteLine($"Total Sold: {QuantitySold}");
        Console.WriteLine($"Total Restocked: {QuantityRestocked}");
    
        }
    }
}
