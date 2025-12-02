namespace OnlineStoreSequence.Models
{
    public class Product
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public string Description { get; }
        public int StockQuantity { get; private set; }
        
        public Product(int id, string name, decimal price, string description, int stockQuantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
        }
        
        public bool ReduceStock(int quantity)
        {
            if (StockQuantity >= quantity)
            {
                StockQuantity -= quantity;
                return true;
            }
            return false;
        }
        
        public override string ToString()
        {
            return $"{Name} - {Price:C} (В наличии: {StockQuantity})";
        }
    }
}
