namespace OnlineStoreSequence.Models
{
    public class CartItem
    {
        public Product Product { get; }
        public int Quantity { get; set; }
        
        public decimal TotalPrice => Product.Price * Quantity;
        
        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
        
        public override string ToString()
        {
            return $"{Product.Name} x{Quantity} = {TotalPrice:C}";
        }
    }
}
