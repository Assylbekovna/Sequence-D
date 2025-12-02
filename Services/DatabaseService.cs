using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineStoreSequence.Models;

namespace OnlineStoreSequence.Services
{
    public class DatabaseService
    {
        private Dictionary<int, Order> _orders;
        private Dictionary<int, User> _users;
        private int _nextOrderId;
        
        public DatabaseService()
        {
            _orders = new Dictionary<int, Order>();
            _users = new Dictionary<int, User>();
            _nextOrderId = 1000;
        }
        
        public async Task<int> SaveOrderAsync(Order order)
        {
            Console.WriteLine($"   [DatabaseService.SaveOrderAsync()] Сохраняем заказ в базе данных");
            
            order.Id = _nextOrderId++;
            _orders[order.Id] = order;
            
            
            await Task.Delay(500);
            
            Console.WriteLine($"   [DatabaseService]  Заказ #{order.Id} успешно сохранен");
            Console.WriteLine($"   [DatabaseService] Детали: {order.Items.Count} товаров, {order.TotalAmount:C}");
            
            return order.Id;
        }
        
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            Console.WriteLine($"   [DatabaseService.UpdateOrderAsync()] Обновляем заказ #{order.Id}");
            
            if (_orders.ContainsKey(order.Id))
            {
                _orders[order.Id] = order;
                await Task.Delay(300);
                
                Console.WriteLine($"   [DatabaseService]  Заказ #{order.Id} обновлен");
                Console.WriteLine($"   [DatabaseService] Новый статус: {order.Status}");
                
                return true;
            }
            
            Console.WriteLine($"   [DatabaseService]  Заказ #{order.Id} не найден");
            return false;
        }
        
        public Order GetOrder(int orderId)
        {
            if (_orders.ContainsKey(orderId))
            {
                return _orders[orderId];
            }
            return null;
        }
        
        public string GetOrderStatus(int orderId)
        {
            var order = GetOrder(orderId);
            return order?.Status.ToString() ?? "Не найден";
        }
        
        public async Task<bool> CancelOrderAsync(int orderId, string reason)
        {
            Console.WriteLine($"   [DatabaseService.CancelOrderAsync()] Отменяем заказ #{orderId}");
            Console.WriteLine($"   [DatabaseService] Причина: {reason}");
            
            var order = GetOrder(orderId);
            if (order != null && order.Status != OrderStatus.Delivered)
            {
                order.UpdateStatus(OrderStatus.Cancelled);
                await UpdateOrderAsync(order);
                
                Console.WriteLine($"   [DatabaseService] Заказ #{orderId} отменен");
                return true;
            }
            
            Console.WriteLine($"   [DatabaseService]  Не удалось отменить заказ #{orderId}");
            return false;
        }
        
        public void SaveUser(User user)
        {
            _users[user.Id] = user;
        }
        
        public User GetUser(int userId)
        {
            return _users.ContainsKey(userId) ? _users[userId] : null;
        }
    }
}
