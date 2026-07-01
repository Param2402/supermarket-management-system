using System;
using System.Data.SqlClient;

namespace Supermarketmanagementsystem
{
    class Program
    {
        static string connectionString =
    @"Server=localhost\SQLEXPRESS;Database=SupermarketDB;Trusted_Connection=True;TrustServerCertificate=True;";


        static void Main(string[] args)
        {
            CustomBST inventoryTree = new CustomBST();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===========================================");
            Console.WriteLine("   APES SUPPLY CHAIN MANAGEMENT SYSTEM  ");
            Console.WriteLine("===========================================\n");
            Console.ResetColor();

            //  Load data the from SQL into BST
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = @"SELECT ProductID, ProductName, Category, Barcode, Supplier, Price, StockQuantity, 
                                               WarehouseZone, QuantitySold, QuantityRestocked 
                                        FROM Products";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string category = reader.GetString(2);
                            double price = (double)reader.GetDecimal(3);
                            int stock = reader.GetInt32(4);
                            string zone = reader.GetString(5);
                            int sold = reader.GetInt32(6);
                            int restocked = reader.GetInt32(7);

                            FurnitureItem fi = new FurnitureItem(id, name, category, price, stock, zone);
                            fi.QuantitySold = sold;
                            fi.QuantityRestocked = restocked;
                            inventoryTree.Insert(fi);
                        }
                    }
                }

                //Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine("Inventory loaded successfully from database.");
                //Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nDatabase connection failed. Please check if your SQL Server is running.");
                Console.WriteLine("Error details: " + ex.Message);
                Console.ResetColor();
                return;
            }

            bool running = true;

            while (running)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n=============== MAIN MENU ===============");
                Console.WriteLine("1. Search Item by ID");
                Console.WriteLine("2. Search Items by Category");
                Console.WriteLine("3. View Full Inventory Report");
                Console.WriteLine("4. Add New Item");
                Console.WriteLine("5. Remove Item");
                Console.WriteLine("6. Sell Item");
                Console.WriteLine("7. Restock Item");
                Console.WriteLine("8. Generate Low Stock Report");
                Console.WriteLine("9. Exit System");
                Console.WriteLine("=========================================");
                Console.ResetColor();

                Console.Write("Select an option (1-9): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SearchItemMenu(inventoryTree);
                        break;
                    case "2":
                        SearchByCategoryMenu(inventoryTree);
                        break;
                    case "3":
                        inventoryTree.DisplayAllInventory();
                        break;
                    case "4":
                        AddItemMenu(inventoryTree);
                        break;
                    case "5":
                        RemoveItemMenu(inventoryTree);
                        break;
                    case "6":
                        UpdateStockMenu(inventoryTree, false);
                        break;
                    case "7":
                        UpdateStockMenu(inventoryTree, true);
                        break;
                    case "8":
                        int limit;
                        while (true)
                        {
                            Console.Write("\nEnter stock limit: ");
                            if (int.TryParse(Console.ReadLine(), out limit)) break;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                            Console.ResetColor();
                        }
                        inventoryTree.DisplayLowStockItems(limit);
                        break;
                    case "9":
                        Console.WriteLine("\nExiting system... Thank you for using the application.");
                        running = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please choose a number between 1 and 9.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        //  function Search by ID 
        static void SearchItemMenu(CustomBST inventoryTree)
        {
            int searchId;
            while (true)
            {
                Console.Write("\nEnter Item ID to search: ");
                if (int.TryParse(Console.ReadLine(), out searchId)) break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Please enter a numeric Item ID.");
                Console.ResetColor();
            }

            FurnitureItem foundItem = inventoryTree.Search(searchId);

            if (foundItem != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nItem found:");
                Console.ResetColor();
                foundItem.DisplayItem();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nNo item found with ID '{searchId}'.");
                Console.ResetColor();
            }
        }

        //  function Search by Category
        static void SearchByCategoryMenu(CustomBST inventoryTree)
        {
            Console.Write("\nEnter category to search (Tables, Seating, Beds, Storage): ");
            string category = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(category))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Category cannot be empty.");
                Console.ResetColor();
                return;
            }

            inventoryTree.SearchByCategory(category);
        }

        //  function Add New Item
        static void AddItemMenu(CustomBST inventoryTree)
        {
            Console.WriteLine("\n--- ADD NEW ITEM ---");

            Console.Write("Enter item name: ");
            string name = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Name cannot be empty.");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter category (Tables, Seating, Beds, Storage): ");
            string category = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(category))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Category cannot be empty.");
                Console.ResetColor();
                return;
            }

            double price;
            while (true)
            {
                Console.Write("Enter price (£): ");
                if (double.TryParse(Console.ReadLine(), out price) && price >= 0) break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid price. Please enter a positive number.");
                Console.ResetColor();
            }

            int stock;
            while (true)
            {
                Console.Write("Enter initial stock level: ");
                if (int.TryParse(Console.ReadLine(), out stock) && stock >= 0) break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid stock. Please enter a non-negative whole number.");
                Console.ResetColor();
            }

            Console.Write("Enter warehouse zone (e.g. Zone A - Aisle 1): ");
            string zone = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(zone))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Warehouse zone cannot be empty.");
                Console.ResetColor();
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // IDENTITY column auto-generates ItemID — retrieve it with SCOPE_IDENTITY()
                    string insertQuery = @"
                        INSERT INTO Furniture (Name, Category, Price, StockLevel, WarehouseZone, QuantitySold, QuantityRestocked)
                        VALUES (@name, @cat, @price, @stock, @zone, 0, 0);
                        SELECT SCOPE_IDENTITY();";

                    int newId;
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@cat", category);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@stock", stock);
                        command.Parameters.AddWithValue("@zone", zone);
                        newId = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // Insert into BST using the real DB-generated ID
                    FurnitureItem newItem = new FurnitureItem(newId, name, category, price, stock, zone);
                    inventoryTree.Insert(newItem);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nItem '{name}' added successfully with ID: {newId}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to save item to database: " + ex.Message);
                Console.ResetColor();
            }
        }

        // function Remove Item
        static void RemoveItemMenu(CustomBST inventoryTree)
        {
            int itemId;
            while (true)
            {
                Console.Write("\nEnter Item ID to remove: ");
                if (int.TryParse(Console.ReadLine(), out itemId)) break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Item ID must be a number.");
                Console.ResetColor();
            }

            FurnitureItem item = inventoryTree.Search(itemId);
            if (item == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Item with ID '{itemId}' not found in inventory.");
                Console.ResetColor();
                return;
            }

            Console.Write($"Are you sure you want to remove '{item.Name}' (ID: {itemId})? (yes/no): ");
            string confirm = Console.ReadLine()?.Trim().ToLower();
            if (confirm != "yes" && confirm != "y")
            {
                Console.WriteLine("Removal cancelled.");
                return;
            }

            inventoryTree.Delete(itemId);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Furniture WHERE ItemID = @id";
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", itemId);
                        command.ExecuteNonQuery();
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nItem '{item.Name}' (ID: {itemId}) removed successfully.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to remove item from database: " + ex.Message);
                Console.ResetColor();
            }
        }

        // function Sell or Restock
        static void UpdateStockMenu(CustomBST inventoryTree, bool isRestock)
        {
            int itemId;
            while (true)
            {
                Console.Write("\nEnter Item ID: ");
                if (int.TryParse(Console.ReadLine(), out itemId)) break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Item ID must be a number.");
                Console.ResetColor();
            }

            FurnitureItem item = inventoryTree.Search(itemId);
            if (item == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Item with ID '{itemId}' not found in inventory.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine($"Current Stock for '{item.Name}': {item.StockLevel}");
            Console.WriteLine($"Total Sold so far: {item.QuantitySold} | Total Restocked so far: {item.QuantityRestocked}");

            int quantity;
            while (true)
            {
                Console.Write(isRestock ? "Enter quantity to restock: " : "Enter quantity to sell: ");
                if (int.TryParse(Console.ReadLine(), out quantity))
                {
                    if (quantity <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Quantity must be greater than 0.");
                        Console.ResetColor();
                    }
                    else if (!isRestock && quantity > item.StockLevel)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Only {item.StockLevel} items available. Cannot sell more than stock.");
                        Console.ResetColor();
                    }
                    else break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a whole number.");
                    Console.ResetColor();
                }
            }

            // Update BST
            if (isRestock)
            {
                item.StockLevel += quantity;
                item.QuantityRestocked += quantity;
            }
            else
            {
                item.StockLevel -= quantity;
                item.QuantitySold += quantity;
            }

            // Persist ALL three fields in teh database
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = @"UPDATE Furniture 
                                           SET StockLevel        = @newStock,
                                               QuantitySold      = @sold,
                                               QuantityRestocked = @restocked
                                           WHERE ItemID = @id";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@newStock", item.StockLevel);
                        command.Parameters.AddWithValue("@sold", item.QuantitySold);
                        command.Parameters.AddWithValue("@restocked", item.QuantityRestocked);
                        command.Parameters.AddWithValue("@id", item.ItemID);
                        command.ExecuteNonQuery();
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nStock updated and saved to database successfully.");
                Console.WriteLine($"New Stock: {item.StockLevel} | Total Sold: {item.QuantitySold} | Total Restocked: {item.QuantityRestocked}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to update database: " + ex.Message);
                Console.ResetColor();
            }
        }
    }
}
