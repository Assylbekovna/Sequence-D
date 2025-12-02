using System;
using System.Collections.Generic;

namespace OnlineStoreSequence.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Paid,
        Shipped,
        Delivered,
        Cancelled,
        Failed
    }
    
    public class Order
    {
        public int Id { get; }
        public int UserId { get; }
        public List<CartItem> Items { get; }
        public decimal TotalAmount { get; }
        public string ShippingAddress { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; set; }
        public string TrackingNumber { get; set; }
        public string PaymentTransactionId { get; set; }
        
        public Order(int id, int userId, List<CartItem> items, decimal totalAmount)
        {
            Id = id;
            UserId = userId;
            Items = items;
            TotalAmount = totalAmount;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.Now;
        }
        
        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
            UpdatedAt = DateTime.Now;
        }
        
        public override string ToString()
        {
            return $"Заказ #{Id} от {CreatedAt:dd.MM.yyyy HH:mm} - {TotalAmount:C} ({Status})";
        }
    }
}
