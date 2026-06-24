using System;

namespace Supermarketmanagementsystem
{
    public class CustomBST
    {
        private BSTNode root;

        public CustomBST() { root = null; }

        // INSERT 
        // Time Complexity: O(h) - O(log n) average, O(n) worst case
        public void Insert( Product newItem)
        {
            root = InsertRecursive(root, newItem);
        }

        private BSTNode InsertRecursive(BSTNode current, Product newItem)
        {
            if (current == null)
                return new BSTNode(newItem);

            if (newItem.ProductID < current.Data.ProductID)
                current.Left = InsertRecursive(current.Left, newItem);
            else if (newItem.ProductID > current.Data.ProductID)
                current.Right = InsertRecursive(current.Right, newItem);
            // Duplicate IDs are ignored

            return current;
        }

        // SEARCH 
        // Time Complexity: O(h) - O(log n) average, O(n) worst case
        public Product Search(int targetID)
        {
            return SearchRecursive(root, targetID);
        }

        private Product SearchRecursive(BSTNode current, int targetID)
        {
            if (current == null) return null;
            if (current.Data.ProductID == targetID) return current.Data;
            if (targetID < current.Data.ProductID)
                return SearchRecursive(current.Left, targetID);
            else
                return SearchRecursive(current.Right, targetID);
        }

        // DELETE 
        // Time Complexity: O(h) - O(log n) average, O(n) worst case
        public bool Delete(int targetID)
        {
            bool deleted = false;
            root = DeleteRecursive(root, targetID, ref deleted);
            return deleted;
        }

        private BSTNode DeleteRecursive(BSTNode current, int targetID, ref bool deleted)
        {
            if (current == null)
                return null;

            if (targetID < current.Data.ProductID)
            {
                current.Left = DeleteRecursive(current.Left, targetID, ref deleted);
            }
            else if (targetID > current.Data.ProductID)
            {
                current.Right = DeleteRecursive(current.Right, targetID, ref deleted);
            }
            else
            {
                
                deleted = true;

                // Case 1: Leaf node (no children)
                if (current.Left == null && current.Right == null)
                    return null;

                // Case 2: One child
                if (current.Left == null) return current.Right;
                if (current.Right == null) return current.Left;

                // Case 3: Two children 
                Product successor = FindMin(current.Right);
                current.Data = successor;
                current.Right = DeleteRecursive(current.Right, successor.ProductID, ref deleted);
            }

            return current;
        }

        private Product FindMin(BSTNode current)
        {
            while (current.Left != null)
                current = current.Left;
            return current.Data;
        }

        // SEARCH BY CATEGORY 
        // Time Complexity: O(n) — must visit every node
        public void SearchByCategory(string category)
        {
            Console.WriteLine($"\n--- Products in Category: {category} ---");
            Console.WriteLine("ID\t| ProductName                 | Stock | Price    | Zone");
            Console.WriteLine("--------------------------------------------------------------");

            bool found = false;
            SearchCategoryRecursive(root, category.ToLower(), ref found);

            if (!found)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No Product found in category '{category}'.");
                Console.ResetColor();
            }
        }

        private void SearchCategoryRecursive(BSTNode current, string category, ref bool found)
        {
            if (current == null) return;

            SearchCategoryRecursive(current.Left, category, ref found);

            if (current.Data.Category.ToLower() == category)
            {
                Console.WriteLine($"{current.Data.ProductID}\t| {current.Data.ProductName,-20} | {current.Data.StockQuantity,-5} | £{current.Data.Price,-7:F2} | {current.Data.WarehouseZone}");
                found = true;
                }

                SearchCategoryRecursive(current.Right, category, ref found);
            }

                public Product SearchByBarcode(string barcode)
            {
                return SearchBarcodeRecursive(root, barcode);
            }


            private Product SearchBarcodeRecursive(BSTNode current, string barcode)
        {
            if (current == null)
            return null;

            if (current.Data.Barcode == barcode)
            return current.Data;

            Product found =
            SearchBarcodeRecursive(current.Left, barcode);

            if (found != null)
            return found;

            return SearchBarcodeRecursive(current.Right, barcode);
        }
        // DISPLAY ALL INVENTORY 
        // Time Complexity: O(n) — visits every node
        public void DisplayAllInventory()
        {
            if (root == null)
            {
                Console.WriteLine("There is no products in inventory.");
                return;
            }

            int totalProducts = 0;
            int totalPhysicalStock = 0;
            CalculateStats(root, ref totalProducts, ref totalPhysicalStock);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n================ INVENTORY SUMMARY ================");
            Console.WriteLine($"Total Unique Products : {totalProducts}");
            Console.WriteLine($"Total Items in Stock  : {totalPhysicalStock}");
            Console.WriteLine("===================================================");
            Console.ResetColor();

            Console.WriteLine("\nID\t| ProductName                 | Stock Left | Sold | Restocked | Status       | Zone");
            Console.WriteLine("------------------------------------------------------------------------------------------------");

            InOrderTraversal(root);
        }

        private void CalculateStats(BSTNode current, ref int totalProducts, ref int totalPhysicalStock)
        {
            if (current != null)
            {
                CalculateStats(current.Left, ref totalProducts, ref totalPhysicalStock);
                totalProducts++;
                totalPhysicalStock += current.Data.StockQuantity;
                CalculateStats(current.Right, ref totalProducts, ref totalPhysicalStock);
            }
        }

        private void InOrderTraversal(BSTNode current)
        {
            if (current != null)
            {
                InOrderTraversal(current.Left);

                string status = current.Data.StockQuantity == 0 ? "OUT OF STOCK"
                    : (current.Data.StockQuantity <= 5 ? "LOW STOCK" : "IN STOCK");
                ConsoleColor color = current.Data.StockQuantity == 0 ? ConsoleColor.Red
                    : (current.Data.StockQuantity <= 5 ? ConsoleColor.Yellow : ConsoleColor.Green);

                Console.Write($"{current.Data.ProductID}\t| {current.Data.ProductName,-20} | {current.Data.StockQuantity,-10} | {current.Data.QuantitySold,-4} | {current.Data.QuantityRestocked,-9} | ");
                Console.ForegroundColor = color;
                Console.Write($"{status,-12}");
                Console.ResetColor();
                Console.WriteLine($" | {current.Data.WarehouseZone}");

                InOrderTraversal(current.Right);
            }
        }

        //  LOW STOCK REPORT 
        // Time Complexity: O(n) — visits every node
        public void DisplayLowStockItems(int limit)
        {
            Console.WriteLine($"\n--- LOW STOCK REPORT (Stock ≤ {limit}) ---");
            Console.WriteLine("ID\t| ProductName                | Stock | Status");
            Console.WriteLine("-------------------------------------------------------");

            bool foundAny = false;
            CheckLowStockRecursive(root, limit, ref foundAny);

            if (!foundAny)
                Console.WriteLine($"All Products are sufficiently stocked. No item is at or below {limit}.");
        }

        private void CheckLowStockRecursive(BSTNode current, int limit, ref bool foundAny)
        {
            if (current != null)
            {
                CheckLowStockRecursive(current.Left, limit, ref foundAny);

                if (current.Data.StockQuantity <= limit)
                {
                    string status = current.Data.StockQuantity == 0 ? "OUT OF STOCK" : "LOW STOCK";
                    ConsoleColor color = current.Data.StockQuantity == 0 ? ConsoleColor.Red : ConsoleColor.Yellow;

                    Console.Write($"{current.Data.ProductID}\t| {current.Data.ProductName,-20} | {current.Data.StockQuantity,-5} | ");
                    Console.ForegroundColor = color;
                    Console.WriteLine(status);
                    Console.ResetColor();

                    foundAny = true;
                }

                CheckLowStockRecursive(current.Right, limit, ref foundAny);
            }
        }

        // GET MAX ID 
        // Time Complexity: O(h) — traverses right spine only
        public int GetMaxProductID()
        {
            if (root == null) return 0;
            BSTNode current = root;
            while (current.Right != null)
                current = current.Right;
            return current.Data.ProductID;
        }
    }
}
