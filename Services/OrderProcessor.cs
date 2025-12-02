using System;
using System.Linq;
using System.Threading.Tasks;
using OnlineStoreSequence.Models;

namespace OnlineStoreSequence.Services
{
    public class OrderResult
    {
        public bool IsSuccess { get; }
        public Order Order { get; }
        public string ErrorMessage { get; }
        
        public OrderResult(bool isSuccess, Order order = null, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            Order = order;
            ErrorMessage = errorMessage;
        }
    }
    
    public class OrderProcessor
    {
        private readonly ShoppingCart _shoppingCart;
        private readonly PaymentSystem _paymentSystem;
        private readonly DatabaseService _database;
        private readonly DeliverySystem _deliverySystem;
        private readonly NotificationService _notificationService;
        
        public OrderProcessor(
            ShoppingCart shoppingCart,
            PaymentSystem paymentSystem,
            DatabaseService database,
            DeliverySystem deliverySystem,
            NotificationService notificationService)
        {
            _shoppingCart = shoppingCart;
            _paymentSystem = paymentSystem;
            _database = database;
            _deliverySystem = deliverySystem;
            _notificationService = notificationService;
        }
        
        public async Task<OrderResult> ProcessOrderAsync(User user)
        {
            Console.WriteLine("\n[OrderProcessor.ProcessOrderAsync()] Начинаем обработку заказа");
            
           
            if (!_shoppingCart.HasItems())
            {
                Console.WriteLine("[OrderProcessor] ❌ Корзина пуста");
                return new OrderResult(false, errorMessage: "Корзина пуста");
            }
            
            
            var items = _shoppingCart.GetItems();
            var totalAmount = _shoppingCart.CalculateTotal();
            
            var order = new Order(0, user.Id, items, totalAmount)
            {
                ShippingAddress = user.Address
            };
            
            Console.WriteLine($"[OrderProcessor] Создан заказ: {items.Count} товаров на сумму {totalAmount:C}");
            
           
            Console.WriteLine("\n[OrderProcessor -> PaymentSystem] Обработка оплаты...");
            var paymentResult = await _paymentSystem.ProcessPaymentAsync(
                order,
                "4111111111111111",
                user.Name,
                "12/25",
                "123"
            );
            
            if (!paymentResult.IsSuccess)
            {
                Console.WriteLine($"[OrderProcessor] ❌ Оплата не удалась: {paymentResult.ErrorMessage}");
                
              
                await _notificationService.SendPaymentFailedAsync(user, order, paymentResult.ErrorMessage);
                
                return new OrderResult(false, errorMessage: paymentResult.ErrorMessage);
            }
            
            order.PaymentTransactionId = paymentResult.TransactionId;
            order.UpdateStatus(OrderStatus.Paid);
            Console.WriteLine($"[OrderProcessor] ✅ Оплата успешна, ID транзакции: {paymentResult.TransactionId}");
            
        
            Console.WriteLine("\n[OrderProcessor -> Database] Сохранение заказа...");
            var orderId = await _database.SaveOrderAsync(order);
            order.Id = orderId;
            
           
            Console.WriteLine("\n[OrderProcessor -> DeliverySystem] Планирование доставки...");
            var deliveryResult = await _deliverySystem.ScheduleDeliveryAsync(order);
            
            if (!deliveryResult.IsScheduled)
            {
                Console.WriteLine($"[OrderProcessor] ❌ Не удалось запланировать доставку: {deliveryResult.ErrorMessage}");
                order.UpdateStatus(OrderStatus.Failed);
                await _database.UpdateOrderAsync(order);
                
                return new OrderResult(false, errorMessage: deliveryResult.ErrorMessage);
            }
            
            order.TrackingNumber = deliveryResult.TrackingNumber;
            order.UpdateStatus(OrderStatus.Processing);
            
            
            await _database.UpdateOrderAsync(order);
            
            Console.WriteLine("\n[OrderProcessor -> NotificationService] Отправка подтверждения...");
            await _notificationService.SendOrderConfirmationAsync(user, order);
      
            await _notificationService.SendShippingUpdateAsync(
                user, 
                order, 
                $"Ваш заказ #{order.Id} запланирован на доставку {deliveryResult.EstimatedDelivery:dd.MM.yyyy}"
            );
            
            Console.WriteLine($"[OrderProcessor] ✅ Заказ #{order.Id} успешно обработан!");
            
            return new OrderResult(true, order);
        }
    }
}
