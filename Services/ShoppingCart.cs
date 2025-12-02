using System;
using System.Collections.Generic;
using System.Linq;
using OnlineStoreSequence.Models;

namespace OnlineStoreSequence.Services
{
    public class ShoppingCart
    {
        private List<CartItem> _items;
        private int _userId;
        
        public ShoppingCart(int userId)
        {
            _userId = userId;
            _items = new List<CartItem>();
        }
        
        public void AddItem(Product product, int quantity)
        {
            Console.WriteLine($"   [ShoppingCart.AddItem()] Добавляем товар: {product.Name}");
            
            var existingItem = _items.FirstOrDefault(item => item.Product.Id == product.Id);
            
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                Console.WriteLine($"   [ShoppingCart] Увеличено количество: {existingItem.Product.Name} -> {existingItem.Quantity}");
            }
            else
            {
                _items.Add(new CartItem(product, quantity));
                Console.WriteLine($"   [ShoppingCart] Добавлен новый товар: {product.Name} x{quantity}");
            }
        }
        
        public void RemoveItem(int productId)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }
        
        public void Clear()
        {
            _items.Clear();
        }
        
        public decimal CalculateTotal()
        {
            return _items.Sum(item => item.TotalPrice);
        }
        
        public List<CartItem> GetItems()
        {
            return new List<CartItem>(_items);
        }
        
        public void DisplayCart()
        {
            Console.WriteLine("\n=== СОДЕРЖИМОЕ КОРЗИНЫ ===");
            
            if (_items.Count == 0)
            {
                Console.WriteLine("Корзина пуста");
                return;
            }
            
            foreach (var item in _items)
            {
                Console.WriteLine($"  - {item}");
            }
            
            Console.WriteLine($"\nОбщая сумма: {CalculateTotal():C}");
            Console.WriteLine($"Количество товаров: {_items.Sum(i => i.Quantity)}");
            Console.WriteLine("=========================\n");
        }
        
        public bool HasItems()
        {
            return _items.Count > 0;
        }
    }
}
